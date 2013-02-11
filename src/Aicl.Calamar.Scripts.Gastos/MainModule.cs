using System.Html;
using System.Runtime.CompilerServices;
using Cayita.Javascript.UI;
using Cayita.Javascript;
using jQueryApi;
using Cayita.Javascript.Plugins;
using Aicl.Calamar.Scripts.Modelos;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Aicl.Calamar.Scripts.ModuloGastos
{
	[IgnoreNamespace]
	public class MainModule
	{
		public MainModule (){}
		Div SearchDiv {get;set;}
		Div FormDiv {get;set;}
		Div GridDiv {get;set;}
		Form Form {get;set;}

		HtmlTable TableGastos {get;set;}
		List<Gasto> ListGastos {get;set;}
		SelectedRow SelectedGasto {get;set;}
		List<TableColumn<Gasto>> Columns {get;set;}

		Button BNew {get;set;}
		Button BDelete {get;set;}
		Button BList {get;set;}

		List<Concepto> ListConceptos {get;set;}
		List<Fuente> ListFuentes {get;set;}
				
		public static void Execute(Element parent )
		{
			new MainModule().Paint(parent);
		}

		void Paint(Element parent)
		{	
			new Div(parent, div=>{
				div.ClassName="span6 offset3 well";
				div.Hide();
			}) ;
			
			SearchDiv= new Div(default(Element), searchdiv=>{
				searchdiv.ClassName= "span6 offset3 nav";

				var inputFecha=new InputText(searchdiv, ip=>{
					ip.ClassName="input-medium search-query";
					ip.SetAttribute("data-mask","99.99.9999");
					ip.SetPlaceHolder("dd.mm.aaaa");
				}).Element(); 
				
				new IconButton(searchdiv, (abn, ibn)=>{
					ibn.ClassName="icon-search icon-large";
					abn.JSelect().Click(evt=>{
						if( ! inputFecha.Value.IsDateFormatted()){
							Div.CreateAlertErrorAfter(SearchDiv.Element(),"Digite una fecha valida");
							return;
						}
						LoadGastos( inputFecha.Value );

					});
				});

				BNew= new IconButton(searchdiv, (abn, ibn)=>{
					ibn.ClassName="icon-plus-sign icon-large";
					abn.JSelect().Click(evt=>{
						FormDiv.FadeIn();
						GridDiv.FadeOut();
						Form.Element().Reset();
						BDelete.Element().Disabled=true;
					});
				});

				BDelete=new IconButton(searchdiv, (abn, ibn)=>{
					ibn.ClassName="icon-remove-sign icon-large";
					abn.Disabled=true;
					abn.JSelect().Click(evt=>{
						RemoveRow();
					});
				});

				BList= new IconButton(searchdiv, (abn, ibn)=>{
					ibn.ClassName="icon-reorder icon-large";
					abn.Disabled=true;
					abn.JSelect().Click(evt=>{
						FormDiv.FadeOut();
						GridDiv.FadeIn();
						abn.Disabled=true;
					});
				});
				
			});
			SearchDiv.AppendTo(parent);
			
			
			FormDiv= new Div(default(Element), formdiv=>{
				formdiv.ClassName="span6 offset3 well";
				Form = new Form(formdiv, f=>{
					f.ClassName="form-horizontal";
					f.Action="api/Gasto/";
					f.Method="post";

					var inputId= new InputText(f, e=>{
						e.Name="Id";
						e.Hide();
					}); 

					var cbConcepto=new SelectField(f, (e)=>{
						e.Name="IdConcepto";
						e.ClassName="span12";
						new HtmlOption(e, o=>{
							o.Value="";
							o.Selected=true;
							o.Text="Seleccione el concepto ...";
						});											
						LoadConceptos(e);
					});
					
					var cbFuente= new SelectField(f, (e)=>{
						e.Name="IdFuente";
						e.ClassName="span12";
						new HtmlOption(e, o=>{
							o.Value="";
							o.Selected=true;
							o.Text="Seleccione la fuente de pago ...";
						});											
						LoadFuentes(e);
					});
					
					var fieldValor= new TextField(f,(field)=>{
						field.ClassName="span12";
						field.Name="Valor";
						field.SetPlaceHolder("$$$$$$$$$$");
						field.AutoNumericInit();
						field.Style.TextAlign="right";
					});
					
					new TextField(f,(field)=>{
						field.ClassName="span12";
						field.Name="Beneficiario";
						field.SetPlaceHolder("Pagado a ....");
					});
					
					new TextField(f,(field)=>{
						field.ClassName="span12";
						field.Name="Descripcion";
						field.SetPlaceHolder("Descripcion");
					});
										
					var bt = new SubmitButton(f, b=>{
						b.JSelect().Text("Guardar");
						b.LoadingText(" Guardando ...");
						b.ClassName="btn btn-info btn-block" ;
					});
					
					var vo = new ValidateOptions()
						.SetSubmitHandler( form=>{
							
							bt.ShowLoadingText();

							var action= form.Action+(string.IsNullOrEmpty(inputId.Value())?"create":"update");
							jQuery.PostRequest<BLResponse<Gasto>>(action, form.AutoNumericGetString(), cb=>{},"json")
								.Success(d=>{
									Cayita.Javascript.Firebug.Console.Log("Success guardar gasto",d);
									if(string.IsNullOrEmpty(inputId.Value()) )
										AppendRow(d.Result[0]);
									else
										UpdateRow(d.Result[0]);

									form.Reset();
								})
									.Error((request,  textStatus,  error)=>{
										Cayita.Javascript.Firebug.Console.Log("request", request );
										Cayita.Javascript.Firebug.Console.Log("error", error );
										Div.CreateAlertErrorBefore(form.Elements[0],
										                           textStatus+": "+ request.StatusText);
									})
									.Always(a=>{
										bt.ResetLoadingText();
									});
							
						})
							.AddRule((rule, msg)=>{
								rule.Element=fieldValor.Element();
								rule.Rule.Required();
								msg.Required("Digite el valor del gasto");
							})

							.AddRule((rule, msg)=>{
								rule.Element=cbConcepto.Element();
								rule.Rule.Required();
								msg.Required("Seleccione el concepto");
							})
							
							.AddRule((rule, msg)=>{
								rule.Element=cbFuente.Element();
								rule.Rule.Required();
								msg.Required("Seleccione al fuente del pago");
							});
					
					f.Validate(vo);				
				});
			});

			FormDiv.AppendTo(parent);
						
			GridDiv= new  Div(default(Element), gdiv=>{
				gdiv.ClassName="span10 offset1";

				TableGastos= new HtmlTable(gdiv, table=>{
					InitTable (table);
				});	
				gdiv.Hide();
			});

			GridDiv.AppendTo(parent);	
		}
		
		void LoadConceptos(SelectElement cbox)
		{
			ListConceptos = new List<Concepto>();
			jQuery.GetData<BLResponse<Concepto>>("api/Concepto/read", new {Tipo="Egreso",SoloDetalles=true}, cb=>{},"json")
				.Success(data=>{
					ListConceptos = data.Result;
					foreach(var d in data.Result )
					{
						new HtmlOption(cbox, option=>{
							option.Value= d.Id.ToString();
							option.Text = d.Nombre;
						});
					}
					
				})
					.Error((request,  textStatus,  error)=>{
						Cayita.Javascript.Firebug.Console.Log("error", request, textStatus, error);
					})
					.Always(a=>{
					});
		}
		
		void LoadFuentes(SelectElement cbox)
		{
			ListFuentes = new List<Fuente>();
			jQuery.GetData<BLResponse<Fuente>>("api/Fuente/read", new {SoloDetalles=true}, cb=>{},"json")
				.Success(data=>{
					ListFuentes= data.Result;
					foreach(var d in data.Result )
					{
						new HtmlOption(cbox, option=>{
							option.Value= d.Id.ToString();
							option.Text = d.Nombre;
						});
					}
					
				})
					.Error((request,  textStatus,  error)=>{ 
						Cayita.Javascript.Firebug.Console.Log("error", request, textStatus, error);
					})
					.Always(a=>{
					});			
		}


		void LoadGastos (string date)
		{
			var table = TableGastos.Element();
			InitListGastos();
			jQuery.GetData<BLResponse<Gasto>>("api/Gasto/read", new {FechaDesde=date}, cb=>{},"json")
			.Success(data=>{
				ListGastos= data.Result;
				table.Load(ListGastos, Columns);
				table.JSelectRows ().AddClass ("rowlink");
			})
				.Error((request,  textStatus,  error)=>{
					table.Load(ListGastos, Columns);
					Cayita.Javascript.Firebug.Console.Log("error", request, textStatus, error);
					Div.CreateAlertErrorBefore(table, textStatus +": " + request.StatusText);
				})
				.Always(a=>{
					FormDiv.Hide();
					GridDiv.FadeIn();
				});
		}


		void RemoveRow ()
		{
			jQuery.PostRequest<BLResponse<Concepto>>("api/Gasto/destroy", new {Id=SelectedGasto.Index}, cb=>{},"json")
				.Success(d=>{
					Cayita.Javascript.Firebug.Console.Log("Success Remove gasto",d);
					jQuery.FromElement(SelectedGasto.Row).Remove();
					var gasto= ListGastos.First(f=>f.Id== int.Parse( SelectedGasto.Index ));
					ListGastos.Remove(gasto);
					SelectedGasto.Index="";
					SelectedGasto.Row= default(TableRowElement);
					BDelete.Element().Disabled=true;
					BList.Element().Disabled=true;
					FormDiv.Hide();
					GridDiv.FadeIn();
				})
					.Error((request,  textStatus,  error)=>{
						Cayita.Javascript.Firebug.Console.Log("request", request );
						Div.CreateAlertErrorBefore(Form.Element().Elements[0],
						                           textStatus+": "+ request.StatusText);
					})
					.Always(a=>{
						
					});
		}

		void AppendRow (Gasto data)
		{
			var table = TableGastos.Element();
			ListGastos.Add(data);
			table.CreateRow(data, Columns);
			
			table.JSelectRows().RemoveClass("info");
			var row = (TableRowElement) table.JSelectRows().Last().AddClass("rowlink info").GetElement(0);
			SelectedGasto.Index= row.GetIndex();
			SelectedGasto.Row= row;

			BDelete.Element().Disabled=true;
			BList.Element().Disabled=false;
		}


		void UpdateRow (Gasto data)
		{
			var gasto= ListGastos.First(f=>f.Id== data.Id );
			gasto.PopulateFrom(data);
			TableGastos.Element().UpdateRow(gasto, Columns);
			BList.Element().Disabled=true;
			FormDiv.Hide();
			GridDiv.FadeIn();

		}

		void InitTable (TableElement table)
		{
			InitListGastos ();
			DefineTableColumns();

			table.ClassName = "table table-striped table-hover table-condensed";
			table.SetAttribute ("data-provides", "rowlink");
			table.CreateHeader (Columns);

			table.JSelectRows ().Live ("click", e =>  {
				Cayita.Javascript.Firebug.Console.Log ("event click row e", e);
				var row = (TableRowElement)e.CurrentTarget;
				table.JSelectRows ().RemoveClass ("info");
				row.JSelect ().AddClass ("info");
				SelectedGasto.Index = row.GetIndex ();
				SelectedGasto.Row = row;
				var gasto = ListGastos.FirstOrDefault (f => f.Id == int.Parse (SelectedGasto.Index));
				BDelete.Element ().Disabled = false;
				BList.Element ().Disabled = false;
				Form.Element ().Reset ();
				Form.Element ().Load (gasto);
				GridDiv.Hide ();
				FormDiv.Element ().FadeIn ();

			});
		}

		void DefineTableColumns()
		{
			Columns= new List<TableColumn<Gasto>>();
			
			Columns.Add( new TableColumn<Gasto>{
				Header=  new TableCell(cell=>{
					new Anchor(cell, a=>{a.InnerText="Concepto";});
				}).Element(),
				Value = f=> {
					return new TableCell( cell=>{
						new Anchor(cell, a=>{a.InnerText=f.GetConcepto(ListConceptos).Nombre;});
					} ).Element(); }
			});
			
			Columns.Add( new TableColumn<Gasto>{
				Header=  new TableCell(cell=>{ cell.InnerText="Fuente"; }).Element(),
				Value = f=> { return new TableCell( cell=>{cell.InnerText=f.GetFuente(ListFuentes).Nombre; } ).Element(); }
			});
			
			Columns.Add( new TableColumn<Gasto>{
				Header=  new TableCell(cell=>{ cell.InnerText="Valor"; cell.Style.TextAlign= "right"; }).Element(),
				Value = f=> { return new TableCell( cell=>{cell.InnerText=f.Valor.ToString(); cell.Style.TextAlign= "right"; cell.AutoNumericInit(); } ).Element(); }
			});
			
			Columns.Add( new TableColumn<Gasto>{
				Header=  new TableCell(cell=>{cell.InnerText="Pagado a";}).Element(),
				Value = f=> { return new TableCell( cell=>{	cell.InnerText=f.Beneficiario;} ).Element();					
				}
			});
			
			Columns.Add( new TableColumn<Gasto>{
				Header=  new TableCell(cell=>{cell.InnerText="Detalle";}).Element(),
				Value = f=> { return new TableCell( cell=>{	cell.InnerText=f.Descripcion;} ).Element();					
				}
			});
			
		}


		void InitListGastos()
		{
			ListGastos = new List<Gasto>();
			SelectedGasto= new SelectedRow();
		}
	}

}


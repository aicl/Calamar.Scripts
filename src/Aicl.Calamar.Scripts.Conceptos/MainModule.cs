using System;
using System.Linq;
using System.Html;
using System.Runtime.CompilerServices;
using Cayita.Javascript.UI;
using Cayita.Javascript;
using jQueryApi;
using Cayita.Javascript.Plugins;
using Aicl.Calamar.Scripts.Modelos;
using System.Collections.Generic;

namespace Aicl.Calamar.Scripts.Conceptos
{

	[IgnoreNamespace]
	public class MainModule
	{
		public MainModule (){}
		Div SearchDiv {get;set;}
		Div FormDiv {get;set;}
		Div GridDiv {get;set;}
		Form Form {get;set;}
		HtmlTable TableConceptos {get;set;}
		List<Concepto> ListConceptos {get;set;}
		SelectedRow SelectedConcepto {get;set;}
		List<TableColumn<Concepto>> Columns {get;set;}

		Button BNew {get;set;}
		Button BDelete {get;set;}
		Button BList {get;set;}
				
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
				
				BNew = new IconButton(searchdiv, (abn, ibn)=>{
					ibn.ClassName="icon-plus-sign icon-large";
					abn.JSelect().Click(evt=>{
						GridDiv.FadeOut();
						FormDiv.FadeIn();
						Form.Element().Reset();
						BDelete.Element().Disabled=true;
						BList.Element().Disabled=false;
					});
				});

				BDelete= new IconButton(searchdiv, (abn, ibn)=>{
					ibn.ClassName="icon-remove-sign icon-large";
					abn.Disabled=true;
					abn.JSelect().Click(evt=>{
						RemoveRow();
					});
				});

				BList= new IconButton(searchdiv, (abn, ibn)=>{
					ibn.ClassName="icon-reorder icon-large";
					abn.JSelect().Click(evt=>{
						FormDiv.FadeOut();
						GridDiv.FadeIn();
						BList.Element().Disabled=true;
					});
				});
				
			});
			SearchDiv.AppendTo(parent);

			GridDiv= new  Div(default(Element), gdiv=>{
				gdiv.ClassName="span6 offset3";
				TableConceptos= new HtmlTable(gdiv, table=>{
					InitTable(table);
					LoadConceptos(table);
				});
			});
			
			GridDiv.AppendTo(parent);

			FormDiv= new Div(default(Element), formdiv=>{
				formdiv.ClassName="span6 offset3 well";
				formdiv.Hide();
				Form = new Form(formdiv, f=>{
					f.ClassName="form-horizontal";
					f.Action="api/Concepto/";

					var inputId= new InputText(f, e=>{
						e.Name="Id";
						e.Hide();
					}); 

					var cbTipo= new SelectField(f, (e)=>{
						e.Name="Tipo";
						e.ClassName="span12";
						new HtmlOption(e, o=>{
							o.Value="Egreso";
							o.Selected=true;
							o.Text="Egreso";
						});											

						new HtmlOption(e, o=>{
							o.Value="Ingreso";
							o.Text="Ingreso";
						});											

					});

					var fieldCodigo=new TextField(f,(field)=>{
						field.ClassName="span12";
						field.Name="Codigo";
						field.SetPlaceHolder("Codigo del Concepto ## Grupo ##.## Item");
					});

					var fieldNombre=new TextField(f,(field)=>{
						field.ClassName="span12";
						field.Name="Nombre";
						field.SetPlaceHolder("Nombre del Concepto");
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
							jQuery.PostRequest<BLResponse<Concepto>>(action, form.Serialize(), cb=>{},"json")
								.Success(d=>{
									Cayita.Javascript.Firebug.Console.Log("Success guardar concepto",d);
									if(string.IsNullOrEmpty(inputId.Value()) )
										AppendRow(d.Result[0]);
									else
										UpdateRow(d.Result[0]);
									FormDiv.FadeOut();
									GridDiv.Show ();
								})
									.Error((request,  textStatus,  error)=>{
										Cayita.Javascript.Firebug.Console.Log("request", request );
										Div.CreateAlertErrorBefore(form.Elements[0],
										                           textStatus+": "+ request.StatusText);
									})
									.Always(a=>{
										bt.ResetLoadingText();
									});
							
						})	
							.AddRule((rule, msg)=>{
								rule.Element=cbTipo.Element();
								rule.Rule.Required();
								msg.Required("Seleccione tipo del Concepto");
							})
							.AddRule((rule,msg)=>{
								rule.Element=fieldNombre.Element();
								rule.Rule.Required().Maxlength(64);
								msg.Required("Indique el nombre del Concepto").Maxlength("Maximo 64 Caracteres");
							}).AddRule((rule,msg)=>{
								rule.Element=fieldCodigo.Element();
								rule.Rule.Required().Maxlength(5);
								msg.Required("Indique el codigo del Concepto").Maxlength("Maximo 5 caracteres");
							});
					
					f.Validate(vo);			
					
				});
				
			});
			FormDiv.AppendTo(parent);
			
		}
		
		void LoadConceptos(TableElement table)
		{
			InitListConceptos();
			jQuery.GetData<BLResponse<Concepto>>("api/Concepto/read", new {}, cb=>{},"json")
				.Success(data=>{
					ListConceptos= data.Result;
					table.Load(ListConceptos, Columns);
					table.JSelectRows ().AddClass ("rowlink");
				})
					.Error((request,  textStatus,  error)=>{
						table.Load(ListConceptos, Columns);
						Cayita.Javascript.Firebug.Console.Log("error", request, textStatus, error);
						Div.CreateAlertErrorBefore(table, textStatus +": " + request.StatusText);
					})
					.Always(a=>{});	
		}


		void RemoveRow ()
		{
			jQuery.PostRequest<BLResponse<Concepto>>("api/Concepto/destroy", new {Id=SelectedConcepto.Index}, cb=>{},"json")
				.Success(d=>{
					Cayita.Javascript.Firebug.Console.Log("Success Remove concepto",d);
					jQuery.FromElement(SelectedConcepto.Row).Remove();
					var concepto= ListConceptos.First(f=>f.Id== int.Parse( SelectedConcepto.Index ));
					ListConceptos.Remove(concepto);
					SelectedConcepto.Index="";
					SelectedConcepto.Row= default(TableRowElement);
					BDelete.Element().Disabled=true;
					BList.Element().Disabled=true;
					FormDiv.FadeOut();
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

		void AppendRow (Concepto data)
		{
			var table = TableConceptos.Element();
			ListConceptos.Add(data);
			table.CreateRow(data, Columns);
			table.JSelectRows().RemoveClass("info");
			var row = (TableRowElement) table.JSelectRows().Last().AddClass("rowlink info").GetElement(0);
			SelectedConcepto.Index= row.GetIndex();
			SelectedConcepto.Row= row;

			BDelete.Element().Disabled=false;
			BList.Element().Disabled=true;
		}

		void UpdateRow (Concepto data)
		{
			var concepto= ListConceptos.First(f=>f.Id== data.Id );
			concepto.PopulateFrom(data);
			TableConceptos.Element().UpdateRow(concepto, Columns);
		}

		void InitTable (TableElement table)
		{
			InitListConceptos();
			DefineTableColumns();

			table.ClassName = "table table-striped table-hover table-condensed";
			table.SetAttribute ("data-provides", "rowlink");
			table.CreateHeader (Columns);

			table.JSelectRows()
				.Live("click", e=>{
					Cayita.Javascript.Firebug.Console.Log("event click row e", e);
					
					var row =(TableRowElement)e.CurrentTarget;
					
					table.JSelectRows().RemoveClass("info");
					row.JSelect().AddClass("info");
					SelectedConcepto.Index= row.GetIndex();
					SelectedConcepto.Row= row;
					var concepto= ListConceptos.FirstOrDefault(f=>f.Id== int.Parse( SelectedConcepto.Index ));
					BDelete.Element().Disabled=false;
					BList.Element().Disabled=false;
					Form.Element().Reset();
					Form.Element().Load(concepto);
					GridDiv.Hide ();
					FormDiv.FadeIn();
				});
		}

		void DefineTableColumns()
		{
			Columns= new List<TableColumn<Concepto>>();
			
			Columns.Add( new TableColumn<Concepto>{
				Header=  new TableCell(cell=>{
					new Anchor(cell, a=>{a.InnerText="Codigo";});
				}).Element(),
				Value = f=> {
					return new TableCell( cell=>{
						new Anchor(cell, a=>{a.InnerText=f.Codigo;});
					} ).Element(); }
			});
			
			Columns.Add( new TableColumn<Concepto>{
				Header=  new TableCell(cell=>{ cell.InnerText="Nombre"; }).Element(),
				Value = f=> { return new TableCell( cell=>{cell.InnerText=f.Nombre; } ).Element(); }
			});
			
			
			Columns.Add( new TableColumn<Concepto>{
				Header=  new TableCell(cell=>{ cell.InnerText="Tipo"; }).Element(),
				Value = f=> { return new TableCell( cell=>{cell.InnerText=f.Tipo; } ).Element(); }
			});
			
			
			Columns.Add( new TableColumn<Concepto>{
				Header=  new TableCell(cell=>{
					cell.InnerText="Acumulado";
					cell.Style.TextAlign= "right";
				}).Element(),
				Value = f=> { return new TableCell( cell=>{
						cell.InnerText=f.Acumulado.ToString();
						cell.Style.TextAlign= "right";
						cell.AutoNumericInit(new {vMin= -999999999.99});
					} ).Element();					
				}
			});
		}

		void InitListConceptos()
		{
			ListConceptos = new List<Concepto>();
			SelectedConcepto= new SelectedRow();
		}

	}

}

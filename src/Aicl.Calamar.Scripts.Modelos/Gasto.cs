using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Model")]
	[PreserveMemberCase]
	public class Gasto
	{
		public Gasto ()
		{
		}

		public int Id {get;set;}

		public DateTime Fecha {get;set;}

		public int IdConcepto {get;set;}

		public int IdFuente {get;set;}

		public decimal Valor {get;set;}

		public decimal Pagado {get;set;}

		public string  Descripcion {get;set;}

		public string  Beneficiario {get;set;}

		public bool Sistema {get;set;}

		public Concepto GetConcepto(List<Concepto> conceptos){
			if(conceptos ==null ) return new Concepto();
			var cp = conceptos.FirstOrDefault(f=>f.Id== IdConcepto);
			return cp??  new Concepto();
		}

		public Fuente GetFuente(List<Fuente> fuente){
			if(fuente ==null ) return new Fuente();
			var cp = fuente.FirstOrDefault(f=>f.Id== IdFuente);
			return cp?? new Fuente();
		}

	}
}

using System;
using System.Runtime.CompilerServices;

namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Model")]
	[PreserveMemberCase]
	public class Ingreso
	{
		public Ingreso (){}

		public int Id {get;set;}
		
		public DateTime Fecha {get;set;}
		
		public int IdConcepto {get;set;}
		
		public int IdFuente {get;set;}
		

		public decimal Valor {get;set;}

		public string  Descripcion {get;set;}

	}
}


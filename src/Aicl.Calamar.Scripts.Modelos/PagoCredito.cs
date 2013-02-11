using System;
using System.Runtime.CompilerServices;

namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Model")]
	[PreserveMemberCase]
	public class PagoCredito
	{
		public PagoCredito (){}

		public int Id {get;set;}
		
		public DateTime Fecha {get;set;}

		public int IdFuenteOrigen {get;set;}
		public int IdFuenteDestino {get;set;}
		/// <summary>
		/// Gets or sets the identifier concepto. Gasto al cual se le cargan los intereses
		/// </summary>
		/// <value>
		/// The identifier concepto.
		/// </value>
		public int IdConcepto {get;set;}
		

		public decimal Capital {get;set;}


		public decimal Intereses {get;set;}

		public string  Descripcion {get;set;}


	}
}


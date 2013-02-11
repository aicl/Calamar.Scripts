using System;
using System.Runtime.CompilerServices;
namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Model")]
	[PreserveMemberCase]
	public partial class Concepto
	{
		public Concepto ()
		{
		}


		public int Id{get;set;}

		/// <summary>
		/// Gets or sets the codigo.
		/// 11 Grupo
		/// 11.01 Item  
		/// </summary>
		/// <value>
		/// The codigo.
		/// </value>

		public string Codigo{get;set;}



		//public string Orden{get;set;}

		/// <summary>
		/// Gets or sets the tipo: Ingreso-Egreso.
		/// </summary>
		/// <value>
		/// The tipo.
		/// </value>

		public string Tipo {get;set;}


		public string Nombre {get;set;}

		public decimal Acumulado {get;set;}

	}
}


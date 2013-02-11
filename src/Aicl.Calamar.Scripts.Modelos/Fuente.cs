using System;
using System.Runtime.CompilerServices;

namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Model")]
	[PreserveMemberCase]
	public class Fuente
	{
		public Fuente (){}

		public int Id {get;set;}

		public string Codigo{get;set;}

		/// <summary>
		/// Gets or sets the tipo: Debito - Credito
		/// </summary>
		/// <value>
		/// The tipo.
		/// </value>
		/// 
		public string  Tipo {get;set;}


		public string  Nombre {get;set;}

		/// <summary>
		/// Gets or sets the identifier concepto.
		/// asignar valor  para tipo="Credito", para saber a que cuenta se le cargan intereses..
		/// </summary>
		/// <value>
		/// The identifier concepto.
		/// </value>

		public int? IdConcepto{get;set;}

		/// <summary>
		/// Gets or sets the saldo inicial. 
		/// Para Credito es es Cupo de la cuenta
		/// </summary>
		/// <value>
		/// The saldo inicial.
		/// </value>

		public decimal SaldoInicial {get;set;}


		public decimal Entradas {get;set;}


		public decimal Salidas {get;set;}

		/// <summary>
		/// Registros del Sistema : Creditos concedidos por terceros NO BAncos!
		/// </summary>
		/// <value>
		/// <c>true</c> if sistema; otherwise, <c>false</c>.
		/// </value>

		public bool Sistema {get;set;}

		public decimal GetSaldo() 
		{
			return SaldoInicial+Entradas-Salidas;
		}


	}
}


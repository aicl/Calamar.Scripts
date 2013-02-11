using System;
using System.Runtime.CompilerServices;

namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Model")]
	[PreserveMemberCase]
	public class Traslado
	{
		public Traslado ()
		{
		}

		public int Id {get;set;}

		public DateTime Fecha {get;set;}

		public int IdFuenteOrigen {get;set;}
		public int IdFuenteDestino {get;set;}

		public decimal Valor {get;set;}

		public string  Descripcion {get;set;}

	}
}


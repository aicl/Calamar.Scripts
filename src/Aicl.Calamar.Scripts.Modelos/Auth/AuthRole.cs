using System.Runtime.CompilerServices;
using System;

namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Auht")]
	[PreserveMemberCase]
	public  class AuthRole
	{
		
		public AuthRole(){}
		

		public int Id { get; set;} 
		

		public string Name { get; set;} 
		

		public string Directory { get; set;} 
		

		public string ShowOrder { get; set;} 
		
		public string Title { get; set;} 
		
	}
}


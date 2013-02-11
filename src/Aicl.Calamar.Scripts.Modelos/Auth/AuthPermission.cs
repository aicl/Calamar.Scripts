using System;
using System.Runtime.CompilerServices;
namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Auht")]
	[PreserveMemberCase]
	public partial class AuthPermission
	{
		
		public AuthPermission(){}
		public int Id { get; set;} 
		public string Name { get; set;} 
		
	}
}
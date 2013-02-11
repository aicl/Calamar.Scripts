using System;
using System.Runtime.CompilerServices;
namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Auht")]
	[PreserveMemberCase]
	public class RolePermission
	{
		public RolePermission ()
		{
		}
		
		public int Id { get; set;} 
		
		public int AuthRoleId { get; set;} 
		
		public int AuthPermissionId { get; set;} 
		
	
		public string Name {get;set;}
		
	}
}


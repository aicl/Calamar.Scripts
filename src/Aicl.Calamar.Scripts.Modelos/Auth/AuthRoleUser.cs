using System;
using System.Runtime.CompilerServices;
namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Auht")]
	[PreserveMemberCase]
	public  class AuthRoleUser
	{
		
		public AuthRoleUser(){}
		
		public int Id { get; set;} 
		
		public int AuthRoleId { get; set;} 
		
		public int UserId { get; set;} 
		
	}

}


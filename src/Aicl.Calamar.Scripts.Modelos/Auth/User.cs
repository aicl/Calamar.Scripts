/*
namespace Aicl.Calamar.Scripts.Modelos
{
	[Serializable]	
	[ScriptNamespace("Calamar.Auht")]
	[PreserveMemberCase]
	public class User
	{
		UserMeta metadata;
		
		public User ()
		{
			Meta= new Dictionary<string, string>();
		}
		
		public int Id { get; set; }
		public string UserName { get; set; } 
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public string Email { get; set; }
		
		public Dictionary<string, string> Meta { private get; set; }
		

		//public string Info {
		//	get{
		//		return Metadata.Info;
		//	} 
		//	set{
		//		Metadata.Info=value;
		//	} 
		//}  
		

		//public bool IsActive {get{
		//		return Metadata.IsActive;
		//	} 
		//	set{
		//		Metadata.IsActive=value;
		//	}
		//}
		

		//public DateTime? ExpiresAt {get{
		//		return Metadata.ExpiresAt;
		//	} 
		//	set{
		//		Metadata.ExpiresAt=value;
		//	}}
		
		
		//UserMeta Metadata 
		//{
		//	get{
		//		if(metadata!=default(UserMeta)) return metadata;
		//		if (Meta==null) Meta= new Dictionary<string, string>();
		//		metadata =new UserMeta();
		//		metadata.PopulateFromMeta(Meta);
		//		return metadata;
		//	}

		//}
		
		
	}

	[Serializable]	
	[ScriptNamespace("Calamar.Auht")]
	[PreserveMemberCase]
	public class UserMeta
	{
		public UserMeta()
		{
			IsActive=true;
		}
		public string Info {get;set;}
		public bool IsActive {get;set;}
		public DateTime? ExpiresAt {get;set;}
		
		//public void PopulateFromMeta(Dictionary<string, string> meta){
		//	if(meta==null) return;
		//	string str = null;
		//	meta.TryGetValue(typeof(UserMeta).Name, out str);
		//	if(str != null) 
		//	{
		//		var t = TypeSerializer.DeserializeFromstring<UserMeta>(str);
		//		IsActive=t.IsActive;
		//		Info=t.Info;
		//		ExpiresAt=t.ExpiresAt;
		//	}
		//}
	}

}

*/
using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("objects")]
	public class ES3UserType_SaveData : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SaveData() : base(typeof(SaveData)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (SaveData)obj;
			
			writer.WriteProperty("objects", instance.objects, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<SaveObjectData>)));
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (SaveData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "objects":
						instance.objects = reader.Read<System.Collections.Generic.List<SaveObjectData>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SaveDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SaveDataArray() : base(typeof(SaveData[]), ES3UserType_SaveData.Instance)
		{
			Instance = this;
		}
	}
}
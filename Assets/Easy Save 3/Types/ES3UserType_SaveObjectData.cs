using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("id", "deployType", "slideObjectDatas", "animations", "objectPath", "imagePath", "text")]
	public class ES3UserType_SaveObjectData : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_SaveObjectData() : base(typeof(SaveObjectData)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (SaveObjectData)obj;
			
			writer.WriteProperty("id", instance.id, ES3Type_uint.Instance);
			writer.WriteProperty("deployType", instance.deployType, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(DeployType)));
			writer.WriteProperty("slideObjectDatas", instance.slideObjectDatas, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<SlideObjectData>)));
			writer.WriteProperty("animations", instance.animations, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<XRAnimation>)));
			writer.WriteProperty("objectPath", instance.objectPath, ES3Type_string.Instance);
			writer.WriteProperty("imagePath", instance.imagePath, ES3Type_string.Instance);
			writer.WriteProperty("text", instance.text, ES3Type_string.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new SaveObjectData();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "id":
						instance.id = reader.Read<System.UInt32>(ES3Type_uint.Instance);
						break;
					case "deployType":
						instance.deployType = reader.Read<DeployType>();
						break;
					case "slideObjectDatas":
						instance.slideObjectDatas = reader.Read<System.Collections.Generic.List<SlideObjectData>>();
						break;
					case "animations":
						instance.animations = reader.Read<System.Collections.Generic.List<XRAnimation>>();
						break;
					case "objectPath":
						instance.objectPath = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "imagePath":
						instance.imagePath = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "text":
						instance.text = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_SaveObjectDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SaveObjectDataArray() : base(typeof(SaveObjectData[]), ES3UserType_SaveObjectData.Instance)
		{
			Instance = this;
		}
	}
}
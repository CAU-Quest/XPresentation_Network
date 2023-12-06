using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("position", "rotation", "scale", "color", "isGrabbable", "isVisible", "isVideo")]
	public class ES3UserType_SlideObjectData : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_SlideObjectData() : base(typeof(SlideObjectData)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (SlideObjectData)obj;
			
			writer.WriteProperty("position", instance.position, ES3Type_Vector3.Instance);
			writer.WriteProperty("rotation", instance.rotation, ES3Type_Quaternion.Instance);
			writer.WriteProperty("scale", instance.scale, ES3Type_Vector3.Instance);
			writer.WriteProperty("color", instance.color, ES3Type_Color.Instance);
			writer.WriteProperty("isGrabbable", instance.isGrabbable, ES3Type_bool.Instance);
			writer.WriteProperty("isVisible", instance.isVisible, ES3Type_bool.Instance);
			writer.WriteProperty("isVideo", instance.isVideo, ES3Type_bool.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new SlideObjectData();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "position":
						instance.position = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "rotation":
						instance.rotation = reader.Read<UnityEngine.Quaternion>(ES3Type_Quaternion.Instance);
						break;
					case "scale":
						instance.scale = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "color":
						instance.color = reader.Read<UnityEngine.Color>(ES3Type_Color.Instance);
						break;
					case "isGrabbable":
						instance.isGrabbable = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isVisible":
						instance.isVisible = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isVideo":
						instance.isVideo = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_SlideObjectDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SlideObjectDataArray() : base(typeof(SlideObjectData[]), ES3UserType_SlideObjectData.Instance)
		{
			Instance = this;
		}
	}
}
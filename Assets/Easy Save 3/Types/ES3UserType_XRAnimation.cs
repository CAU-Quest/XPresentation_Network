using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("presentationObject", "previousData", "nextData", "ease")]
	public class ES3UserType_XRAnimation : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_XRAnimation() : base(typeof(XRAnimation)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (XRAnimation)obj;
			
			writer.WritePropertyByRef("presentationObject", instance.presentationObject);
			writer.WriteProperty("previousData", instance.previousData, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(SlideObjectData)));
			writer.WriteProperty("nextData", instance.nextData, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(SlideObjectData)));
			writer.WriteProperty("ease", instance.ease, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(DG.Tweening.Ease)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (XRAnimation)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "presentationObject":
						instance.presentationObject = reader.Read<PresentationObject>();
						break;
					case "previousData":
						instance.previousData = reader.Read<SlideObjectData>();
						break;
					case "nextData":
						instance.nextData = reader.Read<SlideObjectData>();
						break;
					case "ease":
						instance.ease = reader.Read<DG.Tweening.Ease>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new XRAnimation();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_XRAnimationArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_XRAnimationArray() : base(typeof(XRAnimation[]), ES3UserType_XRAnimation.Instance)
		{
			Instance = this;
		}
	}
}
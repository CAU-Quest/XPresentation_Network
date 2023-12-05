using System.Collections;
using System.Collections.Generic;
//using Dummiesman;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.UI;

public struct SaveObjectData
{
    public uint id;
    public DeployType deployType;
    public List<SlideObjectData> slideObjectDatas;
    public List<XRAnimation> animations;
    public string objectPath;
    public string imagePath;
}

public enum DeployType
{
    Sphere = 1, 
    Cube = 2, 
    Cylinder = 3, 
    Plane = 4, 
    Text = 5, 
    ImportImage = 6, 
    ImportModel = 7
}

public class SaveData : MonoBehaviour
{
    public static SaveData Instance = null;

    public List<SaveObjectData> objects = new List<SaveObjectData>();

    public Transform parent;

    void Start()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    /*
    public void SaveGameData()
    {
        MainSystem.Instance.NotifyObserverSaveData();
        ES3.Save("ObjectData", objects);
    }

    public void LoadGameData()
    {
        objects = ES3.Load<List<SaveObjectData>>("ObjectData");
        for (int i = 0; i < objects.Count; i++)
        {
            SaveObjectData data = objects[i];
            GameObject go = PresentationObjectPool.Instance.Get((int)data.deployType - 1, Vector3.zero, parent);
            if (data.deployType == DeployType.ImportImage)
            {
                string imagePath = data.imagePath.Replace("#", "\\");
                go.GetComponentInChildren<RawImage>().texture = LoadTexture(imagePath);
            } else if (data.deployType == DeployType.ImportModel)
            {
                GameObject element = go.GetComponentInChildren<Grabbable>().gameObject;

                string objectPath = data.objectPath.Replace("#", "\\");
                GameObject model = new OBJLoader().Load(objectPath);
                model.transform.SetParent(element.transform);
                
                string imagePath = data.imagePath.Replace("#", "\\");
                model.GetComponentInChildren<MeshRenderer>().material.mainTexture = LoadTexture(imagePath);
                element.AddComponent<PresentationObject>();
            }
            PresentationObject presentationObject = go.GetComponentInChildren<PresentationObject>();
            presentationObject.animationList = new List<XRAnimation>();

            presentationObject.slideData = new List<SlideObjectData>();
            for (int j = 0; j < data.slideObjectDatas.Count; j++)
            {
                presentationObject.slideData.Add(data.slideObjectDatas[j]);
            }
            for (int j = 0; j < data.animations.Count; j++)
            {
                data.animations[j].presentationObject = presentationObject;
                presentationObject.animationList.Add(data.animations[j]);
            }
        }
        
        Texture2D LoadTexture(string path)
        {
            byte[] fileData = System.IO.File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(2, 2); // 텍스쳐의 가로 세로 크기를 지정합니다. 실제 크기에 맞게 수정하세요.
            bool success = texture.LoadImage(fileData); // 바이트 배열을 텍스쳐에 로드합니다.

            if (success)
            {
                // 텍스쳐 로드 성공 시 반환
                return texture;
            }
            else
            {
                // 텍스쳐 로드 실패 시 null 반환
                return null;
            }
        }
    }
        */
}

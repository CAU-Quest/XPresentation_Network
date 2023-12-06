using System.Collections;
using System.Collections.Generic;
using Dummiesman;
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

    public GameObject[] prefabs;
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
    public void LoadGameData()
    {
        objects = ES3.Load<List<SaveObjectData>>("ObjectData");
        for (int i = 0; i < objects.Count; i++)
        {
            SaveObjectData data = objects[i];
            GameObject go = Instantiate(prefabs[(int)data.deployType - 1], parent);
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

            Texture2D texture = new Texture2D(2, 2);
            bool success = texture.LoadImage(fileData);

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
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public struct SerializeableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializeableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public struct SerializeableQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;
    public SerializeableQuaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
}
public struct SerializeableColor
{
    public float r;
    public float g;
    public float b;
    public float a;
    public SerializeableColor(float r, float g, float b, float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
}

public struct SerializeableSlideObjectData
{
    public SerializeableVector3 position;
    public SerializeableQuaternion rotation;
    public SerializeableVector3 scale;
    public SerializeableColor color;
    public bool isGrabbable;
    public bool isVisible;
    public bool isVideo;
}

public struct SerializableXRAnimation
{
    public SerializeableSlideObjectData previousData;
    public SerializeableSlideObjectData nextData;
    public Ease ease;
}
/*
public struct SlideObjectData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Color color;
    public bool isGrabbable;
    public bool isVisible;
    public bool isVideo;
    
public class XRAnimation
{
    public PresentationObject presentationObject;
    private uint id;

    public SlideObjectData previousData;
    public SlideObjectData nextData;
    public Ease ease;
    
    
*/

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nicknameField;
    public TMP_InputField roomNameField;

    public SaveData saveData;
    
    public TextMeshProUGUI text;

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = nicknameField.text;
        Debug.Log("서버 접속 완료.");
        PhotonNetwork.JoinLobby();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버 연결 끊김.");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장.");
    }

    public string GenerateRoomName()
    {
        string roomName = "";
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        for (int i = 0; i < 12; i++)
        {
            int randomIndex = Random.Range(0, characters.Length); // 문자열 내에서 무작위 인덱스 선택
            roomName += characters[randomIndex]; // 선택된 문자를 방 이름에 추가
        }

        return roomName;
    }
    
    public void CreateRoom()
    {
        string roomName = GenerateRoomName();

        RoomOptions roomOptions = new RoomOptions();

        Hashtable hashtable = new Hashtable();
        List<SaveObjectData> saveObjectDatas = saveData.LoadData();
        int slideCount = saveData.LoadSlideCount();
        List<VideoData> videoDatas = saveData.LoadVideoDatas();
        
        hashtable.Add("SlideCount", slideCount);
        hashtable.Add("VideoData",JsonConvert.SerializeObject(videoDatas));

        int count = 0;
        foreach(SaveObjectData data in saveObjectDatas)
        {
            hashtable.Add( count.ToString() + "ObjectID", (int)data.id);
            
            
            string slideData = JsonConvert.SerializeObject(SerializeSlideObjectDatas(data.slideObjectDatas));
            string animations = JsonConvert.SerializeObject(SerializeAnimationList(data.animations));
            
            hashtable.Add( count.ToString() + "ObjectSlideObjectDatas", slideData);
            hashtable.Add( count.ToString() + "ObjectAnimations", animations);
            hashtable.Add( count.ToString() + "ObjectDeployType", data.deployType);
            if (data.deployType == DeployType.ImportImage)
            {
                hashtable.Add( count.ToString() + "ObjectImagePath", data.imagePath);
            } else if (data.deployType == DeployType.ImportModel)
            {
                hashtable.Add( count.ToString() + "ObjectObjectPath", data.objectPath);
                hashtable.Add( count.ToString() + "ObjectImagePath", data.imagePath);
            } else if (data.deployType == DeployType.Text)
            {
                hashtable.Add( count.ToString() + "ObjectText", data.text);
            }
            count++;
        }

        hashtable.Add("ObjectCount", count);
        hashtable.Add("SlideNum", 0);

        roomOptions.CustomRoomProperties = hashtable;
        
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log(roomName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Hello");
        
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.Instantiate("NetworkPlayer", Vector3.one, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("NetworkPlayer", new Vector3(0f, 0f, -5f), Quaternion.identity);
        }

        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;

        MainSystem.Instance.SetCurrentSlideCount((int)hashtable["SlideCount"]);
        VideoManager.Instance.videoDataList =
            JsonConvert.DeserializeObject<List<VideoData>>((string)hashtable["VideoData"]);
        int count = (int)hashtable["ObjectCount"];

        for (int i = 0; i < count; i++)
        {
            SaveObjectData data = new SaveObjectData();
            data.id = (uint)((int)hashtable[i.ToString() + "ObjectID"]);
            data.slideObjectDatas = DeserializeSlideObjectDatas(JsonConvert.DeserializeObject<List<SerializeableSlideObjectData>>((string)hashtable[i.ToString() + "ObjectSlideObjectDatas"]));
            data.animations = DeserializeAnimationList(JsonConvert.DeserializeObject<List<SerializableXRAnimation>>((string)hashtable[i.ToString() + "ObjectAnimations"]));
            data.deployType = (DeployType)hashtable[i.ToString() + "ObjectDeployType"];
            if (data.deployType == DeployType.ImportImage)
            {
                data.imagePath = (string)hashtable[i.ToString() + "ObjectImagePath"];
            } else if (data.deployType == DeployType.ImportModel)
            {
                data.imagePath = (string)hashtable[i.ToString() + "ObjectImagePath"];
                data.objectPath = (string)hashtable[i.ToString() + "ObjectObjectPath"];
            } else if (data.deployType == DeployType.Text)
            {
                data.text = (string)hashtable[i.ToString() + "ObjectText"];
            }
            saveData.MakeDataBySlideObjectData(data);

            MainSystem.Instance.currentSlideNum = (int)hashtable["SlideNum"];
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNameField.text);
    }

    private void Update()
    {
        text.text = PhotonNetwork.LocalPlayer.NickName;
    }


    #region Serialize Field

    public List<SerializableXRAnimation> SerializeAnimationList(List<XRAnimation> datas)
    {
        List<SerializableXRAnimation> result = new List<SerializableXRAnimation>();

        foreach (XRAnimation data in datas)
        {
            result.Add(SerializeAnimation(data));
        }
        
        return result;
    }
    
    public List<XRAnimation> DeserializeAnimationList(List<SerializableXRAnimation> datas)
    {
        List<XRAnimation> result = new List<XRAnimation>();

        foreach (SerializableXRAnimation data in datas)
        {
            result.Add(DeserializeAnimation(data));
        }
        
        return result;
    }


    public SerializableXRAnimation SerializeAnimation(XRAnimation data)
    {
        SerializableXRAnimation result = new SerializableXRAnimation();

        result.previousData = SerializeSlideObjectData(data.previousData);
        result.nextData = SerializeSlideObjectData(data.nextData);
        result.ease = data.ease;
        
        return result;
    }
    
    
    public XRAnimation DeserializeAnimation(SerializableXRAnimation data)
    {
        XRAnimation result = new XRAnimation();

        result.previousData = DeserializeSlideObjectData(data.previousData);
        result.nextData = DeserializeSlideObjectData(data.nextData);
        result.ease = data.ease;
        
        return result;
    }

    
    
    
    public List<SerializeableSlideObjectData> SerializeSlideObjectDatas(List<SlideObjectData> datas)
    {
        List<SerializeableSlideObjectData> result = new List<SerializeableSlideObjectData>();

        foreach (SlideObjectData data in datas)
        {
            result.Add(SerializeSlideObjectData(data));
        }
        
        return result;
    }


    public List<SlideObjectData> DeserializeSlideObjectDatas(List<SerializeableSlideObjectData> datas)
    {
        List<SlideObjectData> result = new List<SlideObjectData>();

        foreach (SerializeableSlideObjectData data in datas)
        {
            result.Add(DeserializeSlideObjectData(data));
        }
        
        return result;
    }
    

    public SerializeableSlideObjectData SerializeSlideObjectData(SlideObjectData data)
    {
        SerializeableSlideObjectData result = new SerializeableSlideObjectData();

        result.position = new SerializeableVector3(data.position.x, data.position.y, data.position.z);
        result.rotation = new SerializeableQuaternion(data.rotation.x, data.rotation.y, data.rotation.z, data.rotation.w);
        result.scale = new SerializeableVector3(data.scale.x, data.scale.y, data.scale.z);
        result.color = new SerializeableColor(data.color.r, data.color.g, data.color.b, data.color.a);
        result.isGrabbable = data.isGrabbable;
        result.isVideo = data.isVideo;
        result.isVisible = data.isVisible;

        return result;
    }
    
    public SlideObjectData DeserializeSlideObjectData(SerializeableSlideObjectData data)
    {
        SlideObjectData result = new SlideObjectData();

        result.position = new Vector3(data.position.x, data.position.y, data.position.z);
        result.rotation = new Quaternion(data.rotation.x, data.rotation.y, data.rotation.z, data.rotation.w);
        result.scale = new Vector3(data.scale.x, data.scale.y, data.scale.z);
        result.color = new Color(data.color.r, data.color.g, data.color.b, data.color.a);
        result.isGrabbable = data.isGrabbable;
        result.isVideo = data.isVideo;
        result.isVisible = data.isVisible;

        return result;
    }


    #endregion
}

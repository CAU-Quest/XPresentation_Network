using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nicknameField;
    public TMP_InputField roomNameField;

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
        PhotonNetwork.CreateRoom(roomName, new RoomOptions());
        Debug.Log(roomName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.Instantiate("NetworkPlayer", Vector3.one, Quaternion.identity);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNameField.text);
    }

    private void Update()
    {
        text.text = PhotonNetwork.LocalPlayer.NickName;
    }
}

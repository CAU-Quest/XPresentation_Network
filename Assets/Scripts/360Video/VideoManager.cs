using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public struct VideoData
{
    public int slideNumber;
    public String url;
}

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance = null;
    
    public VideoPlayer videoPlayer;

    public GameObject sphere;

    public GameObject stage;

    public List<VideoData> videoDataList = new List<VideoData>();

    private void Start()
    {
        if (null == Instance)
        {
            Instance = this;
            if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void StartVideo(string url)
    {
        stage.SetActive(false);
        sphere.SetActive(true);
        videoPlayer.url = url;
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
    }

    public void FinishVideo()
    {
        videoPlayer.Stop();
        sphere.SetActive(false);
        stage.SetActive(true);
    }
}

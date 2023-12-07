using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MainSystem : MonoBehaviour, ISubject
{
    public static MainSystem Instance = null;

    public List<IObserver> observers = new List<IObserver>();
    
    [SerializeField]
    private int slideCount = 1;
    public int currentSlideNum;
    [SerializeField]
    public float slideInterval;
    public bool isPlayingAnimation = true;

    public PhotonView photonView;
    
    void Awake()
    {
        if (null == Instance)
        {
            Debug.Log("MainSystem Load");
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    void Update()
    {
        if (isPlayingAnimation && slideInterval < 1.0f)
        {
            if (currentSlideNum == slideCount - 1) isPlayingAnimation = false;
            else
            {
                slideInterval += Time.deltaTime;
                if (slideInterval >= 1.0f)
                {
                    slideInterval = 1.0f;
                    if (currentSlideNum + 1 < slideCount)
                    {
                        slideInterval = 0;
                        currentSlideNum += 1;
                        isPlayingAnimation = false;
                        NotifyObserversChangeSlide(currentSlideNum);
                    }
                }
            }
        }
    }
    
    public void SetCurrentSlideCount(int count)
    {
        slideCount = count;
    }

    public void GoToPreviousSlide()
    {
        photonView.RPC("GoToPreviousSlideRPC", RpcTarget.All);
    }

    public void GoToNextSlide()
    {
        photonView.RPC("GoToNextSlideRPC", RpcTarget.All);
    }
    
    public void AnimationToggle()
    {
        photonView.RPC("AnimationToggleRPC", RpcTarget.All);
    }

    [PunRPC]
    public void AnimationToggleRPC()
    {
        isPlayingAnimation = true;
    }
    [PunRPC]
    public void GoToPreviousSlideRPC()
    {
        if (currentSlideNum > 0)
        {
            currentSlideNum -= 1;
            slideInterval = 0;
            isPlayingAnimation = false;
            NotifyObserversChangeSlide(currentSlideNum);
        }   
    }

    [PunRPC]
    public void GoToNextSlideRPC()
    {
        if (currentSlideNum < slideCount - 1)
        {
            currentSlideNum += 1;
            slideInterval = 0;
            isPlayingAnimation = false;
            NotifyObserversChangeSlide(currentSlideNum);
        }
    }

    public void GoToSlideByIndex(int index)
    {
        if (index >= 0 && index < slideCount)
        {
            currentSlideNum = index;
            slideInterval = 0;
            isPlayingAnimation = false;
            NotifyObserversChangeSlide(currentSlideNum);
        }
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObserversChangeSlide(int slide)
    {
        PhotonNetwork.CurrentRoom.CustomProperties["SlideNum"] = currentSlideNum;
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].ObserverChangeSlide(currentSlideNum);
        }
    }
}

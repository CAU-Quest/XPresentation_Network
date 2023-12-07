using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Oculus.Interaction;
using Photon.Pun;
using TMPro;
//using Oculus.Interaction;
//using Oculus.Interaction.PoseDetection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public struct SlideObjectData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Color color;
    public bool isGrabbable;
    public bool isVisible;
    public bool isVideo;
    

    public SlideObjectData(Vector3 pos, Quaternion rot, Vector3 scale, Color col, bool grabbable = false,
        bool visible = false)
    {
        position = pos;
        rotation = rot;
        this.scale = scale;
        color = col;
        isGrabbable = grabbable;
        isVisible = visible;
        isVideo = false;
    }
    
    public SlideObjectData(SlideObjectData currentData, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        position = pos;
        rotation = rot;
        this.scale = scale;
        color = currentData.color;
        isGrabbable = currentData.isGrabbable;
        isVisible = currentData.isVisible;
        isVideo = false;
    }
    
    public SlideObjectData(SlideObjectData currentData, Color col)
    {
        position = currentData.position;
        rotation = currentData.rotation;
        scale = currentData.scale;
        color = col;
        isGrabbable = currentData.isGrabbable;
        isVisible = currentData.isVisible;
        isVideo = false;
    }
    
    public SlideObjectData(SlideObjectData currentData, bool grabbable)
    {
        position = currentData.position;
        rotation = currentData.rotation;
        scale = currentData.scale;
        color = currentData.color;
        isGrabbable = grabbable;
        isVisible = currentData.isVisible;
        isVideo = false;
    }
    
    public SlideObjectData(SlideObjectData currentData)
    {
        position = currentData.position;
        rotation = currentData.rotation;
        scale = currentData.scale;
        color = currentData.color;
        isGrabbable = currentData.isGrabbable;
        isVisible = currentData.isVisible;
        isVideo = false;
    }
}

public class PresentationObject : MonoBehaviourPunCallbacks, IObserver
{
    private static uint idCount = 1;
    public uint id = 1;

    public PhotonView photonView;
    
    public List<XRAnimation> animationList = new List<XRAnimation>();
    public List<SlideObjectData> slideData = new List<SlideObjectData>();
    
    private int currentSlide;
    
    private MeshRenderer meshRenderer;
    private RawImage image;
    private TMP_InputField tmpInputField;
    
    public Transform Transform => transform.parent;
    public Material Material => (meshRenderer) ? meshRenderer.material : null;
    public RawImage Image => image;
    
    public bool isGrabbableInPresentation = true;
    public bool isVisible = true;
    
    private Grabbable grabbable;
    
    
    public void SetVisible()
    {
        isVisible = true;
        if (isGrabbableInPresentation && grabbable)
        {
            grabbable.MaxGrabPoints = -1;
        }
        if (meshRenderer)
        {
            meshRenderer.enabled = true;
        }
    }

    public void SetInvisible()
    {
        isVisible = false;
        if (grabbable)
        {
            grabbable.MaxGrabPoints = 0;
        }
        if (meshRenderer)
        {
            meshRenderer.enabled = false;
        }

    }

    public void ObserverChangeSlide(int slide)
    {
        this.currentSlide = slide;

        ApplyDataToObject(slideData[slide]);
    }
    
    public void ApplyDataToObject(SlideObjectData data) //SetSlideObjectData -> ApplySlideObjectData
    {
        transform.parent.SetPositionAndRotation(data.position, data.rotation);
        transform.parent.localScale = data.scale;
        if (meshRenderer != null) meshRenderer.material.color = data.color;
        if (Image != null) Image.color = data.color;
        if (data.isVisible)
        {
            SetVisible();
        }
        else
        {
            SetInvisible();
        }
        isGrabbableInPresentation = data.isGrabbable;
    }
    
    public void Update()
    {
        if (MainSystem.Instance.isPlayingAnimation)
        { 
            animationList[currentSlide].Play();
        }
    }
    
    public uint GetID()
    {
        return id;
    }
    
    


    public void Init()
    {
        this.id = idCount++;
        
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        tmpInputField = GetComponentInChildren<TMP_InputField>();
        image = GetComponentInChildren<RawImage>();
        
        grabbable = GetComponent<Grabbable>();
        if (grabbable == null) grabbable = GetComponentInParent<Grabbable>();
        MainSystem.Instance.RegisterObserver(this);
    }

    
    /*
    private static uint idCount = 1;
    public uint id = 1;

    public List<XRAnimation> animationList = new List<XRAnimation>();
    public List<SlideObjectData> slideData = new List<SlideObjectData>();

    public DeployType deployType;

    private int currentSlide;
    private MainSystem.Mode mode;

    private Canvas canvas;

    private Material normalModeMaterial;

    private GameObject dottedLine;

    private GameObject ghostObject;
    private PresentationGhostObject ghost;

    private MeshRenderer meshRenderer;
    private RawImage image;
    private Grabbable grabbable;


    public Transform Transform => transform.parent;
    public Material Material => (meshRenderer) ? meshRenderer.material : null;
    public RawImage Image => image;
    public bool isGrabbableInPresentation = true;
    public bool isVisible = true;

    public void ObserverUpdateMode(MainSystem.Mode mode)
    {
        this.mode = mode;
        if (mode == MainSystem.Mode.Animation)
        {
            if(meshRenderer) meshRenderer.material = MainSystem.Instance.beforeSlideMaterial;
            if (currentSlide + 1 < MainSystem.Instance.GetSlideCount())
            {
                if (slideData[currentSlide + 1].isVisible)
                {
                    ghostObject.SetActive(true);
                }
            }
        }
        else
        {
            if(meshRenderer) meshRenderer.material = normalModeMaterial;
            ghostObject.SetActive(false);
        }
    }

    public int GetCurrentSlide()
    {
        return currentSlide;
    }

    public void SetNextSlideObjectDataSameAsCurrent()
    {
        if (MainSystem.Instance.currentSlideNum + 1 < MainSystem.Instance.GetSlideCount())
        {
            SlideObjectData so = slideData[MainSystem.Instance.currentSlideNum];
            animationList[MainSystem.Instance.currentSlideNum].SetNextSlideObjectData(so);
            animationList[MainSystem.Instance.currentSlideNum + 1].SetPreviousSlideObjectData(so);
            slideData[MainSystem.Instance.currentSlideNum + 1] = so;
        }
    }
    
    public void SetPreviousSlideObjectDataSameAsCurrent()
    {
        if (MainSystem.Instance.currentSlideNum - 1 > 0)
        {
            SlideObjectData so = slideData[MainSystem.Instance.currentSlideNum];
            animationList[MainSystem.Instance.currentSlideNum - 1].SetNextSlideObjectData(so);
            animationList[MainSystem.Instance.currentSlideNum].SetPreviousSlideObjectData(so);
            slideData[MainSystem.Instance.currentSlideNum - 1] = so;
        }
    }

    public void ObserverUpdateSave()
    {
        SaveObjectData saveObjectData = new SaveObjectData();
        saveObjectData.slideObjectDatas = slideData;
        saveObjectData.animations = animationList;
        saveObjectData.id = id;
        saveObjectData.deployType = deployType;
        if (deployType == DeployType.ImportModel)
        {
            saveObjectData.objectPath = GetComponentInParent<SelectObject>().objectPath;
            saveObjectData.imagePath = GetComponentInParent<SelectObject>().imagePath;
            saveObjectData.objectPath = saveObjectData.objectPath.Replace("\\", "#");
            saveObjectData.imagePath = saveObjectData.imagePath.Replace("\\", "#");
        } else if (deployType == DeployType.ImportImage)
        {
            saveObjectData.imagePath = GetComponentInParent<SelectObject>().imagePath;
            saveObjectData.imagePath = saveObjectData.imagePath.Replace("\\", "#");
        }
        Debug.Log(saveObjectData.imagePath);
        
        SaveData.Instance.objects.Add(saveObjectData);
    }

    public void ObserverUpdateSlide(int slide)
    {
        this.currentSlide = slide;

        ApplyDataToObject(slideData[slide]);
        ghost.applyTransform();
        if (mode == MainSystem.Mode.Animation)
        {
            if (currentSlide + 1 < MainSystem.Instance.GetSlideCount())
            {
                if (slideData[slide + 1].isVisible)
                {
                    ghostObject.SetActive(true);
                }
                
            }
            else
            {
                ghostObject.SetActive(false);
            }
        }
    }

    public void ObserverRemoveSlide(int index)
    {
        removeSlide(index);
    }

    public void ObserverAddSlide()
    {
        addSlide();
    }
    
    public void ObserverCreateVideo(int index)
    {
        if (index > 0 && index <= MainSystem.Instance.GetSlideCount())
        {
            SlideObjectData objectData = new SlideObjectData(slideData[index - 1]);
            objectData.isVideo = true;
            slideData.Insert(index, objectData);
            XRAnimation animation = new XRAnimation();
            animation.SetParentObject(this);
            animation.SetPreviousSlideObjectData(objectData);
            if(index < MainSystem.Instance.GetSlideCount())
                animation.SetNextSlideObjectData(slideData[index + 1]);
            animationList[index - 1].SetNextSlideObjectData(objectData);
        }
    }
    
    
    public void ObserverAddSlideNextTo(int index)
    {
        if (index > 0 && index <= MainSystem.Instance.GetSlideCount())
        {
            SlideObjectData objectData = new SlideObjectData(slideData[index - 1]);
            slideData.Insert(index, objectData);
            XRAnimation animation = new XRAnimation();
            animation.SetParentObject(this);
            animation.SetPreviousSlideObjectData(objectData);
            if(index < MainSystem.Instance.GetSlideCount())
                animation.SetNextSlideObjectData(slideData[index + 1]);
            animationList[index - 1].SetNextSlideObjectData(objectData);
        }
    }

    
    public void removeSlide(int index)
    {
        if (index > 0 && index < MainSystem.Instance.GetSlideCount())
        {
            animationList.RemoveAt(index);
            slideData.RemoveAt(index);
            ApplyDataToObject(slideData[currentSlide]);
        }
    }
    
    public void addSlide()
    {
        SlideObjectData beforeTransform = slideData[slideData.Count - 1];
        for (int i = slideData.Count; i < MainSystem.Instance.GetSlideCount(); i++)
        {
            XRAnimation anim = new XRAnimation();
            
            anim.SetParentObject(this);
            anim.SetPreviousSlideObjectData(beforeTransform);
            anim.SetNextSlideObjectData(beforeTransform);
            
            animationList.Add(anim);
            slideData.Add(beforeTransform);
        }
    }

    public void SetGrabbable(bool boolean)
    {
        isGrabbableInPresentation = boolean;
        
        if(!isGrabbableInPresentation)
            grabbable.MaxGrabPoints = -1;
        else
            grabbable.MaxGrabPoints = 0;
    }

    public void SetVisible()
    {
        isVisible = true;
        if (isGrabbableInPresentation && grabbable)
        {
            grabbable.MaxGrabPoints = -1;
        }
        if (meshRenderer)
        {
            meshRenderer.enabled = true;
        }
        if (canvas)
        {
            canvas.enabled = true;
        }
    }

    public void SetInvisible()
    {
        isVisible = false;
        if (grabbable)
        {
            grabbable.MaxGrabPoints = 0;
        }
        if (meshRenderer)
        {
            meshRenderer.enabled = false;
        }

        if (canvas)
        {
            canvas.enabled = false;
        }
    }
    

    public void Update()
    {
        if (MainSystem.Instance.isPlayingAnimation)
        {
            animationList[currentSlide].Play();
        }
    }

    public XRAnimation CreateDefaultAnimation(int index)
    {
        XRAnimation element = new XRAnimation();
        if (index + 1 < slideData.Count)
        {
            element.SetParentObject(this);
            element.SetPreviousSlideObjectData(slideData[index]);
            element.SetNextSlideObjectData(slideData[index + 1]);
        }
        
        return element;
    }
    
    public void ObserverMoveSlides(int moved, int count, int into)
    {
        Debug.Log("Move Slides(" + moved + ", " + count + ", " + into + ")");
        if (count == 1)
        {
            SlideObjectData transformElement = slideData[moved];
            slideData.RemoveAt(moved);

            slideData.Insert(into, transformElement);

            if (into < moved)
            {
                animationList.RemoveAt(moved);
                if(moved - 1 >= 0) animationList.RemoveAt(moved - 1);
                if (into - 1 >= 0)
                {
                    animationList.RemoveAt(into - 1);
                    animationList.Insert(into - 1, CreateDefaultAnimation(into - 1));
                }
                animationList.Insert(into, CreateDefaultAnimation(into));
                if(moved - 1 >= 0) animationList.Insert(moved, CreateDefaultAnimation(moved));
            } 
            else
            {
                if (into - 1 >= 0) animationList.RemoveAt(into);
                animationList.RemoveAt(moved);
                if(moved - 1 >= 0) animationList.RemoveAt(moved - 1);
                if(moved - 1 >= 0) animationList.Insert(moved - 1, CreateDefaultAnimation(moved - 1));
                if (into - 1 >= 0) animationList.Insert(into - 1, CreateDefaultAnimation(into - 1));
                animationList.Insert(into, CreateDefaultAnimation(into));   
            }
        }
        else
        {
            List<SlideObjectData> transformDatas = slideData.GetRange(moved, count);
            slideData.RemoveRange(moved, count);
            
            if (into < moved) slideData.InsertRange(into, transformDatas);
            else slideData.InsertRange(into - count + 1, transformDatas);

            List<XRAnimation> animationElements = animationList.GetRange(moved, count);
                

            if (into < moved)
            {
                animationList.RemoveRange(moved, count);
                if(moved - 1 >= 0) animationList.RemoveAt(moved - 1);
                if (into - 1 >= 0) animationList.RemoveAt(into - 1);
                if (into - 1 >= 0) animationList.Insert(into - 1, CreateDefaultAnimation(into - 1));
                animationList.InsertRange(into, animationElements);
                animationList.RemoveAt(into + count - 1);
                animationList.Insert(into + count - 1, CreateDefaultAnimation(into + count - 1));
                if(moved - 1 >= 0) animationList.Insert(moved + count - 1, CreateDefaultAnimation(moved + count - 1));
            }
            else
            {
                animationList.RemoveAt(into);
                animationList.RemoveRange(moved, count);
                if(moved - 1 >= 0) animationList.RemoveAt(moved - 1);
                if(moved - 1 >= 0) animationList.Insert(moved - 1, CreateDefaultAnimation(moved - 1));
                animationList.Insert(into - count, CreateDefaultAnimation(into - count));
                animationList.InsertRange(into - count + 1, animationElements);
                animationList.RemoveAt(into);
                animationList.Insert(into, CreateDefaultAnimation(into));
            }
        }
    }

    public void UpdateCurrentObjectDataInSlide()
    {
        var parent = transform.parent;
        Color color = Color.white;
        if (meshRenderer != null)
            color = meshRenderer.material.color;
        if (Image != null)
            color = Image.color;
        var data = new SlideObjectData(parent.position, parent.rotation, parent.localScale, color, grabbable, isVisible);
        
        ApplyDataToSlide(data);
    }

    public void ApplyDataToSlideWithIndex(SlideObjectData data, int index)
    {
        this.slideData[index] = data;
        this.animationList[index].SetPreviousSlideObjectData(data);
        if(index - 1 >= 0)
            this.animationList[index - 1].SetNextSlideObjectData(data);
    }

    public void ApplyDataToSlide(SlideObjectData data)
    {
        this.slideData[this.currentSlide] = data;
        this.animationList[this.currentSlide].SetPreviousSlideObjectData(data);
        if(this.currentSlide - 1 >= 0)
            this.animationList[this.currentSlide - 1].SetNextSlideObjectData(data);
    }
    
    public void ApplyDataToObject(SlideObjectData data) //SetSlideObjectData -> ApplySlideObjectData
    {
        transform.parent.SetPositionAndRotation(data.position, data.rotation);
        transform.parent.localScale = data.scale;
        if (meshRenderer != null && MainSystem.Instance.mode != MainSystem.Mode.Animation) meshRenderer.material.color = data.color;
        if (Image != null && MainSystem.Instance.mode != MainSystem.Mode.Animation) Image.color = data.color;
        if (data.isVisible)
        {
            SetVisible();
        }
        else
        {
            SetInvisible();
        }
        //SetGrabbable(data.isGrabbable);
    }

    public SlideObjectData GetCurrentSlideObjectData()
    {
        var currentSlideIndex = MainSystem.Instance.currentSlideNum;
        return slideData[currentSlideIndex];
    }

    public SlideObjectData GetSlideObjectDataByIndex(int index)
    {
        if (index < 0 && index >= MainSystem.Instance.GetSlideCount()) 
            Debug.LogError("Out of bound Index");
        return slideData[index];
    }
    public uint GetID()
    {
        return id;
    }
    
    /*
    public SlideObjectData GetSlideObjectData()
    {
        var data = new SlideObjectData();
        data.position = transform.parent.position;
        data.rotation = transform.parent.rotation;
        data.scale = transform.parent.localScale;
        if (meshRenderer != null)
        {
            data.material = meshRenderer.material;
            data.color = data.material.color;
        }

        return data;
    }
    */
}

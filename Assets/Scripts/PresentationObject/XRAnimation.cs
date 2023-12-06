using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class XRAnimation
{
    public PresentationObject presentationObject;
    private uint id;

    public SlideObjectData previousData;
    public SlideObjectData nextData;
    public Ease ease;
    
    private Sequence _sequence;
    private bool _isInitialized, _isPreviewPlaying;
    private Transform _previousTransform;
    private Material _previousMaterial;

    public XRAnimation()
    {
        presentationObject = null;
        previousData = new SlideObjectData();
        nextData = new SlideObjectData();
        ease = Ease.Linear;
        _sequence = null;
        _isInitialized = false;
        _previousTransform = null;
        _previousMaterial = null;
    }
    public void Play()
    {
        float t = DOVirtual.EasedValue(0f, 1f, MainSystem.Instance.slideInterval, ease);

        SlideObjectData lerpData = new SlideObjectData();

        lerpData.position = Vector3.LerpUnclamped(previousData.position, nextData.position, t);
        lerpData.rotation = Quaternion.LerpUnclamped(previousData.rotation, nextData.rotation, t);
        lerpData.scale = Vector3.LerpUnclamped(previousData.scale, nextData.scale, t);
        lerpData.color = Color.LerpUnclamped(previousData.color, nextData.color, t);
        lerpData.isVisible = nextData.isVisible;
        lerpData.isGrabbable = nextData.isGrabbable;
        lerpData.isVideo = false;


        presentationObject.ApplyDataToObject(lerpData);
    }

    public void PlayPreview(Transform prevTrans, Material prevMat)
    {
        UpdateData(prevTrans, prevMat);
        GenerateSequence();
        _sequence.Restart();
        _isPreviewPlaying = true;
    }

    public void UpdateData(Transform prevTrans, Material prevMat)
    {
        _previousTransform = prevTrans;
        _previousMaterial = prevMat;
    }

    public void StopPreview() 
    {
        _previousTransform.position = previousData.position;
        _previousTransform.rotation = previousData.rotation;
        _previousTransform.localScale = previousData.scale;
        _previousMaterial.color = previousData.color;
        _sequence.Pause();
        _isPreviewPlaying = false;
    }

    public void SetEase(Ease newEase)
    {
        ease = newEase;
        GenerateSequence();
    }

    public void GenerateSequence()
    {
        if (!_isInitialized)
        {
            ease = Ease.Linear;
            _isInitialized = true;
        }
        else
        {
            _previousTransform.position = previousData.position;
            _previousTransform.rotation = previousData.rotation;
            _previousTransform.localScale = previousData.scale;
            _previousMaterial.color = previousData.color;
            _sequence.Kill();
        }
        
        _sequence = DOTween.Sequence().SetAutoKill(false)
            .OnStart(() =>
            {
                _previousTransform.position = previousData.position;
                _previousTransform.rotation = previousData.rotation;
                _previousTransform.localScale = previousData.scale;
                _previousMaterial.color = previousData.color;
            })
            .Append(_previousTransform.DOMove(nextData.position, 1.5f).SetEase(ease))
            .Join(_previousTransform.DORotateQuaternion(nextData.rotation, 1.5f).SetEase(ease))
            .Join(_previousTransform.DOScale(nextData.scale, 1.5f).SetEase(ease))
            .Join(_previousMaterial.DOColor(nextData.color, 1.5f).SetEase(ease))
            .AppendInterval(0.7f)
            .SetLoops(-1, LoopType.Restart);
        
        if(_isPreviewPlaying) _sequence.Restart();
    }

    public void SetParentObject(PresentationObject presentationObject)
    {
        this.presentationObject = presentationObject;
        this.id = presentationObject.GetID();
    }

    public void SetPreviousSlideObjectData(SlideObjectData slideObjectData)
    {
        this.previousData = slideObjectData;
    }
    public void SetNextSlideObjectData(SlideObjectData slideObjectData)
    {
        this.nextData = slideObjectData;
    }
    public SlideObjectData GetPreviousSlideObjectData()
    {
        return previousData;
    }
    public SlideObjectData GetNextSlideObjectData()
    {
        return nextData;
    }
}

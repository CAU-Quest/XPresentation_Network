using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public PhotonView photonView;
    
    public Transform root;
    public Transform head;
    public Transform leftController;
    public Transform rightController;

    void Start()
    {
        Player.Instance.leftController.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        Player.Instance.rightController.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        Player.Instance.leftController.GetComponentInChildren<MeshRenderer>().enabled = false;
        Player.Instance.rightController.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            root.transform.position = Player.Instance.root.transform.position;
            root.transform.rotation = Player.Instance.root.transform.rotation;
        
        
            head.transform.position = Player.Instance.head.transform.position;
            head.transform.rotation = Player.Instance.head.transform.rotation;
        
        
            leftController.transform.position = Player.Instance.leftController.transform.position;
            leftController.transform.rotation = Player.Instance.leftController.transform.rotation;
        
        
            rightController.transform.position = Player.Instance.rightController.transform.position;
            rightController.transform.rotation = Player.Instance.rightController.transform.rotation;
            
            
            var Xbutton = OVRInput.GetDown(OVRInput.RawButton.X);
            var Ybutton = OVRInput.GetDown(OVRInput.RawButton.Y);
            
            var Abutton = OVRInput.GetDown(OVRInput.RawButton.A);
            var Bbutton = OVRInput.GetDown(OVRInput.RawButton.B);
        
            if(Ybutton) MainSystem.Instance.AnimationToggle();
            if(Xbutton) MainSystem.Instance.GoToPreviousSlide();
            
            if(Abutton) Player.Instance.transform.position = Vector3.zero;
            if (Bbutton) Player.Instance.transform.position = new Vector3(0f, 0f, -5f);
        }
    }
}

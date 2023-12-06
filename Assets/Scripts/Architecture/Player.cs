using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance = null;
    
    public Transform root;
    public Transform head;
    public Transform leftController;
    public Transform rightController;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(this);
        }
    }
}

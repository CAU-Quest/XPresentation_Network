using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(SaveData))]
public class SaveDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveData saveData = (SaveData)target;
        
        
        if (GUILayout.Button("Load Data"))
        {
            saveData.LoadGameDataRPC();
        }

    }
}
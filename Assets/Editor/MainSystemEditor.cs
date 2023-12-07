using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainSystem))]
public class MainSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MainSystem mainSystem = (MainSystem)target;
        
        GUILayout.Space(10); // 각 섹션 사이에 여백 추가
        EditorGUILayout.LabelField("기능", EditorStyles.boldLabel);
        
        if (GUILayout.Button("애니메이션 실행"))
        {
            mainSystem.AnimationToggle();
        }
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Slide 관리", EditorStyles.boldLabel);

        if (GUILayout.Button("이전 슬라이드로"))
        {
            mainSystem.GoToPreviousSlide();
        }
        if (GUILayout.Button("다음 슬라이드로"))
        {
            mainSystem.GoToNextSlide();
        }
    }
}

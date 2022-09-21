using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LampuLalinController))]
public class LampuLalinControllerEditor : Editor {
    SerializedProperty lampusProperty;

    private void OnEnable() {
        lampusProperty = serializedObject.FindProperty("lampusList");
    }
    public override void OnInspectorGUI() {
        var myScript = target as LampuLalinController;
        
        serializedObject.Update();
        SerializedProperty sp = lampusProperty.Copy();

        myScript.isSingle = EditorGUILayout.Toggle("Is Single" ,myScript.isSingle);
        EditorGUILayout.Space();
        if(myScript.isSingle){
            EditorGUILayout.LabelField("Lampu Object", EditorStyles.boldLabel);
            myScript.lampuMerah = (GameObject) EditorGUILayout.ObjectField("Lampu Merah", myScript.lampuMerah,  typeof(GameObject), true);
            myScript.lampuKuning = (GameObject) EditorGUILayout.ObjectField("Lampu Kuning", myScript.lampuKuning, typeof(GameObject), true);
            myScript.lampuHijau = (GameObject) EditorGUILayout.ObjectField("Lampu Hijau", myScript.lampuHijau, typeof(GameObject), true);
        }else{
            int listSize = lampusProperty.arraySize;
            listSize = EditorGUILayout.DelayedIntField("Size", listSize);

            if(listSize != lampusProperty.arraySize){
                while(listSize > lampusProperty.arraySize){
                    lampusProperty.InsertArrayElementAtIndex(lampusProperty.arraySize);
                }
                while(listSize < lampusProperty.arraySize){
                    lampusProperty.DeleteArrayElementAtIndex(lampusProperty.arraySize - 1);
                }
            }

            for (int i = 0; i < lampusProperty.arraySize; i++)
            {
                SerializedProperty lampus = lampusProperty.GetArrayElementAtIndex(i);
                SerializedProperty lampuMerah = lampus.FindPropertyRelative("lampuMerah");
                SerializedProperty lampuKuning = lampus.FindPropertyRelative("lampuKuning");
                SerializedProperty lampuHijau = lampus.FindPropertyRelative("lampuHijau");

                EditorGUILayout.LabelField("Lampu Ke-"+(i+1), EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(lampuMerah);
                EditorGUILayout.PropertyField(lampuKuning);
                EditorGUILayout.PropertyField(lampuHijau);
            }
            
        }
        EditorGUILayout.Space();
        myScript.trigger = (GameObject) EditorGUILayout.ObjectField("Trigger", myScript.trigger, typeof(GameObject), true);

        serializedObject.ApplyModifiedProperties();
        
    }
}

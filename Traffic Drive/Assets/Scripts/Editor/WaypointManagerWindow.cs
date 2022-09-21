using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointManagerWindow : EditorWindow {

    [MenuItem("Tools/WaypointManagerWindow")]
    public static void Open(){
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;

    private void OnGUI() {
        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if(waypointRoot == null){
            EditorGUILayout.HelpBox("Root transform must be assigned!", MessageType.Warning);
        }else{
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawButtons(){
        if(GUILayout.Button("Create Waypoint")) {
            CreateWaypoint();
        }
        if(GUILayout.Button("Create Waypoint Before")){
            if(Selection.activeGameObject.GetComponent<Waypoint>() != null) CreateWaypointBefore();
        }
        if(GUILayout.Button("Create Waypoint After")){
            if(Selection.activeGameObject.GetComponent<Waypoint>() != null) CreateWaypointAfter();
        }
        if(GUILayout.Button("Create Branch")){
            if(Selection.activeGameObject.GetComponent<Waypoint>() != null) CreateBranch();
        }
        if(GUILayout.Button("Remove Waypoint")){
            if(Selection.activeGameObject.GetComponent<Waypoint>() != null) RemoveWaypoint();
        }
    }

    private void CreateWaypoint(){
        GameObject waypointObject = new GameObject("Waypoint "+waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if(waypointRoot.childCount > 1){
            waypoint.prevWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
            waypoint.prevWaypoint.nextWaypoint = waypoint;

            waypoint.transform.position = waypoint.prevWaypoint.transform.position;
            waypoint.transform.forward = waypoint.prevWaypoint.transform.forward;

            Selection.activeGameObject = waypointObject;
        }
    }
    
    private void CreateWaypointBefore(){
        GameObject waypointObject = new GameObject("Waypoint "+waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        if(selectedWaypoint.prevWaypoint != null){
            waypoint.prevWaypoint = selectedWaypoint.prevWaypoint;
            waypoint.prevWaypoint.nextWaypoint = waypoint;
        }

        waypoint.nextWaypoint = selectedWaypoint;
        selectedWaypoint.prevWaypoint = waypoint;

        waypoint.transform.position = selectedWaypoint.transform.position;
        waypoint.transform.forward = selectedWaypoint.transform.forward;

        waypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
        Selection.activeGameObject = waypointObject.gameObject;
    }
    private void CreateWaypointAfter(){
        GameObject waypointObject = new GameObject("Waypoint "+waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        if(selectedWaypoint.nextWaypoint != null){
            waypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            waypoint.nextWaypoint.prevWaypoint = waypoint;
        }

        waypoint.prevWaypoint = selectedWaypoint;
        selectedWaypoint.nextWaypoint = waypoint;

        waypoint.transform.position = selectedWaypoint.transform.position;
        waypoint.transform.forward = selectedWaypoint.transform.forward;

        waypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
        Selection.activeGameObject = waypointObject.gameObject;
    }
    private void CreateBranch(){
        GameObject waypointObject = new GameObject("Waypoint "+waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint branch = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        selectedWaypoint.Branches.Add(branch);

        branch.transform.position = selectedWaypoint.transform.position;
        branch.transform.forward = selectedWaypoint.transform.forward;

        Selection.activeGameObject = branch.gameObject;
    }
    private void RemoveWaypoint(){
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
        if(selectedWaypoint.nextWaypoint != null){
            selectedWaypoint.nextWaypoint.prevWaypoint = selectedWaypoint.prevWaypoint;
        }
        if(selectedWaypoint.prevWaypoint != null){
            selectedWaypoint.prevWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            Selection.activeGameObject = selectedWaypoint.prevWaypoint.gameObject;
        }

        DestroyImmediate(selectedWaypoint.gameObject);
    }
}

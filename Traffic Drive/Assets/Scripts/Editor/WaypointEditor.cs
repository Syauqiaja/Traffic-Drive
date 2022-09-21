
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected|GizmoType.Selected|GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType){
        if((gizmoType & GizmoType.Selected) != 0){
            Gizmos.color = Color.white;
        }else{
            Gizmos.color = Color.white * 0.7f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, 0.2f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width/2f), 
                        waypoint.transform.position - (waypoint.transform.right * waypoint.width/2f));

        if(waypoint.prevWaypoint != null){
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.width/2f;
            Vector3 offsetTo = waypoint.prevWaypoint.transform.right * waypoint.prevWaypoint.width / 2f;
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.prevWaypoint.transform.position + offsetTo);
        }
        if(waypoint.nextWaypoint != null){
            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right * waypoint.width/2f;
            Vector3 offsetTo = waypoint.nextWaypoint.transform.right * waypoint.nextWaypoint.width / 2f;
            Gizmos.DrawLine(waypoint.transform.position - offset, waypoint.nextWaypoint.transform.position - offsetTo);
        }
        if(waypoint.Branches != null && waypoint.Branches.Count > 0){
            Gizmos.color = Color.blue;
            for (int i = 0; i < waypoint.Branches.Count; i++)
            {
                Gizmos.DrawLine(waypoint.transform.position, waypoint.Branches[i].transform.position);
            }
        }
    }
}

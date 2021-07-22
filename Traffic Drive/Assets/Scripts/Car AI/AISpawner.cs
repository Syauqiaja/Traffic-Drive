using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public List<GameObject> AIPrefabs = new List<GameObject>();
    public float interval = 0.5f;
    public int AgentQuantity = 20;

    private void Start() {
        if(AIPrefabs.Count > 0){
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn(){
        for (int i = 0; i < AgentQuantity; i++)
        {
            Waypoint point = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Waypoint>();
            Waypoint nextPoint;
            if(point.nextWaypoint == null){
                if(point.Branches.Count > 0){
                    nextPoint = point.Branches[Random.Range(0, point.Branches.Count)];
                }else{
                    continue;
                }
            }else{
                nextPoint = point.nextWaypoint;
            }
            GameObject agent =(GameObject) Instantiate(AIPrefabs[Random.Range(0, AIPrefabs.Count)]);
            // Quaternion spawnRot = Quaternion.LookRotation(point.transform.position - point.nextWaypoint.transform.position, Vector3.up);
            agent.GetComponent<CarAIController>().SetDestination(point.nextWaypoint);
            agent.transform.position = point.transform.TransformPoint(new Vector3(0f, 1f, 0f));
            agent.transform.forward = (point.nextWaypoint.transform.position - point.transform.position).normalized;
            yield return new WaitForSeconds(interval);
        }
    }
}

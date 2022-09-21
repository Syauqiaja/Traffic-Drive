using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightSystem : MonoBehaviour
{
    [System.Serializable]
    public class TrafficLight{
    public LampuLalinController controller;
    public float waktuHijau;
}
    public List<TrafficLight> trafficLights = new List<TrafficLight>();
    public float waktuJeda = 4f;
    private void Start() {
        StartCoroutine(lightCycle());
    }

    IEnumerator lightCycle(){
        for (int i = 1; i < trafficLights.Count; i++)
        {
            trafficLights[i].controller.activeRed();
        }
        while(true){
            int last = trafficLights.Count - 1;
            for(int curr = 0; curr<trafficLights.Count; ++curr){
                trafficLights[last].controller.activeRed();
                yield return new WaitForSeconds(waktuJeda);
                trafficLights[curr].controller.activeGreen();
                yield return new WaitForSeconds(trafficLights[curr].waktuHijau);
                last = curr;
            }
        }
    }

}



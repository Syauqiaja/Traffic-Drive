using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightSystem : MonoBehaviour
{
    public List<TrafficLight> trafficLights = new List<TrafficLight>();
    private void Start() {
        StartCoroutine(lightCycle());
    }

    IEnumerator lightCycle(){
        while(true){
            int j = trafficLights.Count - 1;
            for(int i = 0; i<trafficLights.Count; ++i){
                lightOn(i, j);
                yield return new WaitForSeconds(trafficLights[i].waktuHijau);
                j = i;
            }
        }
    }

    void lightOn(int currentIndex, int lastIndex){
        trafficLights[lastIndex].controller.activeRed();
        trafficLights[currentIndex].controller.activeGreen();
    }
}

[System.Serializable]
public class TrafficLight{
    public LampuLalinController controller;
    public float waktuHijau;
}

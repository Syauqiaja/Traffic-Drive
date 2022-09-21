using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class LampuLalinController : MonoBehaviour
{
    [System.Serializable]
public class Lampus{
    public GameObject lampuMerah;
    public GameObject lampuKuning;
    public GameObject lampuHijau;
}
    [Header("Lampu")]
    public bool isSingle = true;
     public GameObject lampuMerah;
     public GameObject lampuKuning;
     public GameObject lampuHijau;

     public List<Lampus> lampusList = new List<Lampus>();

    public GameObject trigger;

    public bool isHijau =  false;

    public int state;

    public void activeGreen(){
        if(isSingle){
            lampuMerah.SetActive(false);
            lampuHijau.SetActive(true);
        }else{
            for (int i=0; i<lampusList.Count; ++i)
            {
                lampusList[i].lampuMerah.SetActive(false);
                lampusList[i].lampuHijau.SetActive(true);
            }
        }
        trigger.SetActive(false);
        state = 2;
        isHijau = true;
    }

    public void activeRed(){
        isHijau = false;
        StartCoroutine(Red());
    }

    IEnumerator Red(){
        if(isSingle){
            lampuHijau.SetActive(false);
            lampuKuning.SetActive(true);
        }else{
            for (int i = 0; i < lampusList.Count; i++)
            {
                lampusList[i].lampuHijau.SetActive(false);
                lampusList[i].lampuKuning.SetActive(true);
            }
        }
        state = 1;
        yield return new WaitForSeconds(0.7f);
        trigger.SetActive(true);
        if(isSingle){
            lampuKuning.SetActive(false);
            lampuMerah.SetActive(true);
        }else{
            for (int i = 0; i < lampusList.Count; i++)
            {
                lampusList[i].lampuKuning.SetActive(false);
                lampusList[i].lampuMerah.SetActive(true);
            }
        }
        state = 0;
        yield return null;
    }
}
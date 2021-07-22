using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampuLalinController : MonoBehaviour
{
    [Header("Lampu")]
    public GameObject lampuMerah;
    public GameObject lampuKuning;
    public GameObject lampuHijau;
    public GameObject trigger;

    [Header("Timer")]
    public float waktuMerah;
    public float waktuKuning;
    public float waktuHijau;

    public bool isHijau =  false;

    // private void Start() {
    //     StartCoroutine(colorChanger());
    // }

    public void activeGreen(){
        lampuMerah.SetActive(false);
        lampuHijau.SetActive(true);
        isHijau = true;
    }

    public void activeRed(){
        isHijau = false;
        StartCoroutine(Red());
    }

    IEnumerator Red(){
        lampuHijau.SetActive(false);
        lampuKuning.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        lampuKuning.SetActive(false);
        lampuMerah.SetActive(true);
    }

    // IEnumerator colorChanger(){
    //     while(true){
    //         if(isHijau){
    //             lampuHijau.SetActive(true);
    //             yield return new WaitForSeconds(waktuHijau);
    //             lampuKuning.SetActive(true);
    //             lampuHijau.SetActive(false);
    //             isHijau = false;
    //             yield return new WaitForSeconds(waktuKuning);
    //             lampuKuning.SetActive(false);
    //         }else{
    //             trigger.SetActive(true);
    //             lampuMerah.SetActive(true);
    //             yield return new WaitForSeconds(waktuMerah);
    //             lampuMerah.SetActive(false);
    //             isHijau = true;
    //             trigger.SetActive(false);
    //         }
    //     }
    // }
}

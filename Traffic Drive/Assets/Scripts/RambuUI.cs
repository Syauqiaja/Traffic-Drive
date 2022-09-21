using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RambuUI : MonoBehaviour
{
    [SerializeField] private Sprite imageToShow;
    [SerializeField] private VisualManager.RambuType rambuType = VisualManager.RambuType.Perintah;

    private bool playerInArea = false;
    private LampuLalinController lalinController = null;
    private void Start() {
        if(rambuType == VisualManager.RambuType.LampuMerah){
            lalinController = transform.parent.GetComponent<LampuLalinController>();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            playerInArea = true;
            if(rambuType == VisualManager.RambuType.LampuMerah)
                StartCoroutine(RambuCoroutine());
            else
                VisualManager.Instance.ShowRambu(rambuType, imageToShow);
        }
    }

    IEnumerator RambuCoroutine(){
        int currentState = lalinController.state;
        VisualManager.Instance.ShowRambu(currentState);

        while (playerInArea)
        {
            if(currentState != lalinController.state){
                Debug.Log(lalinController.state);
                currentState = lalinController.state;
                VisualManager.Instance.ShowRambu(currentState);
            }
            yield return null;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            playerInArea = false;
            VisualManager.Instance.HideRambu(rambuType);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Checkpoint nextCheckpoint;
    public bool isFinal = false;
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(isFinal != true && nextCheckpoint != null){
                VisualManager.Instance.GetCheckpoint();
                nextCheckpoint.gameObject.SetActive(true);
                PlayerManager.Instance.timeLeft += 5f;
            }else if(isFinal){
                PlayerManager.Instance.allowInput = false;
                VisualManager.Instance.WinningWindow();
            }
            gameObject.SetActive(false);
        }
    }
}

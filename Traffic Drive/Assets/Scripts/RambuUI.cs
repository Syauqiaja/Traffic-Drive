using UnityEngine;
using UnityEngine.UI;

public class RambuUI : MonoBehaviour
{
    [SerializeField] private float triggerRadius = 5f;
    [SerializeField] private Sprite imageToShow;

    private void Update() {
        if((PlayerManager.Instance.playerTransform.position - transform.position).sqrMagnitude <= triggerRadius){
            
        }
    }

    void Show(Sprite image){
        VisualManager.Instance.ShowRambu(image);
    }
}

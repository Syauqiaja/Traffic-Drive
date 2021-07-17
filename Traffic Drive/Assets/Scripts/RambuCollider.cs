using UnityEngine;

public class RambuCollider : MonoBehaviour
{
    public bool isTwoSided = true;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(isTwoSided){
                Penalty();
            }else{
                Debug.Log(Vector3.Angle(other.transform.forward, transform.forward));
                if(Vector3.Angle(other.transform.right, transform.right) < 90f){
                    Penalty();
                }
            }
        }
    }

    public void Penalty(){
        VisualManager.Instance.Danger();
        VisualManager.Instance.CameraShake(0.02f, 0.2f);
    }
}

using UnityEngine;

public class RambuStayCollider : MonoBehaviour
{
    public float stayMaxTime = 5f;
    public float refreshRate = 0.5f;
    private float currentTime = 0f;
    private float currentRefreshTime = 0f;
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")){
            if(Vector3.Angle(other.transform.right, transform.right) < 90f){
                currentTime += Time.deltaTime;
                if(Time.time > currentRefreshTime){
                    currentRefreshTime = Time.time + 1/refreshRate;
                    if(currentTime > stayMaxTime) currentTime = stayMaxTime;
                    Penalty();
                }
            }else if(currentTime > 0f) ResetTime();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player") && currentTime > 0f){
            if(Vector3.Angle(other.transform.right, transform.right) < 90f){
                ResetTime();
            }
        }
    }

    public void Penalty(){
        float _timeToShow = Mathf.Round(currentTime * 100f) * 0.01f;
        VisualManager.Instance.ShowTimer(_timeToShow, stayMaxTime);
        if(currentTime >= stayMaxTime) VisualManager.Instance.Danger();
    }

    public void ResetTime(){
        currentTime = 0f;
        VisualManager.Instance.RemoveTimer();
    }
}

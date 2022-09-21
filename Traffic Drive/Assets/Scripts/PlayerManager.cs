using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Transform playerTransform;
    [HideInInspector] public float timeLeft = 0;

    public float startingTime = 120f;
    public bool allowInput = true;

    private void Awake() {
        timeLeft = startingTime;
        if(Instance == null) Instance = this;
        if(playerTransform == null)playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update() {
        if(allowInput == true){
            timeLeft -= Time.deltaTime;
            VisualManager.Instance.UpdateCountdown(timeLeft);
        }
    }
}

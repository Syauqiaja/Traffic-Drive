using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Transform playerTransform;

    private void Awake() {
        if(Instance == null) Instance = this;
        if(playerTransform == null)playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
}

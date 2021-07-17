using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    public static VisualManager Instance;
    [SerializeField] private Image vignetteImage;
    [SerializeField] private Color vignetteStartColor;
    [SerializeField] private float vignetteCooldown = 2f;
    [SerializeField] private Transform camTransform;
    [SerializeField] private GameObject TimerGO;
    [SerializeField] private Text timerText;

    private RectTransform TimerRect;
    private Image TimerImage;
    private void Awake() {
        if(Instance == null) Instance = this;
        if(camTransform == null) camTransform = Camera.main.transform;
        if(TimerGO != null){
            TimerRect = TimerGO.GetComponent<RectTransform>();
            TimerImage = TimerGO.GetComponent<Image>();
        }
    }

    public void Danger(){
        if(vignetteImage.color.a < 0.2f){
            StopCoroutine(removeVignette());
            vignetteImage.color = vignetteStartColor;
            StartCoroutine(removeVignette());
        }
    }

    public void CameraShake(float magnitude, float shakingTime){
        StartCoroutine(camShake(magnitude, shakingTime));
    }

    IEnumerator camShake(float mag, float stime){
        while(stime > 0f){
            Vector3 shakePos = new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f), camTransform.position.z) * mag;
            camTransform.position = camTransform.TransformPoint(shakePos);

            stime -= Time.deltaTime;
            yield return null;
        }
    }
    
    IEnumerator removeVignette(){
        float _timer = vignetteCooldown;
        while(_timer > 0){
            vignetteImage.color = Color.Lerp(Color.clear, vignetteStartColor, _timer);
            _timer -= Time.deltaTime;
            yield return null;
        }
    }

    public void ShowRambu(Sprite image){
        
    }

    public void ShowTimer(float count, float maxCount){
        if(timerText.text == ""){
            StopCoroutine(move(-47.971f));
            StartCoroutine(move(47.975f));
        }
        TimerImage.color = Color.Lerp(Color.white, Color.red, count/maxCount);
        timerText.text = count.ToString();
        
    }

    public void RemoveTimer(){
        StopCoroutine(move(47.975f));
        StartCoroutine(move(-47.971f));
        timerText.text = "";
    }

    IEnumerator move(float y){
        float waktu = 0f;
        while(waktu < 1f){
            TimerRect.anchoredPosition = new Vector2(TimerRect.anchoredPosition.x, Mathf.SmoothStep(TimerRect.anchoredPosition.y, y, waktu));
            waktu += Time.deltaTime*2f;
            yield return null;
        }
    }
}

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VisualManager : MonoBehaviour
{
    public static VisualManager Instance;

    private Animator animator;
    public enum RambuType{
        Perintah = 0,
        Larangan = 1,
        Peringatan = 2,
        LampuMerah = 3,
    }
    [SerializeField] private Transform camTransform;
    [SerializeField] private Text countdownText;

    [Header("Tutorial Section")]
    [SerializeField] private Image tutorialBG;
    [SerializeField] private Text tutorialText;
    [HideInInspector] public List<string> tutorialList = new List<string>();

    [Header("Winning Section")]
    [SerializeField] private GameObject WinningPanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private List<Image> starImg;
    [SerializeField] private Sprite WinSprite;
    [SerializeField] private Sprite LoseSprite;

    [Header("Penalty Section")]
    [SerializeField] private Image vignetteImage;
    [SerializeField] private Color vignetteStartColor;
    [SerializeField] private float vignetteCooldown = 2f;
    [SerializeField] private GameObject TimerGO;
    [SerializeField] private Text timerText;

    [Header("Rambu UI Section")]
    [SerializeField] private Sprite[] spriteLampuMerah = new Sprite[3];
    [SerializeField] private List<RambuUIHolder> rambuUIHolders = new List<RambuUIHolder>();

    private RectTransform TimerRect;
    private Image TimerImage;
    private void Awake() {
        if(Instance == null) Instance = this;
        if(camTransform == null) camTransform = Camera.main.transform;
        if(animator == null) animator = GetComponent<Animator>();
        if(TimerGO != null){
            TimerRect = TimerGO.GetComponent<RectTransform>();
            TimerImage = TimerGO.GetComponent<Image>();
        }
    }

    public void Danger(){
        if(vignetteImage.color.a < 0.2f){
            StopCoroutine(removeVignette());
            vignetteImage.color = vignetteStartColor;
            StartCoroutine(changeFontColor(countdownText, Color.red));
            StartCoroutine(removeVignette());
        }
    }

    public void UpdateCountdown(float count){
        countdownText.text = Mathf.Round(count).ToString() + "." + (Mathf.Round(count * 10)%10).ToString();
    }

    public void ShowPause(){
        animator.SetTrigger("ShowPause");
    }

    public void Resume(){
        animator.SetTrigger("HidePause");
    }

    public void GetCheckpoint(){
        StartCoroutine(changeFontColor(countdownText, Color.green));
    }
    

    public void ShowTutorial(){
        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial(){
        tutorialBG.gameObject.SetActive(true);
        float _timer = 0f;
        tutorialText.color = Color.clear;
        tutorialText.text = tutorialList[0];
        while(_timer < 1f){
            tutorialText.color = Color.Lerp(tutorialText.color, Color.white, _timer);
            tutorialBG.color = Color.Lerp(tutorialBG.color, Color.black, _timer);
            _timer += Time.deltaTime * 0.4f;
            yield return null;
        }

        for (int i = 1; i < tutorialList.Count; i++)
        {
            _timer = 0f;
            while(_timer < 1f){
                tutorialText.color = Color.Lerp(tutorialText.color, Color.clear, _timer);
                _timer += Time.deltaTime * 0.4f;
                yield return null;
            }
            tutorialText.text = tutorialList[i];
            _timer = 0f;
            while(_timer < 1f){
                tutorialText.color = Color.Lerp(tutorialText.color, Color.white, _timer);
                _timer += Time.deltaTime * 0.4f;
                yield return null;
            }
        }
        _timer = 0f;
        while(_timer < 1f){
            tutorialText.color = Color.Lerp(tutorialText.color, Color.clear, _timer);
            tutorialBG.color = Color.Lerp(tutorialBG.color, Color.clear, _timer);
            _timer += Time.deltaTime * 0.4f;
            yield return null;
        }
        tutorialBG.gameObject.SetActive(false);
    }

    IEnumerator changeFontColor(Text _text, Color _color){
        _text.color = _color;
        float _timer = 0f;
        while(_timer < 1f){
            _text.color = Color.Lerp(_text.color, Color.white, _timer);
            _timer += Time.deltaTime * 0.4f;
            yield return null;
        }
        _text.color = Color.white;
        yield return null;
    }

    public void CameraShake(float magnitude, float shakingTime){
        StartCoroutine(camShake(magnitude, shakingTime));
    }

    IEnumerator camShake(float mag, float stime){
        Vector3 startPos = camTransform.position + camTransform.forward * 2f;
        while(stime > 0f){
            Vector3 shakePos = new Vector3(Random.Range(-1f, 1f) * mag,Random.Range(-1f, 1f) * mag, 0);
            camTransform.position = startPos + shakePos;

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

    public void ShowRambu(RambuType rambuType, Sprite image){
        StartCoroutine(RambuCoroutine(rambuType, image));
    }
    public void ShowRambu(int state){
        StartCoroutine(RambuCoroutine(RambuType.LampuMerah, spriteLampuMerah[state]));
    }
    public void HideRambu(RambuType rambuType){
        StartCoroutine(RemoveRambu(rambuType));
    }
    public void WinningWindow(){
        camTransform.GetComponent<AudioListener>().enabled = false;
        int bintang =(int) Mathf.Lerp(1, 4,PlayerManager.Instance.timeLeft/ PlayerManager.Instance.startingTime);
        scoreText.text = (PlayerManager.Instance.timeLeft * 100f).ToString();

        for (int i = 0; i < bintang; i++)
        {
            starImg[i].sprite = WinSprite;
        }
        for (int i = bintang; i < 3; i++)
        {
            starImg[i].sprite = LoseSprite;
        }
        WinningPanel.SetActive(true);
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
    IEnumerator RambuCoroutine(RambuType type, Sprite fill){
        rambuUIHolders[((int)type)].fillTransform.localScale = Vector3.one * 0f;
        rambuUIHolders[((int)type)].fillImage.sprite = fill;
        
        float waktu = 1f;
        while(waktu > 0f){
            rambuUIHolders[((int)type)].borderTransform.localScale = Vector3.one * waktu;
            // rambuUIHolders[((int)type)].borderTransform.localEulerAngles = Vector3.forward * 360f * (1f - waktu);

            waktu -= Time.deltaTime * 10f;
            yield return null;
        }
        waktu = 1f;
        while(waktu > 0f){
            rambuUIHolders[((int)type)].fillTransform.localScale = Vector3.one * (1f-waktu);
            // rambuUIHolders[((int)type)].borderTransform.localEulerAngles = Vector3.forward * 360f * (1f - waktu);

            waktu -= Time.deltaTime * 10f;
            yield return null;
        }
        rambuUIHolders[((int)type)].fillTransform.localScale = Vector3.one;
    }
    IEnumerator RemoveRambu(RambuType type){
        float waktu = 1f;
        while(waktu > 0f){
            rambuUIHolders[((int)type)].fillTransform.localScale = Vector3.one * waktu;
            // rambuUIHolders[((int)type)].borderTransform.localEulerAngles = Vector3.forward * 360f * (1f - waktu);

            waktu -= Time.deltaTime * 10f;
            yield return null;
        }
        waktu = 1f;
        while(waktu > 0f){
            rambuUIHolders[((int)type)].borderTransform.localScale = Vector3.one * (1f-waktu) * 1.1f;
            // rambuUIHolders[((int)type)].borderTransform.localEulerAngles = Vector3.forward * 360f * (1f - waktu);

            waktu -= Time.deltaTime * 10f;
            yield return null;
        }
        rambuUIHolders[((int)type)].fillTransform.localScale = Vector3.zero;
        rambuUIHolders[((int)type)].borderTransform.localScale = Vector3.one;
    }
}

[System.Serializable]
public class RambuUIHolder{
    public RectTransform borderTransform;
    public RectTransform fillTransform;
    public Image fillImage;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [HideInInspector] public bool IsPaused =false;
    [SerializeField] private AudioListener audioListener;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(IsPaused){
                Resume();
            }else{
                Pause();
            }
        }
    }

    public void Pause(){
        IsPaused = true;
        VisualManager.Instance.ShowPause();
        Time.timeScale = 0f;
        audioListener.enabled = false;
    }

    public void Resume(){
        IsPaused = false;
        Time.timeScale = 1f;
        VisualManager.Instance.Resume();
        audioListener.enabled = true;
    }

    public void Restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}

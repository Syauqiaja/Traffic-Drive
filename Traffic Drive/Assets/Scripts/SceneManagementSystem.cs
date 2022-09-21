using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementSystem : MonoBehaviour
{
    public void NewGame(){
        Time.timeScale = 1f;
        SoundSystem.PlaySounds(SoundSystem.Tracks.Tombol);
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }
    public void Continue(){
        SoundSystem.PlaySounds(SoundSystem.Tracks.Tombol);
    }
    public void Exit(){
        SoundSystem.PlaySounds(SoundSystem.Tracks.Tombol);
        Application.Quit();
    }
    public void MainMenu(){
        Time.timeScale = 1f;
        SoundSystem.PlaySounds(SoundSystem.Tracks.Tombol);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}

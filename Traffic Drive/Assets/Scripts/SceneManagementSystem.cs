using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementSystem : MonoBehaviour
{
    public void NewGame(){
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }
    public void Continue(){

    }
    public void Exit(){
        Application.Quit();
    }
}

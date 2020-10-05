using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
    public string gameScene;

    void Start()
    {
    }
    
    void Update()
    {
    }



    public void NewGame()
    {
        SceneManager.LoadScene(this.gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

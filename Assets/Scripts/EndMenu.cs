using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class EndMenu : MonoBehaviour
{
    public string gameScene;
    public Text winOrLoseTag;

    private GameObject intersceneObject;

    void Start()
    {
        this.intersceneObject = GameObject.Find("IntersceneObject");

        if(this.intersceneObject != null)
        {
            if(this.intersceneObject.GetComponent<IntersceneObject>().win)
            {
                this.winOrLoseTag.text = "Win!";
            }
            else
            {
                this.winOrLoseTag.text = "Lose :(";
            }
        }
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

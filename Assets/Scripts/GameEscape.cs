using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;



public class GameEscape : MonoBehaviour
{
    public IntersceneObject intersceneObject;
    public string endMenuScene;

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                this.intersceneObject.win = true;
                SceneManager.LoadScene(this.endMenuScene);
            }
        }
    }
}

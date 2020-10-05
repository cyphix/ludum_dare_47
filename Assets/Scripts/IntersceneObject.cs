using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class IntersceneObject : MonoBehaviour
{
    public bool win = false;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Update()
    {
    }



    public void Win(bool value)
    {
        this.win = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMButtons : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void NewGame()
    {
        GameManager.Instance.LoadSceneAdditive("RunSettings",false,"MainMenu");
    }
}

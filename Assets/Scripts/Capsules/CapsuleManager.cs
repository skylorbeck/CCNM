using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CapsuleManager : MonoBehaviour
{
    async void Start()
    {
        GameManager.Instance.inputReader.Back+=Back;
    }

    public void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu","Capsules");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRunManager : MonoBehaviour
{
    [SerializeField] private UIStateObject uiState;
    void Start()
    {
        uiState.ShowTopBar();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
    
    public void StartRun()
    {
        GameManager.Instance.LoadSceneAdditive("Fight", true,"RunSettings");
    }
}

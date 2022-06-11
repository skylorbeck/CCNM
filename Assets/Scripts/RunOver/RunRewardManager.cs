using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RunRewardManager : MonoBehaviour
{
    async void Start()
    {
        await Task.Delay(1000);
        //todo add rewards
        GameManager.Instance.LoadSceneAdditive("MainMenu",false,"RunOver");
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}

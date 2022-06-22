using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CapsuleManager : MonoBehaviour
{
    async void Start()
    {
        await Task.Delay(1000);
        //todo add rewards
        GameManager.Instance.LoadSceneAdditive("MainMenu","Capsules");
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

}

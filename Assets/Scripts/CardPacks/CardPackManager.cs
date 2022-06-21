using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardPackManager : MonoBehaviour
{
    async void Start()
    {
        await Task.Delay(1000);
        //todo add rewards
        GameManager.Instance.LoadSceneAdditive("MainMenu","CardPacks");
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}

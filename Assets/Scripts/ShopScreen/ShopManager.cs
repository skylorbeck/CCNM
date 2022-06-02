using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    
    async void Start()
    {
        await Task.Delay(1000);
        //todo add events
        GameManager.Instance.LoadSceneAdditive("MapScreen",false,"ShopScreen");
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}

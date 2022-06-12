using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    
    async void Start()
    {
        await Task.Delay(1000);
        //todo add events
        GameManager.Instance.LoadSceneAdditive("MapScreen","ShopScreen");
        // GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);

    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RunRewardManager : MonoBehaviour
{
    async void Start()
    {
        
        await Task.Delay(1000);
        foreach (EquipmentList equipmentList in GameManager.Instance.battlefield.player.playerInventory)
        {
            foreach (EquipmentDataContainer equipmentDataContainer in equipmentList.container)
            {
                if (!equipmentDataContainer.indestructible)//ensures original equipment is not cloned
                {
                    GameManager.Instance.metaPlayer.AddCardToInventory(equipmentDataContainer);
                }
            }
        }
        //todo add rewards
        GameManager.Instance.battlefield.deckChosen = false;
        GameManager.Instance.LoadSceneAdditive("MainMenu","RunOver");
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}

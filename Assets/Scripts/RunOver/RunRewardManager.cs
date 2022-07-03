using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RunRewardManager : MonoBehaviour
{
    async void Start()
    {
        
        await Task.Delay(1000);
        GameManager.Instance.metaPlayer.CopyEgo(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopyCards(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopyCredits(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopyCardSouls(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopyCardPacks(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopyCapsules(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopySuperCapsules(GameManager.Instance.battlefield.player);
        //todo convert credits to ego? create menu to convert credits to ego?

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

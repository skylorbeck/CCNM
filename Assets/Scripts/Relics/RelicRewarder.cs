using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicRewarder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI relicText;
    [SerializeField] private RelicShell relicShell;
    void Start()
    {
        Relic relic = GameManager.Instance.deck.relics[Random.Range(0, GameManager.Instance.deck.relics.Length)];
        relicShell.InsertRelic(relic);
        GameManager.Instance.runPlayer.AddRelic(relic);
        relicText.text = "You found a Relic!";
        
    }

    public void MoveOn()
    {
        GameManager.Instance.saveManager.SaveRun();

        GameManager.Instance.LoadSceneAdditive("MapScreen", "RelicReward");
    }
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}

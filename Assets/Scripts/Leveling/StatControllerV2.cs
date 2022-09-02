using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StatControllerV2 : MonoBehaviour
{
    [field: SerializeField] public EquipmentDataContainer.Stats stat  { get; private set; } = EquipmentDataContainer.Stats.None;
    [field: SerializeField] public TextMeshProUGUI statText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI statValueText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI translatedStatText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI translatedValueText { get; private set; }
    [field: SerializeField] public int initialValue { get; private set; }
    [field: SerializeField] public int currentValue { get; private set; }

    public LevelingManagerV2 parent;
    async void Start()
    {
        await Task.Delay(10);
        PlayerBrain player = GameManager.Instance.metaPlayer;
        statText.text = stat+":";   
        statValueText.text = player.GetUnmodifiedStatValue(stat).ToString();
        string translatedStat = "";
        string translatedValue = "";

        switch (stat)
        {
            case EquipmentDataContainer.Stats.None:
                break;
            case EquipmentDataContainer.Stats.Str:
                translatedStat = "Attack";
                translatedValue = player.GetDamage().ToString();
                break;
            case EquipmentDataContainer.Stats.Spd:
                translatedStat = "Dodge";
                translatedValue = player.GetDodgeChance()+"%";
                break;
            case EquipmentDataContainer.Stats.Vit:
                translatedStat = "Health";
                translatedValue = player.GetHealthMax().ToString();
                break;
            case EquipmentDataContainer.Stats.Dex:
                translatedStat = "Crit Chance";
                translatedValue = player.GetCritChance()+"%";
                break;
            case EquipmentDataContainer.Stats.Skl:
                translatedStat = "Crit Damage";
                translatedValue = player.GetCritDamage()+"%";
                break;
            case EquipmentDataContainer.Stats.Cap:
                translatedStat = "Shield";
                translatedValue = player.GetShieldMax().ToString();
                break;
            case EquipmentDataContainer.Stats.Chg:
                translatedStat = "Recharge";
                translatedValue = player.GetCharge().ToString();
                break;
            case EquipmentDataContainer.Stats.Wis:
                translatedStat = "Wisdom";
                translatedValue = player.GetWisdom().ToString();
                break;
            case EquipmentDataContainer.Stats.Int:
                translatedStat = "Ego Boost";
                translatedValue = player.GetEgoBoost() + "%";
                break;
            case EquipmentDataContainer.Stats.Cha:
                translatedStat = "Credit Boost";
                translatedValue = player.GetCreditBoost() + "%";
                break;
            case EquipmentDataContainer.Stats.Lck:
                translatedStat = "Luck";
                translatedValue = player.GetLuck().ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        translatedStatText.text = translatedStat+":";
        translatedValueText.text = translatedValue;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }

    public void SetStat(EquipmentDataContainer.Stats desiredStat)
    {
        stat = desiredStat;
        initialValue = GameManager.Instance.metaPlayer.GetUnmodifiedStatValue(stat);
        currentValue = initialValue;
        statText.text = stat.ToString();
        statValueText.text = currentValue.ToString();
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        // left.interactable = currentValue > initialValue;
        // right.interactable = GameManager.Instance.metaPlayer.ego >= parent.egoCostNext;
    }

    public int GetLevelDifference()
    {
        return currentValue - initialValue;
    }
    
    public void IncreaseStat()
    {
        currentValue++;
        statValueText.text = currentValue.ToString();
        parent.UpdateEgoCost();
    }

    public void DecreaseStat()
    {
        currentValue--;
        statValueText.text = currentValue.ToString();
        parent.UpdateEgoCost();
    }

    public void ResetInitial()
    {
        initialValue = GameManager.Instance.metaPlayer.GetUnmodifiedStatValue(stat);
        currentValue = initialValue;
    }
}
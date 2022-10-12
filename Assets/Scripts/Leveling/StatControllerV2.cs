using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
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
    [field: SerializeField] public bool highlighted { get; private set; }


    public LevelingManagerV2 parent;
    async void Start()
    {
        await Task.Delay(10);
        PlayerBrain player = GameManager.Instance.metaPlayer;
        initialValue = player.GetUnmodifiedStatValue(stat);
        currentValue = initialValue;
        statText.text = stat+":";
        statValueText.text = currentValue.ToString();
        string translatedStat = EquipmentDataContainer.AttributeName(stat);
        string translatedValue = player.GetAttributeStringNoCard(stat);
        translatedStatText.text = translatedStat+":";
        translatedValueText.text = translatedValue;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }
    
    public void SetSelected(bool selected)
    {
        highlighted = selected;
        TweenColors(selected);
    }

    private void TweenColors(bool selected)
    {
        if (currentValue == initialValue)
        {
            DOTween.To(() => statText.color, x => statText.color = x, selected ? Color.white : Color.gray, 0.1f);
            DOTween.To(() => statValueText.color, x => statValueText.color = x, selected ? Color.white : Color.gray, 0.1f);
            DOTween.To(() => translatedStatText.color, x => translatedStatText.color = x,
                selected ? Color.white : Color.gray, 0.1f);
            DOTween.To(() => translatedValueText.color, x => translatedValueText.color = x,
                selected ? Color.white : Color.gray, 0.1f);
        }
        else
        {
            DOTween.To(() => statText.color, x => statText.color = x, selected ? Color.yellow  : Color.green, 0.1f);
            DOTween.To(() => statValueText.color, x => statValueText.color = x, selected ? Color.yellow  : Color.green, 0.1f);
            DOTween.To(() => translatedStatText.color, x => translatedStatText.color = x, selected ? Color.yellow  : Color.green,
                0.1f);
            DOTween.To(() => translatedValueText.color, x => translatedValueText.color = x, selected ? Color.yellow  : Color.green,
                0.1f);
        }
    }

    public int GetLevelDifference()
    {
        return currentValue - initialValue;
    }
    
    public void IncreaseStat()
    {
        currentValue++;
        if (currentValue>999)
        {
            currentValue = 999;
        }
        DoTweens();
        TweenColors(highlighted);
        parent!.UpdateEgoCost();
    }

    public void DecreaseStat()
    {
        currentValue--;
        if (currentValue < initialValue)
        {
            currentValue = initialValue;
        }
        DoTweens();
        TweenColors(highlighted);
        parent!.UpdateEgoCost();
    }

    private void DoTweens()
    {
        DOTween.To(() => statValueText.text, x => statValueText.text = x, currentValue.ToString(), 0.25f);
        PlayerBrain player = GameManager.Instance.metaPlayer;
        string translatedValue = "";
        int value = GetLevelDifference();
        switch (stat)
        {
            case EquipmentDataContainer.Stats.None:
                break;
            case EquipmentDataContainer.Stats.Str:
                translatedValue = player.GetUnmodifiedDamageNoCard(value).ToString();
                break;
            case EquipmentDataContainer.Stats.Dex:
                translatedValue = player.GetDodgeChanceUnmodifiedNoCard(value).ToString("0.##") + "%";
                break;
            case EquipmentDataContainer.Stats.Vit:
                translatedValue = player.GetHealthMaxUnmodifiedNoCard(value).ToString();
                break;
            case EquipmentDataContainer.Stats.Spd:
                translatedValue = player.GetCritChanceUnmodifiedNoCard(value).ToString("0.##") + "%";
                break;
            case EquipmentDataContainer.Stats.Skl:
                translatedValue = player.GetCritDamageUnmodifiedNoCard(value).ToString("0.##") + "%";
                break;
            case EquipmentDataContainer.Stats.Cap:
                translatedValue = player.GetShieldMaxUnmodifiedNoCard(value).ToString();
                break;
            case EquipmentDataContainer.Stats.Chg:
                translatedValue = player.GetShieldRateUnmodifiedNoCard(value).ToString();
                break;
            case EquipmentDataContainer.Stats.Wis:
                translatedValue = player.GetStatusDamageUnmodifiedNoCard(value).ToString();
                break;
            case EquipmentDataContainer.Stats.Int:
                translatedValue = player.GetEgoBoostUnmodifiedNoCard(value).ToString("0.##") + "%";
                break;
            case EquipmentDataContainer.Stats.Cha:
                translatedValue = player.GetCreditBoostUnmodifiedNoCard(value).ToString("0.##") + "%";
                break;
            case EquipmentDataContainer.Stats.Lck:
                translatedValue = player.GetLootLuckUnmodifiedNoCard(value).ToString();
                break;
       
        }

        DOTween.To(() => translatedValueText.text, x => translatedValueText.text = x, translatedValue, 0.25f);
    }

    public void ResetInitial()
    {
        initialValue = GameManager.Instance.metaPlayer.GetUnmodifiedStatValue(stat);
        currentValue = initialValue;
        TweenColors(highlighted);
    }
}
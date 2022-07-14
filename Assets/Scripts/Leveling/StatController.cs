using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    [field: SerializeField] public EquipmentDataContainer.Stats stat  { get; private set; } = EquipmentDataContainer.Stats.None;
    [field: SerializeField] public TextMeshProUGUI statText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI statValueText { get; private set; }
    [field: SerializeField] public int initialValue { get; private set; }
    [field: SerializeField] public int currentValue { get; private set; }
    [field: SerializeField] public Button left { get; private set; }
    [field: SerializeField] public Button right { get; private set; }

    public LevelingManager parent;
    void Start()
    {

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
        left.interactable = currentValue > initialValue;
        right.interactable = GameManager.Instance.metaPlayer.ego >= parent.egoCostNext;
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
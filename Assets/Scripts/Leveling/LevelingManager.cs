using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LevelingManager : MonoBehaviour
{
    [field:SerializeField] public StatController statControllerPrefab{ get;private set; }
    [field:SerializeField] public GameObject statHouse{ get;private set; }
    [field:SerializeField] public Canvas canvas{ get;private set; }
    [field: SerializeField] public int initialValue { get; private set; }
    [field: SerializeField] public int currentValue { get; private set; }
    [field: SerializeField] public int egoCost { get; private set; }
    [field: SerializeField] public int egoCostNext { get; private set; }
    [field: SerializeField] public TextMeshProUGUI egoCostText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI egoCurrentText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI egoRemainingText { get; private set; }
    List<StatController> statControllers = new List<StatController>();
    async void Start()
    {
        await Task.Delay(10);
        List<EquipmentDataContainer.Stats> stats = new List<EquipmentDataContainer.Stats>((Enum.GetValues(typeof(EquipmentDataContainer.Stats)) as EquipmentDataContainer.Stats[])!);
        stats.Remove(EquipmentDataContainer.Stats.None);
        for (int i = 0; i < stats.Count; i++)
        {
            StatController statController = Instantiate(statControllerPrefab, statHouse.transform);
            statController.parent = this;
            statController.SetStat(stats[i]);
            Transform controllerTransform = statController.transform;
            controllerTransform.localPosition = new Vector3(0, 65-(i * 12.5f), 0);
            statControllers.Add(statController);
        }
        initialValue = GameManager.Instance.metaPlayer.level;
        currentValue = initialValue;
        UpdateEgoCost();
        egoCurrentText.text = GameManager.Instance.metaPlayer.ego.ToString();

        GameManager.Instance.inputReader.Back += Back;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void UpdateEgoCost()
    {
        int totalLevels = 0;
        foreach (StatController controller in statControllers)
        {
            totalLevels += controller.GetLevelDifference();
        }
        
        currentValue = initialValue + totalLevels;

        egoCost = 0;
        if (totalLevels > 0)
        {
            for (int i = 0; i < totalLevels; i++)
            {
                egoCost += (int)Math.Pow(2, initialValue + i);
            }
            egoCostNext = egoCost+ (int)Math.Pow(2, currentValue);
        }
        else
        {
            egoCostNext = (int)Math.Pow(2, currentValue);
        }
        egoCostText.text = egoCost.ToString();
        egoCurrentText.text = GameManager.Instance.metaPlayer.ego.ToString();
        egoRemainingText.text = (GameManager.Instance.metaPlayer.ego-egoCost).ToString();

        foreach (StatController controller in statControllers)
        {
            controller.UpdateButtons();
        }
    }

    public void ConfirmLevelUp()
    {
        GameManager.Instance.metaPlayer.SpendEgo(egoCost);
        foreach (StatController controller in statControllers)
        {
            GameManager.Instance.metaPlayer.LevelUpStat(controller.stat, controller.GetLevelDifference());
        }
        egoCurrentText.text = GameManager.Instance.metaPlayer.ego.ToString();
    }
    
    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }
    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("Hotel","Leveling");
    }
}

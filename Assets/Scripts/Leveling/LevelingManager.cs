using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [field: SerializeField] public Button levelUpButton { get; private set; }
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
                egoCost +=EgoScale(initialValue+i);
            }
            egoCostNext = egoCost+EgoScale(currentValue);
        }
        else
        {
            egoCostNext = EgoScale(currentValue);
        }
        egoCostText.text = egoCost.ToString();
        egoCurrentText.text = GameManager.Instance.metaPlayer.ego.ToString();
        egoRemainingText.text = (GameManager.Instance.metaPlayer.ego-egoCost).ToString();

        foreach (StatController controller in statControllers)
        {
            controller.UpdateButtons();
        }
        levelUpButton.interactable =totalLevels>0 && GameManager.Instance.metaPlayer.ego >= egoCost;
        GameManager.Instance.uiStateObject.Ping("Your Level: "+ GameManager.Instance.metaPlayer.level);
    }

    public int EgoScale(int value)
    {
        float x = Mathf.Max(value - 11,0) * 0.02f;
        float result = (x + 0.1f) * Mathf.Pow(value + 81 ,2) + 1;
        // Debug.Log("Input: "+value+" X:"+x+"  Result: "+result);
        return Mathf.CeilToInt(result);
    }

    public void ConfirmLevelUp()
    {
        GameManager.Instance.metaPlayer.SpendEgo(egoCost);
        foreach (StatController controller in statControllers)
        {
            GameManager.Instance.metaPlayer.LevelUpStat(controller.stat, controller.GetLevelDifference());
            controller.ResetInitial();
        }
        egoCurrentText.text = GameManager.Instance.metaPlayer.ego.ToString();
        initialValue = GameManager.Instance.metaPlayer.level;
        currentValue = initialValue;
        UpdateEgoCost();
        GameManager.Instance.saveManager.SaveMeta();
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

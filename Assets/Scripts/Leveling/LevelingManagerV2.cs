using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelingManagerV2 : MonoBehaviour
{
    [field:SerializeField] public GameObject statHouse{ get;private set; }
    [field:SerializeField] public Canvas canvas{ get;private set; }
    [field: SerializeField] public int initialValue { get; private set; }
    [field: SerializeField] public int currentValue { get; private set; }
    [field: SerializeField] public int egoCost { get; private set; }
    [field: SerializeField] public int egoCostNext { get; private set; }
    [field: SerializeField] public TextMeshProUGUI egoCostText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI egoCurrentText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI egoRemainingText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI selectedStatText { get; private set; }
    [field: SerializeField] public Button levelUpButton { get; private set; }
    [field: SerializeField] public List<EquipmentDataContainer.Stats> stats { get; private set; }
    [field: SerializeField] public StatControllerV2[] statControllerV2s { get; private set; }
    [field: SerializeField] public int statSelected { get; private set; }
    [field: SerializeField] public Image lowlight { get; private set; }

    async void Start()
    {
        await Task.Delay(10);
        initialValue = GameManager.Instance.metaPlayer.level;
        currentValue = initialValue;
        UpdateEgoCost();
        egoCurrentText.text = GameManager.Instance.metaPlayer.ego.ToString();
        GameManager.Instance.inputReader.Back += Back;
        await Task.Delay(10);
        UpdateSelectedStatText();
    }

    private void UpdateSelectedStatText()
    {
        foreach (StatControllerV2 statControllerV2 in statControllerV2s)
        {
            statControllerV2.SetSelected(false);
        }
        DOTween.To(()=>selectedStatText.text, x => selectedStatText.text = x, stats[statSelected].ToString(), 0.25f);
        statControllerV2s[statSelected].SetSelected(true);  
        lowlight.rectTransform.DOSizeDelta(statControllerV2s[statSelected].GetComponent<RectTransform>().sizeDelta, 0.25f);
        lowlight.rectTransform.DOAnchorPos(statControllerV2s[statSelected].GetComponent<RectTransform>().anchoredPosition, 0.25f);
    }
    
    public void IncreaseSelectedStat()
    {
        statControllerV2s[statSelected].IncreaseStat();
        UpdateEgoCost();
    }
    
    public void DecreaseSelectedStat()
    {
        statControllerV2s[statSelected].DecreaseStat();
        UpdateEgoCost();
    }
    
    public void SelectedStatButtonRight()
    {
        
        statSelected++;
        if (statSelected >= stats.Count)
        {
            statSelected = 0;
        }
        UpdateSelectedStatText();
    }
    
    public void SelectedStatButtonLeft()
    {
        statSelected--;
        if (statSelected < 0)
        {
            statSelected = stats.Count - 1;
        }
        UpdateSelectedStatText();
    }
    
    public void UpdateSelectedStat(int value)
    {
        statSelected = value;
        if (statSelected < 0)
        {
            statSelected = stats.Count - 1;
        }
        else if (statSelected >= stats.Count)
        {
            statSelected = 0;
        }
        UpdateSelectedStatText();
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
        foreach (StatControllerV2 controller in statControllerV2s)
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

        // foreach (StatController controller in statControllers)
        // {
        //     controller.UpdateButtons();
        // }
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

    public void LevelUp()
    {
        PopUpController.Instance.ShowPopUp("Are you sure you want to spend Ego and Level up?","Yes","No",ConfirmLevelUp);
    }

    public void ConfirmLevelUp()
    {
        GameManager.Instance.metaPlayer.SpendEgo(egoCost);
        foreach (StatControllerV2 controller in statControllerV2s)
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
        GameManager.Instance.LoadSceneAdditive("MainMenu","Leveling");
    }
}

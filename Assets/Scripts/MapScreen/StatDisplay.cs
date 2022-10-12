using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] statNames;
    [SerializeField] private TextMeshProUGUI[] statValues;
    [SerializeField] private TextMeshProUGUI[] attributeNames;
    [SerializeField] private TextMeshProUGUI[] attributeValues;

    void Start()
    {
        for (int i = 1; i < 9; i++)
        {
            int index = i - 1;
            EquipmentDataContainer.Stats stat = (EquipmentDataContainer.Stats)i;
            TextMeshProUGUI text = statNames[index];
            text.text = stat.ToString();
            text = statValues[index];
            text.text = GameManager.Instance.runPlayer.GetUnmodifiedStatValueWithCard(stat).ToString();
            text = attributeNames[index];
            text.text = EquipmentDataContainer.AttributeName(stat);
            text = attributeValues[index];
            text.text = GameManager.Instance.runPlayer.GetAttributeString(stat);
        }
        
        
        foreach (TextMeshProUGUI text in statNames)
        {
            text.CrossFadeAlpha(0, 0, true);
        }
        foreach (TextMeshProUGUI text in statValues)
        {
            text.CrossFadeAlpha(0, 0, true);
        }
        foreach (TextMeshProUGUI text in attributeNames)
        {
            text.CrossFadeAlpha(0, 0, true);
        }
        foreach (TextMeshProUGUI text in attributeValues)
        {
            text.CrossFadeAlpha(0, 0, true);
        }
    }

    public void FadeInOut()
    {
        foreach (TextMeshProUGUI text in statNames)
        {
            text.CrossFadeAlpha(GameManager.Instance.uiStateObject.isPaused ?1 :0, 0.25f,true);
        }
        foreach (TextMeshProUGUI text in statValues)
        {
            text.CrossFadeAlpha(GameManager.Instance.uiStateObject.isPaused ?1 :0, 0.25f,true);
        }
        foreach (TextMeshProUGUI text in attributeNames)
        {
            text.CrossFadeAlpha(GameManager.Instance.uiStateObject.isPaused ?1 :0, 0.25f,true);
        }
        foreach (TextMeshProUGUI text in attributeValues)
        {
            text.CrossFadeAlpha(GameManager.Instance.uiStateObject.isPaused ?1 :0, 0.25f,true);
        }
    }
}

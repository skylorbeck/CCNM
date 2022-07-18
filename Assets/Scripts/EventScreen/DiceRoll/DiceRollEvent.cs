using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollEvent : Event
{
    [SerializeField] private DiceRollable[] dice;
    [SerializeField] private int[] targetValues;
    [SerializeField] private int targetValueIndex;
    [SerializeField] private TextMeshProUGUI curTargetValue;

    public void OnStart()
    {
        curTargetValue.text = targetValues[targetValueIndex].ToString();
    }
    public async void RollDice()
    {
        foreach (DiceRollable die in dice)
        {
            die.Roll();
            await Task.Delay(50);
        }
    }
    
    public void TargetValueUp()
    {
        targetValueIndex++;
        if (targetValueIndex >= targetValues.Length)
        {
            targetValueIndex = 0;
        }
        curTargetValue.text = targetValues[targetValueIndex].ToString();
    }
    
    public void TargetValueDown()
    {
        targetValueIndex--;
        if (targetValueIndex < 0)
        {
            targetValueIndex = targetValues.Length - 1;
        }
        curTargetValue.text = targetValues[targetValueIndex].ToString();
    }
}

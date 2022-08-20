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
    [SerializeField] private int targetValueIndex =0;
    [SerializeField] private int betValue =0;
    [SerializeField] private TextMeshProUGUI curTargetValue;
    [SerializeField] private TextMeshProUGUI potWinningText;
    [SerializeField] private TextMeshProUGUI winningText;
    [SerializeField] private TextMeshProUGUI multiText;
    [SerializeField] private TextMeshProUGUI popDieText;
    [SerializeField] private TMP_InputField betInput;
    [SerializeField] private Resizer prizePop;
    [SerializeField] private Button leaveButton;
    private bool hasRolled = false;

    private bool isRolling
    {
        get
        {
            bool roll = false;
            foreach (DiceRollable die in dice)
            {
                if (die.isRolling)
                {
                    roll = true;
                }
            }

            return roll;
        }
    }

    public void Start()
    {
        curTargetValue.text = targetValues[targetValueIndex].ToString();
        betValue = (int)(GameManager.Instance.runPlayer.credits * 0.25f);
        betInput.text = betValue.ToString();
        UpdateValues();
    }

    public void Update()
    {
       
    }
    
    public async void RollDice()
    {
        /*if (betInput.text=="")
        {
            TextPopController.Instance.PopNegative("Please enter a bet",Vector3.zero,false);
            return;
        }*/
        if (!hasRolled)
        {
            hasRolled = true;
            foreach (DiceRollable die in dice)
            {
                die.Roll();
            }
        }
        while (isRolling)
        {
            await Task.Delay(100);
        }

        await Task.Delay(1000);
        
        int total = 0;
        foreach (DiceRollable die in dice)
        {
            total += die.diceValue+1;
        }
        if (total >= targetValues[targetValueIndex])
        {
            GameManager.Instance.runPlayer.AddCredits((betValue * (targetValueIndex+1)));
            winningText.text = (betValue * (targetValueIndex+1)).ToString()+" Credits";
            popDieText.text = total.ToString();
            prizePop.MaxScale();
        }
        else
        {
            TextPopController.Instance.PopNegative("You lost "+betValue+" Credits",Vector3.zero,false);
            GameManager.Instance.runPlayer.SpendCredits(betValue);
            potWinningText.text = "0";
        }
        leaveButton.gameObject.SetActive(true);
    }

    public void TargetValueUp()
    {
        targetValueIndex++;
        if (targetValueIndex >= targetValues.Length)
        {
            targetValueIndex = 0;
        }
        UpdateValues();
    }
    
    public void TargetValueDown()
    {
        targetValueIndex--;
        if (targetValueIndex < 0)
        {
            targetValueIndex = targetValues.Length - 1;
        }
        UpdateValues();
    }

    public void UpdateValues()
    {
        if (betInput.text!="")
        {
            betValue = int.Parse(betInput.text);
            if (betValue > GameManager.Instance.runPlayer.credits)
            {
                betValue = GameManager.Instance.runPlayer.credits;
                betInput.text = betValue.ToString();
            }
        }
        else
        {
            betValue = 0;
        }
        potWinningText.text = (betValue * (targetValueIndex+1)).ToString();
        curTargetValue.text = targetValues[targetValueIndex].ToString();
        multiText.text = "x"+(targetValueIndex+1).ToString();
    }
}

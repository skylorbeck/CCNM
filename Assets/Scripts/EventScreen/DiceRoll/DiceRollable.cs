using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollable : MonoBehaviour
{
    [SerializeField] private Image curSprite;
    [SerializeField] Sprite[] diceSprite;
    
    bool isRolling = false;
    [field:SerializeField] public int diceValue{get; private set;}
    float time = 0;
    float timeLimit = 0.5f;
    private int rollCount = 5;
    [SerializeField] private int maxRolls = 5;
    void Start()
    {
        
    }

    void Update()
    {
        curSprite.rectTransform.sizeDelta = Vector2.Lerp(curSprite.rectTransform.sizeDelta, new Vector2(32, 32), Time.deltaTime*10);
        if (isRolling)
        {
            time += Time.deltaTime;
            if (time >= timeLimit)
            {
                time = 0;
                diceValue = Random.Range(0, 6);
                curSprite.sprite = diceSprite[diceValue];
                curSprite.rectTransform.sizeDelta = new Vector2(45, 45);
                rollCount--;
                if (rollCount <= 0)
                {
                    isRolling = false;
                    rollCount = maxRolls;
                }
            }
        }
    }

    public void Roll()
    {
        isRolling = true;
    }
}

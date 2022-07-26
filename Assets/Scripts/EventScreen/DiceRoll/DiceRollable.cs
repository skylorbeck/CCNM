using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollable : MonoBehaviour
{
    [SerializeField] private Image curSprite;
    [SerializeField] Sprite[] diceSprite;

    public bool isRolling { get; private set; } = false;
    [field:SerializeField] public int diceValue{get; private set;}
    float time = 0;
    [SerializeField] float timeLimit = 0.25f;
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
                timeLimit = Random.Range(0.25f, 0.4f);
                if (rollCount <= 0)
                {
                    isRolling = false;
                    rollCount = maxRolls;
                    TextPopController.Instance.PopPositive((diceValue+1).ToString(),Camera.main.ScreenToWorldPoint( transform.position),true);
                }
            }
        }
    }

    public void Roll()
    {
        timeLimit = Random.Range(0.25f, 0.4f);
        maxRolls = Random.Range(4, 8);
        isRolling = true;
    }
}

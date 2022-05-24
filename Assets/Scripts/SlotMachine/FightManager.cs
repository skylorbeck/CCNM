using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    [field:SerializeField]public Lane selectedWheel { get; private set; } = Lane.None;
    [field:SerializeField]public Lane selectedEnemy { get; private set; } = Lane.None;
    [SerializeField] private Image selector;
    [SerializeField] private Image enemySelector;
    private bool selectorLarge = false;
    [SerializeField] private AbilityWheel[] wheels;
    public bool dirty { get; private set; } = false;
    public WheelStates state = WheelStates.Idle;
    [SerializeField] private PreviewTextController previewText;
    [SerializeField] private Enemy[] enemies;
    private Enemy targetEnemy;
    private Symbol targetSymbol;
    async void Start()
    {
        await Task.Delay(1000);
        GameManager.Instance.FixedSecond += SizeSelector;
    }

    private void OnDestroy()
    {
        GameManager.Instance.FixedSecond -= SizeSelector;
    }

    void Update()
    {
            enemySelector.transform.Rotate(Vector3.forward, (selectedEnemy!=Lane.None && selectedWheel !=Lane.None)?-0.5f:-0.2f);
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case WheelStates.Idle:
                SpinWheels();
                break;
            case WheelStates.Spinning:
                break;
            case WheelStates.Selecting:
                bool turnOver = false;
                foreach (AbilityWheel wheel in wheels)
                {
                    if (wheel.winnerChosen && wheel.winner.consumed)
                    {
                        turnOver = true;
                    }
                }

                if (turnOver)
                {
                    state = WheelStates.EnemyTurn;
                }
                break;
            case WheelStates.EnemyTurn:
                break;
        }
    }
    
    public void SizeSelector()
    {
        selectorLarge = !selectorLarge;
        selector.rectTransform.sizeDelta = selectorLarge ? new Vector2(35, 35) : new Vector2(30, 30);
        enemySelector.rectTransform.sizeDelta = selectorLarge ? new Vector2(35, 35) : new Vector2(30, 30);

    }
    
    public void SelectWheel(int wheel)
    {
        switch (wheel)
        {
            case 0:
                SelectWheel(Lane.Left);
                break;
            case 1:
                SelectWheel(Lane.Middle);
                break;
            case 2:
                SelectWheel(Lane.Right);
                break;
        }
    }
    public void SelectWheel(Lane wheel)
    {
        if (state!=WheelStates.Selecting)
        {
            return;
        }

        if (wheel == selectedWheel)
        {
            selectedWheel = Lane.None;
            selector.enabled = false;
            if (selectedEnemy!=Lane.None)
            {
                GetSelectedEnemy();
                SetPreviewText(targetEnemy);
            }
            else
            {
                ClearPreviewText();
            }
            return;
        }

        selector.enabled = true;
        selectedWheel = wheel;
        switch (wheel)
        {
            case Lane.Left:
                selector.rectTransform.anchoredPosition = new Vector2(-35.5f,28.5f);
                break;
            case Lane.Middle:
                selector.rectTransform.anchoredPosition = new Vector2(0f, 28.5f);
                break;
            case Lane.Right:
                selector.rectTransform.anchoredPosition = new Vector2(35.5f,28.5f);
                break;
        }
        GetSelectedSymbol();
        previewText.SetText(targetSymbol!.ability.title, targetSymbol!.ability.baseDamage + " damage", targetSymbol!.ability.description);

    }

    public void SelectEnemy(int enemySlot)
    {
        switch (enemySlot)
        {
            case 0:
                SelectEnemy(Lane.Left);
                break;
            case 1:
                SelectEnemy(Lane.Middle);
                break;
            case 2:
                SelectEnemy(Lane.Right);
                break;
        }
    }
    public void SelectEnemy(Lane enemySlot)
    {
        if (state!=WheelStates.Selecting)
        {
            return;
        }
        if (enemySlot == selectedEnemy)
        {
            switch (selectedWheel)
            {
                case Lane.None:
                    selectedEnemy = Lane.None;
                    enemySelector.enabled = false;
                    ClearPreviewText();
                    break;
                default:
                    PlayerAttack();
                    break;
            }
            return;
        }
        enemySelector.enabled = true;
        selectedEnemy = enemySlot;
        switch (enemySlot)
        {
            case Lane.Left:
                enemySelector.rectTransform.anchoredPosition = new Vector2(-33.5f,-52);
                break;
            case Lane.Middle:
                enemySelector.rectTransform.anchoredPosition = new Vector2(0f, -52);
                break;
            case Lane.Right:
                enemySelector.rectTransform.anchoredPosition = new Vector2(33.5f,-52);
                break;
        }
        GetSelectedEnemy(); 
        SetPreviewText(targetEnemy);
    }

    public async void SpinWheels()
    {
        SetState(WheelStates.Spinning);
        foreach (AbilityWheel wheel in wheels)
        {
            wheel.Spin();
        }
        do
        {
            dirty = false;
            foreach (AbilityWheel wheel in wheels)
            {
                if (wheel.isSpinning)
                {
                    dirty = true;
                }
            }
            await Task.Delay(100);
        } while (dirty);

        SetState(WheelStates.Selecting);
    }

    public enum WheelStates
    {
        Idle,
        Spinning,
        Selecting,
        EnemyTurn
    }
    public enum Lane
    {
        None,
        Left,
        Middle,
        Right
    }
    
    public void SetState(WheelStates newState)
    {
        ClearSelected();
        state = newState;
    }

    private void ClearSelected()
    {
        selectedWheel = Lane.None;
        selectedEnemy = Lane.None;
        targetEnemy = null;
        targetSymbol = null;
        selector.enabled = false;
        enemySelector.enabled = false;
    }

    public void SetPreviewText(Symbol symbol)
    {
        previewText.SetText(symbol!.ability.title, symbol!.ability.baseDamage + " damage", symbol!.ability.description);
    }
    
    public void SetPreviewText(Enemy enemy)
    {
        previewText.SetText(enemy!.title, enemy!.description, "");
    }

    public void ClearPreviewText()
    {
        previewText.SetText("","","");
    }
    
    public void GetSelectedEnemy()
    {
        targetEnemy = null;
        switch (selectedEnemy)
        {
            case Lane.Left:
                targetEnemy = enemies[0];
                break;
            case Lane.Middle:
                targetEnemy = enemies[1];
                break;
            case Lane.Right:
                targetEnemy = enemies[2];
                break;
        }
    }
    
    public void GetSelectedSymbol()
    {
        targetSymbol = null;
        switch (selectedWheel)
        {
            case Lane.Left:
                targetSymbol = wheels[0].GetWinner();
                break;
            case Lane.Middle:
                targetSymbol = wheels[1].GetWinner();
                break;
            case Lane.Right:
                targetSymbol = wheels[2].GetWinner();
                break;
        }
    }

    public void PlayerAttack()
    {
        targetSymbol.Consume();
        targetEnemy.Damage(targetSymbol.ability.baseDamage);//todo do damage calculation
        ClearSelected();
    }
}

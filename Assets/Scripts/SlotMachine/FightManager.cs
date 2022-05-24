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
    private bool selectorLarge = false;
    [SerializeField] private AbilityWheel[] wheels;
    public bool dirty { get; private set; } = false;
    public WheelStates state = WheelStates.Idle;

    async void Start()
    {
        await Task.Delay(1000);
        GameManager.Instance.FixedSecond += SizeSelector;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (state == WheelStates.Idle)
        {
            SpinWheels();
        }
    }
    
    public void SizeSelector()
    {
        selectorLarge = !selectorLarge;
        selector.rectTransform.sizeDelta = selectorLarge ? new Vector2(35, 35) : new Vector2(30, 30);
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
        selectedWheel = wheel;
        switch (wheel)
        {
            case Lane.Left:
                selector.rectTransform.anchoredPosition = new Vector2(-35,28.5f);
                break;
            case Lane.Middle:
                selector.rectTransform.anchoredPosition = new Vector2(0, 28.5f);
                break;
            case Lane.Right:
                selector.rectTransform.anchoredPosition = new Vector2(35,28.5f);
                break;
        }
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
        selectedEnemy = enemySlot;
        switch (enemySlot)
        {
            case Lane.Left:
                selector.rectTransform.anchoredPosition = new Vector2(-35,140);
                break;
            case Lane.Middle:
                selector.rectTransform.anchoredPosition = new Vector2(0, 140);
                break;
            case Lane.Right:
                selector.rectTransform.anchoredPosition = new Vector2(35,140);
                break;
        }
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
        Selecting
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
        selectedWheel = Lane.None;
        selector.enabled = newState == WheelStates.Selecting;
        state = newState;
    }
}

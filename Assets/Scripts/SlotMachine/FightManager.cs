using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    [field:SerializeField]public int selectedWheel { get; private set; } = 0;
    [SerializeField] private Image selector;
    private bool isSelecting = false;
    private bool selectorLarge = false;
    [SerializeField]private AbilityWheel[] wheels;
    public bool isSpinning { get; private set; } = false;
    async void Start()
    {
        await Task.Delay(1000);
        GameManager.Instance.FixedSecond += SizeSelector;
        GameManager.Instance.inputReader.ClickEvent += SpinWheels;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
    
    public void SizeSelector()
    {
        selectorLarge = !selectorLarge;
        selector.rectTransform.sizeDelta = selectorLarge ? new Vector2(35, 35) : new Vector2(30, 30);
    }
    
    public void SelectWheel(int wheel)
    {
        selectedWheel = wheel;
        switch (wheel)
        {
            case 0:
                selector.rectTransform.anchoredPosition = new Vector2(-35,28.5f);
                break;
            case 1:
                selector.rectTransform.anchoredPosition = new Vector2(0, 28.5f);
                break;
            case 2:
                selector.rectTransform.anchoredPosition = new Vector2(35,28.5f);
                break;
        }
    }

    public async void SpinWheels()
    {
        GameManager.Instance.inputReader.ClickEvent -= SpinWheels;
        foreach (AbilityWheel wheel in wheels)
        {
            wheel.Spin();
        }
        do
        {
            isSpinning = false;
            foreach (AbilityWheel wheel in wheels)
            {
                if (wheel.isSpinning)
                {
                    isSpinning = true;
                }
            }
            await Task.Delay(100);
        } while (isSpinning);
        GameManager.Instance.inputReader.ClickEvent += SpinWheels;//todo move to start of player turn

    }
}

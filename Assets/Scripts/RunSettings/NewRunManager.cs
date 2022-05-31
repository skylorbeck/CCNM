using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewRunManager : MonoBehaviour
{
    [SerializeField] private UIStateObject uiState;

    [SerializeField] private SpriteSwitcher healthSwitcher;
    [SerializeField] private SpriteSwitcher shieldSwitcher;
    [SerializeField] private SpriteSwitcher attackSwitcher;
    [SerializeField] private SpriteSwitcher overallSwitcher;
    [SerializeField] private SpriteSwitcher experienceSwitcher;
    [SerializeField] private SpriteSwitcher creditSwitcher;
    
    public float health{get; private set;}
    public float defense{get; private set;}
    public float attack{get; private set;}
    public float xp{get; private set;}
    public float credits{get; private set;}
    public float multiplier{get; private set;}
    public float finalMultiplier{get; private set;}

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private AnimationCurve healthCurve;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private Slider defenseSlider;
    [SerializeField] private AnimationCurve defenseCurve;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private Slider attackSlider;
    [SerializeField] private AnimationCurve attackCurve;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private AnimationCurve xpCurve;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private Slider creditsSlider;
    [SerializeField] private AnimationCurve creditsCurve;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Slider multiplierSlider;
    [SerializeField] private AnimationCurve multiplierCurve;

    [SerializeField] private TextMeshProUGUI finalMultiplierText;

    void Start()
    {
        uiState.ShowTopBar();
        UpdateHealth();
        UpdateDefense();
        UpdateAttack();
        UpdateXp();
        UpdateCredits();
        UpdateMultiplier();
    }

    public void UpdateHealth()
    {
        health = healthSlider.value;
        healthText.text = "x"+healthCurve.Evaluate(health).ToString("0.00");
        healthSwitcher.SwapSprite((int)health-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateDefense()
    {
        defense = defenseSlider.value;
        defenseText.text = "x"+defenseCurve.Evaluate(defense).ToString("0.00");
        shieldSwitcher.SwapSprite((int)defense-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateAttack()
    {
        attack = attackSlider.value;
        attackText.text = "x"+attackCurve.Evaluate(attack).ToString("0.00");
        attackSwitcher.SwapSprite((int)attack-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateXp()
    {
        xp = xpSlider.value;
        xpText.text = "x"+xpCurve.Evaluate(xp).ToString("0.00");
        experienceSwitcher.SwapSprite((int)xp-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateCredits()
    {
        credits = creditsSlider.value;
        creditsText.text = "x"+creditsCurve.Evaluate(credits).ToString("0.00");
        creditSwitcher.SwapSprite((int)credits-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateMultiplier()
    {
        multiplier = multiplierSlider.value;
        multiplierText.text = "x"+(multiplier).ToString("0.00");
        overallSwitcher.SwapSprite((int)multiplier-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateFinalMultiplier()
    {
        finalMultiplier = Math.Max(0,(health + defense + attack - (xp + credits)) * multiplier);
        finalMultiplierText.text = "Ego x"+Math.Round(finalMultiplier*0.5f,2).ToString("0.00");
    }
    
    public void StartRun()
    {
        GameManager.Instance.LoadSceneAdditive("Fight", false,"RunSettings");
    }
}

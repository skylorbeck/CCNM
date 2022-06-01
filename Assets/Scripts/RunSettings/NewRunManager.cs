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
    
    public double healthMod{get; private set;}
    public double defenseMod{get; private set;}
    public double attackMod{get; private set;}
    public double xpMod{get; private set;}
    public double creditsMod{get; private set;}

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
        healthMod = (Math.Round(healthCurve.Evaluate(health) * 4) * 0.25f);
        healthText.text = "x"+healthMod.ToString("0.00");
        healthSwitcher.SwapSprite((int)health-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateDefense()
    {
        defense = defenseSlider.value;
        defenseMod = (Math.Round(defenseCurve.Evaluate(defense) * 4) * 0.25f);
        defenseText.text = "x"+defenseMod.ToString("0.00");
        shieldSwitcher.SwapSprite((int)defense-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateAttack()
    {
        attack = attackSlider.value;
        attackMod = (Math.Round(attackCurve.Evaluate(attack) * 4) * 0.25f);
        attackText.text = "x"+attackMod.ToString("0.00");
        attackSwitcher.SwapSprite((int)attack-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateXp()
    {
        xp = xpSlider.value;
        xpMod = (Math.Round(xpCurve.Evaluate(xp) * 4) * 0.25f);
        xpText.text = "x"+xpMod.ToString("0.00");
        experienceSwitcher.SwapSprite((int)xp-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateCredits()
    {
        credits = creditsSlider.value;
        creditsMod = (Math.Round(creditsCurve.Evaluate(credits) * 4) * 0.25f);
        creditsText.text = "x"+creditsMod.ToString("0.00");
        creditSwitcher.SwapSprite((int)credits-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateMultiplier()
    {
        multiplier = multiplierSlider.value;
        // multiplierText.text = "x"+multiplier.ToString("0.00");
        multiplierText.text = ((Difficulty)multiplier).ToString();
        overallSwitcher.SwapSprite((int)multiplier);
        healthSlider.value = 1;
        defenseSlider.value = 1;
        attackSlider.value = 1;
        UpdateFinalMultiplier();
    }
    
    public void UpdateFinalMultiplier()
    {
        finalMultiplier = (float)(Math.Max(0,(multiplier+1)*(health + defense + attack-3)/3) + Math.Pow(multiplier+1,3));
        finalMultiplierText.text = "Ego x"+(Math.Round(finalMultiplier*4)*0.25f).ToString("0.00");
    }
    
    public void StartRun()
    {
        GameManager.Instance.LoadSceneAdditive("Fight", false,"RunSettings");
    }
    
    public enum Difficulty
    {
        Casual,
        Easy,
        Medium,
        Hard,
        Insane,
        Impossible,
        Impossible2
    }
}

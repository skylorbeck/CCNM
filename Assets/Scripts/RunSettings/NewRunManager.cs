using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewRunManager : MonoBehaviour
{
    [SerializeField] private RunSettings runSettings;

    [SerializeField] private SpriteSwitcher healthSwitcher;
    [SerializeField] private SpriteSwitcher shieldSwitcher;
    [SerializeField] private SpriteSwitcher attackSwitcher;
    [SerializeField] private SpriteSwitcher overallSwitcher;
    [SerializeField] private SpriteSwitcher experienceSwitcher;
    [SerializeField] private SpriteSwitcher creditSwitcher;
    
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

    async void Start()
    {
        healthSlider.value = runSettings.health;
        defenseSlider.value = runSettings.shield;
        attackSlider.value = runSettings.attack;
        multiplierSlider.SetValueWithoutNotify(runSettings.multiplier);
        UpdateMultiplier();
        UpdateHealth();
        UpdateDefense();
        UpdateAttack();
    }


    public void UpdateHealth()
    {
        runSettings.health = healthSlider.value;
        runSettings.healthMod = (Math.Round(healthCurve.Evaluate(runSettings.health) * 4) * 0.25f);
        healthText.text = "x"+runSettings.healthMod.ToString("0.00");
        healthSwitcher.SwapSprite((int)runSettings.health-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateDefense()
    {
        runSettings.shield = defenseSlider.value;
        runSettings.shieldMod = (Math.Round(defenseCurve.Evaluate(runSettings.shield) * 4) * 0.25f);
        defenseText.text = "x"+runSettings.shieldMod.ToString("0.00");
        shieldSwitcher.SwapSprite((int)runSettings.shield-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateAttack()
    {
        runSettings.attack = attackSlider.value;
        runSettings.attackMod = (Math.Round(attackCurve.Evaluate(runSettings.attack) * 4) * 0.25f);
        attackText.text = "x"+runSettings.attackMod.ToString("0.00");
        attackSwitcher.SwapSprite((int)runSettings.attack-1);
        UpdateFinalMultiplier();
    }
    

    
    public void UpdateMultiplier()
    {
        runSettings.multiplier = multiplierSlider.value;
        // multiplierText.text = "x"+multiplier.ToString("0.00");
        multiplierText.text = ((Difficulty)runSettings.multiplier).ToString();
        overallSwitcher.SwapSprite((int)runSettings.multiplier);
        UpdateFinalMultiplier();
    }

    public void ResetEnemySliders()
    {
        healthSlider.value = 1;
        defenseSlider.value = 1;
        attackSlider.value = 1;
    }

    public void UpdateFinalMultiplier()
    {
        runSettings.finalMultiplier =(Math.Round((Math.Max(0,Math.Pow(10,runSettings.multiplier)*(runSettings.health + runSettings.shield + runSettings.attack-3)/3) + Math.Pow(10,runSettings.multiplier))));
        finalMultiplierText.text = "Enemy Lv. "+runSettings.finalMultiplier;
    }
    
    public void StartRun()
    {
        GameManager.Instance.battlefield.Reset();
        GameManager.Instance.battlefield.SetLevel((int)runSettings.finalMultiplier);
        GameManager.Instance.LoadSceneAdditive("MapScreen", "RunSettings");
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

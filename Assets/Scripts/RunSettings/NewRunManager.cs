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

    void Start()
    {
        uiState.ShowTopBar();
        healthSlider.value = runSettings.health;
        defenseSlider.value = runSettings.defense;
        attackSlider.value = runSettings.attack;
        xpSlider.value = runSettings.xp;
        creditsSlider.value = runSettings.credits;
        multiplierSlider.SetValueWithoutNotify(runSettings.multiplier);
        UpdateMultiplier();
        UpdateHealth();
        UpdateDefense();
        UpdateAttack();
        UpdateXp();
        UpdateCredits();
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);

    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu",false,"RunSettings");
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
        runSettings.defense = defenseSlider.value;
        runSettings.defenseMod = (Math.Round(defenseCurve.Evaluate(runSettings.defense) * 4) * 0.25f);
        defenseText.text = "x"+runSettings.defenseMod.ToString("0.00");
        shieldSwitcher.SwapSprite((int)runSettings.defense-1);
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
    
    public void UpdateXp()
    {
        runSettings.xp = xpSlider.value;
        runSettings.xpMod = (Math.Round(xpCurve.Evaluate(runSettings.xp) * 4) * 0.25f);
        xpText.text = "x"+runSettings.xpMod.ToString("0.00");
        experienceSwitcher.SwapSprite((int)runSettings.xp-1);
        UpdateFinalMultiplier();
    }
    
    public void UpdateCredits()
    {
        runSettings.credits = creditsSlider.value;
        runSettings.creditsMod = (Math.Round(creditsCurve.Evaluate(runSettings.credits) * 4) * 0.25f);
        creditsText.text = "x"+runSettings.creditsMod.ToString("0.00");
        creditSwitcher.SwapSprite((int)runSettings.credits-1);
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
        runSettings.finalMultiplier =(Math.Round((Math.Max(0,Math.Pow(10,runSettings.multiplier)*(runSettings.health + runSettings.defense + runSettings.attack-3)/3) + Math.Pow(10,runSettings.multiplier))*4)*0.25f);
        finalMultiplierText.text = "Ego x"+runSettings.finalMultiplier.ToString("0.00");
    }
    
    public void StartRun()
    {
        GameManager.Instance.battlefield.Reset();
        GameManager.Instance.LoadSceneAdditive("MapScreen", false,"RunSettings");
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

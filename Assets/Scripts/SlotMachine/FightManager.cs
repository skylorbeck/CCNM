using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FightManager : MonoBehaviour
{
    [field:SerializeField]public Lane selectedWheel { get; private set; } = Lane.None;
    [field:SerializeField]public Lane selectedEnemy { get; private set; } = Lane.None;
    [SerializeField] private Image selector;
    [SerializeField] private Image enemySelector;
    [SerializeField] private Image enemySelectorInner;
    private bool selectorLarge = false;
    [SerializeField] private AbilityWheel[] wheels;
    [SerializeField] private EnemyAbilityWheel[] enemyWheels;
    [SerializeField] private float enemyWheelUpY =10;
    [SerializeField] private float enemyWheelDownY = 0;
    public bool dirty { get; private set; } = false;
    public bool BothTargeted { get; private set; } = false;
    public WheelStates state = WheelStates.Idle;
    [SerializeField] private PreviewTextController previewText;
    [SerializeField] private EnemyShell[] enemies;
    [SerializeField] private PlayerShell player;
    private Shell targetEnemy;
    private Symbol targetSymbol;
    [SerializeField] private Battlefield battlefield;
    [SerializeField] private bool turnOver = false;
    [SerializeField] private GameObject startingSelection;
    [SerializeField] private GameObject pauseStartingSelection;
    [SerializeField] private Button[] allButtons;
    [SerializeField] private TextMeshProUGUI[] pauseText;
    [SerializeField] private GraphicRaycaster pauseRaycaster;
    [SerializeField] private StatDisplay playerStatDisplay;
    private CancellationTokenSource cancellationTokenSource;
    [SerializeField] private ConsumableMenu consumableMenu;
    private int enemiesAlive
    {
        get
        {
            int count = 0;
            foreach (var enemy in enemies)
            {
                if (!enemy.isDead)
                {
                    count++;
                }
            }
            return count;
        }
    }

    async void Start()
    {
        if (battlefield.randomState != null) Random.state = battlefield.randomState.Value;
        cancellationTokenSource = new CancellationTokenSource();
        for (var i = 0; i < battlefield.enemies.Length; i++)
        {
            enemies[i].InsertBrain(battlefield.enemies[i]);
        }

        player.InsertBrain(battlefield.player);
        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(0, 0,true);
        }
        await Task.Delay(1000);
        foreach (EnemyAbilityWheel w in enemyWheels)
        {
            w.Spin();
        }
        GameManager.Instance.FixedSecond += SizeSelector;
        GameManager.Instance.eventSystem.SetSelectedGameObject(startingSelection);

        GameManager.Instance.inputReader.Back+=Back;

    }

    public void Back()
    {
        GameManager.Instance.uiStateObject.TogglePause();
        if (GameManager.Instance.uiStateObject.isPaused)
        {
            GameManager.Instance.FixedSecond -= SizeSelector;
            GameManager.Instance.eventSystem.SetSelectedGameObject(pauseStartingSelection);
        }
        else
        { 
            GameManager.Instance.FixedSecond += SizeSelector;
            GameManager.Instance.eventSystem.SetSelectedGameObject(startingSelection);
        }
        foreach (Button button in allButtons)
        {
            button.interactable = !button.interactable;
        }
        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(GameManager.Instance.uiStateObject.isPaused ?1 :0, 0.25f,true);
        }
        consumableMenu.Back();
        playerStatDisplay.FadeInOut();
        pauseRaycaster.enabled = GameManager.Instance.uiStateObject.isPaused;
        
    }

    public void Quit()
    {
        GameManager.Instance.uiStateObject.Clear();
        GameManager.Instance.battlefield.deckChosen = false;//todo replace with save system
        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(0,0.25f,true);
        }
        GameManager.Instance.LoadSceneAdditive("MainMenu","Fight");
    }

    
   
   



    private void OnDestroy()
    {
        GameManager.Instance.FixedSecond -= SizeSelector;

        GameManager.Instance.inputReader.Back -= Back;
        cancellationTokenSource.Cancel();
    }

    void Update()
    {
        if (!GameManager.Instance.uiStateObject.isPaused)
        {
            enemySelector.transform.Rotate(Vector3.forward, (BothTargeted ? -200f : -100f) * Time.deltaTime);
        }
    }


    void FixedUpdate()
    {
        switch (state)
        {
            case WheelStates.Idle:
                SpinWheels();
                player.ShieldCheck();
                break;
            case WheelStates.Spinning:
                break;
            case WheelStates.Selecting:
                turnOver = true;
                foreach (AbilityWheel wheel in wheels)
                {
                    if (!wheel.winnerChosen)
                    {
                        turnOver = false;
                    } else if (!wheel.winner.consumed)
                    {
                        turnOver = false;
                    }
                }

                if (turnOver)
                {
                    SetState(WheelStates.PlayerPostTurn);
                    PlayerStatuses();
                    ClearSelected();
                    ClearPreviewText();
                }
                break;
            case WheelStates.PlayerPostTurn:
                if (turnOver)
                {
                    MoveEnemySlots(enemyWheelDownY, false);
                    SetState(WheelStates.EnemyPreTurn);
                }
                break;
            case WheelStates.EnemyPreTurn:
                if (turnOver)
                {
                    PreFireEnemies();
                    SetState(WheelStates.EnemyTurn);
                }
                break;
            case WheelStates.EnemyTurn:
                if (turnOver) 
                {
                    EnemyTurnEnd();
                    SetState(WheelStates.EnemyPostTurn);
                }
                break;
            case WheelStates.EnemyPostTurn:
                if (turnOver)
                {
                    SetState(WheelStates.Idle);
                }
                break;
        }

        if (state!=WheelStates.FightOver)
        {
            CheckForAllDead();
        }

    }

    private void CheckForAllDead()
    {
        if (player.isDead)
        {
            SetState(WheelStates.FightOver);
            battlefield.randomState = null;//todo replace with gameOver
            GameManager.Instance.LoadSceneAdditive("RunOver","Fight");

        } else if (enemies.All(x => x.isDead))
        {
            SetState(WheelStates.FightOver);
            if (battlefield.runOver)
            {
                //todo add run over win screen
                battlefield.randomState = null;
                GameManager.Instance.LoadSceneAdditive("RunOver","Fight");
            }
            else
            {
                int credits = 0;
                foreach (var enemy in enemies)
                {
                    credits += enemy.enemyBrain.credits;
                }
                GameManager.Instance.battlefield.player.AddCredits(credits);
                battlefield.randomState = null;
                GameManager.Instance.LoadSceneAdditive("FightWon",  "Fight");
            }
        }
    }

    public async void PlayerStatuses()
    {
        turnOver = false;
        await player.TickStatusEffects();
        turnOver = true;
    }

    public async Task MoveEnemySlots(float targetY, bool ignoreDead)
    {
        if (!ignoreDead)
        {
            turnOver = false;
        }

        List<Task> tasks = new List<Task>();
        for (var i = 0; i < enemyWheels.Length; i++)
        {
            if (!enemies[i].isDead || ignoreDead)
            {
                if(!ignoreDead)
                {
                    enemies[i].statusDisplayer.DisableVisuals();
                    enemies[i].Dim();
                }
                tasks.Add(enemyWheels[i].Move(new Vector3(enemyWheels[i].transform.localPosition.x, targetY, 0f)));
            }
        }

        await Task.WhenAll(tasks.ToArray());
        for (var i = 0; i < enemyWheels.Length; i++)
        {
            if (ignoreDead)
            {
                enemies[i].statusDisplayer.EnableVisuals();
            }
        }

        await Task.Delay(500);
        if (!ignoreDead)
        {
            turnOver = true;
        }
    }

    public async void PreFireEnemies()
    {
        turnOver = false;
        
        // List<Task> tasks = new List<Task>();
        for (var i = 0; i < enemies.Length; i++)
        {
            EnemyShell enemy = enemies[i];
            if (!enemy.isDead)
            {
               // tasks.Add(enemyWheels[i].Spin());
               enemyWheels[i].Spin();
            }
        }
        bool anySpinning;
        do
        {
            anySpinning = false;
            foreach (EnemyAbilityWheel enemyWheel in enemyWheels)
            {
                if (enemyWheel.isSpinning)
                {
                    anySpinning = true;
                    break;
                }
            }
            SoundManager.instance.PlayEffect("click");
            await Task.Delay(100);
        } while (anySpinning && !cancellationTokenSource.Token.IsCancellationRequested);
        // await Task.WhenAll(tasks.ToArray());
        
        await Task.Delay(1000);//todo replace with jackpot check

        await MoveEnemySlots(enemyWheelUpY,true);

        for (var i = 0; i < enemies.Length; i++)
        {
            EnemyShell enemy = enemies[i];
            if (!enemy.isDead)
            {
                await enemy.Attack(player, enemyWheels[i].GetWinner());
                await Task.Delay(250);
                await enemy.TickStatusEffects();
                await Task.Delay(500);
                enemy.Dim();
                await Task.Delay(250);

            }
        }
        foreach (EnemyShell enemy in enemies)
        {
            if (!enemy.isDead)
            {
                enemy.Light();
            }
        }
        
        turnOver = true;
    }

    public async void EnemyTurnEnd()
    {
        turnOver = false;
        foreach (EnemyShell enemy in enemies)
        {
            
            await enemy.OnTurnEnd();
            // await Task.Delay(500);//potentially remove await for effects applying while other enemies attack?
        }
        turnOver = true;
    }
    
    public void SizeSelector()
    {
        selectorLarge = !selectorLarge;
        selector.rectTransform.sizeDelta = selectorLarge ? new Vector2(35, 35) : new Vector2(30, 30);
        // enemySelector.rectTransform.sizeDelta = selectorLarge ? new Vector2(35, 35) : new Vector2(30, 30);

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
        if (state != WheelStates.Selecting ||wheels[(int)wheel - 1].GetWinner().consumed)
        {
            return;
        }

        if (wheel == selectedWheel)
        {
            selectedWheel = Lane.None;
            selector.enabled = false;
            if (selectedEnemy != Lane.None)
            {
                GetSelectedEnemy();
                SetPreviewText(targetEnemy);
                
            }
            else
            {
                ClearPreviewText();
            }
            CheckForBothSelected();
            return;
        }
    
        selector.enabled = true;
        selectedWheel = wheel;
        switch (wheel)
        {
            case Lane.Left:
                selector.rectTransform.anchoredPosition = new Vector2(-35.5f, 28.5f);
                break;
            case Lane.Middle:
                selector.rectTransform.anchoredPosition = new Vector2(0f, 28.5f);
                break;
            case Lane.Right:
                selector.rectTransform.anchoredPosition = new Vector2(35.5f, 28.5f);
                break;
        }

        GetSelectedSymbol();
        SetPreviewText(targetSymbol);
        CheckForBothSelected();
    }

    public void SelectEnemy(int enemySlot)
    {
        if (enemies[enemySlot].isDead)
        {
            return;
        }
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
                    CheckForBothSelected();
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
        CheckForBothSelected();
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
            SoundManager.instance.PlayEffect("click");
            await Task.Delay(100);

        } while (dirty && !cancellationTokenSource.Token.IsCancellationRequested);

        SetState(WheelStates.Selecting);
    }

    public enum WheelStates
    {
        Idle,
        Spinning,
        Selecting,
        PlayerPostTurn,
        EnemyPreTurn,
        EnemyTurn,
        EnemyPostTurn,
        FightOver
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
        if (newState == WheelStates.Selecting)
        {
            GameManager.Instance.inputReader.EnableUI();
        } else
        {
            GameManager.Instance.inputReader.DisableUI();
            GameManager.Instance.uiStateObject.DisableCursor();
        }
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
        enemySelectorInner.enabled = false;
    }

    public void SetPreviewText(Symbol symbol)
    {
        player.statusDisplayer.DisableVisuals();
        previewText.SetText(symbol!.ability.title, symbol!.ability.descriptionA, symbol!.ability.descriptionB);
    }
    
    public void SetPreviewText(Shell shell)
    {
        player.statusDisplayer.DisableVisuals();
        previewText.SetText(shell!.title, shell!.description, "");
    }

    public void ClearPreviewText()
    {
        player.statusDisplayer.EnableVisuals();
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
        player.Attack(targetEnemy,targetSymbol);
        CheckForBothSelected();
        ClearSelected();
        ClearPreviewText();
    }

    public void CheckForBothSelected()
    {
        BothTargeted = selectedEnemy != Lane.None && selectedWheel != Lane.None;
        enemySelectorInner.enabled = BothTargeted;
    }
}

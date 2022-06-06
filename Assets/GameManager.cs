using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = Unity.Mathematics.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public InputReader inputReader;
    public Battlefield battlefield;
    public UIStateObject uiStateObject;
    public EventSystem eventSystem;
    public SaveData currentRunData;
    public RunSettings runSettings;
    public EquipmentRegistry equipmentRegistry;
    public AbilityRegistry abilityRegistry;
    bool playerAccepted = false;
    private float target = 0;
    
    [SerializeField] private GameObject TapToContinue;
    [SerializeField] private GameObject TEMP;
    [field:SerializeField] public Color[] colors { get; private set; }
    
    private int fixedSecondClock = 0;
    public event UnityAction FixedSecond = delegate {  };
    private int totalFixedSeconds = 0;
    public event UnityAction FixedHalfSecond = delegate {  };
    private int fixedMinuteClock = 0;
    public event UnityAction FixedMinute = delegate {  };
    async void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Application.targetFrameRate = 60;
        bool mainMenuLoaded = false;
        bool uiLoaded = false;
        uiStateObject.Clear();
        
        
        if (SceneManager.sceneCount!=1)
        {
            mainMenuLoaded = true;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name.Equals("UIOverlay"))
                {
                    uiLoaded = true;
                }
            }
        }


        if (!mainMenuLoaded)
        {
            LoadSceneAdditive("MainMenu", false);
        }
        if (!uiLoaded)
        {
            LoadSceneAdditive("UIOverlay",false);
        }
        inputReader.EnableUI();
        SceneManager.sceneLoaded += DestroyTEMP;
    }

    private void DestroyTEMP(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Equals("UIOverlay"))
        {
            Destroy(TEMP);
            SceneManager.sceneLoaded -= DestroyTEMP;
        }
    }

    public void DestroyTEMP()
    {
        
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        FixedClockActions();
    }

    public void LoadSceneAdditive(string sceneName,params string[] sceneToUnload)
    {
        LoadScene(sceneName, LoadSceneMode.Additive, true, sceneToUnload);
    }
    public void LoadSceneAdditive(string sceneName, bool waitForInput,params string[] sceneToUnload)
    {
        LoadScene(sceneName, LoadSceneMode.Additive, waitForInput, sceneToUnload);
    }

    private async void LoadScene(string sceneToLoad, LoadSceneMode mode, bool waitForInput,
        params string[] sceneToUnload)
    {
        inputReader.DisableUI();
        uiStateObject.FadeOut();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad, mode);
        operation.allowSceneActivation = false;
        await Task.Delay(1000); //this is to wait for the black screen to fade in before unloading the scene
        foreach (string scene in sceneToUnload)
        {
            SceneManager.UnloadSceneAsync(scene);
        }

        do
        {
            target = Mathf.Clamp01(operation.progress);
            await Task.Delay(100); //without this, the loading is synced and the game, and editor, will hang
        } while
            (target < 0.89); //wait for the loadingbar to be at least 90% done to give the illusion that it's loading

        // await Task.Delay(5000);//artificial loading delay to test the loading screen.
        if (waitForInput)
        {
            inputReader.EnableUI();
            inputReader.PushAnyButton += PlayerAccept;
            TapToContinue.SetActive(true);
            do
            {
                await Task.Delay(100);
            } while (!playerAccepted);

            playerAccepted = false;
        }
        inputReader.EnableUI();
        uiStateObject.FadeIn();
        operation.allowSceneActivation = true;

    }

    public void PlayerAccept()
    {
        playerAccepted = true;
        inputReader.PushAnyButton -= PlayerAccept;
        TapToContinue.SetActive(false);

    }
    
    void FixedClockActions()
    {
        fixedSecondClock++;
        if (fixedSecondClock == 25)
        {
            FixedHalfSecond.Invoke();
        }

        if (fixedSecondClock == 50)
        {
            totalFixedSeconds++;
            fixedSecondClock = 0;
            fixedMinuteClock++;
            FixedHalfSecond.Invoke();
            FixedSecond.Invoke();
            if (fixedMinuteClock == 60)
            {
                fixedMinuteClock = 0;
                FixedMinute.Invoke();
            }
        }
    }
    
}

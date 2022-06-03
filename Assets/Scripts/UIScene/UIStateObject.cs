using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UIState", menuName = "UI/UIState")]
public class UIStateObject : ScriptableObject
{
    [field: SerializeField] public bool isPaused { get; private set; }
    public UnityAction OnPause = delegate {  };
    public UnityAction OnResume = delegate {  };
    [field: SerializeField] public bool showTopBar { get; private set; }
    public UnityAction<bool> TopBarToggle = delegate {  };
    [field: SerializeField] public bool showFadeOut { get; private set; }
    public UnityAction OnFadeOut = delegate {  };
    public UnityAction OnFadeIn = delegate {  };

    public void Pause()
    {
        isPaused = true;
        OnPause.Invoke();
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    public void Resume()
    {
        isPaused = false;
        OnResume.Invoke();
    }

    public void ToggleTopBar()
    {
        showTopBar = !showTopBar;
        TopBarToggle.Invoke(showTopBar);
    }
    
    public void ShowTopBar()
    {
        showTopBar = true;
        TopBarToggle.Invoke(true);
    }
    public void HideTopBar()
    {
        showTopBar = false;
        TopBarToggle.Invoke(false);
    }
    
    public void FadeOut()
    {
        showFadeOut = true;
        OnFadeOut.Invoke();
    }
    
    public void FadeIn()
    {
        showFadeOut = false;
        OnFadeIn.Invoke();
    }

    public void ToggleFade()
    {
        if (showFadeOut)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }
}

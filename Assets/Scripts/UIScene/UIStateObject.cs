using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UIState", menuName = "UI/UIState")]
public class UIStateObject : ScriptableObject
{
    [field: SerializeField] public bool isPaused { get; private set; }
    public UnityAction onPause = delegate {  };
    public UnityAction onResume = delegate {  };
    [field: SerializeField] public bool showTopBar { get; private set; }
    public UnityAction<bool> TopBarToggle = delegate {  };
    [field: SerializeField] public bool showFadeOut { get; private set; }
    public UnityAction onFadeOut = delegate {  };
    public UnityAction onFadeIn = delegate {  };

    public void Pause()
    {
        isPaused = true;
        onPause.Invoke();
    }
    
    public void Resume()
    {
        isPaused = false;
        onResume.Invoke();
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
        onFadeOut.Invoke();
    }
    
    public void FadeIn()
    {
        showFadeOut = false;
        onFadeIn.Invoke();
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

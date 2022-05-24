using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image topBar;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private Animator blackout;
    public UIStateObject uiStateObject;
    void Start()
    {
        uiStateObject.TopBarToggle += ShowHideTopBar;
        uiStateObject.onFadeIn += FadeIn;
        uiStateObject.onFadeOut += FadeOut;
    }

 

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
    
    public void ShowTopBar()
    {
        ShowHideTopBar(true);
    }
    
    public void HideTopBar()
    {
        ShowHideTopBar(false);
    }
    public void ShowHideTopBar(bool show)
    {
        notificationText.enabled = show;
        topBar.enabled = show;
    }

    public void FadeOut()
    {
        blackout.ResetTrigger("FadeIn");
        blackout.SetTrigger("FadeOut");
    }
    
    public void FadeIn()
    {
        blackout.SetTrigger("FadeIn");
    }
}


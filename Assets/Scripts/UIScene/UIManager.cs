using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image topBar;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private Image blackout;
    [SerializeField] private List<Image> blackoutImages;
    [SerializeField] private ImageSwitcher loading;
    public UIStateObject uiStateObject;

    async void Start()
    {
        blackoutImages = new List<Image>();
        uiStateObject.TopBarToggle += ShowHideTopBar;
        uiStateObject.OnFadeIn += FadeIn;
        uiStateObject.OnFadeOut += FadeOut;
        uiStateObject.OnPause += PauseOut;
        uiStateObject.OnResume += PauseIn;
        await Task.Delay(1);
        GameManager.Instance.FixedHalfSecond += NextSprite;
        await Task.Delay(249);
        foreach (Image image in blackout.GetComponentsInChildren<Image>())
        {
            blackoutImages.Add(image);
            image.CrossFadeAlpha(0, 0.5f, false);
        }
    }

    private void OnDestroy()
    {
        uiStateObject.TopBarToggle -= ShowHideTopBar;
        uiStateObject.OnFadeIn -= FadeIn;
        uiStateObject.OnFadeOut -= FadeOut;
        uiStateObject.OnPause -= PauseOut;
        uiStateObject.OnResume -= PauseIn;
        GameManager.Instance.FixedHalfSecond -= NextSprite;

    }

    public void NextSprite()
    {
        loading.NextSprite();
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
        foreach (Image image in blackoutImages)
        {
            image.CrossFadeAlpha(1, 0.5f, true);
        }
    }

    public void FadeIn()
    {
        foreach (Image image in blackoutImages)
        {
            image.CrossFadeAlpha(0, 0.5f, true);
        }
    }

    public void PauseOut()
    {
        blackout.CrossFadeAlpha(0.75f, 0.5f, true);
    }

    public void PauseIn()
    {
        blackout.CrossFadeAlpha(0, 0.5f, true);
    }
}
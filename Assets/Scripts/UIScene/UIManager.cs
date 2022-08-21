using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image cursor;
    [SerializeField] private Image topBar;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private Image blackout;
    [SerializeField] private List<Image> blackoutImages;
    [SerializeField] private ImageSwitcher loading;
    public UIStateObject uiStateObject;
    private bool cursorPumped = false;


    async void Start()
    {
        blackoutImages = new List<Image>();
        uiStateObject.TopBarToggle += ShowHideTopBar;
        uiStateObject.OnFadeIn += FadeIn;
        uiStateObject.OnFadeOut += FadeOut;
        uiStateObject.OnPause += PauseOut;
        uiStateObject.OnResume += PauseIn;
        uiStateObject.OnDisableCursor += DisablePointer;
        uiStateObject.TopBarPing += SetTopBarText;
        
        await Task.Delay(1);
        GameManager.Instance.inputReader.PadAny += NavUpdate;
        GameManager.Instance.inputReader.ButtonDown += NavUpdate;
        GameManager.Instance.inputReader.ClickEvent += NavUpdateMouse;
        GameManager.Instance.FixedHalfSecond += NextSprite;
        await Task.Delay(249);
        foreach (Image image in blackout.GetComponentsInChildren<Image>())
        {
            blackoutImages.Add(image);
            image.CrossFadeAlpha(0, 0.5f, false);
        }
    
        // GameManager.Instance.inputReader.ClickEventWithContext += SetCursorPosition;
        
        GameManager.Instance.FixedSecond += PumpCursor;
    }

    private void OnDestroy()
    {
        uiStateObject.TopBarToggle -= ShowHideTopBar;
        uiStateObject.OnFadeIn -= FadeIn;
        uiStateObject.OnFadeOut -= FadeOut;
        uiStateObject.OnPause -= PauseOut;
        uiStateObject.OnResume -= PauseIn;
        uiStateObject.OnDisableCursor -= DisablePointer;
        uiStateObject.TopBarPing -= SetTopBarText;

        GameManager.Instance.inputReader.PadAny -= NavUpdate;
        GameManager.Instance.inputReader.ButtonDown -= NavUpdate;
        GameManager.Instance.inputReader.ClickEvent -= NavUpdateMouse;
        GameManager.Instance.FixedHalfSecond -= NextSprite;
        
        // GameManager.Instance.inputReader.ClickEventWithContext -= SetCursorPosition;

        GameManager.Instance.FixedSecond -= PumpCursor;
    }
    
    public void SetCursorPosition(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            cursor.enabled = true;
            cursor.rectTransform.position = Camera.main.WorldToScreenPoint(Vector3.zero);
            cursor.color = Color.blue;
        }

        if (context.canceled)
        {
            cursor.enabled = true;

            cursor.color = Color.red;
        }
        
    }

    public void SetTopBarText(string text)
    {
        // notificationText.text = text;
        DOTween.To(() => notificationText.text, x => notificationText.text = x, text, 0.5f);
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

    public void PumpCursor()
    {
        cursorPumped = !cursorPumped;
        if (GameManager.Instance.eventSystem.currentSelectedGameObject != null)
        {
            var cursorTransform = cursor.rectTransform;
            cursorTransform.localScale = cursorPumped
                ? GameManager.Instance.eventSystem.currentSelectedGameObject.transform.localScale * 1.25f
                : GameManager.Instance.eventSystem.currentSelectedGameObject.transform.localScale * 1.1f;
        }
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
        DisablePointer();
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
    
    private async void NavUpdate()
    {
        if (!cursor.enabled)
        {
            EnablePointer();
            // GameManager.Instance.eventSystem.SetSelectedGameObject(startingSelection);
        }
        await Task.Delay(1);
        if (!GameManager.Instance.uiStateObject.showFadeOut && GameManager.Instance.eventSystem.currentSelectedGameObject!=null)
        {
            cursorPumped = true;
            var cursorTransform = cursor.rectTransform;
            cursorTransform.position = GameManager.Instance.eventSystem.currentSelectedGameObject.transform.position;
            cursorTransform.sizeDelta = GameManager.Instance.eventSystem.currentSelectedGameObject.GetComponent<RectTransform>()!.sizeDelta;
            cursorTransform.localScale = GameManager.Instance.eventSystem.currentSelectedGameObject.transform.localScale * 1.25f;
        }
    }
    public void DisablePointer()
    {
        cursor.enabled = false;
    }

    private void EnablePointer()
    {
        cursor.enabled = true;
    }
    private void NavUpdateMouse()
    {
        DisablePointer();
    }
    
    
}
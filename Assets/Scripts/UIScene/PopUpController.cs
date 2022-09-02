using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour
{
    public static PopUpController Instance;
    
    public delegate void VoidDelegate();

    private VoidDelegate voidDelegate;

    [SerializeField] private Image background;
    [SerializeField] private RectTransform acceptButton;
    [SerializeField] private RectTransform declineButton;
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField]private TextMeshProUGUI acceptButtonText;
    [SerializeField]private TextMeshProUGUI declineButtonText;
    [SerializeField] private Image raycastBlocker;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
    
    public void Accept()
    {
        Cancel();
        if (voidDelegate != null)
        {
            voidDelegate.DynamicInvoke();
        }
    }
    
    public void Cancel()
    {
        transform.DOScale(0, 0.25f);
        raycastBlocker.raycastTarget = false;
        DOTween.To(()=> raycastBlocker.color.a, x=> raycastBlocker.color = new Color(raycastBlocker.color.r, raycastBlocker.color.g, raycastBlocker.color.b, x), 0, 0.25f);
        // Debug.Log("Cancelled");
    }

    public void ShowPopUp(string message, string yes, string no, VoidDelegate action)
    {
        raycastBlocker.raycastTarget = true;
        DOTween.To(()=> raycastBlocker.color.a, x=> raycastBlocker.color = new Color(raycastBlocker.color.r, raycastBlocker.color.g, raycastBlocker.color.b, x), 0.5f, 0.25f);
        transform.DOKill(true);
        acceptButtonText.text = yes;
        if (no == null||no.Equals(""))
        {
            declineButton.gameObject.SetActive(false);
        }
        else
        {
            declineButtonText.text = no;
            declineButton.gameObject.SetActive(true);
        }
        transform.DOScale(1, 0.25f);
        voidDelegate = action;
        text.text = message;
        text.ForceMeshUpdate();
        background.rectTransform.sizeDelta = new Vector2(text.renderedWidth*1.25f, text.renderedHeight*1.25f);
        var sizeDelta = background.rectTransform.sizeDelta;

        var anchoredPosition = background.rectTransform.anchoredPosition;
        anchoredPosition = new Vector2(anchoredPosition.x, text.rectTransform.anchoredPosition.y);
        background.rectTransform.anchoredPosition = anchoredPosition;
        
        var acceptButtonTransform = acceptButton.transform;
        acceptButtonTransform.localPosition = new Vector2(acceptButtonTransform.localPosition.x, anchoredPosition.y-sizeDelta.y/2);
        
        var declineButtonTransform = declineButton.transform;
        declineButtonTransform.localPosition = new Vector2(declineButtonTransform.localPosition.x, anchoredPosition.y-sizeDelta.y/2);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableMenu : MonoBehaviour
{
    public bool isOpen { get; private set; } = false;
    [SerializeField] private Transform backgroundLeft;
    [SerializeField] private Transform backgroundRight;
    [SerializeField] private Button buttonLeft;
    [SerializeField] private Button buttonRight;
    [SerializeField] private Button[] consumableButtons;
    
    void Start()
    {
    }

    public void OnDestroy()
    {
    }

    void Update()
    {
        backgroundLeft.transform.localPosition = Vector3.Lerp(backgroundLeft.transform.localPosition, new Vector3(isOpen? -1.5f:-5, -0.85f, 0), Time.deltaTime * 10);
        backgroundRight.transform.localPosition = Vector3.Lerp(backgroundRight.transform.localPosition, new Vector3(isOpen? 1.5f:5, -0.85f, 0), Time.deltaTime * 10);

    }

    void FixedUpdate()
    {
        
    }
    
    public void ToggleMenu()
    {
        isOpen = !isOpen;
        foreach (Button button in consumableButtons)
        {
            button.interactable = isOpen;
        }
    }
    public void Back()
    {
        
        buttonLeft.interactable = !buttonLeft.interactable;
        buttonRight.interactable = !buttonRight.interactable;
    }
}

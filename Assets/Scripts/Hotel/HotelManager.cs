using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotelManager : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI buttonText;
    
    [SerializeField] int state = 0;
    [SerializeField] MenuLink[] menuLinks;


    private async void Start()
    {
        await Task.Delay(10);
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        GameManager.Instance.uiStateObject.ShowTopBar();
        GameManager.Instance.uiStateObject.Ping("The Hotel");
        GameManager.Instance.inputReader.PadLeft += DecreaseState;
        GameManager.Instance.inputReader.PadRight += IncreaseState;
        GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void Update()
    {
        for (var i = 0; i < menuLinks.Length; i++)
        {
            menuLinks[i].transform.localPosition = Vector3.Lerp(menuLinks[i].transform.localPosition, new Vector3(i*100-(state*100), 0, 0),Time.deltaTime*10);
        }
    }

    public void EnableButtons()
    {
        button.interactable = true;
    }
    
    public void SetMenuState(int newState)
    {
        state = newState;
    }

    public void IncreaseState()
    {
        state++;
        SoundManager.Instance.PlayUiClick();
        if (state > menuLinks.Length-1)
        {
            state = 0;
        }
        UpdateButtonText();
    }

    public void DecreaseState()
    {
        state--;
        SoundManager.Instance.PlayUiClick();
        if (state < 0)
        {
            state = menuLinks.Length-1;
        }
        UpdateButtonText();
    }
    
    private void UpdateButtonText()
    {
        buttonText.text = menuLinks[state].text;
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
        GameManager.Instance.inputReader.PadLeft -= DecreaseState;
        GameManager.Instance.inputReader.PadRight -= IncreaseState;
    }

    public void Back()
    {
        SoundManager.Instance.PlayUiBack();
        GameManager.Instance.LoadSceneAdditive("MainMenu","Hotel");
    }
    public void Enter()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive(menuLinks[state].scene,"Hotel");
    }
}

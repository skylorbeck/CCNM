using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FightRewardDealer : MonoBehaviour
{
    public EquipmentCardShell previewCard;
    public EquipmentCardShell[] lootCards;
    public bool[] keepState;
    public int curCard = 0;
    public Button[] lootButtons;
    public Button leftButton;
    public Button rightButton;
    public Button moveOnButton;
    public TextMeshProUGUI[] lootText;
    public GameObject CardPrefab;
    public GameObject CardHouse;
    void Start()
    {
        foreach (Button lootButton in lootButtons)
        {
            lootButton.gameObject.SetActive(false);
        }
        lootCards = new EquipmentCardShell[Random.Range(1,4)];
        for (int i = 0; i < lootCards.Length; i++)
        {
            lootCards[i] = Instantiate(CardPrefab, CardHouse.transform).GetComponent<EquipmentCardShell>();
            lootCards[i].InsertItem(GameManager.Instance.lootManager.GetItemCard());
            EquipmentCardShell card = lootCards[i];
            Vector3 localPosition = new Vector3(-(lootCards.Length-1)*0.75f+(i*1.5f), 0, 0);
            var cardTransform = card.transform;
            cardTransform.localPosition = localPosition;
            cardTransform.localScale = new Vector3(0.5f,0.5f,0.5f);
            lootButtons[i].gameObject.SetActive(true);
            lootText[i].text = "Keep";
            keepState[i] = true;
            lootButtons[i].transform.localPosition = new Vector3(-(lootCards.Length-1)*15f+(i*30f), 0, 0);
        }

        previewCard = Instantiate(CardPrefab,CardHouse.transform).GetComponent<EquipmentCardShell>();
        previewCard.transform.localPosition = new Vector3(0,-4,0);
        InsertPreview(0);
        GameManager.Instance.eventSystem.SetSelectedGameObject(moveOnButton.gameObject);
    }

    public void InvertKeepState(int index)
    {
        keepState[index] = !keepState[index];
        lootButtons[index].image.color = keepState[index] ? Color.green : Color.red;
        lootText[index].text = keepState[index] ? "Keep" : "Shred";
    }

    public void NextCard()
    {
        curCard++;
        if (curCard >= lootCards.Length)
        {
            curCard = 0;
        }
        InsertPreview(curCard);
    }

    public void PreviousCard()
    {
        curCard--;
        if (curCard < 0)
        {
            curCard = lootCards.Length - 1;
        }
        InsertPreview(curCard);
    }

    public void InsertPreview(int index)
    {
        previewCard.InsertItem(lootCards[index].EquipmentData);
    }

    public void CheckButtons()
    {
        if (curCard==0)
        {
            leftButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
        }

        if (curCard == lootCards.Length)
        {
            rightButton.interactable = false;
        }
        else
        {
            rightButton.interactable = true;
        }
    }

    public void MoveOn()
    {
        GameManager.Instance.LoadSceneAdditive("MapScreen","FightWon");
        for (var i = 0; i < lootCards.Length; i++)
        {
            if (keepState[i])
            {
                GameManager.Instance.battlefield.player.AddCardToInventory(lootCards[i].EquipmentData);
            }
            else
            {
                //todo shred card
            }
        }
    }
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}

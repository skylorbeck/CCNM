using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardShredder : MonoBehaviour
{
    private ObjectPool<EquipmentCardShell> cardPool;

    public EquipmentCardShell cardPrefab;
    public GameObject handHolder;
    public GameObject[] cardHands;
    public List<EquipmentCardShell>[] cards;
    public Slider slider;
    public Slider handSlider;
    public TMP_Dropdown sortDropdown;
    public float2 cardSpacing = 1.5f;
    public TextMeshProUGUI countText;
    private void Start()
    {
        GameManager.Instance.inputReader.Back += Back;
        // GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        GameManager.Instance.uiStateObject.ShowTopBar();
        GameManager.Instance.uiStateObject.Ping("Card Shredder");
        cards = new List<EquipmentCardShell>[cardHands.Length];

        cardPool = new ObjectPool<EquipmentCardShell>(
            () =>
            {
                EquipmentCardShell card = Instantiate(cardPrefab);
                return card;
            },
            card => { card.gameObject.SetActive(true); },
            card => { card.gameObject.SetActive(false); },
            card => { Destroy(card); },
            true, 10, 100
        );

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = new List<EquipmentCardShell>();
            SetEquipment(i);
        }

        // cards.Sort((card1, card2) =>card1.EquipmentData.quality.CompareTo(card2.EquipmentData.quality) );
        handSlider.maxValue = GameManager.Instance.metaPlayer.playerInventory.Length - 1;
        UpdateHand();

    }

    private void Update()
    {
        for (var i = 0; i < cards.Length; i++)
        {
            for (var j = 0; j < cards[i].Count; j++)
            {
                Transform cardTransform = cards[i][j].transform;
                Vector3 localPosition = cardTransform.localPosition;

                localPosition = Vector3.Lerp(localPosition,
                    new Vector3(cardSpacing.x*(j - slider.value), cardSpacing.y*(i - handSlider.value), 0), Time.deltaTime * 10);
                if (Math.Abs(i - handSlider.value) < 0.1f && Math.Abs(j - slider.value) < 0.1f)
                {
                    cardTransform.localScale = Vector3.Lerp(cardTransform.localScale, Vector3.one, Time.deltaTime * 10);
                    localPosition = new Vector3(localPosition.x, localPosition.y, 0);
                }
                else
                {
                    cardTransform.localScale =
                        Vector3.Lerp(cardTransform.localScale, Vector3.one * 0.5f, Time.deltaTime * 10);
                    localPosition = new Vector3(localPosition.x, localPosition.y, 1);
                }

                cardTransform.localPosition = localPosition;

            }
        } 
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu", "CardShredding");
    }


    public void SetEquipment(int i)
    {
        foreach (EquipmentCardShell equipmentCardShell in cards[i])
        {
            cardPool.Release(equipmentCardShell);
        }

        cards[i].Clear();

        EquipmentList equipmentList = GameManager.Instance.metaPlayer.playerInventory[i];
        for (var j = 0; j < equipmentList.container.Count; j++)
        {
            EquipmentDataContainer container = equipmentList.container[j];
            EquipmentCardShell card = cardPool.Get();
            card.transform.SetParent(cardHands[i].transform);
            card.InsertItem(container);
            cards[i].Add(card);
        }
    }

    public void UpdateHand()
    {
        slider.maxValue = cards[(int)handSlider.value].Count - 1;
    }

    public void UpdateCount()
    {
        int count = 0;
        foreach (List<EquipmentCardShell> list in cards)
        {
            foreach (EquipmentCardShell shell in list)
            {
                if (shell.MarkedForShred)
                {
                    count++;
                }
            }
        }
        countText.text = count.ToString();
    }

    public void ToggleShredMark()
    {
        cards[(int)handSlider.value][(int)slider.value].ToggleShredMark();
        UpdateCount();
    }

    public void ShredAllOfQuality(int quality)
    {
        for (var i = 0; i < cards.Length; i++)
        {
            for (var j = 0; j < cards[i].Count; j++)
            {
                if (cards[i][j].EquipmentData.quality <= (EquipmentDataContainer.Quality)quality - 1)
                {
                    cards[i][j].SetShredMark(true);
                }
            }
        }
        UpdateCount();
    }

    public void ClearAllShredMarks()
    {
        for (var i = 0; i < cards.Length; i++)
        {
            for (var j = 0; j < cards[i].Count; j++)
            {
                cards[i][j].SetShredMark(false);
            }
        }
    }

    public void SetSortMode()
    {
        foreach (List<EquipmentCardShell> list in cards)
        {
            switch (sortDropdown.value)
            {
                default:
                    list.Sort((card1, card2) => Random.Range(-1, 1));
                    break;
                case 1:
                    list.Sort((card1, card2) => card1.EquipmentData.quality.CompareTo(card2.EquipmentData.quality));
                    break;
                case 2:
                    list.Sort((card1, card2) => card1.EquipmentData.quality.CompareTo(card2.EquipmentData.quality)*-1);
                    break;
                case 3:
                    list.Sort((card1, card2) => card1.EquipmentData.level.CompareTo(card2.EquipmentData.level));
                    break;
                case 4:
                    list.Sort((card1, card2) => card1.EquipmentData.level.CompareTo(card2.EquipmentData.level)*-1);
                    break;
            }
        }
    }

    public void InitiateShred()
    {
        //todo confirmation dialogue
        int totalSouls = 0;
        foreach (List<EquipmentCardShell> list in cards)
        {
            foreach (EquipmentCardShell shell in list)
            {
                if (shell.MarkedForShred)
                {
                    totalSouls += GameManager.Instance.metaPlayer.ShredCard(shell.EquipmentData);
                }
            }
        }
        
        TextPopController.Instance.PopPositive("You have gained " + totalSouls + " souls.",Vector3.zero,false);
        
        ClearAllShredMarks();
        for (var i = 0; i < cards.Length; i++)
        {
            SetEquipment(i);
        }
        SetSortMode();
    }
    
    public void HandUp()
    {
        handSlider.value += 1;
        if (handSlider.value > handSlider.maxValue)
        {
            handSlider.value = handSlider.maxValue;
        }
    }
    
    public void HandDown()
    {
        handSlider.value -= 1;
        if (handSlider.value < 0)
        {
            handSlider.value = 0;
        }
    }
    
    public void SliderUp()
    {
        slider.value += 1;
        if (slider.value > slider.maxValue)
        {
            slider.value = slider.maxValue;
        }
    }
    
    public void SliderDown()
    {
        slider.value -= 1;
        if (slider.value < 0)
        {
            slider.value = 0;
        }
    }
}
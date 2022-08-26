using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public TMP_Dropdown sortDropdown;
    public float2 cardSpacing = 1.5f;
    public float selectedX = 0;
    public float selectedY = 0;
    public int selectedIndex = 0;
    public int selectedHand = 0;
    [SerializeField] private bool userIsHolding = false;
    [SerializeField] private bool sticky = false;
    [SerializeField] private float stickiness = 1f;
    public TextMeshProUGUI countText;
    private void Start()
    {
        sticky = PlayerPrefs.GetInt("StickyMenu",0) == 1;

        GameManager.Instance.inputReader.Back += Back;
        GameManager.Instance.inputReader.DragWithContext += OnDrag;
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
        sortDropdown.value = PlayerPrefs.GetInt("EquipmentSortMode",0);
        // handSlider.maxValue = GameManager.Instance.metaPlayer.playerInventory.Length - 1;
    }

    private void OnDrag(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 delta = context.ReadValue<Vector2>();
            selectedX -= delta.x * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
            selectedY -= delta.y * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
        }
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
                    new Vector3(cardSpacing.x*(j - selectedX), cardSpacing.y*(i - selectedY), 0), Time.deltaTime * 10);
                if (Math.Abs(i - selectedY) < 0.1f && Math.Abs(j - selectedX) < 0.1f)
                {
                    cardTransform.localScale = Vector3.Lerp(cardTransform.localScale, Vector3.one, Time.deltaTime * 10);
                    localPosition = new Vector3(localPosition.x, localPosition.y, 0);
                }
                else
                {
                    cardTransform.localScale =
                        Vector3.Lerp(cardTransform.localScale, Vector3.one * 0.5f, Time.deltaTime * 10);
                    localPosition = new Vector3(localPosition.x, localPosition.y, 2);
                }

                cardTransform.localPosition = localPosition;

            }
        }
        float target = (float)(Math.Round(selectedY, MidpointRounding.AwayFromZero));
        if (sticky || !userIsHolding)
        {
            selectedY = Mathf.Lerp(selectedY, target, Time.deltaTime * 5 * stickiness);
        }
                
        selectedY = Mathf.Clamp(selectedY, 0,cards.Length - 1);
                
        selectedHand = Mathf.Abs((int)Math.Round(selectedY, MidpointRounding.AwayFromZero));
        
        
        target = (float)(Math.Round(selectedX, MidpointRounding.AwayFromZero));
        if (sticky || !userIsHolding)
        {
            selectedX = Mathf.Lerp(selectedX, target, Time.deltaTime * 5 * stickiness);
        }
                
        selectedX = Mathf.Clamp(selectedX, 0,cards[selectedHand].Count - 1);
                
        selectedIndex = Mathf.Abs((int)Math.Round(selectedX, MidpointRounding.AwayFromZero));
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
        GameManager.Instance.inputReader.DragWithContext -= OnDrag;
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
        cards[selectedHand][selectedIndex].ToggleShredMark();
        UpdateCount();
    }

    public void ShredAllOfQuality(int quality)
    {
        if (quality==8)
        {
            ShredAllOfLevelOrBelow(GameManager.Instance.metaPlayer.level-1);
            return;
        }
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
    
    public void ShredAllOfLevelOrBelow(int level)
    {
        for (var i = 0; i < cards.Length; i++)
        {
            for (var j = 0; j < cards[i].Count; j++)
            {
                if (cards[i][j].EquipmentData.level <= level)
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
                    //sort by quality then level
                    list.Sort((card1, card2) =>
                    {
                        if (card1.EquipmentData.quality == card2.EquipmentData.quality)
                        {
                            return card1.EquipmentData.level.CompareTo(card2.EquipmentData.level)*-1;
                        }
                        return card1.EquipmentData.quality.CompareTo(card2.EquipmentData.quality)*-1;
                    }
                    );
                    break;
                case 2:
                    list.Sort((card1, card2) => card1.EquipmentData.level.CompareTo(card2.EquipmentData.level)*-1);
                    break;
                case 3:
                    list.Sort((card1, card2) => card1.EquipmentData.gemSlots.CompareTo(card2.EquipmentData.gemSlots)*-1);
                    break;
                case 4:
                    list.Sort((card1, card2) =>
                        {
                            if (card1.EquipmentData.quality == card2.EquipmentData.quality)
                            {
                                return card1.EquipmentData.level.CompareTo(card2.EquipmentData.level)*-1;
                            }
                            return card1.EquipmentData.quality.CompareTo(card2.EquipmentData.quality);
                        }
                    );
                    break;
                case 5:
                    list.Sort((card1, card2) => card1.EquipmentData.level.CompareTo(card2.EquipmentData.level));
                    break;
                case 6:
                    list.Sort((card1, card2) => card1.EquipmentData.gemSlots.CompareTo(card2.EquipmentData.gemSlots));
                    break;
            }
            
        }
        PlayerPrefs.SetInt("EquipmentSortMode",sortDropdown.value);

    }

    public void ConfirmShred()
    {
        PopUpController.Instance.ShowPopUp("Are you sure you want to destroy ALL marked items?","Yes","No",InitiateShred);
    }
    
    public void InitiateShred()
    {
        int[] totalSouls = new int[7];
        foreach (List<EquipmentCardShell> list in cards)
        {
            foreach (EquipmentCardShell shell in list)
            {
                if (shell.MarkedForShred)
                {
                    int2 int2 =GameManager.Instance.metaPlayer.ShredCard(shell.EquipmentData);
                    totalSouls[int2.y]+=int2.x;
                }
            }
        }
        int differentSouls = 0;
        for (var i = 0; i < totalSouls.Length; i++)
        {
            if (totalSouls[i] > 0)
            {
                differentSouls++;
                TextPopController.Instance.PopPositive(
                    "+" +totalSouls[i] + " " + (EquipmentDataContainer.Quality)i + " souls.",
                    Vector3.zero +
                    ((Vector3.down * differentSouls) * 0.5f), true);
            }
        }

        ClearAllShredMarks();
        for (var i = 0; i < cards.Length; i++)
        {
            SetEquipment(i);
        }
        SetSortMode();
        UpdateCount();
    }
    
}
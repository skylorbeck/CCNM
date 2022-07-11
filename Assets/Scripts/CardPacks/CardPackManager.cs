using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CardPackManager : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI packCount;
    [field: SerializeField] private EquipmentCardShell cardPrefab;
    [field: SerializeField] private GameObject cardPackHolder;
    [field: SerializeField] private Transform packSprite;
    [field: SerializeField] private List<EquipmentCardShell> cards;
    [field: SerializeField] private List<EquipmentCardShell> tempCards;
    [field: SerializeField] private Vector3[] cardPositions;
    [field: SerializeField] private bool isPackOpen = false;
    [field: SerializeField] private Button openButton;
    [field: SerializeField] private TextMeshProUGUI openButtonText;
    private CancellationTokenSource cancellationTokenSource;

    private ObjectPool<EquipmentCardShell> cardPool;

    async void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();

        cardPool = new ObjectPool<EquipmentCardShell>(
            () =>
            {
                EquipmentCardShell card = Instantiate(cardPrefab, cardPackHolder.transform);
                return card;
            },
            card =>
            {
                card.gameObject.SetActive(true);
            },
            card =>
            {
                card.gameObject.SetActive(false);
            },
            card => {
                Destroy(card);
            },
            true, 10, 100
        );
        
        await Task.Delay(10);
        GameManager.Instance.inputReader.Back+= Back;
        
        UpdatePackCount();
    }

    private void UpdatePackCount()
    {
        packCount.text = "x" + GameManager.Instance.metaPlayer.cardPacks;
    }

    public void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
        cancellationTokenSource.Cancel();
    }

    void Update()
    {
        for (var i = 0; i < cards.Count; i++)
        {
            Transform cardTransform = cards[i].transform;
            cardTransform.localPosition = Vector3.Lerp(cardTransform.localPosition,cardPositions[i] , Time.deltaTime * 5);
        }

            packSprite.localPosition = Vector3.Lerp(packSprite.localPosition, new Vector3(0, isPackOpen?-2.5f:0, 0), Time.deltaTime * 5);
    }

    void FixedUpdate()
    {
    }

    public async void OpenPack()
    {
        openButton.interactable = false;
        isPackOpen = true;
        GameManager.Instance.metaPlayer.RemoveCardPack(1);
        UpdatePackCount();
        for (int i = 0; i < 5; i++)
        {
            EquipmentCardShell cardShell = cardPool.Get();
            cardShell.InsertItem(GameManager.Instance.lootManager.GetItemCard());
            Transform cardTransform = cardShell.transform;
            cardTransform.localScale = Vector3.one * 0.75f;
            cardTransform.localPosition = Vector3.down*3;
            tempCards.Add(cardShell);
            GameManager.Instance.metaPlayer.AddCardToInventory(cardShell.EquipmentData);
        }
        GameManager.Instance.saveManager.SaveMeta();

        foreach (EquipmentCardShell card in tempCards)
        {
            await Task.Delay(500, cancellationTokenSource.Token);
            cards.Add(card);
        }

        openButton.interactable = true;
    }

    public void OpenAcceptPack()
    {
        if (isPackOpen)
        {
            cancellationTokenSource.Cancel();

            isPackOpen = false;
            
            foreach (EquipmentCardShell card in tempCards)
            {
                cardPool.Release(card);
            }
            cards.Clear();
            tempCards.Clear();
            openButtonText.text = "Open";
            cancellationTokenSource = new CancellationTokenSource();
        }
        else
        {
            if (GameManager.Instance.metaPlayer.cardPacks>0)
            {
                openButtonText.text = "Accept";
                OpenPack();
            }
            else
            {
                TextPopController.Instance.PopNegative("You don't have",new Vector3(0,0.5f,0), true);
                TextPopController.Instance.PopNegative("any card packs!",new Vector3(0,0,0), true);
            }
        }
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("Hotel","CardPacks");
    }
}

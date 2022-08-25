using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardPackManager : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI packCount;
    [field: SerializeField] private EquipmentCardShell fullCardPrefab;
    [field: SerializeField] private EquipmentCardShell fullCard;
    [field: SerializeField] private MicroCard cardPrefab;
    [field: SerializeField] private GameObject cardPackHolder;
    [field: SerializeField] private SafeAnimator safeAnimator;
    [field: SerializeField] private List<MicroCard> tempCards;
    [field: SerializeField] private Vector3[] cardPositions;
    [field: SerializeField] private bool isPackOpen = false;
    [field: SerializeField] private Button openButton;
    [field: SerializeField] private TextMeshProUGUI openButtonText;
    private CancellationTokenSource cancellationTokenSource;
    [field: SerializeField] private ParticleSystem particleSystem;
    [field: SerializeField] private ParticleSystem bigParticleSystem;

    private ObjectPool<MicroCard> cardPool;


    async void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();

        cardPool = new ObjectPool<MicroCard>(
            () =>
            {
                MicroCard card = Instantiate(cardPrefab, cardPackHolder.transform);
                return card;
            },
            card => { card.gameObject.SetActive(true); },
            card => { card.gameObject.SetActive(false); },
            card => { Destroy(card); },
            true, 10, 100
        );
        fullCard = Instantiate(fullCardPrefab, cardPackHolder.transform);
        fullCard.transform.localPosition = new Vector3(0, -15, 0);
        // fullCard.gameObject.SetActive(false);
        await Task.Delay(10);
        GameManager.Instance.inputReader.Back += Back;

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
    }

    void FixedUpdate()
    {
    }

    public async void OpenPack()
    {
        GameManager.Instance.inputReader.Back -= Back;

        openButton.interactable = false;
        GameManager.Instance.metaPlayer.RemoveCardPack(1);
        UpdatePackCount();

        //the small ones
        for (int i = 0; i < 5; i++)
        {
            MicroCard cardShell = cardPool.Get();
            cardShell.InsertItem(GameManager.Instance.lootManager.GetItemCard());
            Transform cardTransform = cardShell.transform;
            // cardTransform.localScale = Vector3.one * 0.75f;
            cardTransform.localPosition = Vector3.down * 10;
            tempCards.Add(cardShell);
            GameManager.Instance.metaPlayer.AddCardToInventory(cardShell.EquipmentData);
        }

        GameManager.Instance.metaStats.safesOpened++;

        GameManager.Instance.saveManager.SaveMeta();
        GameManager.Instance.inputReader.Back += Back;
        
        tempCards.Sort((card1, card2) =>
        {
            if (card1.EquipmentData.quality == card2.EquipmentData.quality)
            {
                return card1.EquipmentData.level.CompareTo(card2.EquipmentData.level);
            }

            return card1.EquipmentData.quality.CompareTo(card2.EquipmentData.quality);
        });

//the one big one
        fullCard.InsertItem(tempCards[tempCards.Count - 1].EquipmentData);
        cardPool.Release(tempCards[tempCards.Count - 1]);
        tempCards.RemoveAt(tempCards.Count - 1);

        Transform fullCardTransform = fullCard.transform;
        // fullCardTransform.localScale = Vector3.one * 0.75f;
        fullCardTransform.localPosition = Vector3.down * 10;

        await safeAnimator.Open();

        isPackOpen = true;
        safeAnimator.transform.DOLocalMove(new Vector3(0,-4,0),0.25f).SetEase(Ease.OutSine);

        for (var i = 0; i < tempCards.Count; i++)
        {
            particleSystem.Play();
            MicroCard card = tempCards[i];
            card.transform.DOMove(cardPositions[i], 0.5f);
            SoundManager.Instance.PlayEffect("LootBoxOpen"+(i+1),0.75f+(i*0.05f),true);
            await Task.Delay(500, cancellationTokenSource.Token);
        }
        SoundManager.Instance.PlayEffect("LootSafeOpen",1f,true);

        bigParticleSystem.Play();
        await Task.Delay(250, cancellationTokenSource.Token);
        fullCard.gameObject.SetActive(true);
        fullCard.transform.DOMove(cardPositions[4], 0.5f).OnComplete(() => { openButton.interactable = true; });
        // SoundManager.Instance.PlayEffect("LootBoxOpen5",1f,true);

    }

    public void OpenAcceptPack()
    {
        

        if (isPackOpen)
        {
            cancellationTokenSource.Cancel();

            isPackOpen = false;
            openButton.interactable = false;
            foreach (MicroCard card in tempCards)
            {
                card.transform.DOMove(Vector3.up * 10, 0.5f).OnComplete(() =>
                {
                    cardPool.Release(card);
                });
                // cardPool.Release(card);
            }

            fullCard.transform.DOMove(Vector3.up * 10, 0.5f).OnComplete(() =>
            {
                fullCard.gameObject.SetActive(false);
                openButton.interactable = true;
            });
            // fullCard.gameObject.SetActive(false);
            // cards.Clear();
            tempCards.Clear();
            openButtonText.text = "Open";
            cancellationTokenSource = new CancellationTokenSource();
            safeAnimator.Close();
            // MusicManager.Instance.StopTrack();
            /*if (GameManager.Instance.metaPlayer.totalEquipment>GameManager.Instance.metaPlayer.maximumEquipmentSlots-5)
            {
                openButton.interactable = false;
            }*/
        }
        else
        {
            if (GameManager.Instance.metaPlayer.totalEquipment > GameManager.Instance.metaPlayer.maximumEquipmentSlots - 5)
            {
                TextPopController.Instance.PopNegative("Your inventory is full!",Vector3.zero,false);
                return;
            }
            
            if (GameManager.Instance.metaPlayer.cardPacks > 0)
            {
                openButtonText.text = "Accept";
                OpenPack();
            }
            else
            {
                TextPopController.Instance.PopNegative("You don't have", new Vector3(0, 0.5f, 0), true);
                TextPopController.Instance.PopNegative("any card packs!", new Vector3(0, 0, 0), true);
            }
        }
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu", "CardPacks");
    }
}
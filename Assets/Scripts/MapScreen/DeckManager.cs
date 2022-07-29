using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private float yDistance = 3f;
    [SerializeField] private float currentY = 0f;
    [SerializeField] private float xDistance = 1.5f;
    [field:SerializeField] public int selected{ get; private set; }
    [SerializeField] private DeckRegistry deckRegistry;
    [SerializeField] private DeckPreviewer deckPrefab;
    [SerializeField] private DeckPreviewer CurrentDeck;
    [SerializeField] private List<DeckPreviewer> decks;
    [SerializeField] TextMeshProUGUI totalCardsText;

    void Start()
    {
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Drag -= OnDrag;
    }

    public void InitalizeDecks()
    {
        GameManager.Instance.inputReader.Drag += OnDrag;

        for (int i = 0; i < deckRegistry.deckCount; i++)
        {
            DeckPreviewer deck = Instantiate(deckPrefab, transform);
            deck.InsertDeck(deckRegistry.GetDeck(i));
            decks.Add(deck);
        }
    }

    void Update()
    {
        float target;

        target = (float)(Math.Round(currentY / yDistance, MidpointRounding.AwayFromZero) * yDistance);
        currentY = Mathf.Clamp(Mathf.Lerp(currentY, target, Time.deltaTime * yDistance),
            yDistance - decks.Count * yDistance, 0);
        int newSelected = Math.Abs((int)Math.Round(currentY / yDistance, MidpointRounding.AwayFromZero));
        if (newSelected != selected)
        {
            selected = newSelected;
            UpdateDecks();
        }
        for (var i = 0; i < decks.Count; i++)
        {
            DeckPreviewer deck = decks[i];
            Transform deckTransform = deck.transform;
            Vector3 deckPosition = deckTransform.position;
            deckPosition.y = (i * yDistance) + currentY;
            deckPosition.x = Mathf.Cos(Mathf.Abs(deckPosition.y * 0.5f)) * xDistance;
            deckTransform.localPosition = deckPosition;
        }
        
    }

    private void UpdateDecks()
    {
       CurrentDeck.InsertDeck(GetDeck());
           totalCardsText.text = "0/" + GetDeck().BossAt;
    }

    void FixedUpdate()
    {

    }

    public void OnDrag(Vector2 delta)
    {
        currentY += delta.y * Time.deltaTime;
    }

    public void SelectDeck()
    {
        GameManager.Instance.battlefield.InsertDeck(GetDeck());
    }
    
    public DeckObject GetDeck()
    {
        return deckRegistry.GetDeck(selected);
    }
}
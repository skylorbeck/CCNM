using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private float yDistance = 3f;
    [SerializeField] private float xDistance = 1.5f;
    [SerializeField] private float currentX = 0f;
    [field:SerializeField] public int selected{ get; private set; }
    [SerializeField] private DeckRegistry deckRegistry;
    [SerializeField] private DeckPreviewer deckPrefab;
    [SerializeField] private List<DeckPreviewer> decks;

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

        target = (float)(Math.Round(currentX / xDistance, MidpointRounding.AwayFromZero) * xDistance);
        currentX = Mathf.Clamp(Mathf.Lerp(currentX, target, Time.deltaTime * xDistance),
            xDistance - decks.Count * xDistance, 0);
        int newSelected = Math.Abs((int)Math.Round(currentX / xDistance, MidpointRounding.AwayFromZero));
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
            deckTransform.localScale = Vector3.Lerp(Vector3.one,new Vector3(.5f, .5f, .5f), 
                Mathf.Abs(deckPosition.x*0.5f));
            deckPosition.y = yDistance+(i==selected?0:0.5f);
            deckPosition.x =(currentX+xDistance*i)-(i>selected+1?0.5f:0)+(i<selected-1?0.5f:0);
            deckPosition.z = i==selected ? -1 :Math.Abs(selected-i);
            deckTransform.localPosition = Vector3.Lerp(deckTransform.localPosition, deckPosition, Time.deltaTime * 5f);
            
        }
        
    }

    private void UpdateDecks()
    {
        GameManager.Instance.battlefield.InsertDeck(GetDeck());
        mapManager.InsertDeck();
    }

    void FixedUpdate()
    {

    }

    public void OnDrag(Vector2 delta)
    {
        currentX += delta.x * Time.deltaTime*PlayerPrefs.GetFloat("TouchSensitivity",1f);
    }

    public void SelectDeck()
    {
        UpdateDecks();
    }
    
    public DeckObject GetDeck()
    {
        return deckRegistry.GetDeck(selected);
    }

    public void SetSelectedDeck(DeckObject battlefieldDeck)
    {
        selected = deckRegistry.GetDeckIndex(battlefieldDeck.name);
        currentX = -selected * xDistance;
    }
}
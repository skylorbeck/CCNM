using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckPreviewer : MonoBehaviour
{
    [SerializeField] private DeckObject deck;
    [SerializeField] private SpriteRenderer cardRenderer;
    [SerializeField] private SpriteRenderer deckPreview;
    [SerializeField] private TextMeshPro deckTitle;
    void Start()
    {
        
    }

    public void InsertDeck(DeckObject deck)
    {
        this.deck = deck;
        cardRenderer.sprite = deck.deckBack;
        deckPreview.sprite = deck.icon;
        deckTitle.text = deck.name;
        name = deck.name;
    }
    
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}

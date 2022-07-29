using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckPreviewer : MonoBehaviour
{
    [SerializeField] private DeckObject deck;
    [SerializeField] private SpriteRenderer[] cardRenderers;
    [SerializeField] private SpriteRenderer deckPreview;
    [SerializeField] private TextMeshPro deckTitle;
    void Start()
    {
        
    }

    public void InsertDeck(DeckObject deck)
    {
        this.deck = deck;
        cardRenderers[0].color = deck.colors[0];
        cardRenderers[1].color = deck.colors[1];
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

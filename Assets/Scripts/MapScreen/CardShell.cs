using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CardShell : MonoBehaviour
{
    [SerializeField] private Sprite bossSprite;
    [SerializeField] private Sprite miniBossSprite;
    [SerializeField] private Sprite eventSprite;
    [SerializeField] private Sprite shopSprite;
    [field: SerializeField] public CardObject card { get; private set; }
    [SerializeField] private TextMeshPro text;
    [field:SerializeField] public SpriteRenderer cardSprite { get; private set; }
    [field:SerializeField] public SpriteRenderer enemySprite { get; private set; }
    [field: SerializeField] public bool isFaceUp { get; private set; }
    [field: SerializeField] public bool textAbove { get; private set; }
    [field: SerializeField] public bool hasBrain { get; private set; } = false;
    
    private Vector3 TargetPosition = Vector3.zero;
    
    [SerializeField] private float speed = 25f;
    public void InsertCard(MapCard cardObject, bool above)
    {
        TargetPosition = transform.localPosition;
        card = cardObject;
        textAbove = above;
        text.text = card.cardTitle;
        text.transform.localPosition = new Vector3(0, textAbove?0.75f:-0.75f, 0);
        enemySprite.sprite = card.icon;
        switch (cardObject.mapCardType)
        {
            case MapCard.MapCardType.Shop:
                cardSprite.sprite = shopSprite;
                break;
            case MapCard.MapCardType.Boss:
                cardSprite.sprite = bossSprite;
                break;
            case MapCard.MapCardType.MiniBoss:
                cardSprite.sprite = miniBossSprite;
                break;
            case MapCard.MapCardType.Event:
                cardSprite.sprite = eventSprite;
                break;
        }
        transform.localPosition = new Vector3(0, -3.5f, 0);
        hasBrain = true;
    }

    public void Update()
    {
        if (isFaceUp && Vector3.Distance(transform.localPosition,TargetPosition)>0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetPosition, speed * Time.deltaTime);
        }
    }

    public void Flip()
    {
        isFaceUp = !isFaceUp;
        text.enabled = isFaceUp;
    }
}
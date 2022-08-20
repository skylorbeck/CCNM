using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
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
    [field: SerializeField] public SpriteRenderer cardSpriteBack { get; private set; }
    [field: SerializeField] public SpriteRenderer cardSpriteFront { get; private set; }
    [field: SerializeField] public SpriteRenderer enemySprite { get; private set; }
    [field: SerializeField] public SpriteRenderer typeSprite { get; private set; }

    [field: SerializeField] public bool isFaceUp { get; private set; }

    // [field: SerializeField] public bool textAbove { get; private set; }
    [field: SerializeField] public bool hasBrain { get; private set; } = false;

    private Vector3 TargetPosition = Vector3.zero;

    [SerializeField] private float speed = 25f;

    public void InsertCard(MapCard cardObject, bool above)
    {
        // TargetPosition = transform.localPosition;
        card = cardObject;
        // textAbove = above;
        // text.text = card.cardTitle;
        DOTween.To(() => text.text, x => text.text = x, card.cardTitle, 0.5f);

        // text.transform.localPosition = new Vector3(0, textAbove?0.75f:-0.75f, 0);
        enemySprite.sprite = card.icon;
        switch (cardObject.mapCardType)
        {
            case MapCard.MapCardType.Shop:
                typeSprite.sprite = shopSprite;
                break;
            case MapCard.MapCardType.Boss:
                enemySprite.transform.localPosition += new Vector3(0, 0.5f, 0);
                typeSprite.sprite = bossSprite;
                break;
            case MapCard.MapCardType.MiniBoss:
                enemySprite.transform.localPosition += new Vector3(0, 0.5f, 0);
                typeSprite.sprite = miniBossSprite;
                break;
            case MapCard.MapCardType.Event:
                typeSprite.sprite = eventSprite;
                break;
        }
        cardSpriteBack.color = GameManager.Instance.battlefield.deck.colors[1];
        cardSpriteFront.color = GameManager.Instance.battlefield.deck.colors[0];

        transform.localPosition = new Vector3(0, -2.5f, 0);
        hasBrain = true;
    }

    public void Update()
    {
        /*if (isFaceUp && Vector3.Distance(transform.localPosition, TargetPosition) > 0.1f)
        {
            transform.localPosition =
                Vector3.MoveTowards(transform.localPosition, TargetPosition, speed * Time.deltaTime);
        }*/
    }

    public void SetTargetPosition(Vector3 position)
    {
        TargetPosition = position;
    }

    public void Flip()
    {
        isFaceUp = !isFaceUp;
        text.enabled = isFaceUp;
        if (isFaceUp)
        {
            this.transform.DOLocalJump(TargetPosition, 0.5f, 1, 0.5f);
        }
    }
}
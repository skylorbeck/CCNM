using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CardShell : MonoBehaviour
{

    [field: SerializeField] public CardObject card { get; private set; }
    [SerializeField] private TextMeshPro text;
    [SerializeField] private SpriteRenderer sprite;
    [field: SerializeField] public bool isFaceUp { get; private set; }
    [field: SerializeField] public bool textAbove { get; private set; }
    
    private Vector3 TargetPosition = Vector3.zero;
    
    [SerializeField] private float speed = 25f;
    public void InsertCard(CardObject cardObject, bool above)
    {
        TargetPosition = transform.localPosition;
        card = cardObject;
        textAbove = above;
        text.text = card.name;
        text.transform.localPosition = new Vector3(0, textAbove?0.75f:-0.75f, 0);
        sprite.sprite = card.icon;
        transform.localPosition = new Vector3(0, -3.5f, 0);
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
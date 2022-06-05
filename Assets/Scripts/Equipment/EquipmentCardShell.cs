using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentCardShell : MonoBehaviour, IPointerClickHandler
{

    [field: Header("Front")]
    [field: SerializeField]
    public ItemCard itemCore { get; private set; }

    [field: SerializeField] public SpriteRenderer itemSprite { get; private set; }
    [field: SerializeField] public TextMeshPro title { get; private set; }
    [field: SerializeField] public TextMeshPro stat1 { get; private set; }
    [field: SerializeField] public TextMeshPro stat2 { get; private set; }
    [field: SerializeField] public TextMeshPro stat3 { get; private set; }
    [field: SerializeField] public TextMeshPro stat4 { get; private set; }
    [field: SerializeField] public TextMeshPro stat5 { get; private set; }

    [field: Header("Back")]
    [field: SerializeField]
    public SpriteRenderer abilitySprite { get; private set; }

    [field: SerializeField] public TextMeshPro abilityTitle { get; private set; }
    [field: SerializeField] public TextMeshPro abilityDesc { get; private set; }
    [field: SerializeField] public TextMeshPro abilityDesc2 { get; private set; }

    [field: Header("Misc")]
    [field: SerializeField]
    public SpriteRenderer shadowSprite { get; private set; }

    [field: SerializeField] public Transform innerTransform { get; private set; }
    [field: SerializeField] public int rotateSpeed { get; private set; } = 5;
    [field: SerializeField] public bool MouseOver { get; private set; } = false;
    [field: SerializeField] public bool Selected { get; private set; } = false;

    public void Update()
    {
        if (Selected)
        {
            if (MouseOver)
            {
                innerTransform.localRotation = Quaternion.Lerp(innerTransform.localRotation,
                    Quaternion.Euler(0, 180, 0),
                    Time.deltaTime * rotateSpeed);
                shadowSprite.transform.localPosition = Vector3.Lerp(shadowSprite.transform.localPosition,
                    new Vector3(-0.2f, -0.15f, 0), Time.deltaTime * rotateSpeed);
            }
            else
            {
                innerTransform.localRotation = Quaternion.Lerp(innerTransform.localRotation, Quaternion.Euler(0, 0, 0),
                    Time.deltaTime * rotateSpeed);
                shadowSprite.transform.localPosition = Vector3.Lerp(shadowSprite.transform.localPosition,
                    new Vector3(0.2f, -0.15f, 0), Time.deltaTime * rotateSpeed);
            }
        }
    }

    public void InsertItem(ItemCard item)
    {
        itemCore = item;
        itemSprite.sprite = item.icon;
        title.text = item.cardTitle;
        stat1.text = item.stat1 != ItemCard.Stats.None
            ? item.stat1.ToString() + " " + item.stat1Value.ToString("%")
            : "";
        stat2.text = item.stat2 != ItemCard.Stats.None
            ? item.stat2.ToString() + " " + item.stat2Value.ToString("%")
            : "";
        stat3.text = item.stat3 != ItemCard.Stats.None
            ? item.stat3.ToString() + " " + item.stat3Value.ToString("%")
            : "";
        stat4.text = item.stat4 != ItemCard.Stats.None
            ? item.stat4.ToString() + " " + item.stat4Value.ToString("%")
            : "";
        stat5.text = item.stat5 != ItemCard.Stats.None
            ? item.stat5.ToString() + " " + item.stat5Value.ToString("%")
            : "";
        if (item.ability != null)
        {
            abilitySprite.sprite = item.ability.icon;
            abilityTitle.text = item.ability.title;
            abilityDesc.text = item.ability.descriptionA;
            abilityDesc2.text = item.ability.descriptionB;
        }
        else
        {
            abilitySprite.sprite = null;
            abilityTitle.text = "";
            abilityDesc.text = "";
            abilityDesc2.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Selected)
        {
            MouseOver = !MouseOver;;
        }
    }
}
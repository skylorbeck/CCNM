using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentCardShell : MonoBehaviour, IPointerClickHandler
{
    //this class should be only used for data display
    [field: Header("Front")]
    [field: SerializeField] public EquipmentDataContainer EquipmentData { get; private set; }
    [field: SerializeField] public SpriteRenderer itemSprite { get; private set; }
    [field: SerializeField] public TextMeshPro title { get; private set; }
    [field: SerializeField] public TextMeshPro stat1text { get; private set; }
    [field: SerializeField] public TextMeshPro stat2text { get; private set; }
    [field: SerializeField] public TextMeshPro stat3text { get; private set; }
    [field: SerializeField] public TextMeshPro stat4text { get; private set; }
    [field: SerializeField] public TextMeshPro stat5text { get; private set; }

    [field: Header("Back")]
    [field: SerializeField]
    public SpriteRenderer abilitySprite { get; private set; }

    [field: SerializeField] public TextMeshPro abilityTitle { get; private set; }
    [field: SerializeField] public TextMeshPro abilityDesc { get; private set; }
    [field: SerializeField] public TextMeshPro abilityDesc2 { get; private set; }
    [field: Header("Misc")]
    [field: SerializeField]
    public SpriteRenderer shadowSprite { get; private set; }
    [field: SerializeField] public SpriteRenderer arrowSprite { get; private set; } 
    [field: SerializeField] public SpriteRenderer cardSprite { get; private set; }
    [field: SerializeField] public SpriteRenderer cardBackSprite { get; private set; }
    [field: SerializeField] public TextMeshPro qualityText { get; private set; }
    [field: SerializeField] public Transform innerTransform { get; private set; }
    [field: SerializeField] public int rotateSpeed { get; private set; } = 5;
    [field: SerializeField] public bool MouseOver { get; private set; } = false;
    [field: SerializeField] public bool Selected { get; set; } = false;



    public void Update()
    {
        if (Selected && MouseOver)
        {
            innerTransform.localRotation = Quaternion.Lerp(innerTransform.localRotation,
                Quaternion.Euler(0, 180, 0),
                Time.deltaTime * rotateSpeed);
            shadowSprite.transform.localPosition = Vector3.Lerp(shadowSprite.transform.localPosition,
                new Vector3(-0.2f, -0.15f, 0.05f), Time.deltaTime * rotateSpeed);
        }
        else
        {
            MouseOver = false;
            innerTransform.localRotation = Quaternion.Lerp(innerTransform.localRotation, Quaternion.Euler(0, 0, 0),
                Time.deltaTime * rotateSpeed);
            shadowSprite.transform.localPosition = Vector3.Lerp(shadowSprite.transform.localPosition,
                new Vector3(0.2f, -0.15f, 0.05f), Time.deltaTime * rotateSpeed);
        }
    }

    public void InsertItem(EquipmentDataContainer item)
    {
        EquipmentData = item;
        itemSprite.sprite = item.itemCore.icon;
        title.text = item.itemCore.cardTitle;
        qualityText.text = item.quality.ToString();
        stat1text.text = item.stat1 != EquipmentDataContainer.Stats.None
            ? "+" + item.stat1Value+" " + item.stat1.ToString()
            : "";
        stat2text.text = item.stat2 != EquipmentDataContainer.Stats.None
            ? "+" + item.stat2Value+" " + item.stat2.ToString()
            : "";
        stat3text.text = item.stat3 != EquipmentDataContainer.Stats.None
            ? "+" + item.stat3Value+" " + item.stat3.ToString()
            : "";
        stat4text.text = item.stat4 != EquipmentDataContainer.Stats.None
            ? "+" + item.stat4Value+" " + item.stat4.ToString()
            : "";
        stat5text.text = item.stat5 != EquipmentDataContainer.Stats.None
            ? "+" + item.stat5Value+" " + item.stat5.ToString()
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
            arrowSprite.enabled = false;
            abilitySprite.sprite = null;
            abilityTitle.text = "";
            abilityDesc.text = "";
            abilityDesc2.text = "";
        }

        switch (item.quality)
        {
            case EquipmentDataContainer.Quality.Noteworthy:
                cardSprite.color = Color.green;
                cardBackSprite.color = Color.green;
                break;
            case EquipmentDataContainer.Quality.Remarkable:
                cardSprite.color = Color.blue;
                cardBackSprite.color = Color.blue;
                break;
            case EquipmentDataContainer.Quality.Choice:
                cardSprite.color = Color.Lerp(Color.blue, Color.red, 0.4f);
                cardBackSprite.color = Color.Lerp(Color.blue, Color.red, 0.4f);
                break;
            case EquipmentDataContainer.Quality.Signature:
                cardSprite.color = Color.Lerp(Color.yellow, Color.red, 0.4f);
                cardBackSprite.color = Color.Lerp(Color.yellow, Color.red, 0.4f);
                break;
            case EquipmentDataContainer.Quality.Fabled:
                cardSprite.color = Color.yellow;
                cardBackSprite.color = Color.yellow;
                break;
            case EquipmentDataContainer.Quality.Curator:
                cardSprite.color = Color.red;
                cardBackSprite.color = Color.red;
                break;
          
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
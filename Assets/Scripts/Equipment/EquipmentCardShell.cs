using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentCardShell : MonoBehaviour, IPointerClickHandler
{
    //this class should be only used for data display
    [field: Header("Front")]
    [field: SerializeField] public EquipmentDataContainer EquipmentData { get; private set; }
    [field: SerializeField] public SpriteRenderer itemSprite { get; private set; }
    [field: SerializeField] public TextMeshPro title { get; private set; }
    [field: SerializeField] public TextMeshPro[] statText { get; private set; }

    [field: Header("Back")]
    [field: SerializeField]
    public SpriteRenderer abilitySprite { get; private set; }

    [field: SerializeField] public TextMeshPro abilityTitle { get; private set; }
    [field: SerializeField] public TextMeshPro abilityDesc { get; private set; }
    [field: SerializeField] public TextMeshPro abilityDesc2 { get; private set; }
    [field: Header("Misc")]

    [field: SerializeField] public SpriteRenderer arrowSprite { get; private set; } 
    [field: SerializeField] public SpriteRenderer cardSprite { get; private set; }
    [field: SerializeField] public SpriteRenderer cardBackSprite { get; private set; }
    [field: SerializeField] public TextMeshPro qualityText { get; private set; }
    [field: SerializeField] public TextMeshPro levelText { get; private set; }
    [field: SerializeField] public Transform innerTransform { get; private set; }
    [field: SerializeField] public int rotateSpeed { get; private set; } = 5;
    [field: SerializeField] public bool MouseOver { get; private set; } = false;
    [field: SerializeField] public bool Selected { get; set; } = false;


    public void Start()
    {
        GameManager.Instance.inputReader.ButtonLeft += FlipCard;
    }

    public void OnDestroy()
    {
        GameManager.Instance.inputReader.ButtonLeft -= FlipCard;

    }

    public void Update()
    {
        if (Selected && MouseOver)
        {
            innerTransform.localRotation = Quaternion.Lerp(innerTransform.localRotation,
                Quaternion.Euler(0, 180, 0),
                Time.deltaTime * rotateSpeed);
            
        }
        else
        {
            MouseOver = false;
            innerTransform.localRotation = Quaternion.Lerp(innerTransform.localRotation, Quaternion.Euler(0, 0, 0),
                Time.deltaTime * rotateSpeed);
            
        }
    }

    public void InsertItem(EquipmentDataContainer item)
    {
        EquipmentData = item;
        itemSprite.sprite = item.itemCore.icon;
        title.text = item.itemCore.cardTitle;
        qualityText.text = item.quality.ToString();
        qualityText.color = GameManager.Instance.colors[(int)item.quality];
        levelText.text = "lv."+ item.level;
        levelText.color = GameManager.Instance.colors[(int)item.quality];
        for (var i = 0; i < statText.Length; i++)
        {
            switch (item.stats[i])
            {
                case EquipmentDataContainer.Stats.None:
                    statText[i].text = "";
                    break;
                case EquipmentDataContainer.Stats.Damage:
                    statText[i].text = "+" + item.statValue[i] + " Damage";
                    break;
                case EquipmentDataContainer.Stats.Shield:
                    statText[i].text = "+" + item.statValue[i] + " Shield";
                    break;
                case EquipmentDataContainer.Stats.CriticalChance:
                    statText[i].text = "+" + item.statValue[i] + " Crit %";
                    break;
                case EquipmentDataContainer.Stats.CriticalDamage:
                    statText[i].text = "+" + item.statValue[i] + " Crit Dmg";
                    break;
                case EquipmentDataContainer.Stats.Health:
                    statText[i].text = "+" + item.statValue[i] + " Health";
                    break;
            }
        }
        
        if (item.ability != null)
        {
            arrowSprite.enabled = true;
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

        cardSprite.color= GameManager.Instance.colors[(int)item.quality];
        cardBackSprite.color = GameManager.Instance.colors[(int)item.quality];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FlipCard();
    }

    public void FlipCard()
    {
        if (Selected)
        {
            MouseOver = !MouseOver;;
        }
    }
   
}
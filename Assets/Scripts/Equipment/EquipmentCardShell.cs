using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentCardShell : MonoBehaviour
{
    
    [field: Header("Front")]

    [field:SerializeField] public ItemCard itemCore { get; private set; }
    [field:SerializeField] public SpriteRenderer itemSprite { get; private set; }
    [field:SerializeField] public TextMeshPro title { get; private set; }
    [field:SerializeField] public TextMeshPro stat1 { get; private set; }
    [field:SerializeField] public TextMeshPro stat2 { get; private set; }
    [field:SerializeField] public TextMeshPro stat3 { get; private set; }
    [field:SerializeField] public TextMeshPro stat4 { get; private set; }
    [field:SerializeField] public TextMeshPro stat5 { get; private set; }
    [field: Header("Back")]
    [field:SerializeField] public SpriteRenderer abilitySprite { get; private set; }
    [field:SerializeField] public TextMeshPro abilityTitle { get; private set; }
    [field:SerializeField] public TextMeshPro abilityDesc { get; private set; }
    [field:SerializeField] public TextMeshPro abilityDesc2 { get; private set; }

    public void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 50);
    }
    
    public void InsertItem(ItemCard item)
    {
        itemCore = item;
        itemSprite.sprite = item.icon;
        title.text = item.cardTitle;
        stat1.text = item.stat1.ToString()+ " " + item.stat1Value.ToString("%");
        stat2.text = item.stat2.ToString()+ " " + item.stat2Value.ToString("%");
        stat3.text = item.stat3.ToString()+ " " + item.stat3Value.ToString("%");
        stat4.text = item.stat4.ToString()+ " " + item.stat4Value.ToString("%");
        stat5.text = item.stat5.ToString()+ " " + item.stat5Value.ToString("%");
        abilitySprite.sprite = item.ability.icon;
        abilityTitle.text = item.ability.title;
        abilityDesc.text = item.ability.descriptionA;
        abilityDesc2.text = item.ability.descriptionB;
    }
}

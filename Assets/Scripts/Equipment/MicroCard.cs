using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MicroCard : MonoBehaviour
{
    [field: SerializeField] public EquipmentDataContainer EquipmentData { get; private set; }
    [field: SerializeField] public SpriteRenderer itemSprite { get; private set; }
    [field: SerializeField] public SpriteRenderer background { get; private set; }
    [field: SerializeField] public SpriteRenderer highlight { get; private set; }
    [field: SerializeField] public SpriteRenderer shredMark { get; private set; }
    [field: SerializeField] public SpriteRenderer[] gemslots { get; private set; }
    [field: SerializeField] public TextMeshPro levelText { get; private set; }

    public void InsertItem(EquipmentDataContainer item)
    {
        EquipmentData = item;
        itemSprite.sprite = item.itemCore.icon;
        // background.color = GameManager.Instance.colors[(int)item.quality];
        DOTween.To(() => background.color, x => background.color = x,  GameManager.Instance.colors[(int)item.quality], 0.5f);

        highlight.gameObject.SetActive(false);
        shredMark.gameObject.SetActive(false);
        // levelText.text ="Lv."+ item.level.ToString();
        DOTween.To(() => levelText.text, x => levelText.text = x, "Lv."+ item.level.ToString(), 0.5f);

        // levelText.color = GameManager.Instance.colors[(int)item.quality];
        DOTween.To(() => levelText.color, x => levelText.color = x,  GameManager.Instance.colors[(int)item.quality], 0.5f);

        for (var i = 0; i < gemslots.Length; i++)
        {
            gemslots[i].color = Color.clear;
        }
        
        for (int i = 0; i < item.gemSlots; i++)
        {
            AbilityObject ability = item.GetAbility(i)?.GetAbility();
            Color color = Color.clear;
            if (ability != null)
            {
                if (item.lockedSlots[i])
                {
                    color = Color.white;
                }
                else
                {
                    color = Color.gray;
                }
                /*switch (ability.slotType)
                    {
                        case EquipmentDataContainer.SlotType.Offense:
                            color = Color.red;
                            break;
                        case EquipmentDataContainer.SlotType.Defense:
                            color = Color.blue;
                            break;
                        case EquipmentDataContainer.SlotType.Utility:
                            color = Color.yellow;
                            break;
                        default:
                           
                            break;
                    }*/
            }
            else
            {
                if (item.lockedSlots[i])
                {
                    color = Color.white;
                }
                else
                {
                    color = Color.gray;
                }
            }
            
            gemslots[i].color = color;

        }
    }

    public void SetHighlighted(bool value)
    {
        highlight.gameObject.SetActive(value);
        highlight.color = GameManager.Instance.colors[(int)this.EquipmentData.quality];
    }

    public void SetShredMark(bool value)
    {
        shredMark.gameObject.SetActive(value);
    }

    public void TestInsert()
    {
        InsertItem(GameManager.Instance.lootManager.GetItemCard());
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}

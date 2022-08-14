using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MicroCard : MonoBehaviour
{
    [field: SerializeField] public EquipmentDataContainer EquipmentData { get; private set; }
    [field: SerializeField] public SpriteRenderer itemSprite { get; private set; }
    [field: SerializeField] public SpriteRenderer background { get; private set; }
    [field: SerializeField] public SpriteRenderer highlight { get; private set; }
    [field: SerializeField] public SpriteRenderer shredMark { get; private set; }
    [field: SerializeField] public SpriteRenderer[] gemslots { get; private set; }

    public void InsertItem(EquipmentDataContainer item)
    {
        EquipmentData = item;
        itemSprite.sprite = item.itemCore.icon;
        background.color = GameManager.Instance.colors[(int)item.quality];
        highlight.gameObject.SetActive(false);
        shredMark.gameObject.SetActive(false);

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
                switch (ability.slotType)
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
                            if (item.lockedSlots[i])
                            {
                                color = Color.gray;
                            }
                            else
                            {
                                color = Color.white;
                            }
                            break;
                    }
            }
            else
            {
                if (item.lockedSlots[i])
                {
                    color = Color.gray;
                }
                else
                {
                    color = Color.white;
                }
            }
            
            gemslots[i].color = color;

        }
    }

    public void SetHighlighted(bool value)
    {
        highlight.gameObject.SetActive(value);
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
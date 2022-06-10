using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PlayerObject", menuName = "Combat/PlayerSO")]
public class PlayerBrain : Brain
{
    [field: SerializeField] public EquipmentDataContainer[] equippedCards { get; private set; }
    [field: SerializeField] public bool[] equippedSlots { get; private set; }
    [field: SerializeField] public EquipmentList[] equipmentDataContainers { get; private set; }

    [field: SerializeField] public int damageBonus { get; private set; }
    [field: SerializeField] public int shieldBonus { get; private set; }
    [field: SerializeField] public int healthBonus { get; private set; }
    [field: SerializeField] public int critDamageBonus { get; private set; }
    [field: SerializeField] public int critChanceBonus { get; private set; }
    


    public void Equip(EquipmentDataContainer equipmentDataContainer)
    {
        equippedCards[(int)equipmentDataContainer.itemCore.itemType] = equipmentDataContainer;
        equippedSlots[(int)equipmentDataContainer.itemCore.itemType] = true;
        CalculateStats();
    }

    private void CalculateStats()
    {
        damageBonus = 0;
        shieldBonus = 0;
        healthBonus = 0;
        critDamageBonus = 0;
        critChanceBonus = 0;
        for (var i = 0; i < equippedSlots.Length; i++)
        {
            if (!equippedSlots[i])
            {
                continue;
            }

            for (var j = 0; j < equippedCards[i].stats.Length; j++)
            {
                EquipmentDataContainer.Stats stat = equippedCards[i].stats[j];
                switch (stat)
                {
                    case EquipmentDataContainer.Stats.Damage:
                        damageBonus += equippedCards[i].statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Health:
                        healthBonus += equippedCards[i].statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Shield:
                        shieldBonus += equippedCards[i].statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.CriticalDamage:
                        critDamageBonus += equippedCards[i].statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.CriticalChance:
                        critChanceBonus += equippedCards[i].statValue[j];
                        break;
                }
            }
        }
    }

    public void Unequip(int slot)
    {
        equippedCards[slot] = null;
        equippedSlots[slot] = false;
        CalculateStats();
    }
    
    public void AddCardToInventory(EquipmentDataContainer equipmentDataContainer)
    {
       
        equipmentDataContainers[(int)equipmentDataContainer.itemCore.itemType].container.Add(equipmentDataContainer);
    }
    
    public void RemoveCardFromInventory(EquipmentDataContainer equipmentDataContainer)
    {
        equipmentDataContainers[(int)equipmentDataContainer.itemCore.itemType].container.Remove(equipmentDataContainer);
    }
}

[Serializable]
public class EquipmentList
{
    public List<EquipmentDataContainer> container;
}
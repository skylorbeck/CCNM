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
    // [field: SerializeField] public EquipmentDataContainer[] equippedCards { get; private set; }
    [field: SerializeField] public int[] equippedSlots { get; private set; }
    [field: SerializeField] public EquipmentList[] equipmentDataContainers { get; private set; }
    [field: SerializeField] public EquipmentDataContainer[] defaultEquipment { get; private set; }

    [field: SerializeField] public int damageBonus { get; private set; }
    [field: SerializeField] public int shieldBonus { get; private set; }
    [field: SerializeField] public int healthBonus { get; private set; }
    [field: SerializeField] public int critDamageBonus { get; private set; }
    [field: SerializeField] public int critChanceBonus { get; private set; }
    


    public void Equip(int dataContainerIndex,int cardIndex)
    {
        equippedSlots[dataContainerIndex] = cardIndex;
        CalculateStats();
    }

    private void CalculateStats()
    {
        damageBonus = 0;
        shieldBonus = 0;
        healthBonus = 0;
        critDamageBonus = 0;
        critChanceBonus = 0;
        ClearAbilities();
        for (var i = 0; i < equippedSlots.Length; i++)
        {
            EquipmentDataContainer equippedCard;
            if (equippedSlots[i]==-1)
            {
                if (i<3)
                {
                    equippedCard = defaultEquipment[i];
                } else {
                    continue;
                }
            }
            else
            {
                equippedCard = equipmentDataContainers[i].container[equippedSlots[i]];
            }
            for (var j = 0; j <equippedCard.stats.Length; j++)
            {
                EquipmentDataContainer.Stats stat = equippedCard.stats[j];
                switch (stat)
                {
                    case EquipmentDataContainer.Stats.Damage:
                        damageBonus += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Health:
                        healthBonus += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Shield:
                        shieldBonus += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.CriticalDamage:
                        critDamageBonus += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.CriticalChance:
                        critChanceBonus += equippedCard.statValue[j];
                        break;
                }

                
            }
            if (equippedCard.ability != null)
            {
                AddAbility(equippedCard.ability);
            }
        }
    }

    public void Unequip(int slot)
    {
        equippedSlots[slot] = -1;
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
    
    public EquipmentDataContainer GetCard(int equipmentSlot,int cardIndex)
    {
        return equipmentDataContainers[equipmentSlot].container[cardIndex];
    }
    
    public bool EquippedCardExists(int equipmentList)
    {
        if (equippedSlots[equipmentList]>=equipmentDataContainers[equipmentList].container.Count)
        {
            return false;
        }
        return equipmentDataContainers[equipmentList].container[equippedSlots[equipmentList]]!=null;
    }
    
    public EquipmentDataContainer GetEquippedCard(int equipmentList)
    {
        return equipmentDataContainers[equipmentList].container[equippedSlots[equipmentList]];
    }
}

[Serializable]
public class EquipmentList
{
    public List<EquipmentDataContainer> container;
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
[CreateAssetMenu(fileName = "PlayerObject", menuName = "Combat/PlayerSO")]
public class PlayerBrain : Brain
{

    [field: SerializeField] public int[] equippedSlots { get; private set; }
    [field: SerializeField] public EquipmentList[] playerInventory { get; private set; }
    [field: SerializeField] public EquipmentDataContainer[] defaultEquipment { get; private set; }

    [field: SerializeField] public int damageBonus { get; private set; }
    [field: SerializeField] public int shieldBonus { get; private set; }
    [field: SerializeField] public int healthBonus { get; private set; }
    [field: SerializeField] public int healBonus { get; private set; }
    [field: SerializeField] public int skillBonus { get; private set; }
    [field: SerializeField] public int speedBonus { get; private set; }
    [field: SerializeField] public int egoBonus { get; private set; }
    [field: SerializeField] public int creditBonus { get; private set; }
    
    [field: SerializeField] public int credits { get; private set; } = 0;
    [field: SerializeField] public int ego { get; private set; } = 0;
    [field: SerializeField] public int level { get; private set; } = 0;
    [field: SerializeField] public int cardPacks { get; private set; } = 0;
    [field: SerializeField] public int capsules { get; private set; } = 0;
    [field: SerializeField] public int superCapsules { get; private set; } = 0;
    [field: SerializeField] public int[] cardSouls { get; private set; } = new int[7];
    

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
        skillBonus = 0;
        speedBonus = 0;
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
                equippedCard = playerInventory[i].container[equippedSlots[i]];
            }
            for (var j = 0; j <equippedCard.stats.Length; j++)
            {
                EquipmentDataContainer.Stats stat = equippedCard.stats[j];
                switch (stat)
                {
                    case EquipmentDataContainer.Stats.Strength:
                        damageBonus += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Vitality:
                        healthBonus += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Dexterity:
                        shieldBonus += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Skill:
                        skillBonus += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Speed:
                        speedBonus += equippedCard.statValue[j];
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
        playerInventory[(int)equipmentDataContainer.itemCore.itemType].container.Add(equipmentDataContainer);
    }
    
    public void RemoveCardFromInventory(EquipmentDataContainer equipmentDataContainer)
    {
        playerInventory[(int)equipmentDataContainer.itemCore.itemType].container.Remove(equipmentDataContainer);
    }
    
    public EquipmentDataContainer GetCard(int equipmentSlot,int cardIndex)
    {
        return playerInventory[equipmentSlot].container[cardIndex];
    }
    
    public bool EquippedCardExists(int equipmentList)
    {
        if (equippedSlots[equipmentList]==-1 || equippedSlots[equipmentList]>=playerInventory[equipmentList].container.Count)
        {
            return false;
        }
        return playerInventory[equipmentList].container[equippedSlots[equipmentList]]!=null;
    }
    
    public EquipmentDataContainer GetEquippedCard(int equipmentList)
    {
        return playerInventory[equipmentList].container[equippedSlots[equipmentList]];
    }
    
    public void ClearInventory()
    {
        for (var i = 0; i < playerInventory.Length; i++)
        {
            playerInventory[i].container = new List<EquipmentDataContainer>();
        }
    }

    public int ShredCard(EquipmentDataContainer card)
    {
        if (playerInventory[(int)card.itemCore.itemType].container.Contains(card))
        {
            RemoveCardFromInventory(card);
        }
        int soulsGained = Random.Range(1, 10);
        cardSouls[(int)card.quality]+=soulsGained;
        return soulsGained;
    }

    public void CopyCards(PlayerBrain source)
    {
        for (var i = 0; i < source.playerInventory.Length; i++)
        {
            for (var j = 0; j < source.playerInventory[i].container.Count; j++)
            {
                AddCardToInventory(source.playerInventory[i].container[j]);
            }
        }
    }
    
    public void Clone(PlayerBrain sourcePlayer)
    {
        ClearInventory();
        for (var index = 0; index < sourcePlayer.equippedSlots.Length; index++)
        {
            if (sourcePlayer.EquippedCardExists(index))
            {
                equippedSlots[index] = 0;
                EquipmentDataContainer newcard = sourcePlayer.GetEquippedCard(index);
                newcard.SetIndestructible(true);
                AddCardToInventory(newcard);
            }
        }
        defaultEquipment = sourcePlayer.defaultEquipment;
        CalculateStats();
    }
    public void AddCredits(int amt)
    {
        amt = (int)(amt * GameManager.Instance.metaPlayer.creditBonus);
        credits += amt;
    }

    public void AddEgo(int amt)
    {
        amt = (int)(amt * GameManager.Instance.metaPlayer.egoBonus); 
        ego += amt;
    }
    
    public void AddCardSoul(int index, int amt)
    {
        cardSouls[index] += amt;
    }
    
    public void AddCapsule(int amt)
    {
        capsules += amt;
    }
    
    public void AddSuperCapsule(int amt)
    {
        superCapsules += amt;
    }
    
    public void AddCardPack(int amt)
    {
        cardPacks += amt;
    }
    
    public void CopyCardSouls(PlayerBrain source)
    {
        for (var i = 0; i < source.cardSouls.Length; i++)
        {
            cardSouls[i] += source.cardSouls[i];
        }
    }
    
    public void CopyEgo(PlayerBrain source)
    {
        ego += source.ego;
    }
    
    public void CopyCredits(PlayerBrain source)
    {
        credits += source.credits;
    }
    
    public void CopyCapsules(PlayerBrain source)
    {
        capsules += source.capsules;
    }
    
    public void CopySuperCapsules(PlayerBrain source)
    {
        superCapsules += source.superCapsules;
    }
    
    public void CopyCardPacks(PlayerBrain source)
    {
        cardPacks += source.cardPacks;
    }
}

[Serializable]
public class EquipmentList
{
    public List<EquipmentDataContainer> container;
}
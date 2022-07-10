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

    
    [field: SerializeField] public int credits { get; private set; } = 0;
    [field: SerializeField] public int ego { get; private set; } = 0;
    [field: SerializeField] public int level { get; private set; } = 0;
    [field: SerializeField] public int cardPacks { get; private set; } = 0;
    [field: SerializeField] public int capsules { get; private set; } = 0;
    [field: SerializeField] public int superCapsules { get; private set; } = 0;
    [field: SerializeField] public int[] cardSouls { get; private set; } = new int[3];
    [field: SerializeField] public int[] consumables { get; private set; } = new int[3];

    #region precomputedStats
    [field:SerializeField] public int cardStrength { get; private set; } = 0;
    [field:SerializeField] public int cardDexterity { get; private set; } = 0;
    [field:SerializeField] public int cardVitality { get; private set; } = 0;
    [field:SerializeField] public int cardSpeed { get; private set; } = 0;
    [field:SerializeField] public int cardSkill { get; private set; } = 0;
    [field:SerializeField] public int cardLuck { get; private set; } = 0;
    [field:SerializeField] public int cardGrit { get; private set; } = 0;
    [field:SerializeField] public int cardResolve { get; private set; } = 0;
    [field:SerializeField] public int cardIntelligence { get; private set; } = 0;
    [field:SerializeField] public int cardCharisma { get; private set; } = 0;
    public void CalculateCardStats()
    {
        cardStrength = 0;
        cardDexterity = 0;
        cardVitality = 0;
        cardSpeed = 0;
        cardSkill = 0;
        cardLuck = 0;
        cardGrit = 0;
        cardResolve = 0;
        cardIntelligence = 0;
        cardCharisma = 0;
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
                    default:
                    case EquipmentDataContainer.Stats.None:
                        break;
                    case EquipmentDataContainer.Stats.Strength:
                        cardStrength += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Dexterity:
                        cardDexterity += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Vitality:
                        cardVitality += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Speed:
                        cardSpeed += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Skill:
                        cardSkill += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Luck:
                        cardLuck += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Grit:
                        cardGrit += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Resolve:
                        cardResolve += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Intelligence:
                        cardIntelligence += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Charisma:
                        cardCharisma += equippedCard.statValue[j];
                        break;
                }
            }
            if (equippedCard.ability != null)
            {
                AddAbility(equippedCard.ability);
            }
        }
    }
    #endregion
    
    #region computedStatGetters
    public override float GetDamage()
    {
        return GetStrength() + cardStrength;
    }

    public override float GetShieldMax()
    {
        return GetGrit() + cardGrit;
    }

    public override float GetShieldRate()
    {
        return GetResolve() + cardResolve;
    }

    public override float GetCritChance()
    {
        return GetSpeed() + cardSpeed;
    }

    public override float GetCritDamage()
    {
        return GetSkill() + cardSkill;
    }

    public override float GetDodgeChance()
    {
        return GetDexterity() + cardDexterity;
    }
    public override float GetMaxHealth()
    {
        return GetVitality() + cardVitality;
    }
    public override float GetLootLuck()
    {
        return GetLuck() + cardLuck;
    }
    public override float GetEgoBoost()
    {
        return GetIntelligence() + cardIntelligence;
    }

    public override float GetCreditBoost()
    {
        return GetCharisma() + cardCharisma;
    }
    #endregion
    
    

    public void Equip(int dataContainerIndex,int cardIndex)
    {
        equippedSlots[dataContainerIndex] = cardIndex;
        CalculateCardStats();
    }

    public void Unequip(int slot)
    {
        equippedSlots[slot] = -1;
        CalculateCardStats();
    }
    
    public void AddCardToInventory(EquipmentDataContainer equipmentDataContainer)
    {
        playerInventory[(int)equipmentDataContainer.itemCore.itemType].container.Add(equipmentDataContainer);
    }
    
    public void RemoveCardFromInventory(EquipmentDataContainer equipmentDataContainer)
    {
        List<EquipmentDataContainer> container = playerInventory[(int)equipmentDataContainer.itemCore.itemType].container;
        if (equippedSlots[(int)equipmentDataContainer.itemCore.itemType] == container.IndexOf(equipmentDataContainer))
        {
            equippedSlots[(int)equipmentDataContainer.itemCore.itemType] = -1;
        }
        container.Remove(equipmentDataContainer);
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
            else
            {
                equippedSlots[index] = -1;
            }
        }
        defaultEquipment = sourcePlayer.defaultEquipment;
        SetCurrentHealth((int)sourcePlayer.GetMaxHealth());
        CalculateCardStats();
    }
    public void AddCredits(int amt)
    {
        amt = (int)(amt * GetCreditBoost());
        credits += amt;
    }

    public void SpendCredits(int amt)
    {
        credits -= amt;
    }

    public void AddEgo(int amt)
    {
        amt = (int)(amt * GetEgoBoost());
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

    public void ClearConsumables()
    {
        consumables = new int[3];
    }
    
    public void AddConsumables(int index, int amt)
    {
        consumables[index] += amt;
    }
    
    public void RemoveCardPack(int amt)
    {
        cardPacks -= amt;
    }
}

[Serializable]
public class EquipmentList
{
    public List<EquipmentDataContainer> container;
}
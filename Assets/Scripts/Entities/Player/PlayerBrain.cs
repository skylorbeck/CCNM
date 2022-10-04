using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using SaveSystem;
using Unity.Mathematics;
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
    [field: SerializeField] public AbilityGem[] ownedGems { get; private set; } = new AbilityGem[0];

    [field: SerializeField] public int cardPacks { get; private set; }
    [field: SerializeField] public int capsules { get; private set; }
    [field: SerializeField] public int superCapsules { get; private set; }
    [field: SerializeField] public int[] cardSouls { get; private set; } = new int[7];
    [field: SerializeField] public int[] consumables { get; private set; } = new int[3];
    [OptionalField] public TrackableStats trackableStats = new TrackableStats();
    public bool isDead => currentHealth <= 0;

    [OptionalField] public int maximumEquipmentSlots = 100;
    
    public int totalEquipment
    {
        get
        {
            int total = 0;
            for (int i = 0; i < playerInventory.Length; i++)
            {
                total += playerInventory[i].container.Count;
            }

            return total;
        }
    }

    #region precomputedStats

    [field: SerializeField] public int cardStrength { get; private set; }
    [field: SerializeField] public int cardDexterity { get; private set; }
    [field: SerializeField] public int cardVitality { get; private set; }
    [field: SerializeField] public int cardSpeed { get; private set; }
    [field: SerializeField] public int cardSkill { get; private set; }
    [field: SerializeField] public int cardLuck { get; private set; }
    [field: SerializeField] public int cardGrit { get; private set; }
    [field: SerializeField] public int cardResolve { get; private set; }
    [field: SerializeField] public int cardIntelligence { get; private set; }
    [field: SerializeField] public int cardCharisma { get; private set; }
    [field: SerializeField] public int cardSagacity { get; private set; }

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
        cardSagacity = 0;
        ClearAbilities();
        for (var i = 0; i < equippedSlots.Length; i++)
        {
            EquipmentDataContainer equippedCard;
            if (equippedSlots[i] == -1)
            {
                if (i < 3)
                {
                    equippedCard = defaultEquipment[i];
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (equippedSlots[i] >= playerInventory[i].container.Count)
                {
                    equippedSlots[i] = -1;
                    equippedCard = defaultEquipment[i];
                }
                else
                {
                    try
                    {
                        equippedCard = playerInventory[i].container[equippedSlots[i]];
                    }
                    catch
                    {
                        equippedSlots[i] = -1;
                        equippedCard = defaultEquipment[i];
                        Debug.Log("Invalid Equipment Slot, resetting to default");
                    }
                }
            }

            for (var j = 0; j < equippedCard.stats.Length; j++)
            {
                EquipmentDataContainer.Stats stat = equippedCard.stats[j];
                switch (stat)
                {
                    default:
                    case EquipmentDataContainer.Stats.None:
                        break;
                    case EquipmentDataContainer.Stats.Str:
                        cardStrength += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Dex:
                        cardDexterity += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Vit:
                        cardVitality += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Spd:
                        cardSpeed += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Skl:
                        cardSkill += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Lck:
                        cardLuck += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Cap:
                        cardGrit += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Chg:
                        cardResolve += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Int:
                        cardIntelligence += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Cha:
                        cardCharisma += equippedCard.statValue[j];
                        break;
                    case EquipmentDataContainer.Stats.Wis:
                        cardSagacity += equippedCard.statValue[j];
                        break;
                }
            }

            AbilityGem[] abilities = equippedCard.GetAbilities();
            if (abilities != null)
            {
                for (var j = 0; j < abilities.Length; j++)
                {
                    if (abilities[j] != null)
                    {
                        AddAbility(abilities[j]);
                        
                    }
                }
            }
        }
    }

    #endregion

    public int GetUnmodifiedStatValueWithCard(EquipmentDataContainer.Stats desiredStat)
    {
        switch (desiredStat)
        {
            default:
            case EquipmentDataContainer.Stats.None:
                return 0;
            case EquipmentDataContainer.Stats.Str:
                return GetStrength()+cardStrength;
            case EquipmentDataContainer.Stats.Spd:
                return GetSpeed()+cardSpeed;
            case EquipmentDataContainer.Stats.Vit:
                return GetVitality()+cardVitality;
            case EquipmentDataContainer.Stats.Dex:
                return GetDexterity()+cardDexterity;
            case EquipmentDataContainer.Stats.Skl:
                return GetSkill()+cardSkill;
            case EquipmentDataContainer.Stats.Lck:
                return GetLuck()+cardLuck;
            case EquipmentDataContainer.Stats.Cap:
                return GetCap()+cardGrit;
            case EquipmentDataContainer.Stats.Chg:
                return GetCharge()+cardResolve;
            case EquipmentDataContainer.Stats.Int:
                return GetIntelligence()+cardIntelligence;
            case EquipmentDataContainer.Stats.Cha:
                return GetCharisma()+cardCharisma;
            case EquipmentDataContainer.Stats.Wis:
                return GetWisdom()+cardSagacity;
        }
    }
    
    #region StatGettersNoCard

    public int GetUnmodifiedDamageNoCard(int temp =0)
    {
        return base.GetUnmodifiedDamage(temp);
    }

    public int GetShieldMaxUnmodifiedNoCard(int temp = 0)
    {
        return base.GetShieldMaxUnmodified(temp);
    }

    public int GetHealthMaxUnmodifiedNoCard(int temp = 0)
    {
        return base.GetHealthMaxUnmodified(temp);
    }

    public int GetShieldRateUnmodifiedNoCard(int temp = 0)
    {
        return base.GetShieldRateUnmodified(temp);
    }

    public float GetCritChanceUnmodifiedNoCard(int temp = 0)
    {
        return base.GetCritChanceUnmodified(temp);
    }

    public float GetCritDamageUnmodifiedNoCard(int temp = 0)
    {
        return base.GetCritDamageUnmodified(temp);
    }

    public float GetDodgeChanceUnmodifiedNoCard(int temp = 0)
    {
        return base.GetDodgeChanceUnmodified(temp);
    }

    public int GetLootLuckUnmodifiedNoCard(int temp = 0)
    {
        return base.GetLootLuckUnmodified(temp);
    }

    public int GetEgoBoostUnmodifiedNoCard(int temp = 0)
    {
        return base.GetEgoBoostUnmodified(temp);
    }

    public int GetCreditBoostUnmodifiedNoCard(int temp = 0)
    {
        return base.GetCreditBoostUnmodified(temp);
    }

    public int GetStatusDamageUnmodifiedNoCard(int temp = 0)
    {
        return base.GetStatusDamageUnmodified(temp);
    }

    #endregion

    #region unModifiedStatGetters

    public override int GetUnmodifiedDamage(int temp=0)
    {
        return base.GetUnmodifiedDamage(temp) + (cardStrength*5);
    }

    public override int GetShieldMaxUnmodified(int temp=0)
    {
        return base.GetShieldMaxUnmodified(temp) + (cardGrit*5);
    }

    public override int GetHealthMaxUnmodified(int temp=0)
    {
        return base.GetHealthMaxUnmodified(temp) + cardVitality*5;
    }

    public override int GetShieldRateUnmodified(int temp=0)
    {
        return base.GetShieldRateUnmodified(temp) + cardResolve*5;
    }

    public override float GetCritChanceUnmodified(int temp=0)
    {
        return base.GetCritChanceUnmodified(temp) + (cardSpeed*0.05f);
    }

    public override float GetCritDamageUnmodified(int temp=0)
    {
        return base.GetCritDamageUnmodified(temp) + cardSkill*0.05f;
    }

    public override float GetDodgeChanceUnmodified(int temp=0)
    {
        return base.GetDodgeChanceUnmodified(temp) + cardDexterity*0.01f;
    }

    public override int GetLootLuckUnmodified(int temp=0)
    {
        return base.GetLootLuckUnmodified(temp) + cardLuck;
    }

    public override int GetEgoBoostUnmodified(int temp=0)
    {
        return base.GetEgoBoostUnmodified(temp) + cardIntelligence;
    }

    public override int GetCreditBoostUnmodified(int temp=0)
    {
        return base.GetCreditBoostUnmodified(temp) + cardCharisma;
    }

    public override int GetStatusDamageUnmodified(int temp=0)
    {
        return base.GetStatusDamageUnmodified(temp) + cardSagacity*5;
    }

    #endregion

  
    public void Equip(int slot, EquipmentDataContainer equipment)
    {
        if (slot < 0 || slot >= equippedSlots.Length)
        {
            return;
        }
       
        equippedSlots[slot] = playerInventory[slot].container.IndexOf(equipment);
        CalculateCardStats();
    }
    
    public void Equip(int dataContainerIndex, int cardIndex)
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
        trackableStats.cardsObtained++;
        playerInventory[(int)equipmentDataContainer.itemCore.itemType].container.Add(equipmentDataContainer);
    }

    public void RemoveCardFromInventory(EquipmentDataContainer equipmentDataContainer)
    {
        if ( equipmentDataContainer.guid.Equals(GetEquippedCard((int)equipmentDataContainer.itemCore.itemType).guid))
        {
            Unequip((int)equipmentDataContainer.itemCore.itemType);
        }
        playerInventory[(int)equipmentDataContainer.itemCore.itemType].container.Remove(equipmentDataContainer);

    }
    

    public EquipmentDataContainer GetCard(int equipmentSlot, int cardIndex)
    {
        return playerInventory[equipmentSlot].container[cardIndex];
    }

    public bool EquippedCardExists(int equipmentList)
    {
        if (equippedSlots[equipmentList] == -1 ||
            equippedSlots[equipmentList] >= playerInventory[equipmentList].container.Count)
        {
            return false;
        }

        return playerInventory[equipmentList].container[equippedSlots[equipmentList]] != null;
    }

    public EquipmentDataContainer GetEquippedCard(int equipmentList)
    {
        if (equippedSlots[equipmentList] == -1|| equippedSlots[equipmentList] >= playerInventory[equipmentList].container.Count)
        {
            equippedSlots[equipmentList] = -1;
            return defaultEquipment[equipmentList];
        }
        return playerInventory[equipmentList].container[equippedSlots[equipmentList]];
    }

    public void ClearInventory()
    {
        playerInventory = new EquipmentList[3];
        equippedSlots = new int[3];
        for (var i = 0; i < playerInventory.Length; i++)
        {
            playerInventory[i] = new EquipmentList();
            playerInventory[i].container = new List<EquipmentDataContainer>();
            equippedSlots[i] = -1;
        }
        
    }
    

    public int2 ShredCard(EquipmentDataContainer card)
    {
        if (playerInventory[(int)card.itemCore.itemType].container.Contains(card))
        {
            RemoveCardFromInventory(card);
        }
        trackableStats.cardsDismantled++;
        int soulsGained = Random.Range(1, 10);
        cardSouls[(int)card.quality] += soulsGained;
        GameManager.Instance.metaStats.cardSoulsGained[(int)card.quality] += soulsGained;
        
        for (var i = 0; i < card.abilities?.Length; i++)
        {
            if (card.abilities[i] != null && card.abilities[i].abilityIndex!= -1)
            {
                AddAbilityGem(card.abilities[i]);
                trackableStats.gemsObtained++;
            }
        }

        return new int2(soulsGained, (int)card.quality);
    }

    public void CopyCards(PlayerBrain source)
    {
        for (var i = 0; i < source.playerInventory.Length; i++)
        {
            for (var j = 0; j < source.playerInventory[i].container.Count; j++)
            {
                if (source.playerInventory[i].container[j].indestructible)
                {
                    continue;
                }
                AddCardToInventory(source.playerInventory[i].container[j]);
            }
        }
    }

    public void Clone(PlayerBrain sourcePlayer)
    {
        for (var index = 0; index < sourcePlayer.equippedSlots.Length; index++)
        {
            if (sourcePlayer.EquippedCardExists(index))
            {
                equippedSlots[index] = 0;
                EquipmentDataContainer equippedCard = new EquipmentDataContainer(sourcePlayer.GetEquippedCard(index));
                equippedCard.SetIndestructible(true);
                //this sets a default ability if the card has none
                //but we actually don't want that
                /*if (equippedCard.abilities[0].abilityIndex==-1)
                {
                    equippedCard.abilities[0] = new AbilityGem(0);
                }*/
                AddCardToInventory(equippedCard);
            }
            else
            {
                equippedSlots[index] = -1;
            }
        }
        level = sourcePlayer.level;
        defaultEquipment = sourcePlayer.defaultEquipment;
        
        SetStrength(sourcePlayer.GetStrength());
        SetDexterity(sourcePlayer.GetDexterity());
        SetVitality(sourcePlayer.GetVitality());
        SetSpeed(sourcePlayer.GetSpeed());
        SetSkill(sourcePlayer.GetSkill());
        SetLuck(sourcePlayer.GetLuck());
        SetGrit(sourcePlayer.GetCap());
        SetResolve(sourcePlayer.GetCharge());
        SetIntelligence(sourcePlayer.GetIntelligence());
        SetCharisma(sourcePlayer.GetCharisma());
        SetSagacity(sourcePlayer.GetWisdom());

        SetCurrentHealth(sourcePlayer.GetHealthMax());
        CalculateCardStats();
    }

    public void AddCredits(int amt)
    {
        amt += (int)(Math.Ceiling(amt * GetCreditBoost()));
        // Debug.Log("Credit Boost "+GetCreditBoost());
        trackableStats.creditsEarned += amt;
        credits += amt;
        NotificationPopController.Instance.PopCredits("+" + amt + "C", Camera.main.ViewportToWorldPoint(new Vector3(0.6f,0.6f,0)));
    }

    public void SpendCredits(int amt)
    {
        trackableStats.creditsSpent += amt;
        credits -= amt;
    }

    public void AddEgo(int amt)
    {
        amt = (int)(amt * GetEgoBoost());
        trackableStats.egoGained += amt;
        ego += amt;
        NotificationPopController.Instance.PopEgo("+" + amt + "Ego",Camera.main.ViewportToWorldPoint(new Vector3(0.6f,0.525f,0)));
    }

    public void SpendEgo(int amt)
    {
        trackableStats.egoSpent += amt;
        ego -= amt;
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
        trackableStats.safesObtained += amt;
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

    public void RemoveGem(int gemIndex, int amt)
    {
        for (var i = 0; i < ownedGems.Length; i++)
        {
            if (i == gemIndex)
            {
                ownedGems[i].AddAmountOwned(-amt);
                if (ownedGems[i].amountOwned < 0)
                {
                    ownedGems[i].SetAmountOwned(0);
                }
            }
        }
        
    }

    public void ClearPlayerObject()
    {
        SetDefaultEquipment();
        ClearInventory();
        ClearRelics();
        ClearConsumables();
        cardSouls = new int[7];
        consumables = new int[3];
        capsules = 0;
        superCapsules = 0;
        cardPacks = 0;
        ego = 0;
        credits = 0;
        level = 1;
        SetStrength(1);
        SetDexterity(1);
        SetVitality(5);
        SetSpeed(1);
        SetSkill(1);
        SetLuck(1);
        SetGrit(5);
        SetResolve(1);
        SetIntelligence(1);
        SetCharisma(1);
        SetSagacity(1);
        SetCurrentHealth(1);
        CalculateCardStats();
        trackableStats = new TrackableStats();
        List<AbilityGem> abilityGems = new List<AbilityGem>();
        GameManager.Instance.abilityRegistry.values.ForEach(abilityObject =>
        {
            abilityGems.Add(new AbilityGem(abilityObject));
        });
        ownedGems = abilityGems.ToArray();
        maximumEquipmentSlots = 25;
    }
    public void AddAbilityGem(AbilityGem gem)
    {
        List<AbilityGem> newGems = new List<AbilityGem>();
        if (ownedGems!=null && ownedGems.Length>0)
        {
            newGems.AddRange(ownedGems);
        }
        
        bool found = false;
        for (var j = 0; j < newGems.Count; j++)
        {
            if (newGems[j].abilityIndex == gem.abilityIndex && newGems[j].gemLevel == gem.gemLevel)//this will break when I implement gem levels because the iterator is not adding blank ones for ever level
            {
                newGems[j].AddAmountOwned(1);
                found = true;
                break;
            }
        }
        if (!found)
        {
            newGems.Add(gem);
        }

        
        ownedGems = newGems.ToArray();
    }
    public void InsertSaveFile(SavablePlayerBrain saveFile)
    {
        ClearPlayerObject();
        equippedSlots = saveFile.equippedSlots;
        playerInventory = new EquipmentList[saveFile.playerInventory.Length];
        for (var i = 0; i < saveFile.playerInventory.Length; i++)
        {
            playerInventory[i] = new EquipmentList();
            playerInventory[i].container = new List<EquipmentDataContainer>();
            for (var j = 0; j < saveFile.playerInventory[i].Length; j++)
            {
                playerInventory[i].container.Add(new EquipmentDataContainer(saveFile.playerInventory[i][j]));
            }
        }

        foreach (int i in saveFile.relicIndexes)
        {
            AddRelic(GameManager.Instance.relicRegistry.GetRelic(i));
        }
        List<AbilityGem> abilityGems = new List<AbilityGem>();
        GameManager.Instance.abilityRegistry.values.ForEach(abilityObject =>
        {
            abilityGems.Add(new AbilityGem(abilityObject));
        });
        ownedGems = abilityGems.ToArray();
        foreach (AbilityGem saveFileOwnedGem in saveFile.ownedGems)//this should convert old save files into the new format
        {
            for (var i = 0; i < ownedGems.Length; i++)
            {
                if (ownedGems[i].abilityIndex == saveFileOwnedGem.abilityIndex)
                {
                    ownedGems[i].AddAmountOwned(saveFileOwnedGem.amountOwned);
                    break;
                }
            }
        }
        
        cardSouls = saveFile.cardSouls;
        consumables = saveFile.consumables;
        capsules = saveFile.capsules;
        superCapsules = saveFile.superCapsules;
        cardPacks = saveFile.cardPacks;
        ego = saveFile.ego;
        credits = saveFile.credits;
        level = saveFile.level<=0?1:saveFile.level;
        maximumEquipmentSlots = saveFile.maximumEquipmentSlots;
        if (saveFile.trackableStats==null)
        {
            trackableStats = new TrackableStats();
        }
        else
        {
            trackableStats = saveFile.trackableStats;
        }
        SetStrength(saveFile.strength);
        SetDexterity(saveFile.dexterity);
        SetVitality(saveFile.vitality);
        SetSpeed(saveFile.speed);
        SetSkill(saveFile.skill);
        SetLuck(saveFile.luck);
        SetGrit(saveFile.grit);
        SetResolve(saveFile.resolve);
        SetIntelligence(saveFile.intelligence);
        SetCharisma(saveFile.charisma);
        SetCurrentHealth(saveFile.currentHealth);
        CalculateCardStats();
    }

    public void SetDefaultEquipment()
    {
        defaultEquipment = new EquipmentDataContainer[3];
        for (var i = 0; i < defaultEquipment.Length; i++)
        {
            defaultEquipment[i] = new EquipmentDataContainer();
        }

        defaultEquipment[0].InsertItem(GameManager.Instance.equipmentRegistries[0].GetCard(0));
        defaultEquipment[0].SetGemSlots(1);
        defaultEquipment[0].SetLockedSlots(new bool[] { false, true, true });
        defaultEquipment[0].SetAbilities(new AbilityGem[3]);
        AbilityGem defaultAbility = new AbilityGem(GameManager.Instance.abilityRegistry.GetAbility("Fireball"),0);
        defaultAbility.SetAmountOwned(1);
        defaultEquipment[0].InsertAbility(defaultAbility,0);
        defaultEquipment[0].SetIndestructible(true);
        defaultEquipment[0].SetStatValue(new int[] { 2, 2 });
        defaultEquipment[0].SetStats(new EquipmentDataContainer.Stats[]
            { EquipmentDataContainer.Stats.Cap, EquipmentDataContainer.Stats.Chg });

        defaultEquipment[1].InsertItem(GameManager.Instance.equipmentRegistries[1].GetCard(0));
        defaultEquipment[1].SetGemSlots(1);
        defaultEquipment[1].SetLockedSlots(new bool[] { false, true, true });
        defaultEquipment[1].SetAbilities(new AbilityGem[3]);
        defaultAbility = new AbilityGem(GameManager.Instance.abilityRegistry.GetAbility("Immolate"),0);
        defaultAbility.SetAmountOwned(1);
        defaultEquipment[1].InsertAbility(defaultAbility,0);
        defaultEquipment[1].SetIndestructible(true);
        defaultEquipment[1].SetStatValue(new int[] { 2 });
        defaultEquipment[1].SetStats(new EquipmentDataContainer.Stats[] { EquipmentDataContainer.Stats.Str });

        defaultEquipment[2].InsertItem(GameManager.Instance.equipmentRegistries[2].GetCard(0));
        defaultEquipment[2].SetGemSlots(1);
        defaultEquipment[2].SetLockedSlots(new bool[] { false, true, true });
        defaultEquipment[2].SetAbilities(new AbilityGem[3]);
        defaultAbility = new AbilityGem(GameManager.Instance.abilityRegistry.GetAbility("Combust"),0);
        defaultAbility.SetAmountOwned(1);
        defaultEquipment[2].InsertAbility(defaultAbility,0);
        defaultEquipment[2].SetIndestructible(true);
        defaultEquipment[2].SetStatValue(new int[] { 2 });
        defaultEquipment[2].SetStats(new EquipmentDataContainer.Stats[] { EquipmentDataContainer.Stats.Vit });
    }


    public void SpendSouls(int cardQuality, int cardLevel)
    {
        cardSouls[cardQuality] -= cardLevel;
        GameManager.Instance.metaStats.cardSoulsSpent[cardQuality] += cardLevel;
    }
}

[Serializable]
public class EquipmentList
{
    public List<EquipmentDataContainer> container;
}

[Serializable]
public class SavablePlayerBrain
{
    //brain
    public int currentHealth;
    public int[] relicIndexes;
    [OptionalField] public AbilityGem[] ownedGems;
    //playerbrain
    public int[] equippedSlots;
    public SavableDataContainer[][] playerInventory;
    public TrackableStats trackableStats;
    public int credits;
    public int ego;
    public int level;
    public int cardPacks;
    public int capsules;
    public int superCapsules;
    public int[] cardSouls;
    public int[] consumables;
    public int strength;
    public int dexterity;
    public int vitality;
    public int speed;
    public int skill;
    public int luck;
    public int grit;
    public int resolve;
    public int intelligence;
    public int charisma;
    public int maximumEquipmentSlots;


    public SavablePlayerBrain(PlayerBrain player)
    {
        currentHealth = player.currentHealth;
        equippedSlots = player.equippedSlots;
        playerInventory = new SavableDataContainer[player.playerInventory.Length][];
        for (var i = 0; i < player.playerInventory.Length; i++)
        {
            playerInventory[i] = new SavableDataContainer[player.playerInventory[i].container.Count];
            EquipmentList equipmentList = player.playerInventory[i];
            for (var j = 0; j < equipmentList.container.Count; j++)
            {
                EquipmentDataContainer dataContainer = equipmentList.container[j];
                playerInventory[i][j] = new SavableDataContainer(dataContainer);
            }
        }

        relicIndexes = new int[player.relics.Length];
        for (var i = 0; i < player.relics.Length; i++)
        {
            Relic relic = player.relics[i];
            relicIndexes[i] = GameManager.Instance.relicRegistry.GetRelicIndex(relic.name);
        }

        ownedGems = player.ownedGems;

        credits = player.credits;
        ego = player.ego;
        level = player.level;
        cardPacks = player.cardPacks;
        capsules = player.capsules;
        superCapsules = player.superCapsules;
        cardSouls = player.cardSouls;
        consumables = player.consumables;
        strength = player.GetStrength();
        dexterity = player.GetDexterity();
        vitality = player.GetVitality();
        speed = player.GetSpeed();
        skill = player.GetSkill();
        luck = player.GetLuck();
        grit = player.GetCap();
        resolve = player.GetCharge();
        intelligence = player.GetIntelligence();
        charisma = player.GetCharisma();
        trackableStats = player.trackableStats;
        maximumEquipmentSlots = player.maximumEquipmentSlots;
    }
}
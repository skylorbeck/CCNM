using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerObject", menuName = "Combat/PlayerSO")]
public class PlayerBrain : Brain
{
    [field: SerializeField] public EquipmentDataContainer[] equippedCards { get; private set; }
    [field: SerializeField] public bool[] equippedSlots { get; private set; }
    [field: SerializeField] public EquipmentList[] equipmentDataContainers { get; private set; }


    public void Equip(EquipmentDataContainer equipmentDataContainer)
    {
        equippedCards[(int)equipmentDataContainer.itemCore.itemType] = equipmentDataContainer;
        equippedSlots[(int)equipmentDataContainer.itemCore.itemType] = true;
    }
    
    public void Unequip(int slot)
    {
        equippedCards[slot] = null;
        equippedSlots[slot] = false;
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
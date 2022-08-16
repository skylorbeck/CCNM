using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [field: SerializeField] public int[] qualityWeights { get; private set; }

    public EquipmentDataContainer GetItemCard()
    {
        return GetItemCard(Random.Range(0, 3));
    }

    public EquipmentDataContainer GetItemCard(ItemCard.ItemType itemType)
    {
        return GetItemCard((int)itemType);
    }

    public EquipmentDataContainer GetItemCard(int itemType)
    {
        EquipmentDataContainer dataContainer = new EquipmentDataContainer();
        dataContainer.InsertItem(
            GameManager.Instance.equipmentRegistries[itemType].GetRandomCard());
        dataContainer.GenerateDataOfLevel(GameManager.Instance.metaPlayer.level + Random.Range(-3, 3));
        return dataContainer;
    }

    public EquipmentDataContainer.Quality GetRandomQuality()
    {
        int randomIndex = 0;
        int randomValue = Random.Range(0, 100);
        
        List<int> luckyWeights = new List<int>(qualityWeights);
        int lootLuck = GameManager.Instance.metaPlayer.GetLootLuck();
        
        for (int i = 0; i < luckyWeights.Count; i++)
        {
            if (lootLuck==0)
            {
                Debug.Log("Loot luck is 0");
                break;
            }
            
            Debug.Log("Before: "+luckyWeights[i]);
            
            if (luckyWeights[i]>lootLuck)
            {
                Debug.Log(i+" Weight Higher, lowering by "+lootLuck);
                luckyWeights[i]-=lootLuck;
                lootLuck = 0;
            }
            else
            {
                Debug.Log(i+" Loot Luck is higher, lowering by "+(luckyWeights[i]-1));
                lootLuck -= (luckyWeights[i]-1);
                luckyWeights[i] = 1;
            }
            Debug.Log("After: "+luckyWeights[i]);
        }

        for (int i = 0; i < luckyWeights.Count; i++)
        {
            if (randomValue < luckyWeights[i])
            {
                randomIndex = i;
                break;
            }
            else
            {
                randomValue -= luckyWeights[i];
            }
        }

        return (EquipmentDataContainer.Quality)randomIndex;
    }
}
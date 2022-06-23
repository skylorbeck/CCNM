using UnityEngine;

public class LootManager: MonoBehaviour
{
    [field: SerializeField] public int[] qualityWeights { get;private set; }

    public EquipmentDataContainer GetItemCard()
    {
        return GetItemCard(Random.Range(0, qualityWeights.Length));
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
        dataContainer.GenerateDataOfLevel(GameManager.Instance.metaPlayer.level+Random.Range(-3,3));
        return dataContainer;
    }

    public EquipmentDataContainer.Quality GetRandomQuality()
    {
        int randomIndex = 0;
        int randomValue = Random.Range(0, 100);
        for (int i = 0; i < qualityWeights.Length; i++)
        {
            if (randomValue < qualityWeights[i])
            {
                randomIndex = i;
                break;
            }
            else
            {
                randomValue -= qualityWeights[i];
            }
        }
        return (EquipmentDataContainer.Quality)randomIndex;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemStatCompare : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI ItemName { get; private set; }
    [field: SerializeField] public List<TextMeshProUGUI> ItemStats { get; private set; }

    public void InsertItemStats(EquipmentDataContainer EquippedItem, EquipmentDataContainer newItem)
    {
        ItemName.text = newItem.itemCore.cardTitle;
        for (int i = 0; i < ItemStats.Count; i++)
        {
            if (newItem.stats[i] == EquipmentDataContainer.Stats.None)
            {
                ItemStats[i].text = "";
                continue;
            }
            ItemStats[i].text = newItem.statValue[i] + " " + newItem.stats[i];
            List<EquipmentDataContainer.Stats> stats = new List<EquipmentDataContainer.Stats>(EquippedItem.stats);
            if (stats.Contains(newItem.stats[i]))
            {
                int difference = (newItem.statValue[i]-EquippedItem.statValue[stats.IndexOf(newItem.stats[i])]);
                if (difference > 0)
                {
                    ItemStats[i].text += " (+" + difference+")";
                    ItemStats[i].color = Color.green;
                }
                else if (difference < 0)
                {
                    ItemStats[i].text += " (" + difference+")";
                    ItemStats[i].color = Color.red;
                } else 
                {
                    ItemStats[i].color = Color.white;
                }
            }
            else
            {
                ItemStats[i].color = Color.green;
            }
            // if (EquippedItem.stats.Contains(newItem.stats[i]))
            // {
            //     ItemStats[i].text += " (" + (newItem.statValue[i] - EquippedItem.statValue[i]) + ")";
            // }
        }
    }

    public void InsertAbilityGem(AbilityGem gem)
    {
        if (gem!=null)
        {
            AbilityObject abilityObject = gem.GetAbility();
            ItemName.text = abilityObject.title;
            ItemStats[0].text = abilityObject.descriptionA;
            ItemStats[1].text = abilityObject.descriptionB;
        }
        else
        {
            ItemName.text = "";
            ItemStats[0].text = "";
            ItemStats[1].text = "";
        }
  
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

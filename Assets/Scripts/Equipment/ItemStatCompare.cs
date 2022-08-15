using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStatCompare : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI ItemName { get; private set; }
    [field: SerializeField] public Image background { get; private set; }
    [field: SerializeField] public List<TextMeshProUGUI> ItemStats { get; private set; }

    public void InsertItemStats(EquipmentDataContainer EquippedItem, EquipmentDataContainer newItem)
    {
        ItemName.text = newItem.itemCore.cardTitle +" Lv."+ newItem.level;
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
        if (gem!=null && gem.abilityIndex!=-1)
        {
            AbilityObject abilityObject = gem.GetAbility();
            ItemName.text = abilityObject.title;
            ItemStats[0].text = abilityObject.descriptionA;
            ItemStats[1].text = abilityObject.descriptionB;
            background.color = Color.white;
        }
        else
        {
            ItemName.text = "Empty Socket";
            ItemStats[0].text = "";
            ItemStats[1].text = "Ready to Use";
            background.color = Color.white;
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

    public void Clear()
    {
        ItemName.text = "";
        ItemStats[0].text = "";
        ItemStats[1].text = "";
        background.color = Color.clear;
    }
}

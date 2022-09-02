using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStatCompare : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI ItemName { get; private set; }
    [field: SerializeField] public Image background { get; private set; }
    [field: SerializeField] public List<TextMeshProUGUI> ItemStats { get; private set; }
    [field: SerializeField] public bool autosize { get; private set; } = false;
    public void InsertItemStats(EquipmentDataContainer EquippedItem, EquipmentDataContainer newItem)
    {
        
        DOTween.To(() => background.color, x => background.color = x,Color.white, 0.5f);

        // ItemName.text = newItem.itemCore.cardTitle +" Lv."+ newItem.level;
        int totalStats = 0;
        for (int i = 0; i < ItemStats.Count; i++)
        {
            int i1 = i;
            string finalString = "";
            Color color = Color.white;
            if (newItem.stats[i1] == EquipmentDataContainer.Stats.None)
            {
                DOTween.To(() => ItemStats[i1].text, x => ItemStats[i1].text = x,"", 0.5f);

                // ItemStats[i1].text = "";
                continue;
            }
            totalStats++;
            finalString= newItem.statValue[i1] + " " + newItem.stats[i1];
            List<EquipmentDataContainer.Stats> stats = new List<EquipmentDataContainer.Stats>(EquippedItem.stats);
            if (stats.Contains(newItem.stats[i1]))
            {
                int difference = (newItem.statValue[i1]-EquippedItem.statValue[stats.IndexOf(newItem.stats[i1])]);
                if (difference > 0)
                {
                    finalString += " (+" + difference+")";
                    color = Color.green;
                }
                else if (difference < 0)
                {
                    finalString += " (" + difference+")";
                    color = Color.red;
                } else 
                {
                    color = Color.white;
                }
            }
            else
            {
                color = Color.green;
            }
            
            DOTween.To(() => ItemStats[i1].text, x => ItemStats[i1].text = x,finalString, 0.5f);
            DOTween.To(() => ItemStats[i1].color, x => ItemStats[i1].color = x,color, 0.5f);
        }
        
        DOTween.To(() => ItemName.text, x => ItemName.text = x, newItem.itemCore.cardTitle, 0.5f).OnUpdate(() =>
        {
            if (autosize)
            {
                ItemName.ForceMeshUpdate();
                background.rectTransform.sizeDelta = new Vector2(background.rectTransform.sizeDelta.x, ItemName.fontSize+(totalStats * ItemStats[0].fontSize*0.6f ));
            }
        });
    }

    public void InsertAbilityGem(AbilityGem gem)
    {
        if (gem!=null && gem.abilityIndex!=-1)
        {
            AbilityObject abilityObject = gem.GetAbility();
            DOTween.To(() => ItemName.text, x => ItemName.text = x, abilityObject.title+" "+(gem.gemLevel+1), 0.5f).OnUpdate(() =>
            {
                if (autosize)
                {
                    ItemName.ForceMeshUpdate();
                    background.rectTransform.sizeDelta = new Vector2(ItemName.preferredWidth + 20, 40);
                }
            });
            DOTween.To(() => ItemStats[0].text, x => ItemStats[0].text = x,abilityObject.descriptionA, 0.5f);
            DOTween.To(() => ItemStats[1].text, x => ItemStats[1].text = x,abilityObject.descriptionB, 0.5f);
            DOTween.To(() => background.color, x => background.color = x,Color.white, 0.5f);
        }
        else
        {
            DOTween.To(() => ItemName.text, x => ItemName.text = x, "Empty Socket", 0.5f);
            DOTween.To(() => ItemStats[0].text, x => ItemStats[0].text = x,"", 0.5f);
            DOTween.To(() => ItemStats[1].text, x => ItemStats[1].text = x,"Ready to Use", 0.5f);
            DOTween.To(() => background.color, x => background.color = x,Color.white, 0.5f);
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
        DOTween.To(() => ItemName.text, x => ItemName.text = x, "", 0.5f);
        for (var i = 0; i < ItemStats.Count; i++)
        {
            var i1 = i;
            DOTween.To(() => ItemStats[i1].text, x => ItemStats[i1].text = x,"", 0.5f);
        }
        DOTween.To(() => background.color, x => background.color = x,Color.clear, 0.5f);
    }
}

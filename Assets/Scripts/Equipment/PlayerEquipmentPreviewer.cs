using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerEquipmentPreviewer : MonoBehaviour
{
    [field: SerializeField] public List<TextMeshProUGUI> ItemStats { get; private set; }
    private EquipmentDataContainer previewedCard;
    private int curEquipmentSlot = 0;

    public void InsertCard(EquipmentDataContainer newItem, int slot)
    {
        previewedCard = newItem;
        curEquipmentSlot = slot;
        UpdatePlayerStats();
    }

    public void UpdatePlayerStats()
    {
        //for each stat in EquipmentDataContainer.Stats, update the text in the UI
        List<EquipmentDataContainer.Stats> allStats = new List<EquipmentDataContainer.Stats>();
        allStats.AddRange(Enum.GetValues(typeof(EquipmentDataContainer.Stats)).Cast<EquipmentDataContainer.Stats>());
        allStats.Remove(EquipmentDataContainer.Stats.None);
        allStats.Remove(EquipmentDataContainer.Stats.Lck);
        allStats.Remove(EquipmentDataContainer.Stats.Int);
        allStats.Remove(EquipmentDataContainer.Stats.Cha);

        //sort allStats by the order of the enum
        allStats.Sort((x, y) => x.CompareTo(y));
        
        

        for (var index = 0; index < allStats.Count; index++)
        {
            EquipmentDataContainer.Stats stat = allStats[index];
            // Debug.Log(stat);
            int statValue = GameManager.Instance.metaPlayer.GetUnmodifiedStatValueWithCard(stat);
            int statValueNewCard = GameManager.Instance.metaPlayer.GetUnmodifiedStatValue(stat) +
                                   previewedCard.GetStatValue(stat);
            int otherCards = 0;
            for (int i = 0; i < GameManager.Instance.metaPlayer.equippedSlots.Length; i++)
            {
                if (i == curEquipmentSlot) continue;
                if (GameManager.Instance.metaPlayer.GetEquippedCard(i) == null) continue;
                otherCards += GameManager.Instance.metaPlayer.GetEquippedCard(i).GetStatValue(stat);    
            }
            statValueNewCard+= otherCards;
            int difference = statValueNewCard-statValue;
            var index1 = index;
            string finalText = statValueNewCard + " " + stat.ToString();
            Color color = Color.white;
            if (difference>0)
            {
                finalText += " (+"+difference+")";
                color = Color.green;
            }
            else if (difference<0)
            {
                finalText += " ("+difference+")";
                color = Color.red;
            }

            DOTween.To(() => ItemStats[index1].text, x => ItemStats[index1].text = x, finalText, 0.5f);
            DOTween.To(() => ItemStats[index1].color, x => ItemStats[index1].color = x,color, 0.5f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Relic", menuName = "Cards/Relic/Blank")]
public class Relic : ScriptableObject
{
    public Sprite sprite;
    public string title;
    public string description;
    public int cost;
    public int level;
    public EquipmentDataContainer.Quality rarity;
    public EquipmentDataContainer.Stats[] stats;//unused
    public int[] statValues;//unused
    
    public virtual void Subscribe(Shell shell)
    {
        
    }
    
    public virtual void Unsubscribe(Shell shell)
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "EventObject", menuName = "Event/EventObject")]
public class EventObject : ScriptableObject
{
    public string eventName = "event";
    public string eventDescription = "event description";
    public string[] dialogue = new string[0];
    public Sprite eventImage;
    public bool rewardsEquipment = false;
    public bool healsPlayer = false;
    public bool rewardsXP = false;
    public bool rewardsCredits = false;
    
}

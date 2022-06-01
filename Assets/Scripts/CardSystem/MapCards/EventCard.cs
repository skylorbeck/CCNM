using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EventCard", menuName = "Cards/MapCard/EventCard")]
public class EventCard : MapCard
{
    // EventObject event//todo
    public new MapCardType mapCardType { get; protected set; } = MapCardType.Event;
    
}
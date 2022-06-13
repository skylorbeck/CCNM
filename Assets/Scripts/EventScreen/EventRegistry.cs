using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EventRegistry", menuName = "Data/EventRegistry")]
public class EventRegistry : ScriptableObject,  ISerializationCallbackReceiver
{
    public EventObject[] events;
    public List<string> keys = new List<string> ();
    public List<int> values = new List<int>();
    public Dictionary<string, int>  EventDictionary = new Dictionary<string, int>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        for (var index = 0; index < events.Length; index++)
        {
            values.Add(index);
            keys.Add(events[index].name);
        }
    }

    public void OnAfterDeserialize()
    {
        EventDictionary = new Dictionary<string, int>();

        for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
            EventDictionary.Add(keys[i], values[i]);
        
    }
    public EventObject GetEvent(string cardTitle)
    {
        if (EventDictionary.ContainsKey(cardTitle))
        {
            return events[EventDictionary[cardTitle]];
        }
        else
        {
            Debug.LogError("Event not found in registry: " + cardTitle);
            return null;
        }
    }
}

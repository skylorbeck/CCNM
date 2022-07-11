using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "EventObject", menuName = "Event/EventObject")]
public class EventObject : ScriptableObject
{
    public string eventName = "event";
    public string eventDescription = "event description";
    public Sprite eventImage;

    public virtual async Task StartEvent()
    {
        await Task.Delay(1000);
        //todo add events
        GameManager.Instance.battlefield.TotalHandsPlus();
        GameManager.Instance.saveManager.SaveRun();

        GameManager.Instance.LoadSceneAdditive("MapScreen","EventScreen");
    } 
    public virtual void UpdateEvent()
    {
        
    }
    public virtual void FixedUpdateEvent()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    protected EventManager eventManager;
    
    public virtual void Init( EventManager eventManager)
    {
        SetEventManager(eventManager);
    }
    public void SetEventManager(EventManager eventManager)
    {
        this.eventManager = eventManager;
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

    public virtual void MoveOn()
    {
        eventManager.MoveOn();
    }
}

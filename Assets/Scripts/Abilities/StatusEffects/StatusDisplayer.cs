using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDisplayer : MonoBehaviour
{
    [SerializeField] private EffectInstance statusPrefab;
    private List<EffectInstance> statusList = new List<EffectInstance>();

    public void AddStatus(StatusEffect statusEffect, Shell shell)
    {
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect == statusEffect)
            {
                instance.AddDuration(statusEffect.duration);
                return;
            }
        }
        EffectInstance status = Instantiate(statusPrefab, transform);
        status.SetStatusEffect(statusEffect, shell);
        status.transform.localPosition= new Vector3(statusList.Count * 0.5f, 0, 0);
        statusList.Add(status);
    }
    
    public void Tick()
    {
        for (int i = 0; i < statusList.Count; i++)
        {
            statusList[i].Tick();
        }

        // Remove expired status
        for (int i = statusList.Count - 1; i >= 0; i--)
        {
            if (!statusList[i].isActive)
            {
                Destroy(statusList[i].gameObject);
                statusList[i].Expire();
                statusList.RemoveAt(i);
            }
        }
    }    
    public void RemoveStatus(StatusEffect statusEffect)
    {
        for (int i = 0; i < statusList.Count; i++)
        {
            if (statusList[i].statusEffect == statusEffect)
            {
                statusList[i].Expire();
                statusList.RemoveAt(i);
                
                Destroy(statusList[i].gameObject);
                break;
            }
        }
    }
}

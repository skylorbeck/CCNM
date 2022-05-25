using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StatusDisplayer : MonoBehaviour
{
    [SerializeField] private EffectInstance statusPrefab;
    private List<EffectInstance> statusList = new List<EffectInstance>();

    public void AddStatus(StatusEffect statusEffect, Shell target)
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
        status.SetStatusEffect(statusEffect, target);
        status.transform.localPosition= new Vector3(statusList.Count * 0.5f, 0, 0);
        statusList.Add(status);
    }
    
    public async Task Tick()
    {
        for (int i = 0; i < statusList.Count; i++)
        {
            await statusList[i].Tick();
        }

        // Remove expired status
        for (int i = statusList.Count - 1; i >= 0; i--)
        {
            if (!statusList[i].isActive)
            {
                statusList[i].Expire();
                statusList.RemoveAt(i);
                Destroy(statusList[i].gameObject);
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

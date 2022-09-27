using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class StatusDisplayer : MonoBehaviour
{
    [SerializeField] private EffectInstance statusPrefab;
    private bool isDisplaying = true;
    public List<EffectInstance> statusList { get; private set; } = new List<EffectInstance>();
    private CancellationTokenSource cts = new CancellationTokenSource();
    

    public int OnAttack(Shell target, Shell attacker, int baseDamage)
    {
        int damage = baseDamage;
        foreach (EffectInstance instance in statusList)
        {
            damage = instance.OnAttack(target, attacker, damage);
        }

        return damage;
    }

    public int OnDamage([CanBeNull] Shell attacker, Shell defender, int baseDamage)
    {
        int damage = baseDamage;
        foreach (EffectInstance instance in statusList)
        {
            damage = instance.OnDamage(attacker, defender, damage);
        }

        return damage;
    }

    public int OnDodge([CanBeNull] Shell attacker, Shell defender, int baseDamage)
    {
        int damage = baseDamage;
        foreach (EffectInstance instance in statusList)
        {
            damage = instance.OnDodge(attacker, defender, damage);
        }

        return damage;
    }

    public int OnHeal([CanBeNull] Shell healer, Shell target, int baseHeal)
    {
        int heal = baseHeal;
        foreach (EffectInstance instance in statusList)
        {
            heal = instance.OnHeal(healer, target, heal);
        }

        return heal;
    }

    public int OnShield([CanBeNull] Shell shielder, Shell target, int baseShield)
    {
        int shield = baseShield;
        foreach (EffectInstance instance in statusList)
        {
            shield = instance.OnShield(shielder, target, shield);
        }

        return shield;
    }

    public void AddStatus(StatusEffect statusEffect, Shell target, Shell source,int duration)
    {
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect == statusEffect && statusEffect.isStackable)
            {
                instance.AddDuration(duration);
                instance.MoreOrEqualPower(source.brain.GetStatusDamage());
                return;
            }
        }

        EffectInstance status = Instantiate(statusPrefab, transform);
        status.SetStatusEffect(statusEffect, target,source,duration,source.brain.GetStatusDamage());
        if (!isDisplaying || statusEffect.isHidden)
        {
            status.DisableVisuals();
        }

        statusList.Add(status);
        SetStatusLocation();
    }

    public async Task Tick()
    {
        List<EffectInstance> toRemove = new List<EffectInstance>();

        for (int i = 0; i < statusList.Count; i++)
        {
            statusList[i].Tick();
            await Task.Delay(500);
            if (cts.IsCancellationRequested)
            {
                return;
            }
            if (!statusList[i].isActive)
            {
                toRemove.Add(statusList[i]);
            }
        }

        foreach (EffectInstance instance in toRemove)
        {
            statusList.Remove(instance);
            if (!instance.gameObject.IsDestroyed())
            {
                Destroy(instance.gameObject);
            }
        }

        SetStatusLocation();
    }

    public void RemoveStatus(Type type)
    {
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect.GetType() == type)
            {
                instance.Expire();
                Destroy(instance.gameObject);
                statusList.Remove(instance);
                break;
            }
        }
        SetStatusLocation();
    }

    public void RemoveStacks(Type type, int amount)
    {
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect.GetType() == type)
            {
                instance.RemoveStacks(amount);
                if (instance.duration <= 0)
                {
                    Destroy(instance.gameObject);
                    statusList.Remove(instance);
                }
                break;
            }
        }
        SetStatusLocation();
    }
    
    public void RemoveStatus(StatusEffect statusEffect)
    {
        for (int i = 0; i < statusList.Count; i++)
        {
            if (statusList[i].statusEffect == statusEffect)
            {
                statusList[i].Expire();
                Destroy(statusList[i].gameObject);
                statusList.RemoveAt(i);
                break;
            }
        }

        SetStatusLocation();
    }

    public void Clear()
    {
        Cancel();
        foreach (EffectInstance instance in statusList)
        {
            if (!instance.gameObject.IsDestroyed())
            {
                Destroy(instance.gameObject);
            }
        }

        statusList.Clear();
    }
    
    public int GetStatusCount()
    {
        return statusList.Count;
    }
    
   

    public bool HasStatus(Type statusType)
    {
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect.GetType() == statusType)
            {
                return true;
            }
        }

        return false;
    }
    
    public int GetStatusCount(Type statusType)
    {
        int count = 0;
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect.GetType() == statusType)
            {
                count++;
            }
        }

        return count;
    }
    
    public int GetStatusDuration(StatusEffect statusEffect)
    {
        int count = 0;
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect == statusEffect)
            {
                count+=instance.duration;
            }
        }

        return count;
    }
    public int GetStatusDuration(Type statusType)
    {
        int count = 0;
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect.GetType()==statusType)
            {
                count+=instance.duration;
            }
        }

        return count;
    }
    public bool HasStatus(StatusEffect statusEffect)
    {
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect == statusEffect)
            {
                return true;
            }
        }

        return false;
    }
    
    public bool HasStatus(string statusEffect)
    {
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect.name.ToLower().Equals(statusEffect.ToLower()))
            {
                return true;
            }
        }

        return false;
    }

    public void SetStatusLocation()
    {
        List<EffectInstance> tempstack = statusList.FindAll(instance => !instance.statusEffect.isHidden);
        int xOffset = Math.Min(tempstack.Count - 1, 2);

        for (var i = 0; i < tempstack.Count; i++)
        {
            int offset = i / 3 % 3;
            tempstack[i].transform.localPosition =
                new Vector3((i * 0.5f) - (offset * 1.5f) - (xOffset * 0.25f), offset * 0.5f, 0);
        }
    }

    public void DisableVisuals()
    {
        isDisplaying = false;
        foreach (EffectInstance instance in statusList)
        {
            instance.DisableVisuals();
        }
    }

    public void EnableVisuals()
    {
        isDisplaying = true;
        foreach (EffectInstance instance in statusList)
        {
            instance.EnableVisuals();
        }
    }

    public void Cancel()
    {
        cts.Cancel();
    }
}

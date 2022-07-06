using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class StatusDisplayer : MonoBehaviour
{
    [SerializeField] private EffectInstance statusPrefab;
    private bool isDisplaying = true;
    public List<EffectInstance> statusList { get; private set; } = new List<EffectInstance>();

    public async Task<int> OnAttack(Shell target,Shell attacker,int baseDamage)
    {
        int damage = baseDamage;
        foreach (EffectInstance instance in statusList)
        {
            damage = await instance.OnAttack(target,attacker,damage);
        }

        return damage;
    }
     public async Task<int> OnDamage([CanBeNull] Shell attacker,Shell defender,int baseDamage)
    {
        int damage = baseDamage;
        foreach (EffectInstance instance in statusList)
        {
            damage = await instance.OnDamage(attacker,defender,damage);
        }

        return damage;
    }
     public async Task<int> OnDodge([CanBeNull] Shell attacker,Shell defender,int baseDamage)
    {
        int damage = baseDamage;
        foreach (EffectInstance instance in statusList)
        {
            damage = await instance.OnDodge(attacker,defender,damage);
        }

        return damage;
    }
     public async Task<int> OnHeal([CanBeNull] Shell healer,Shell target,int baseHeal)
    {
        int heal = baseHeal;
        foreach (EffectInstance instance in statusList)
        {
            heal = await instance.OnHeal(healer,target,heal);
        }

        return heal;
    }
     public async Task<int> OnShield([CanBeNull] Shell shielder,Shell target,int baseShield)
    {
        int shield = baseShield;
        foreach (EffectInstance instance in statusList)
        {
            shield = await instance.OnShield(shielder,target,shield);
        }

        return shield;
    }
    
    public void AddStatus(StatusEffect statusEffect, Shell target)
    {
        foreach (EffectInstance instance in statusList)
        {
            if (instance.statusEffect == statusEffect && statusEffect.isStackable)
            {
                instance.AddDuration(statusEffect.duration);
                return;
            }
        }
        EffectInstance status = Instantiate(statusPrefab, transform);
        status.SetStatusEffect(statusEffect, target);
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
            await statusList[i].Tick();
            await Task.Delay(500);

            if (!statusList[i].isActive)
            {
                toRemove.Add(statusList[i]);
            }
        }
        
        foreach (EffectInstance instance in toRemove)
        {
            statusList.Remove(instance);
            Destroy(instance.gameObject);
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
        foreach (EffectInstance instance in statusList)
        {
            Destroy(instance.gameObject);
        }
        statusList.Clear();
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
        int xOffset = Math.Min(tempstack.Count - 1,2);

        for (var i = 0; i < tempstack.Count; i++)
        {
            int offset = i / 3 % 3;
            tempstack[i].transform.localPosition = new Vector3((i * 0.5f)-(offset*1.5f)-(xOffset*0.25f) ,offset*0.5f , 0);
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
    }}

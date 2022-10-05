using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Relic", menuName = "Combat/Relic")]
public class Relic : ScriptableObject
{
    public Sprite sprite;
    public string description;
    public int cost;
    public int level;
    public EquipmentDataContainer.Quality rarity;

    [field: Header("OnCrit")]
    [field: SerializeField] public bool healCrit { get; private set; } = false;
    [field: SerializeField] public float healCritPercent { get; private set; } = 0;
    [field: SerializeField] public bool shieldCrit { get; private set; } = false;
    [field: SerializeField] public float shieldCritPercent { get; private set; } = 0;
    [field: SerializeField] public bool statusCrit { get; private set; } = false;
    [field: SerializeField] public StatusEffect critStatus { get; private set; }
    [field: SerializeField] public int critStatusDuration { get; private set; } = 1;
    
    [field: Header("OnHit")]
    [field: SerializeField] public float hitChance { get; private set; } = 0;
    [field: SerializeField] public bool healHit { get; private set; } = false;
    [field: SerializeField] public float healHitPercent { get; private set; } = 0;
    [field: SerializeField] public bool shieldHit { get; private set; } = false;
    [field: SerializeField] public float shieldHitPercent { get; private set; } = 0;
    [field: SerializeField] public bool statusHit { get; private set; } = false;
    [field: SerializeField] public StatusEffect hitStatus { get; private set; }
    [field: SerializeField] public int hitStatusDuration { get; private set; } = 0;

    [field: Header("OnDodge")]
    [field: SerializeField] public bool healDodge { get; private set; } = false;
    [field: SerializeField] public float healDodgePercent { get; private set; } = 0;
    [field: SerializeField] public bool shieldDodge { get; private set; } = false;
    [field: SerializeField] public float shieldDodgePercent { get; private set; } = 0;
    [field: SerializeField] public bool statusDodge { get; private set; } = false;
    [field: SerializeField] public StatusEffect dodgeStatus { get; private set; }
    [field: SerializeField] public int dodgeStatusDuration { get; private set; } = 0;

    [field: Header("OnKill")]
    [field: SerializeField] public bool healKill { get; private set; } = false;
    [field: SerializeField] public float healKillPercent { get; private set; } = 0;
    [field: SerializeField] public bool shieldKill { get; private set; } = false;
    [field: SerializeField] public float shieldKillPercent { get; private set; } = 0;
    
    [field: Header("StatusSpecific")]
    [field: SerializeField] public bool playerStart { get; private set; } = false;
    [field: SerializeField] public StatusEffect playerStartStatus { get; private set; }
    [field: SerializeField] public int playerStartAmount { get; private set; } = 1;
    [field: SerializeField] public bool enemyStart { get; private set; } = false;
    [field: SerializeField] public StatusEffect enemyStartStatus { get; private set; }
    [field: SerializeField] public int enemyStartAmount { get; private set; } = 1;

    [field: Header("OnDamageTaken")]
    [field: SerializeField] public bool reflect { get; private set; } = false;
    [field: SerializeField] public float reflectPercent { get; private set; } = 0;
    
    [field: Header("Passive")]
    [field: SerializeField] public bool modifyHealth { get; private set; } = false;
    [field: SerializeField] public float modifyHealthPercent { get; private set; } = 1;
    [field: SerializeField] public bool modifyShield { get; private set; } = false;
    [field: SerializeField] public float modifyShieldPercent { get; private set; } = 1;
    [field: SerializeField] public bool modifyShieldRegen { get; private set; } = false;
    [field: SerializeField] public float modifyShieldRegenPercent { get; private set; } = 1;
    [field: SerializeField] public bool modifyShieldDelay { get; private set; } = false;
    [field: SerializeField] public float modifyShieldDelayPercent { get; private set; } = 1;
    [field: SerializeField] public bool modifyCritChance { get; private set; } = false;
    [field: SerializeField] public float modifyCritChancePercent { get; private set; } = 1;
    [field: SerializeField] public bool modifyCritDamage { get; private set; } = false;
    [field: SerializeField] public float modifyCritDamagePercent { get; private set; } = 1;
    [field: SerializeField] public bool modifyDodgeChance { get; private set; } = false;
    [field: SerializeField] public float modifyDodgeChancePercent { get; private set; } = 1;
    [field: SerializeField] public bool modifyStatusDamage { get; private set; } = false;
    [field: SerializeField] public float modifyStatusDamagePercent{ get; private set; } = 1;
    [field: SerializeField] public bool modifyDamage { get; private set; } = false;
    [field: SerializeField] public float modifyDamagePercent{ get; private set; } = 1;

    [field: Header("OnStatusGiven")]
    [field: SerializeField] public StatusEffect statusGiven { get; private set; }
    [field: SerializeField] public bool doubleStatus { get; private set; } = false;
    [field: SerializeField] public bool addStatusEffectDuration { get; private set; } = false;
    [field: SerializeField] public int addStatusEffectDurationAmount{ get; private set; } = 1;

    [field: Header("OnStatusTaken")]
    [field: SerializeField] public StatusEffect statusTaken { get; private set; }
    [field: SerializeField] public bool immuneToStatus { get; private set; } = false;
    
    [field: Header("OnStatusTick")]
    [field: SerializeField] public StatusEffect statusTicking { get; private set; }
    [field: SerializeField] public bool userDecayStops { get; private set; } = false;
    [field: SerializeField] public bool enemyDecayStops { get; private set; } = false;


    //called at the start of combat when the shell is initiated
    public virtual void Subscribe(Shell shell)
    {
        if (shell.isPlayer)
        {
            if (playerStart)
            {
                shell.AddStatusEffect(playerStartStatus, shell, playerStartAmount);
            }
        }
        else
        {
            if (enemyStart)
            {
                shell.AddStatusEffect(enemyStartStatus, shell, enemyStartAmount);
            }
        }
    }

    //called when shell gameobject is destroyed
    public virtual void Unsubscribe(Shell shell)
    {
    }
}
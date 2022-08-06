using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemShell : MonoBehaviour
{
    [field:SerializeField] public AbilityObject ability { get; private set; }
    [field:SerializeField] public SpriteRenderer gemRenderer { get; private set; }
    
    public void InsertAbility(AbilityObject ability)
    {
        this.ability = ability;
        gemRenderer.sprite = ability.gemIcon;
    }

    public void TestInsert()
    {
        InsertAbility(GameManager.Instance.abilityRegistry.GetRandomAbility());
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
}

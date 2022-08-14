using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemShell : MonoBehaviour
{
    [field:SerializeField] public AbilityGem ability { get; private set; }
    [field:SerializeField] public SpriteRenderer gemRenderer { get; private set; }
    [field:SerializeField] public SpriteRenderer levelRenderer { get; private set; }
    
    public void InsertAbility(AbilityGem ability)
    {
        this.ability = ability;
        gemRenderer.sprite = ability.GetAbility().gemIcon;
        Sprite[] levelSprites = Resources.LoadAll<Sprite>("IconSheet");
        foreach (Sprite sprite in levelSprites)
        {
            if (sprite.name == "Roman"+ability.gemLevel)
            {
                levelRenderer.sprite = sprite;
                break;
            }
        }
    }

    public void TestInsert()
    {
        InsertAbility(new AbilityGem(GameManager.Instance.abilityRegistry.GetRandomAbility(),Random.Range(0,5)));
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

    public void ClearAbility()
    {
        this.gemRenderer.sprite = null;
        this.levelRenderer.sprite = null;
        this.ability = null;
    }
}

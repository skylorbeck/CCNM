using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicShell : MonoBehaviour
{
    [field:SerializeField] public Relic relic{get;private set;}
    
    [field:SerializeField] public SpriteRenderer numberRenderer{get;private set;}
    [field:SerializeField] public SpriteRenderer spriteRenderer{get;private set;}
    [field:SerializeField] public SpriteRenderer outlineRenderer{get;private set;}
    [field:SerializeField] public SpriteMask spriteMask{get;private set;}
    [field:SerializeField] public TextMeshPro relicName{get;private set;}
    [field:SerializeField] public TextMeshPro relicLevelText{get;private set;}
    [field:SerializeField] public TextMeshPro relicDescriptionText{get;private set;}

    void Start()
    {
        InsertRelic(relic);
    }

    void Update()
    {
        spriteMask.transform.localScale = Vector3.one * Mathf.Lerp(1.1f,1.2f,Mathf.Sin(Time.time*2.5f));
        Color color = outlineRenderer.color;
        color.a = Mathf.Lerp(1,0.5f,Mathf.Sin(Time.time*2.5f));
        outlineRenderer.color = color;
    }

    void FixedUpdate()
    {
        
    }

    public void InsertRelic(Relic relic)
    {
        this.relic = relic;
        spriteRenderer.sprite = relic.sprite;
        spriteMask.sprite = relic.sprite;
        outlineRenderer.color = GameManager.Instance.colors[(int)relic.rarity];
        Sprite[] levelSprites = Resources.LoadAll<Sprite>("IconSheet");
        foreach (Sprite sprite in levelSprites)
        {
            if (sprite.name == "Roman"+relic.level)
            {
                numberRenderer.sprite = sprite;
                break;
            }
        }

        // numberRenderer.color = GameManager.Instance.colors[(int)relic.rarity];
        relicDescriptionText.text = relic.description;
        relicDescriptionText.color = GameManager.Instance.colors[(int)relic.rarity];
        relicName.text = relic.title;
        relicName.color = GameManager.Instance.colors[(int)relic.rarity];
        relicLevelText.color = GameManager.Instance.colors[(int)relic.rarity];
    }
}

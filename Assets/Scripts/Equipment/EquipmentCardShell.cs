using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentCardShell : MonoBehaviour, IPointerClickHandler
{
    //this class should be only used for data display
    [field: Header("Front")]
    [field: SerializeField] public EquipmentDataContainer EquipmentData { get; private set; }
    // [field: SerializeField] public SpriteRenderer itemSprite { get; private set; }
    [field: SerializeField] public MicroCard microCard { get; private set; }
    [field: SerializeField] public TextMeshPro title { get; private set; }
    [field: SerializeField] public TextMeshPro[] statTitleText { get; private set; }
    [field: SerializeField] public TextMeshPro[] statValueText { get; private set; }
    [field: SerializeField] public GemSlot[] gemSlotsFront { get; private set; } 


    [field: Header("Back")]
    // [field: SerializeField] public GemSlot[] gemSlots { get; private set; } 
    [field: Header("Misc")]

    [field: SerializeField] public SpriteRenderer[] cardHighlightRenderers { get; private set; }
    [field: SerializeField] public SpriteRenderer cardSprite { get; private set; }
    // [field: SerializeField] public SpriteRenderer cardBackSprite { get; private set; }
    [field: SerializeField] public SpriteRenderer shredMark { get; private set; }
    [field: SerializeField] public TextMeshPro qualityText { get; private set; }
    [field: SerializeField] public TextMeshPro levelText { get; private set; }
    [field: SerializeField] public Transform innerTransform { get; private set; }
    [field: SerializeField] public int rotateSpeed { get; private set; } = 5;
    [field: SerializeField] public bool MouseOver { get; private set; } = false;
    [field: SerializeField] public bool Selected { get; set; } = false;
    [field: SerializeField] public bool Highlighted { get; set; } = false;
    [field: SerializeField] public bool MarkedForShred { get; set; } = false;


    public void Start()
    {
        GameManager.Instance.inputReader.ButtonLeft += FlipCard;
    }

    public void OnDestroy()
    {
        GameManager.Instance.inputReader.ButtonLeft -= FlipCard;

    }

    public void Update()
    {
        if (Selected && MouseOver)
        {
            innerTransform.localRotation = Quaternion.Lerp(innerTransform.localRotation,
                Quaternion.Euler(0, 180, 0),
                Time.deltaTime * rotateSpeed);
            
        }
        else
        {
            MouseOver = false;
            innerTransform.localRotation = Quaternion.Lerp(innerTransform.localRotation, Quaternion.Euler(0, 0, 0),
                Time.deltaTime * rotateSpeed);
            
        }
    }

    public void InsertItem(EquipmentDataContainer item)
    {
        MouseOver = false;
        SetShredMark(false);
        EquipmentData = item;
        // itemSprite.sprite = item.itemCore.icon;
        microCard.InsertItem(item);
        // title.text = item.itemCore.cardTitle;
        // qualityText.text = item.quality.ToString();
        // levelText.text = "lv."+ item.level;
        DOTween.To(() => levelText.text, x => levelText.text = x, "lv."+ item.level, 0.5f);
        DOTween.To(() => qualityText.text, x => qualityText.text = x, item.quality.ToString(), 0.5f);
        DOTween.To(() => title.text, x => title.text = x, item.itemCore.cardTitle, 0.5f);

        DOTween.To(() => qualityText.color, x => qualityText.color = x,  GameManager.Instance.colors[(int)item.quality], 0.5f);
        DOTween.To(() => levelText.color, x => levelText.color = x,  GameManager.Instance.colors[(int)item.quality], 0.5f);
        for (var i = 0; i < statTitleText.Length; i++)
        {
            var i1 = i;

            if (i<item.stats.Length&&item.stats[i] !=EquipmentDataContainer.Stats.None)
            {
                DOTween.To(() => statValueText[i1].text, x => statValueText[i1].text = x,item.statValue[i1].ToString(), 0.5f);
                DOTween.To(() => statTitleText[i1].text, x => statTitleText[i1].text = x,item.stats[i1].ToString(), 0.5f);

                // statValueText[i].text = item.statValue[i].ToString();
                // statTitleText[i].text = item.stats[i].ToString();
            }
            else
            {
                DOTween.To(() => statValueText[i1].text, x => statValueText[i1].text = x,"", 0.5f);
                DOTween.To(() => statTitleText[i1].text, x => statTitleText[i1].text = x,"", 0.5f);
            }

            /*if (i<item.itemCore.guaranteeStats.Length)
            {
                statText[i].fontStyle = FontStyles.Underline;
            }
            else
            {
                statText[i].fontStyle = FontStyles.Normal;
            }*/
        }
        
        for (var i = 0; i < gemSlotsFront.Length; i++)
        {
            // GemSlot gemSlot = gemSlots[i];
            // gemSlot.ClearGem();
            // gemSlot.SetLock(item.lockedSlots[i]);
            
            GemSlot gemSlot = gemSlotsFront[i];
            gemSlot.ClearGem();
            gemSlot.SetLock(item.lockedSlots[i]);
        }

        for (int i = 0; i < item.gemSlots; i++)
        {
            // gemSlots[i].gameObject.SetActive(true);
            // gemSlots[i].SetQuality(item.quality);
            gemSlotsFront[i].gameObject.SetActive(true);
            gemSlotsFront[i].SetQuality(item.quality);
            AbilityGem ability = item.GetAbility(i);
            if (ability?.GetAbility() != null)
            {
                // gemSlots[i].SetGem(ability);
         
                gemSlotsFront[i].SetGem(ability);
            }
        }
        
        for(int i = item.gemSlots; i < 3; i++)
        {
            // gemSlots[i].gameObject.SetActive(false);
            gemSlotsFront[i].gameObject.SetActive(false);
        }

        DOTween.To(() => cardSprite.color, x => cardSprite.color = x,  GameManager.Instance.colors[(int)item.quality], 0.5f);
        // cardBackSprite.color = GameManager.Instance.colors[(int)item.quality];
        for (var i = 0; i < cardHighlightRenderers.Length; i++)
        {
            cardHighlightRenderers[i].gameObject.SetActive(false);
        }
    }
    
    public void SetHighlighted(bool highlighted)
    {
        Highlighted = highlighted;
        for (var i = 0; i < cardHighlightRenderers.Length; i++)
        {
            int color = (int)EquipmentData.quality;
            var i1 = i;
            DOTween.To(() => cardHighlightRenderers[i1].color, x => cardHighlightRenderers[i1].color = x,  GameManager.Instance.colors[color], 0.5f);

            cardHighlightRenderers[i].gameObject.SetActive(highlighted);
        }
    }

    public void TestInsert()
    {
        InsertItem(GameManager.Instance.lootManager.GetItemCard());
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        FlipCard();
    }

    public void FlipCard()
    {
        if (Selected)
        {
            MouseOver = !MouseOver;;
        }
    }

    public void ShredMarkUpdate()
    {
        shredMark.gameObject.SetActive(MarkedForShred);
    }

    public void ToggleShredMark()
    {
        SetShredMark(!MarkedForShred);
    }
    public void SetShredMark(bool value)
    {
        MarkedForShred = value;
        ShredMarkUpdate();
    }
}
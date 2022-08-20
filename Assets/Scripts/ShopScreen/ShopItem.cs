using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemIcon;
    [SerializeField] private SpriteRenderer shadow;
    [SerializeField] private TextMeshPro itemCountText;
    [SerializeField] private bool selected;
    [field:SerializeField] public string itemName{get;private set;}
    [field:SerializeField] public string itemDescription{get;private set;}
    [field:SerializeField] public int itemPrice{get;private set;}
    [field:SerializeField] public ItemType consumableID{get;private set;}
    [field:SerializeField] public int itemCount{get;private set;}

    void Start()
    {
    }

    void Update()
    {
        Vector3 iconPosition = itemIcon.transform.localPosition;
        iconPosition = Vector3.Lerp(iconPosition, new Vector3(iconPosition.x, selected ? .5f : 0, iconPosition.z),
            Time.deltaTime * 10);
        itemIcon.transform.localPosition = iconPosition;

        Transform shadowTransform = shadow.transform;
        iconPosition = shadowTransform.localPosition;
        iconPosition = Vector3.Lerp(iconPosition, new Vector3(iconPosition.x, selected ? -1f : -0.5f, iconPosition.z),
            Time.deltaTime * 10);
        shadow.transform.localPosition = iconPosition;

        //iconposition becomes shadowscale
        iconPosition = shadowTransform.localScale;
        iconPosition = Vector3.Lerp(iconPosition, selected ? new Vector3(0.5f, 0.25f, 2) : new Vector3(1, 0.5f, 2),
            Time.deltaTime * 10);
        shadow.transform.localScale = iconPosition;
        itemCountText.transform.localPosition = shadowTransform.localPosition + (Vector3.up*0.5f);
    }

    void FixedUpdate()
    {

    }

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }

    public void ToggleSelected()
    {
        selected = !selected;
    }

    public void Buy()
    {
        if (itemCount > 0)
        {
            if (GameManager.Instance.runPlayer.credits >= itemPrice)
            {
                GameManager.Instance.runPlayer.SpendCredits(itemPrice);
                itemCount--;
                UpdateVisual();
                TextPopController.Instance.PopPositive("+1 "+itemName, transform.position, false);

            }
            else
            {
                TextPopController.Instance.PopNegative("Too Expensive", transform.position, false);
            }
        }
        else
        {
            TextPopController.Instance.PopNegative("No Stock", transform.position, false);
        }
    }

    public void UpdateVisual()
    {
        itemIcon.color = itemCount>0 ? Color.white : Color.gray;
        itemCountText.text = "x"+itemCount;
    }

    public void SetAmount(int amount)
    {
        itemCount = amount;
        UpdateVisual();
    }
    
    public enum ItemType
    {
        Coffee,
        Pie,
        Tea
    }
}

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
    [field:SerializeField] public int consumableID{get;private set;}
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
        iconPosition = Vector3.Lerp(iconPosition, new Vector3(iconPosition.x, selected ? -4f : -2, iconPosition.z),
            Time.deltaTime * 10);
        shadow.transform.localPosition = iconPosition;

        iconPosition = shadowTransform.localScale;
        iconPosition = Vector3.Lerp(iconPosition, selected ? new Vector3(2, 1, 2) : new Vector3(4, 2, 4),
            Time.deltaTime * 10);
        shadow.transform.localScale = iconPosition;
        itemCountText.transform.localPosition = shadowTransform.localPosition + Vector3.up;
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
            if (GameManager.Instance.battlefield.player.credits >= itemPrice)
            {
                GameManager.Instance.battlefield.player.SpendCredits(itemPrice);
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
}

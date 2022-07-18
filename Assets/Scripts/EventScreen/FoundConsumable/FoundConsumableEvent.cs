using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoundConsumableEvent : Event
{
    [field: SerializeField] public ShopItem.ItemType itemType { get; private set; } = ShopItem.ItemType.Pie;
    [field: SerializeField] public int itemAmt { get; private set; } = 1;
    [field: SerializeField] public TextMeshProUGUI[] text { get; private set; }

    [field: SerializeField] public TextMeshProUGUI leaveButtonText { get; private set; }
    [field: SerializeField] public Button leaveButton { get; private set; }

    private bool isDone = false;

    async void Start()
    {
        await Task.Delay(1000);
        leaveButton.interactable = true;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    public void ToggleText(int textIndex, bool show)
    {
        text[textIndex].gameObject.SetActive(show);
    }

    public void SetItemType(ShopItem.ItemType type)
    {
        itemType = type;
    }

    public void SetItemAmt(int amt)
    {
        itemAmt = amt;
    }

    public void SetItem(ShopItem.ItemType type, int amt)
    {
        SetItemType(type);
        SetItemAmt(amt);
    }

    public void MoveOn()
    {
        eventManager.MoveOn();
    }

    public async void AcceptItems()
    {
        if (!isDone)
        {
            isDone = true;
            ToggleText(1, true);
            leaveButtonText.text = "Leave";
            GameManager.Instance.battlefield.player.AddConsumables((int)itemType, itemAmt);
            await Task.Delay(250);
            TextPopController.Instance.PopPositive("+" + itemAmt + " " + itemType, Vector3.down, true);
            await Task.Delay(250);
            leaveButton.interactable = true;
        }
        else
        {
            MoveOn();
        }
    }
}
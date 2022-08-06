using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ShopItem[] items;
    [SerializeField] Button[] itemButtons;
    [SerializeField] Button[] cardButtons;
    [SerializeField] private TextMeshProUGUI[] itemText;
    [SerializeField] private TextMeshProUGUI creditText;
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private Transform consumableTable;
    [SerializeField] private Transform specialStockTable;
    [SerializeField] private EquipmentCardShell equipmentCardShell;
    private int currentItem = -1;
    private bool viewingSpecialStock = false;
    private bool boughtCard = false;

    async void Start()
    {
        equipmentCardShell.InsertItem(GameManager.Instance.lootManager.GetItemCard());
        foreach (ShopItem item in items)
        {
            item.SetAmount(Random.Range(1, 4));
        }

        UpdateItemText();
        UpdateCreditText();
        await Task.Delay(10);
        GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);

    }

    void Update()
    {
        var localPosition = consumableTable.localPosition;
        localPosition = Vector3.Lerp(localPosition,
            new Vector3(viewingSpecialStock ? -7.5f : 0, localPosition.y, localPosition.z), Time.deltaTime * 10);
        consumableTable.localPosition = localPosition;
        localPosition = specialStockTable.localPosition;
        localPosition = Vector3.Lerp(localPosition,
            new Vector3(viewingSpecialStock ? 0 : 7.5f, localPosition.y, localPosition.z), Time.deltaTime * 10);
        specialStockTable.localPosition = localPosition;
    }

    void FixedUpdate()
    {

    }

    public void BuyShopItem(int slot)
    {
        foreach (ShopItem item in items)
        {
            item.Deselect();
        }

        if (currentItem == slot)
        {
            currentItem = -1;
            items[slot].Buy();
            GameManager.Instance.battlefield.player.AddConsumables((int)items[slot].consumableID, 1);
        }
        else
        {
            currentItem = slot;
            items[slot].Select();
        }

        UpdateItemText();
        UpdateCreditText();
    }

    public void UpdateItemText()
    {
        if (currentItem == -1)
        {
            foreach (TextMeshProUGUI text in itemText)
            {
                text.text = "";
            }

            confirmText.gameObject.SetActive(false);
        }
        else
        {
            itemText[0].text = items[currentItem].itemName;
            itemText[1].text = items[currentItem].itemDescription;
            itemText[2].text = items[currentItem].itemPrice + "C";
            confirmText.gameObject.SetActive(true);
        }
    }

    public void ExitShop()
    {
        GameManager.Instance.battlefield.TotalHandsPlus();
        GameManager.Instance.saveManager.SaveRun();

        GameManager.Instance.LoadSceneAdditive("MapScreen", "ShopScreen");
    }

    public void UpdateCreditText()
    {
        creditText.text = "Wallet: " + GameManager.Instance.battlefield.player.credits + "C";
    }

    public void SpecialStockToggle()
    {
        viewingSpecialStock = !viewingSpecialStock;
        foreach (Button button in itemButtons)
        {
            button.gameObject.SetActive(!viewingSpecialStock);
        }
        foreach (Button button in cardButtons)
        {
            button.gameObject.SetActive(viewingSpecialStock && !boughtCard);
        }
        ClearSelectedItem();

        if (viewingSpecialStock && !boughtCard)
        {
            itemText[0].text = equipmentCardShell.title.text;
            // itemText[1].text = equipmentCardShell.abilityTitle.text;//todo replace with gem description
            itemText[2].text =
                equipmentCardShell.EquipmentData.itemCore.cardCost +
                "C"; //todo replace with value calculated from equipmentData
        }
        
    }

    private void ClearSelectedItem()
    {
        currentItem = -1;
        foreach (ShopItem item in items)
        {
            item.Deselect();
        }

        UpdateItemText();
    }
    
    public void BuyEquipmentCard()
    {
        if (GameManager.Instance.battlefield.player.credits>equipmentCardShell.EquipmentData.itemCore.cardCost)//todo replace with value calculated from equipmentData
        {
            boughtCard = true;
            GameManager.Instance.battlefield.player.AddCardToInventory(equipmentCardShell.EquipmentData);
            GameManager.Instance.battlefield.player.SpendCredits(equipmentCardShell.EquipmentData.itemCore.cardCost);//todo replace with value calculated from equipmentData
            TextPopController.Instance.PopPositive("Card Purchased", equipmentCardShell.transform.position, false);
            equipmentCardShell.gameObject.SetActive(false);
            SpecialStockToggle();
            UpdateCreditText();
        }
        else
        {
            TextPopController.Instance.PopNegative("Too Expensive", equipmentCardShell.transform.position, false);
        }
    }
}
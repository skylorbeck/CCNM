using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class StoreProduct : MonoBehaviour
{
    [SerializeField] private string productId;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button button;

    public void SetProduct(Product product)
    {
        title.text = product.metadata.localizedTitle;
        price.text = product.metadata.localizedPriceString;
        description.text = product.metadata.localizedDescription;
    }

    async void Start()
    {
        while (!IAPManager.Instance.IsInitialized())
        {
            await Task.Delay(500);
        }

        Product product = IAPManager.Instance.GetProduct(productId);
        SetProduct(product);
        button.interactable = product.availableToPurchase && !product.hasReceipt;
    }

    public void ListPurchases()
    {
        IAPManager.Instance.ListPurchases();
    }
    
    public void RestorePurchases()
    {
        IAPManager.Instance.RestorePurchases();
    }

    public void BuyProduct()
    {
        IAPManager.Instance.BuyProductID(productId);
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}
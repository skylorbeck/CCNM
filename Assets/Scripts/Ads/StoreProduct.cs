using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class StoreProduct : MonoBehaviour
{
    [SerializeField] private string productId;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI description;

    public void SetProduct(Product product)
    {
        this.title.text = product.metadata.localizedTitle;
        this.price.text = product.metadata.localizedPrice.ToString();
        this.description.text = product.metadata.localizedDescription;
    }

    async void Start()
    {
        while (!IAPManager.Instance.IsInitialized())
        {
            await Task.Delay(500);
        }
        SetProduct(IAPManager.Instance.GetProduct(productId));
        
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
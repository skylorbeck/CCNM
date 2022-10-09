using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEditor.Advertisements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using Product = UnityEngine.Purchasing.Product;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Instance;

    private IStoreController controller;
    private IExtensionProvider extensions;
    IGooglePlayStoreExtensions googlePlayStoreExtensions;
    private static Product PRODUCT;

    public ProductCollection products { get; private set; }
    public static string DOUBLER = "egodoubler";
    public static string STARTERPACK = "starterpack";

    public UnityAction OnPurchaseComplete;
    async void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //todo remove this
        AdvertisementSettings.testMode = true;
        StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            InitializePurchasing();
        }
        catch (ConsentCheckException e)
        {
            Debug.LogWarning("Consent: :" +
                             e); // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }

    }

    public void OnDestroy()
    {
    }



    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.Configure<IGooglePlayConfiguration>().SetDeferredPurchaseListener(OnDeferredPurchase);

        builder.AddProduct(STARTERPACK, ProductType.NonConsumable, new IDs
        {
            { STARTERPACK, GooglePlay.Name },
        }, new PayoutDefinition(PayoutType.Resource, "Ego", 5000));
        builder.AddProduct(DOUBLER, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
        products = controller.products;
        googlePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
        // Debug.Log("OnInitialized: PASS");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        bool validPurchase = true;
        PRODUCT = args.purchasedProduct;
#if !UNITY_EDITOR
        try
        {
            var validator =
                new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
            var result = validator.Validate(args.purchasedProduct.receipt);
        }
        catch (IAPSecurityException)
        {
            validPurchase = false;
        }
#endif
        if (googlePlayStoreExtensions.IsPurchasedProductDeferred(PRODUCT))
        {
            return PurchaseProcessingResult.Pending;
        }

        if (validPurchase)
        {
            switch (PRODUCT.definition.id)
            {
                case "egodoubler":
                    Debug.Log("Double");
                    GameManager.Instance.metaPlayer.doublerOwned = true;
                    GameManager.Instance.adManager.HideBanner();
                    break;

                case "starterpack":
                    if (!GameManager.Instance.metaPlayer.doublerOwned)
                    {
                        GameManager.Instance.metaPlayer.doublerOwned = true;
                        GameManager.Instance.metaPlayer.AddEgo(5000);
                    }
                    GameManager.Instance.adManager.HideBanner();
                    Debug.Log("Starter Pack");
                    break;
            }
            OnPurchaseComplete?.Invoke();
            return PurchaseProcessingResult.Complete;
        }
        else
        {
            Debug.Log("Invalid Purchase");
            return PurchaseProcessingResult.Pending;
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogWarning(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
            product.definition.storeSpecificId, failureReason));
    }
    

    public bool IsInitialized()
    {
        return controller != null && extensions != null;
    }

    void OnDeferredPurchase(Product product)
    {
        Debug.Log($"Purchase of {product.definition.id} is deferred");
    }

    public Product GetProduct(string productID)
    {
        return controller.products.WithID(productID);
    }

    public void ListPurchases()
    {
        foreach (Product item in controller.products.all)
        {
            if (item.hasReceipt)
            {
                Debug.Log("In list for  " + item.receipt);
            }
            else
                Debug.Log("No receipt for " + item.definition.id);
        }
    }

    public void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = controller.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product:" + product.definition.id));
                controller.InitiatePurchase(product);
            }
            else
            {
                Debug.LogWarning(
                    "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.LogWarning("BuyProductID FAIL. Not initialized.");
        }
    }

    public void CompletePurchase()
    {
        if (PRODUCT == null)
            Debug.LogWarning("Cannot complete purchase, product not initialized.");
        else
        {
            controller.ConfirmPendingPurchase(PRODUCT);
            Debug.LogWarning("Completed purchase with " + PRODUCT.transactionID);
        }

    }

    public void RestorePurchases()
    {
        foreach (Product item in controller.products.all)
        {
            if (item.hasReceipt)
            {
                switch (item.definition.id)
                {
                    case "egodoubler":
                        Debug.Log("Double");
                        GameManager.Instance.metaPlayer.doublerOwned = true;
                        GameManager.Instance.adManager.HideBanner();
                        break;

                    case "starterpack":
                        if (!GameManager.Instance.metaPlayer.doublerOwned)
                        {
                            GameManager.Instance.metaPlayer.doublerOwned = true;
                            GameManager.Instance.metaPlayer.AddEgo(5000);
                        }
                        GameManager.Instance.adManager.HideBanner();
                        Debug.Log("Starter Pack");
                        break;
                }
            }
            
        }
        
        /*extensions.GetExtension<IAppleExtensions>().RestoreTransactions(result =>
        {

            if (result)
            {
                Debug.Log("Restore purchases succeeded.");
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "restore_success", true },
                };
                AnalyticsService.Instance.CustomData("myRestore", parameters);
            }
            else
            {
                Debug.LogWarning("Restore purchases failed.");
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "restore_success", false },
                };
                AnalyticsService.Instance.CustomData("myRestore", parameters);
            }

            AnalyticsService.Instance.Flush();
        });*/

    }
}
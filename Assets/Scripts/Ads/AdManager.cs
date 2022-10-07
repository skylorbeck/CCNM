using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string androidGameId;
    [SerializeField] string iOSGameId;
    [SerializeField] bool testMode = false;
    private string gameId;

    [SerializeField] private BannerAd bannerAd;
    void Awake()
    {
        InitializeAds();
    }
 
    public void InitializeAds()
    {
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOSGameId
            : androidGameId;
        Advertisement.Initialize(gameId, testMode, this);
    }
    
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        if (!GameManager.Instance.metaPlayer.doublerActive)
        {
            bannerAd.LoadBanner();
        }
    }
 
    public void HideBanner()
    {
        bannerAd.HideBannerAd();
    }
    
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}

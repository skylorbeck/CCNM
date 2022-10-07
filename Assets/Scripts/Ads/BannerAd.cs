using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
 
public class BannerAd : MonoBehaviour
{
    [SerializeField] BannerPosition bannerPosition = BannerPosition.TOP_CENTER;
 
    [SerializeField] string androidAdUnitId = "Banner_Android";
    [SerializeField] string iOSAdUnitId = "Banner_iOS";
    string adUnitId = null; // This will remain null for unsupported platforms.
 
    void Start()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif
        // Set the banner position:
    }
 
    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        Advertisement.Banner.SetPosition(bannerPosition);
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
 
        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(adUnitId, options);
    }
 
    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        ShowBannerAd();
    }
 
    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }
 
    // Implement a method to call when the Show Banner button is clicked:
    void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
 
        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(adUnitId, options);
    }
 
    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }
 
    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }
 
    void OnDestroy()
    {
        
    }
}
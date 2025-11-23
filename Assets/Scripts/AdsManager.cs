using System;
using System.IO;
using GoogleMobileAds.Api;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{

    // Singleton instence
    public static AdsManager Instance;

    // Ad Unit IDs (use test IDs for development)
#if UNITY_ANDROID
    private string bannerAdUnitId = "ca-app-pub-6947253833983054/7290154465";
    private string interestitialAdUnitId = "ca-app-pub-6947253833983054/5154945717";
#elif UNITY_IPHONE
    private string bannerAdUnitId = "ca-app-pub-6947253833983054/7290154465";
    private string interestitialAdUnitId = "ca-app-pub-6947253833983054/5154945717";
#else
    private string bannerAdUnitId = "unused";
    private string interestitialAdUnitId = "unused";
    
#endif


    private BannerView bannerView;
    private InterstitialAd interstitialAd;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }


    void Start()
    {
        // Initialize the google mobile ads SDK
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Admob SDK Initilized");
            //Load ads after initialization
            LoadBannerAd();
            LoadInterstitialAd();
            
        });
    }

    private void LoadBannerAd()
    {
        // Create a banner view at the bottom of the screen
        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request
        AdRequest request = new AdRequest();

        // Register event handlers for the banner ad
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded.");
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("Banner ad failed to load: " + error.GetMessage());
        };

        // Load the banner ad
        bannerView.LoadAd(request);


    }

    private void LoadInterstitialAd() {

        // Clean up any existing interstitial ad
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        // Load a new interstitial ad
        InterstitialAd.Load(interestitialAdUnitId, new AdRequest(), (InterstitialAd ad, LoadAdError error) => {

            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + error?.GetMessage());
                return;
            };

            interstitialAd = ad;
            Debug.Log("Interstitial ad loaded");

            // Register ad events
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial ad closed");
                LoadInterstitialAd(); // Preload the next ad
            };

            interstitialAd.OnAdFullScreenContentFailed += (error) =>
            {
                Debug.Log("Interstitial ad failed to show: " + error.GetMessage());
            };
        });

    }


    public void ShowInterstialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready");
        }
    }
}
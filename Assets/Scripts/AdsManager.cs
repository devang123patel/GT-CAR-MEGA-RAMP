using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void DestroyAd()
    {
        // Placeholder for ad destruction logic
        Debug.Log("Ad destroyed");
    }
    
    public void ShowInterstitialAd()
    {
        // Placeholder for showing interstitial ads
        Debug.Log("Showing interstitial ad");
    }
    
    public void ShowRewardedAd()
    {
        // Placeholder for showing rewarded ads
        Debug.Log("Showing rewarded ad");
    }
}
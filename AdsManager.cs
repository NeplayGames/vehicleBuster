
using UnityEngine;
using UnityEngine.Advertisements;
public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    string gameId = "3990731";
    [SerializeField] bool testMode = true;
    // Start is called before the first frame update
    [SerializeField] GameObject adButton;
    SoundController soundController;
     [SerializeField] GameInfoSaver gameInfoSaver;
    [SerializeField] MenuManager menuManager;
    [SerializeField] ShopManager shopManager;
    Player player;
    bool coinBonusAd = false;
    bool continueBonusAd = false;
    public bool showContinue = false;
    
    public void ShowContinueButton()
    {
        if (Advertisement.IsReady(myPlacementId))
            showContinue = true;
    }
    public void OnUnityAdsDidError(string message)
    {

    }
    public bool AdsCheck()
    {
        if (Advertisement.IsReady(myPlacementId)) return true;
        return false;

    }
    string interstitialAd = "video";
    public void ShowRewardedVideo()
    {
        coinBonusAd = true;
        if (Advertisement.IsReady(myPlacementId))
            Advertisement.Show(myPlacementId);
    }
    public void ShowRewardedVideoForContinue()
    {
        continueBonusAd = true;
        menuManager.tempTime = 0f;
        if (Advertisement.IsReady(myPlacementId))
        {
            Advertisement.Show(myPlacementId);
        }
    }
    public void ShowInterstitialAdVideo()
    {
        if (Advertisement.IsReady(interstitialAd))
            Advertisement.Show(interstitialAd);
    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {

        if ((showResult == ShowResult.Failed || showResult == ShowResult.Finished) && coinBonusAd)
        {
            player = gameInfoSaver.ReadPlayerFile();
            gameInfoSaver.SaveInfo(new Player(player.TotalBuster() + 100, player.CurrentHighScore(), player.CarName()));
            shopManager.UpdatePrice(100);
            adButton.SetActive(false);
        }
        else if ((showResult == ShowResult.Failed || showResult == ShowResult.Finished) && continueBonusAd && (soundController.gamePlayed +1 )%6 !=0)
        {
            //print("ave");
            menuManager.Continue();
        }
        

    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }
    string myPlacementId = "rewardedVideo";
    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == myPlacementId && !menuManager.startGame)
        {

            adButton.SetActive(true);
        }




    }
    private void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
    void Start()
    {
        soundController = FindObjectOfType<SoundController>();
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }



}

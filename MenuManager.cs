
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button StatButton;
    [SerializeField] GameObject shopManager;
    [SerializeField] GameObject shopCamera;
    [SerializeField] GameObject mainCamera;
    [SerializeField] AudioClip buttonPress;
    [SerializeField] GameObject gameController;
    [SerializeField] GameObject gameControllerEnclosedMode;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject restartContinueButton;
    [SerializeField] GameObject pauseButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button statButton;
    [SerializeField] GameObject rawImage;
    [SerializeField] GameObject backGroundImage;
    [SerializeField] GameObject statCanvas;
    [SerializeField] GameObject VehiclestatCanvas;
  [SerializeField] GameObject plane;
  [SerializeField] GameObject footballGround;
    [SerializeField] GameObject HealthBarCanvas;
    [SerializeField] GameObject TotalBusterCanvas;
    [SerializeField] GameObject AdsButton;
    [SerializeField] GameObject musicButton;
    [SerializeField] GameObject GameSelectionCanvas;
    //  [SerializeField] GameObject highScoreCanvas;
    [SerializeField] GameObject backGroundCanvas;
    [SerializeField] GameObject vibrationButton;
    [SerializeField] GameObject continueGameButton;
    [SerializeField] GameObject playerStatCanvas;
    [SerializeField] Button upgradeButton;
    [SerializeField] Sprite pause;
    [SerializeField] Sprite play;
    [SerializeField] AdsManager adsManager;

    SoundController soundController;
    public GameObject currentItem;
    ShopManager shopMana;
    VehicleMovement vehicleMovement;
   //public GameObject currentPlayer;
    // public bool showShop = false;
    public bool startGame = false;
    float perFrameTime;
    public bool freeMode = true;
    // Start is called before the first frame update
    private void Start()
    {

        soundController = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundController>();
        shopMana = shopManager.GetComponent<ShopManager>();
        source = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
       // vehicleMovement = FindObjectOfType<VehicleMovement>();
        //      AdsButton.SetActive(true);
        playAudio = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().isPlaying;
        musicButton.GetComponent<Image>().sprite = playAudio ? play : pause;
        perFrameTime = Time.deltaTime;
        ShowShop();
    }

  public  void Upgrade()
    {
        PressedButton(upgradeButton);
        soundController.PlayCrash(buttonPress);
        // shopManager.GetComponent<ShopManager>().HideVehicle();
        shopManager.GetComponent<ShopManager>().showShop = false;
        VehiclestatCanvas.SetActive(true);
        rawImage.SetActive(false);
        playerStatCanvas.SetActive(false);
        shopMana.showShop = false;

        TotalBusterCanvas.SetActive(true);
    }
    public void ShowGameOption()
    {
        if (!shopMana.carLockedState)
        {
            soundController.PlayCrash(buttonPress);
            GameSelectionCanvas.SetActive(true);
           playFootballGroundMode= shopMana.FootballGroundRunStatus();
        }
        else
        {
            shopMana.PurchaseVehicle();
        }

       
    }
    public void HideGameOption()
    {
        soundController.PlayCrash(buttonPress);
        GameSelectionCanvas.SetActive(false);
    }
    public void ShowShop() {
        soundController.PlayCrash(buttonPress);
        shopMana.showShop = true;
        shopMana.ChangeItem(0);
        shopManager.SetActive(true);
        shopCamera.SetActive(true);
        playerStatCanvas.SetActive(false);
        VehiclestatCanvas.SetActive(false);
        TotalBusterCanvas.SetActive(true);
        rawImage.SetActive(true);
        PressedButton(shopButton);    
    }
    Transform temp;
    void PressedButton(Button button)
    {
        if(temp!=null)
        temp.position -= temp.up * 20f;
        button.transform.position += button.transform.up * 20f;
        temp = button.transform;
    }
    public void ShowStatCanvas()
    {
        shopMana.showShop = false;

        soundController.PlayCrash(buttonPress);
        PressedButton(statButton);
        playerStatCanvas.SetActive(true);
        VehiclestatCanvas.SetActive(false);
        rawImage.SetActive(false);
        shopMana.UpdateStat();
    }
    public void Continue()
    {
        if (freeMode)
            gameController.SetActive(true);
        else
            gameControllerEnclosedMode.SetActive(true);
        restartContinueButton.SetActive(false);
        continueGameButton.SetActive(false);
        soundController.gamePlayed++;
        vehicleMovement.source.Play();
        pauseButton.SetActive(true);
        currentItem.SetActive(true);
        vehicleMovement.health = 3;
        vehicleMovement.realHealth = 3;
        vehicleMovement.UpdateSlider();
        IsPause();
        restartButton.SetActive(false);


    }
    public void Restart()
    {
        Time.timeScale = 1;
   if(freeMode)
        gameController.GetComponent<GameController>().SavePlayerInfo();
      else
            gameControllerEnclosedMode.GetComponent<GameControllerForEnclisedMode>().SavePlayerInfo();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }
   
 //   [SerializeField] AudioClip audioClip;

    public void StartGame()
    {
       
           
        
        playerStatCanvas.SetActive(false);
        shopMana.EndEffect();

        soundController.PlayCrash(buttonPress);
       AdsButton.SetActive(false);
        HealthBarCanvas.SetActive(true);
        upgradeButton.gameObject.SetActive(false);
           shopMana.selectedCar.gameObject.SetActive(false);
        shopCamera.SetActive(false);
        mainCamera.SetActive(true);
        currentItem.transform.parent = GameObject.FindGameObjectWithTag("car").transform;
        statCanvas.SetActive(true);
        vehicleMovement = currentItem.GetComponent<VehicleMovement>();
        vehicleMovement.showCase = false;

        currentItem.SetActive(true);
        shopButton.gameObject.SetActive(false);
      //  currentPlayer = GameObject.FindGameObjectWithTag("Player");
        pauseButton.SetActive(true);
        startGame = true;
       
        playButton.SetActive(false);
        shopManager.SetActive(false);
        vibrationButton.SetActive(false);
        TotalBusterCanvas.SetActive(false);
        rawImage.SetActive(false);
        backGroundImage.SetActive(false);
        GameSelectionCanvas.SetActive(false);
        StatButton.gameObject.SetActive(false);
    }
    [SerializeField] Image iamge;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] Sprite playSprite;
    public bool isPaused;

    public void IsPause()
    {

        soundController.PlayCrash(buttonPress);
        isPaused = !isPaused;
        if (isPaused)
        {
          
            vibrationButton.SetActive(true);
          //  adsManager.ShowBannerAds();

            statCanvas.SetActive(false);
            restartButton.SetActive(true);
            Time.timeScale = 0;
            iamge.sprite = pauseSprite;
        }

        else

        {
          //  adsManager.HideBannerAds();

            restartButton.SetActive(false);
            AdsButton.SetActive(false);
        
            vibrationButton.SetActive(false);
            statCanvas.SetActive(true);

            Time.timeScale = 1;
            iamge.sprite = playSprite;

        }
    }
    public void PlayFootBallGroundMode()
    {
      //  adsManager.HideBannerAds();
        playerStatCanvas.SetActive(false);
      
        soundController.PlayCrash(buttonPress);
        AdsButton.SetActive(false);
        HealthBarCanvas.SetActive(true);
        upgradeButton.gameObject.SetActive(false);
        Destroy(shopMana.selectedCar.gameObject);
        shopCamera.SetActive(false);
        mainCamera.SetActive(true);
        Instantiate(currentItem);
        statCanvas.SetActive(true);
        currentItem.SetActive(true);
        shopButton.gameObject.SetActive(false);
       // currentPlayer = GameObject.FindGameObjectWithTag("Player");
        pauseButton.SetActive(true);
        startGame = true;  
        playButton.SetActive(false);
        shopManager.SetActive(false);
        vibrationButton.SetActive(false);
        TotalBusterCanvas.SetActive(false);
        rawImage.SetActive(false);
        backGroundImage.SetActive(false);
        GameSelectionCanvas.SetActive(false);
        StatButton.gameObject.SetActive(false);
    }
     AudioSource source;
   // [SerializeField] AudioSource source2;
    bool playAudio = true;
    public void PlayAudio()
    {
        playAudio = !playAudio;
        if (playAudio)
        {
            musicButton.GetComponent<Image>().sprite = play;

            source.Play();
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = pause;

            source.Pause();
        }
    }
    void FinalPage()
    {
        continueGameButton.SetActive(false);
        restartContinueButton.SetActive(false);
        backGroundCanvas.SetActive(true);
        restartButton.SetActive(true);
        gameController.SetActive(false);
        print("here");
        vibrationButton.SetActive(true);

        pauseButton.SetActive(false);
    }
    bool runAds = true;
    
    public void ContinuePage()
    {
        adsManager.ShowContinueButton();
        if (adsManager.showContinue) { 
            if ( runAds)
            {
                runAds = false;
                Invoke("ContinueGame", 0.3f);
            }

            else
            {
                runAds = true;
                Invoke("FinalPage", 0.3f);

            }
    }
        else
        {
            runAds = true;
            Invoke("FinalPage", 0.3f);

        }
    }
    public float tempTime = 0;
   
    private void Update()
    {
        if (tempTime > 0)
        {
            tempTime -= perFrameTime;
            if (tempTime < 1)
            {
               // FinalPage();
            }
        }
       
    }
    void ContinueGame()
    {
       // gameController.SetActive(false);

        //backGroundCanvas.SetActive(true);
        restartContinueButton.SetActive(true);
        pauseButton.SetActive(false);
        continueGameButton.SetActive(true);
        tempTime = 6.5f;
            Time.timeScale = 0;

    }
    bool playFootballGroundMode = false;
    public void PlayFootballMode()
    {
        if (playFootballGroundMode)
        {
            shopMana.FootballGroundRunStatus();
            soundController.PlayCrash(buttonPress);
            currentItem = shopMana.carNew;
            StartGame();
            freeMode = false;
            VehiclestatCanvas.SetActive(false);
            plane.SetActive(false);
            footballGround.SetActive(true);
            mainCamera.SetActive(true);
            gameControllerEnclosedMode.SetActive(true);
            shopCamera.SetActive(false);
            // purchaseButton.SetActive(false);
            TotalBusterCanvas.SetActive(false);
            shopMana.SaveCarInfo();
        }
        else
        {
            
        }
      



      
    }
    public void PurchasedGoButton()
    {
        
            soundController.PlayCrash(buttonPress);

            currentItem = shopMana.carNew;

        gameController.SetActive(true);

        StartGame();
            VehiclestatCanvas.SetActive(false);
           // VehiclestatImage.SetActive(false);
            mainCamera.SetActive(true);
            shopCamera.SetActive(false);
           // purchaseButton.SetActive(false);
            TotalBusterCanvas.SetActive(false);
            shopMana.SaveCarInfo();
        
     
    }
}

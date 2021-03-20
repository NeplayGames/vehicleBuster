using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
   // [SerializeField] GameObject[] car;
    [SerializeField] Text purchasedInfo;
    [SerializeField] Slider speed;
    [SerializeField] Slider sterring;
    [SerializeField] Slider mechanics;
    [SerializeField] GameObject speedButton;
    [SerializeField] GameObject steeringButton;
    [SerializeField] GameObject healthButton;
    [SerializeField] Text speedText;
    [SerializeField] Text sterringText;
    [SerializeField] Text mechanicsText;
    [SerializeField] GameInfoSaver gameInfoSaver;
    [SerializeField] GameObject infoProvider;
    [SerializeField] GameObject purchaseCoinImage;
   // [SerializeField] GameObject coinImage;
    [SerializeField] AudioClip upgradeSound;
    [SerializeField] AudioClip upgradeStatSound;
    [SerializeField] AudioClip buttonPress;
    [SerializeField] Text totalBusterText;
    [SerializeField] Text totalUnlockCarText;
    [SerializeField] Text unlockFourCarText;
    [SerializeField] Text highScoreOnFootballGround;
    VehicleMovement vMoment;
    private Vector2 initialPosition;
    public bool carLockedState;
    Touch touch;
    float rotation;
    public Transform selectedCar;
    float tempValue;
    Vector2 direction;
    public GameObject carNew;
    int totalBuster;
    int highScore;
    public Car selectCar;
    CarStatus selectCarStatus;
    string carName;
    SoundController soundController;
    public bool showShop = false;
    bool allowEffects = false;
    int highScoreFootball;
    [SerializeField] Image carLockStateImage;
    [SerializeField] Image footballModeLockImage;
    [SerializeField] Sprite playIcon;[SerializeField] Sprite lockIcon;
    bool start = false;
    int j = 0;
    private void OnEnable()
    {
        StartCoroutine(StartGame());

    }
    public int childCount;
    IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        gameInfoSaver.UpdateInfo();
        totalBuster = gameInfoSaver.totalBuster;
        highScore = gameInfoSaver.highScore;
        highScoreFootball = gameInfoSaver.highScoreOnFootballMode;
        tMPro.text = totalBuster.ToString();
        carName = gameInfoSaver.carName;
        soundController = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundController>();
        start = true;
        childCount = transform.childCount;
        for(int i = 0;i< childCount; i++)
        {
            if (carName != transform.GetChild(i).name)
                j++;
            else
            {
                ChangeItem(j);
                break;
            }
        }
    }
    
    bool CheckUIObjectsInPosition(Vector2 position)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = position;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
        if (raycastResults.Count > 0)
        {
            foreach (var go in raycastResults)
            {
                if (go.gameObject.name == "ImageCar" || go.gameObject.name == "RawImageCar") return true;
            }
        }
        return false;
    }
        void Update()
    {
        if (!start) return;
        selectedCar.Rotate(Vector3.up * 100 * Time.deltaTime, Space.World);

        if (timeOfEffects > 0)
        {
            allowEffects = true;
            timeOfEffects -= Time.fixedDeltaTime;

            valueToLerp =endValue- Mathf.Lerp(startValue, endValue, timeOfEffects / 2f);


            UpdateEffects();
        }
        else if(allowEffects)
        {
            allowEffects = false;

            EndEffect();
        }
        if (showShop)
        if (Input.touchCount > 0)
        {
            touch = Input.touches[0];
                print(touch);

                if (!CheckUIObjectsInPosition(Input.GetTouch(0).position))
            { 
            
            }
            else
                if (touch.phase == TouchPhase.Began)
            {
                initialPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {     
               direction = touch.position - initialPosition;
            }
        }
        else if(direction.x!=tempValue)
        {
            

                ChangeItem(-(int)Mathf.Sign(direction.x));
            tempValue = direction.x;
        }



    }

    public void EndEffect()
    {
        foreach (Transform g in selectedCar.transform.GetComponentsInChildren<Transform>())
        {
            if (g.GetComponent<MeshRenderer>())
            {
                foreach (Material a in g.GetComponent<MeshRenderer>().materials)
                {
                    a.DisableKeyword("_EMISSION");
                }
            }
        }
    }

    int currentItem = 0;
    public void ChangeItem(int changeValue)
    {
   
        if (childCount <= 0) return;


        currentItem += changeValue;

    
        if (currentItem > childCount - 1) currentItem = 0;
        else if (currentItem < 0) currentItem = childCount - 1;

  
        if (selectedCar)
        {
   
            rotation = selectedCar.eulerAngles.y;
            selectedCar.GetComponent<VehicleMovement>().showCase = false;

            //Destroy(selectedCar.gameObject);
            selectedCar.gameObject.SetActive(false);
        }

 
        if (transform.GetChild(currentItem))
        {
            carNew = transform.GetChild(currentItem).gameObject;
            //  selectedCar = Instantiate(car[currentItem].transform, new Vector3(3f, 1.9f, 0), Quaternion.identity) as Transform;
            selectedCar = transform.GetChild(currentItem).transform;
            selectedCar.gameObject.SetActive(true);
            selectedCar.position = new Vector3(3f, 1.9f, 0);
          
            gameInfoSaver.currentCar = selectCar;
           vMoment = selectedCar.GetComponent<VehicleMovement>();
            selectCar = gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name, vMoment.lockState, vMoment.busterRequired, (int)((vMoment.forwardSpeed - 29) / 1f), (int)((vMoment.turnSpeed - 170) / 20), vMoment.health - 3);
            selectCarStatus = gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name, selectCar.SpeedLevel(), selectCar.SteeringLevel(), selectCar.MechanicsLevel());

            UpdateSlider(selectCar,selectCarStatus);
            UpdateCarLevel();
           vMoment.showCase = true;
            selectedCar.localScale = new Vector3(0.3f, 0.3f, 0.3f) ;
            selectedCar.eulerAngles = Vector3.up * rotation;
            PurchasedInfo(selectCar.CarPrice());

        }

    }

    private void UpdateCarLevel()
    {
        if (selectCar.SteeringLevel() > selectCarStatus.SteeringLevel())
            steeringButton.SetActive(false);
            else
        steeringButton.SetActive(true);
        if (selectCar.SpeedLevel()> selectCarStatus.SpeedLevel())
            speedButton.SetActive(false);
        else
        speedButton.SetActive(true);
        if (selectCar.MechanicsLevel() > selectCarStatus.MechanicsLevel())
            healthButton.SetActive(false);
            else
        healthButton.SetActive(true);

    }

    public void UpdatePrice(int price)
    {

        totalBuster += price;
        tMPro.text = totalBuster.ToString();
      

    }
    public int TotalCar()
    {
        int j = 0;
        for(int i = 0; i < childCount; i++)
        {
            if (gameInfoSaver.ReadCarFile(transform.GetChild(i).name)!=null && !gameInfoSaver.ReadCarFile(transform.GetChild(i).name).LockState()) j++;
            
        }
        return j;
    }
    public bool FootballGroundRunStatus()
    {
        if (TotalCar()<5)
        {
           
            unlockFourCarText.text = "Unlock 5  Cars";
            footballModeLockImage.sprite = lockIcon;
            return false;
        }
        else
        {
            unlockFourCarText.text = "Start";
            footballModeLockImage.sprite = playIcon;
            return true;
        }
        
    }
    public void UpdateStat()
    {

        totalBusterText.text = totalBuster.ToString();
        totalUnlockCarText.text = TotalCar() + "/7";
        highScoreOnFootballGround.text = highScoreFootball.ToString();
    }
    private void UpdateSlider(Car car,CarStatus carStatus)
    {
     
        tMPro.text = totalBuster.ToString();
        carLockedState = car.LockState();
        selectCarStatus = carStatus;
        selectCar = car;
        speed.value= carStatus.SpeedLevel();
        sterring.value=carStatus.SteeringLevel();
        mechanics.value= carStatus.MechanicsLevel();
      
      
        if (speed.value == 6)
            speedText.text = "Max";
        else
        speedText.text = ((car.SpeedLevel()+1) * 50).ToString();
        if (sterring.value == 6)
            sterringText.text = "Max";
        else
            sterringText.text = ((car.SteeringLevel()+1) * 50).ToString();
        if (mechanics.value == 6)
            mechanicsText.text = "Max";
        else
            mechanicsText.text = ((car.MechanicsLevel()+1) * 50).ToString();
    }
    public void InfoProvider(string info)
    {
        infoProvider.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = info;
        Instantiate(infoProvider);
    }
    public void PurchaseVehicle()
    {
        int carPrice = selectedCar.GetComponent<VehicleMovement>().busterRequired;
        if (carPrice <= totalBuster)
        {
            UpdateEffects();
            timeOfEffects = 1.5f;

            soundController.PlayCrash(upgradeSound);
            totalBuster -= carPrice;
            gameInfoSaver.SaveCarInfo(new Car(selectCar.CarName(), false, selectCar.CarPrice(), selectCar.SpeedLevel(), selectCar.SteeringLevel(), selectCar.MechanicsLevel()));
            gameInfoSaver.SaveInfo(new Player(totalBuster, highScore,highScoreFootball, selectCar.CarName()));
           carLockedState = false;
            PurchasedInfo(selectCar.CarPrice());
        }
        else
        {
            InfoProvider("You don't have enough buster coins.");
        }
    }
    public void PurchasedInfo(int price)
    {
        if (carLockedState)
        {
            purchasedInfo.text =price.ToString() ;
         //   purchaseCoinImage.SetActive(true);
            carLockStateImage.sprite = lockIcon;
        }
        else
        {
            purchasedInfo.text = "Start";
           // purchaseCoinImage.SetActive(false);
            carLockStateImage.sprite = playIcon;

        }
        tMPro.text = totalBuster.ToString();
    }
    float timeOfEffects = 0f;
    public void MechanicsUpgrade() 
    {
        if (carLockedState)
        {
            InfoProvider("Purchase the car first.");
            return;
        }
        if (!allowEffects  && selectCar.MechanicsLevel() < 6 && totalBuster >= ((selectCar.MechanicsLevel() + 1) * 50))
        {

           // UpdateEffects();
            timeOfEffects = 0.5f;
            soundController.PlayCrash(upgradeStatSound);
            totalBuster -= (selectCar.MechanicsLevel()+1) * 50;
            gameInfoSaver.SaveCarInfo(new Car(selectCar.CarName(), carLockedState, selectCar.CarPrice(), selectCar.SpeedLevel() , selectCar.SteeringLevel(), selectCar.MechanicsLevel() + 1));
            gameInfoSaver.SaveCarStatusInfo(new CarStatus(selectCarStatus.CarName(), selectCarStatus.SpeedLevel(), selectCarStatus.SteeringLevel(), selectCarStatus.MechanicsLevel() + 1));
            gameInfoSaver.SaveInfo(new Player(totalBuster, highScore,highScoreFootball, carName));
            UpdateSlider(gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name),gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name));
        }
        else if (selectCar.MechanicsLevel() >= 6)
        {
            InfoProvider("You have upgraded to max level.");

        }
        else if (!allowEffects)
        {
            InfoProvider("You don't have enough buster coins.");

        }
    }
    [SerializeField] TextMeshProUGUI tMPro;
  public void SaveCarInfo()
    {
        gameInfoSaver.SaveInfo(new Player(totalBuster, highScore,highScoreFootball, selectCar.CarName()));

    }
    public void SpeedUpgrade()
    {
        if (carLockedState)
        {
            InfoProvider("Purchase the car first.");
            return;
        }
        if (!allowEffects && !carLockedState&& selectCar.SpeedLevel() < 6 && totalBuster >= ((selectCar.SpeedLevel() + 1) * 50))
        {
            timeOfEffects = 0.5f;
            soundController.PlayCrash(upgradeStatSound);
            totalBuster -= (selectCar.SpeedLevel()+1) * 50;
          
            gameInfoSaver.SaveCarInfo(new Car(selectCar.CarName(), carLockedState, selectCar.CarPrice(), selectCar.SpeedLevel() + 1, selectCar.SteeringLevel(), selectCar.MechanicsLevel()));
            gameInfoSaver.SaveCarStatusInfo(new CarStatus(selectCarStatus.CarName(), selectCarStatus.SpeedLevel()+1, selectCarStatus.SteeringLevel(), selectCarStatus.MechanicsLevel()));

            gameInfoSaver.SaveInfo(new Player(totalBuster, highScore,highScoreFootball, carName));
            UpdateSlider(gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name), gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name));

        }
        else if (selectCar.SpeedLevel() >= 6)
        {
            InfoProvider("You have upgraded to max level.");

        }
        else if (!allowEffects)
        {
            InfoProvider("You don't have enough buster coins.");

        }

    }
    public void SterringUpgrade()
    {

        if (carLockedState)
        {
            InfoProvider("Purchase the car first.");
            return;
        }
        if (!allowEffects && !carLockedState && selectCar.SteeringLevel() < 6 && totalBuster >= ((selectCar.SteeringLevel() + 1) * 50))
        {
            timeOfEffects = 0.5f;
            soundController.PlayCrash(upgradeStatSound);

            totalBuster -= (selectCar.SteeringLevel()+1) * 50;
            gameInfoSaver.SaveCarInfo(new Car(selectCar.CarName(), carLockedState, selectCar.CarPrice(), selectCar.SpeedLevel(), selectCar.SteeringLevel() + 1, selectCar.MechanicsLevel()));
            gameInfoSaver.SaveCarStatusInfo(new CarStatus(selectCarStatus.CarName(), selectCarStatus.SpeedLevel(), selectCarStatus.SteeringLevel()+1, selectCarStatus.MechanicsLevel()));

            gameInfoSaver.SaveInfo(new Player(totalBuster, highScore,highScoreFootball, carName));
            UpdateSlider(gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name), gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name));

        }
        else if (selectCar.SteeringLevel() >= 6)
        {
            InfoProvider("You have upgraded to max level.");

        }
        else if (!allowEffects)
        {
            InfoProvider("You don't have enough buster coins.");

        }
    }

    public void ReduceSteeringLevel()
    {

        if (carLockedState)
        {
            InfoProvider("Purchase the car first.");
            return;
        }
        if (selectCarStatus.SteeringLevel() <=  selectCar.SteeringLevel() &&  selectCarStatus.SteeringLevel()> 0 && !carLockedState)
        {
            soundController.PlayCrash(buttonPress);
            gameInfoSaver.SaveCarStatusInfo(new CarStatus( selectCarStatus.CarName(),  selectCarStatus.SpeedLevel(),  selectCarStatus.SteeringLevel() - 1,  selectCarStatus.MechanicsLevel()));
            UpdateSlider(gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name), gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name));
                steeringButton.SetActive(false);

          
        }
       
    }
    public void ReduceSpeedLevel()
    {

        if (carLockedState)
        {
            InfoProvider("Purchase the car first.");
            return;
        }

        if ( selectCarStatus.SpeedLevel() <=  selectCar.SpeedLevel() &&  selectCarStatus.SpeedLevel() > 0 && !carLockedState)
        {
            soundController.PlayCrash(buttonPress);
            gameInfoSaver.SaveCarStatusInfo(new CarStatus( selectCarStatus.CarName(),  selectCarStatus.SpeedLevel() - 1,  selectCarStatus.SteeringLevel(),  selectCarStatus.MechanicsLevel()));
            UpdateSlider(gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name), gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name));
                speedButton.SetActive(false);

         
        }
    }
    public void ReduceHealthLevel()
    {

        if (carLockedState)
        {
            InfoProvider("Purchase the car first.");
            return;
        }
        if ( selectCarStatus.MechanicsLevel() <=  selectCar.MechanicsLevel() &&  selectCarStatus.MechanicsLevel() > 0 && !carLockedState)
        {
            soundController.PlayCrash(buttonPress);
            gameInfoSaver.SaveCarStatusInfo(new CarStatus( selectCarStatus.CarName(),  selectCarStatus.SpeedLevel(),  selectCarStatus.SteeringLevel(),  selectCarStatus.MechanicsLevel() - 1));
            UpdateSlider(gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name), gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name));
            healthButton.SetActive(false);

          
        }
    }
    public void IncreaseSteeringLevel()
    {

        if (carLockedState)
        {
            InfoProvider("Purchase the car first.");
            return;
        }
        if ( selectCarStatus.SteeringLevel() <  selectCar.SteeringLevel() && !carLockedState)
        {
            soundController.PlayCrash(buttonPress);
            gameInfoSaver.SaveCarStatusInfo(new CarStatus( selectCarStatus.CarName(),  selectCarStatus.SpeedLevel(),  selectCarStatus.SteeringLevel() + 1,  selectCarStatus.MechanicsLevel()));
            UpdateSlider(gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name), gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name));
            if (selectCar.SteeringLevel() == selectCarStatus.SteeringLevel())

                steeringButton.SetActive(true);
        }
        else if (selectCarStatus.SteeringLevel() >= 6)
        {
            InfoProvider("You have upgraded to max level.");
        }
        else if (selectCarStatus.SteeringLevel() == selectCar.SteeringLevel())
        {
            InfoProvider("Upgrade vehicle Steering.");

        }
    }
    public void IncreaseSpeedLevel()
    {

        if (carLockedState)
        {
            InfoProvider("Purchase the car first.");
            return;
        }
        if (selectCarStatus.SpeedLevel() < selectCar.SpeedLevel() && !carLockedState)
        {
            soundController.PlayCrash(buttonPress);
            gameInfoSaver.SaveCarStatusInfo(new CarStatus(selectCarStatus.CarName(),  selectCarStatus.SpeedLevel() + 1,  selectCarStatus.SteeringLevel(),  selectCarStatus.MechanicsLevel()));
            UpdateSlider(gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name), gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name));
            if (selectCar.SpeedLevel() == selectCarStatus.SpeedLevel())

                speedButton.SetActive(true);
        }
        else if (selectCarStatus.SpeedLevel() >= 6)
        {
            InfoProvider("You have upgraded to max level.");
        }
        else if (selectCarStatus.SpeedLevel() == selectCar.SpeedLevel())
        {
            InfoProvider("Upgrade vehicle Speed.");

        }
    }
    public void IncreaseHealthLevel()
    {

        if (carLockedState)
        {
            InfoProvider("Purchase the car first.");
            return;
        }
        if (selectCarStatus.MechanicsLevel() < selectCar.MechanicsLevel() && !carLockedState)
        {
            soundController.PlayCrash(buttonPress);
            gameInfoSaver.SaveCarStatusInfo(new CarStatus( selectCarStatus.CarName(),  selectCarStatus.SpeedLevel(),  selectCarStatus.SteeringLevel(),  selectCarStatus.MechanicsLevel() + 1));
            UpdateSlider(gameInfoSaver.ReadCarFile(transform.GetChild(currentItem).name), gameInfoSaver.ReadCarStatusFile(transform.GetChild(currentItem).name));
            if (selectCar.MechanicsLevel() == selectCarStatus.MechanicsLevel())

                healthButton.SetActive(true);
        }
        else if (selectCarStatus.MechanicsLevel() >= 6)
        {
            InfoProvider("You have upgraded to max level.");
        }
        else if(selectCarStatus.MechanicsLevel() == selectCar.MechanicsLevel())
        {
            InfoProvider("Upgrade vehicle Health.");

        }
    }
    float startValue = 0;
    float endValue = 1f;
    float valueToLerp;

  
    public Color hurtFlashColor = new Color(0f, 0f, 0f, 1);
    Color temphurtFlashColor;
    void UpdateEffects()
    {
        temphurtFlashColor = hurtFlashColor + new Color(valueToLerp, 0, 0, 1f);
        foreach (Transform g in selectedCar.transform.GetComponentsInChildren<Transform>())
        {
            if (g.GetComponent<MeshRenderer>())
            {
                foreach (Material a in g.GetComponent<MeshRenderer>().materials)
                {
                    a.EnableKeyword("_EMISSION");
                     a.SetColor("_EmissionColor", temphurtFlashColor);


                }
            }
        }
    }
}

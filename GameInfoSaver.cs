using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class GameInfoSaver : MonoBehaviour
{
    public Player player;
  // [SerializeField] GameObject[] players;
    [SerializeField] MenuManager menuManager;
    [SerializeField] Text text;
    [SerializeField] Sprite vibrates;
    [SerializeField] Sprite NoVibrate;
    [SerializeField] Image vibrateImage;
    [SerializeField] Image soundImage;
    [SerializeField] Sprite pause;
    [SerializeField] Sprite play;
    [SerializeField] AudioClip clip;
    SoundController soundController;
    public int highScoreOnFootballMode;
    public Car currentCar;
   public  int totalBuster;
    public int highScore;
    public string carName;
    public bool firstGame = false;
    private void Start()
    {
        string path = Application.persistentDataPath + "/player.sav";
        soundController = FindObjectOfType<SoundController>();
        if (File.Exists(path))
        {

            using (FileStream stream = File.Open(path, FileMode.Open))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                player = (Player)formatter.Deserialize(stream);
                
            }
        }
           
        else
        {
            firstGame = true;
            player = new Player(0,0, 0,"Car1");
            SaveInfo(player);
        }
        totalBuster = player.TotalBuster();
        highScore = player.CurrentHighScore();
        highScoreOnFootballMode = player.HighScoreOnFootballMode();
        carName = player.CarName();
        text.text = highScore.ToString();
        FindCurrentPlayer();
        vibrate = soundController.vibrate;
        playSound = soundController.playSound;
            vibrateImage.sprite = vibrate?vibrates:NoVibrate;
        soundImage.sprite = playSound ? play : pause;
      
    }
    bool vibrate = true;
    public void ToggleVibration()
    {
        soundController.vibrate = !soundController.vibrate;
        vibrate = soundController.vibrate;
        Vibrate();
    }
   
   public void Vibrate()
    {
        if (vibrate)
        {
            vibrateImage.sprite = vibrates;
            Handheld.Vibrate();
            return;
        }
        vibrateImage.sprite = NoVibrate;

    }
    private void FindCurrentPlayer()
    {
      for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).name == carName)
            {
                menuManager.currentItem = transform.GetChild(i).gameObject;
                
            }
        }
    }

    public void SaveInfo(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sav";
        using (FileStream stream = File.Open(path, FileMode.Create))
        {

            formatter.Serialize(stream, player);
           
        }
    }
    public void UpdateInfo()
    {
        player = ReadPlayerFile();
        totalBuster = player.TotalBuster();
        highScore = player.CurrentHighScore();
        highScoreOnFootballMode = player.HighScoreOnFootballMode();
        carName = player.CarName();

        FindCurrentPlayer();
    }
   public bool playSound = true;
    public void PauseSound()
    {
        playSound = !playSound;
        soundController.playSound = playSound;
        soundImage.sprite = playSound ? play : pause;
        if (playSound) soundController.PlayCrash(clip);
    }
    public void SaveCarInfo(Car car)
    {
        carName = car.CarName();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + car.CarName() + ".sav";
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            formatter.Serialize(stream, car);

        }
    }
    public void SaveCarStatusInfo(CarStatus car)
    {
        carName = car.CarName();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + car.CarName() + "Status.sav";
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            formatter.Serialize(stream, car);

        }
    }
    public Player ReadPlayerFile()
    {
        string path = Application.persistentDataPath + "/player.sav";
        if (File.Exists(path))
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Player)formatter.Deserialize(stream);
            }
        else
        {
            return null;
        }
    }
    public Car ReadCarFile(string carName)
    {
        string path = Application.persistentDataPath + "/" + carName + ".sav";
        if (File.Exists(path))
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Car)formatter.Deserialize(stream);
            }
        else
            return null;
    }
    public CarStatus ReadCarStatusFile(string carName)
    {
        string path = Application.persistentDataPath + "/" + carName + "Status.sav";
        if (File.Exists(path))
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (CarStatus)formatter.Deserialize(stream);
            }
        else
            return null;
    }
    public Car ReadCarFile(string carNames,bool lockState,int busterRequired,int speedLevel,int steeringLevel,int healthLevel)
    {
       
        string path = Application.persistentDataPath + "/" + carNames + ".sav";
        if (File.Exists(path))
            using (FileStream stream = File.Open(path, FileMode.Open))
            {             
                BinaryFormatter formatter = new BinaryFormatter();
                return (Car)formatter.Deserialize(stream);
            }
        else
        {
            using (FileStream stream = File.Open(path, FileMode.Create))
            {

                Car car = new Car(carNames, lockState, busterRequired, speedLevel, steeringLevel, healthLevel);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, car);
                return car;
            }
        }
    }
    public CarStatus ReadCarStatusFile(string carNames,  int speedLevel, int steeringLevel, int healthLevel)
    {

        string path = Application.persistentDataPath + "/" + carNames + "Status.sav";
        if (File.Exists(path))
            using (FileStream stream = File.Open(path, FileMode.Open))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                return (CarStatus)formatter.Deserialize(stream);
            }
        else
        {
            using (FileStream stream = File.Open(path, FileMode.Create))
            {

                CarStatus car = new CarStatus(carNames, speedLevel, steeringLevel, healthLevel);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, car);
                return car;
            }
        }
    }
}

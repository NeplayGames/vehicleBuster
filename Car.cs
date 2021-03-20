

[System.Serializable]
public class Car
{
    string carName;
     bool lockState;
     int carPrice;
    int speedLevel;

    int steeringLevel;
     int mechanicsLevel;

    public Car(string carNam,bool lockst,int carRs, int speedLev, int steerLevel,int mecLevel)
    {
        carName = carNam;
        lockState = lockst;
        carPrice = carRs;
        steeringLevel = steerLevel;
        speedLevel = speedLev;
        mechanicsLevel= mecLevel ;
    }
    public int SteeringLevel()
    {
        return steeringLevel;
    }
    public string CarName()
    {
        return carName;
    }
   public bool LockState()
    {
        return lockState;
    }
    public int CarPrice()
    {
        return carPrice;
    }
    public int SpeedLevel()
    {
        return speedLevel;
    }
    public int MechanicsLevel()
    {
        return mechanicsLevel;
    }
}

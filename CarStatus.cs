

[System.Serializable]
public class CarStatus
{
    string carName;
    int speedLevel;

    int steeringLevel;
    int mechanicsLevel;

    public CarStatus(string carNam ,int speedLev, int steerLevel, int mecLevel)
    {
        carName = carNam;
      
        steeringLevel = steerLevel;
        speedLevel = speedLev;
        mechanicsLevel = mecLevel;
    }
    public int SteeringLevel()
    {
        return steeringLevel;
    }
    public string CarName()
    {
        return carName;
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


[System.Serializable]
public class Player
{
    int totalBuster;
     int currentHighScore;
    int highScoreOnFootballGround;
    string carName;
    public Player(int totalBust, int highScore, string carNam)
    {
        totalBuster = totalBust;
        currentHighScore = highScore;
        carName = carNam;
    }
    public Player(int totalBust, int highScore,int highScoreOnFootballMode, string carNam)
    {
        totalBuster = totalBust;
        highScoreOnFootballGround = highScoreOnFootballMode;
        currentHighScore = highScore;
        carName = carNam;
    }
    public int HighScoreOnFootballMode()
    {
        return highScoreOnFootballGround;
    }
    public int TotalBuster()
    {
        return totalBuster;
    }
public int CurrentHighScore()
    {
        return currentHighScore;
    }
    public string CarName()
    {
        return carName;
    }
}

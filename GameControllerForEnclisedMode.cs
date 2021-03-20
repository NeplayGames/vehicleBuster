
using TMPro;
using UnityEngine;

public class GameControllerForEnclisedMode : MonoBehaviour
{

    // public int num;
    #region variable declaration
    Transform player;
    Player playerInfo;
    int tempVehicleDestroy = 0;
    VehicleMovement vehicleMovement;
    internal float perFrameTime;
    public int vehicleDestroy = 0;
    public float flashColorTime = 0;
    public Transform[] position;
    [SerializeField] public bool footballGroundMode;
    [SerializeField] float groundTextureSpeed;
    [SerializeField] TextMeshProUGUI tMPro;
    [SerializeField] GameInfoSaver gameInfoSaver;
    [SerializeField] LayerMask discludePlayer;
    int highScore;
    public int totalBuster;
    [SerializeField] GameObject tutorialImage;
    [SerializeField] MenuManager menuManager;
    [SerializeField] SphereCollider sphereCollider;
    #endregion

    [System.Serializable]
    public class SpawnGroup
    {
        public Transform spawnObjects;
        public float totalTime = 5;
        internal float timePassed;
        internal int index;
        public Vector2 distanceFromThePlyer = new Vector2(100, 70);
    }
    public SpawnGroup[] spawnGroups;
    public float startDelay = 1;
    public string name;
    #region start and awake methods
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    void Start()
    {
        player = FindObjectOfType<VehicleMovement>().transform;
        player.position = Vector3.zero;
        vehicleMovement = player.GetComponent<VehicleMovement>();
        perFrameTime = Time.deltaTime;
        playerInfo = gameInfoSaver.ReadPlayerFile();
        sphereCollider = player.GetComponent<SphereCollider>();
        if (gameInfoSaver.firstGame) { tutorialImage.SetActive(true); Time.timeScale = 0; }
        name = menuManager.currentItem.name;

    }
    float tempTime = 0;
    #endregion
    #region update methods
    // Update is called once per frame
    void Update()
    {
       
        if (tempVehicleDestroy != vehicleDestroy)
        {
            ChangeScore(50);
            ChangeText();
            tempVehicleDestroy = vehicleDestroy;
            flashColorTime = 0.5f;
        }
        if (vehicleMovement.health == 0 || player == null)
        {
            this.gameObject.SetActive(false);
            SavePlayerInfo();
            return;
        }
        else
        {
            if (tempTime <= 0)
            {
                ScorePerSecond();
                tempTime = 2f;
            }
            else if (Time.timeScale == 1)
            {

                tempTime -= perFrameTime;
            }
        }

        if (menuManager.isPaused) return;

        for (int i = 0; i < spawnGroups.Length; i++)
        {
            perFrameTime = Time.deltaTime;
            if (spawnGroups[i].timePassed < spawnGroups[i].totalTime) spawnGroups[i].timePassed += Time.deltaTime;
            else
            {

                Spawn(spawnGroups[i].spawnObjects, spawnGroups[i].index, spawnGroups[i].distanceFromThePlyer);

                spawnGroups[i].index++;


                if (spawnGroups[i].index > spawnGroups[i].spawnObjects.childCount - 1) spawnGroups[i].index = 0;


                spawnGroups[i].timePassed = 0;
            }
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").transform;

        }
        if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").transform;
           

        
       
            CollisionCheck(sphereCollider);
        
    }

    public int coinCollection = 0;
    public void ChangeText()
    {
        tMPro.text = (vehicleDestroy * 5 + coinCollection * 3).ToString();

    }
    Vector3 turnVector;
    float angle;
    private void CollisionCheck(SphereCollider sphereCol)
    {
        Collider[] overlaps = new Collider[4];
        Collider myCollider = new Collider();
        int num = 0;
        if (sphereCol != null)
        {
            num = Physics.OverlapSphereNonAlloc(player.transform.TransformPoint(sphereCol.center), sphereCol.radius, overlaps, discludePlayer, QueryTriggerInteraction.UseGlobal);
            myCollider = sphereCol;
        }
        for (int i = 0; i < num; i++)
        {

            Transform t = overlaps[i].transform;


            if (Physics.ComputePenetration(myCollider, player.transform.position, player.transform.rotation, overlaps[i], t.position, t.rotation, out Vector3 dir, out float dist))
            {
                Vector3 penetrationVector = dir * dist;
              
                player.transform.position = player.transform.position + penetrationVector;
               }
      }
    }
    int highScoreFootballMode;
    public void SavePlayerInfo()
    {
        totalBuster = playerInfo.TotalBuster();
        highScore = playerInfo.CurrentHighScore();
        highScoreFootballMode = playerInfo.HighScoreOnFootballMode();
        if (highScoreFootballMode < score)
            highScoreFootballMode = score;
        totalBuster += (vehicleDestroy * 5 + coinCollection * 3);
        gameInfoSaver.SaveInfo(new Player(totalBuster, highScore, highScoreFootballMode, playerInfo.CarName()));

    }
    #endregion

    public int score = 0;


    public int scorePerSecond = 1;

    public TextMeshProUGUI scoreText;
    public void ScorePerSecond()
    {
        if (vehicleMovement.health > 0)
            ChangeScore(scorePerSecond);
    }
    public void ChangeScore(int changeValue)
    {
        if (vehicleMovement.health == 0) return;
        score += changeValue;


        if (scoreText)
        {
            scoreText.text = score.ToString();


        }
    }
    #region  spawn methods
    public void Spawn(Transform spawnArray, int spawnIndex, Vector2 spawnGap)
    {

        if (spawnArray == null) return;


        Transform newSpawn = spawnArray.GetChild(spawnIndex) as Transform;
        newSpawn.gameObject.SetActive(true);

        if (player) newSpawn.position = player.transform.position;


        newSpawn.eulerAngles = Vector3.up * Random.Range(0, 360);
        newSpawn.Translate(Vector3.forward * Random.Range(spawnGap.x, spawnGap.y), Space.Self);





      

        //if (Physics.Raycast(newSpawn.position + Vector3.up * 5, -10 * Vector3.up, out hit, 100)) newSpawn.position = hit.point;


    }
    #endregion
}

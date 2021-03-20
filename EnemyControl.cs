
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    #region variable
    Transform player;
    
    VehicleMovement movement;

    [SerializeField] float forwardSpeed;
    [SerializeField] float randomAddSpeed;
    [SerializeField]public int health;
    [SerializeField] Transform wheel1;
    [SerializeField] Transform wheel2;
    [SerializeField] Transform hitEffect;

    float rotationClockWise;
    public float reviveTime;
    public float reviveLeft;
  GameController gameController;
    GameControllerForEnclisedMode game;
    MenuManager menuManager;
    float perFrameTime;
    [SerializeField] AudioClip crash;
   
    public float detectAngle = 2;
    public Color hurtFlashColor = new Color(0.5f, 0.5f, 0.5f, 1);
    GameInfoSaver gameInfoSaver;
    public float vehicleRotationSpeed = 220;
    public float obstacleDetectionDistance = 3;
    [SerializeField] float chaseAngle = 10f;
    Vector3 tempPosition;
    float y;
    public float totalTime;
    float actualSpeed;
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f);
    public Vector3 wheel1Ins, wheel2Ins;
    bool hitAnimation = false;
   // float distance = 0;
    public float leanAngle = 10;
    SoundController soundController;
    public CarStatus car;
    [SerializeField] GameObject afterEffects;
    SphereCollider sphereCollider;
    public bool freeMode = true;
    ParticleSystem particleSystem;
    #endregion
    #region Start And Awake Methods
    private void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundController>();
        menuManager = FindObjectOfType<MenuManager>();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        wheel1Ins = wheel1.localEulerAngles;
        wheel2Ins = wheel2.localEulerAngles;
       
        sphereCollider = GetComponent<SphereCollider>();
        particleSystem = hitEffect.GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        if (!GameObject.FindGameObjectWithTag("Player")) { this.gameObject.SetActive(false); }
        if (FindObjectOfType<GameController>())
            gameController = FindObjectOfType<GameController>();
        if (FindObjectOfType<GameControllerForEnclisedMode>())
            game = FindObjectOfType<GameControllerForEnclisedMode>();

        gameInfoSaver = FindObjectOfType<GameInfoSaver>();
        if (gameController != null)
            car = gameInfoSaver.ReadCarStatusFile(gameController.name);
        else
            car = gameInfoSaver.ReadCarStatusFile(game.name);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        movement = player.GetComponent<VehicleMovement>();

       
        health = 2;
        chaseAngle = Random.Range(chaseAngle, 30);
        forwardSpeed = Random.Range(forwardSpeed, forwardSpeed + randomAddSpeed);
        vehicleRotationSpeed = Random.Range(150, vehicleRotationSpeed);
        MaintainVehicleSpeed();
        freeMode = menuManager.freeMode;
    }
    private void MaintainVehicleSpeed()
    {
        forwardSpeed += car.SpeedLevel() * 1.8f ;
        vehicleRotationSpeed += car.SteeringLevel() * 16;
        forwardSpeed += movement.levelAddSpeed * 0.8f;

    }
    [SerializeField] LayerMask discludePlayer;
    Vector3 playerPosition;
    #endregion
  
    void Update()
    {
      

        if (menuManager.isPaused || movement.health==0) return;
        playerPosition = player != null?  player.position: Vector3.zero;
        wheel1.localEulerAngles = wheel1Ins;
        wheel2.localEulerAngles = wheel2Ins;
        actualSpeed = forwardSpeed;
        perFrameTime = Time.deltaTime;

        if (!freeMode)
        {
           
            CollisionCheck(sphereCollider);
        }
        if (health == 0)
        {
            gameInfoSaver.Vibrate();
            if (gameController != null)
                gameController.vehicleDestroy++;
            else
                game.vehicleDestroy++;
          
            afterEffects.SetActive(true); afterEffects.transform.position = transform.position;


            this.gameObject.SetActive(false);
        }

        if (reviveLeft > 0)
        {
            reviveLeft -= perFrameTime;
            if (hitAnimation == false)
            {
                gameInfoSaver.Vibrate();
              
                hitEffect.position = transform.position;
                particleSystem.Play();
                hitAnimation = true;
            }

            foreach (Transform g in transform.GetComponentsInChildren<Transform>())
            {
                if (g.GetComponent<MeshRenderer>())
                {
                    foreach (Material a in g.GetComponent<MeshRenderer>().materials)
                    {
                        a.EnableKeyword("_EMISSION");
                        if (Mathf.Round(reviveLeft * 10) % 2 == 0) a.SetColor("_EmissionColor", Color.black);
                        else a.SetColor("_EmissionColor", hurtFlashColor);


                    }
                }
            }

        }
        else if (hitAnimation)
        {
            hitAnimation = false;

            foreach (Transform g in transform.GetComponentsInChildren<Transform>())
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


        //distance = Vector3.Distance(transform.position, player.position);
        //if (distance < 1.4f)
        //{
        ////   if (distance < 1f && distance>0.2f)
        ////   {
        ////        actualSpeed = vehicleMovement;
        ////    }else
        //   actualSpeed = vehicleMovement+vehicleMovement*0.15f;
        // }

    
        tempPosition = playerPosition  + player.forward * -0.5f;
        
        transform.position += transform.forward * actualSpeed *  perFrameTime + transform.right * y *  perFrameTime;
       
        CollisionCheck();
    }
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


            if (Physics.ComputePenetration(myCollider, transform.localPosition, transform.rotation, overlaps[i], t.position, t.rotation, out Vector3 dir, out float dist))
            {
                Vector3 penetrationVector = dir * dist;

                transform.localPosition = transform.localPosition + penetrationVector;
            //    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 120 * perFrameTime, 0);

            }

            //else if (t.CompareTag("Terrain") && !sliding)
            //{

            //	new Vector3(treePos.x * terrainData.size.x, treePos.y * terrainData.size.y, treePos.z * terrain.terrainData.size.z);
            //	if (terrainData.treeInstances.Length == 0)
            //	{
            //		Debug.LogError("No trees could be obtained");
            //		return;
            //	}

            //	NearestPos = new Vector3((terrainData.treeInstances[0].position.x * terrainData.size.x) + manager.iId * 250, terrainData.treeInstances[0].position.y * terrainData.size.y,( terrainData.treeInstances[0].position.z * terrain.terrainData.size.z) + manager.jId * 250);
            //	nearestTreeDistance = Vector3.Distance(NearestPos, transform.position);
            //	for (int jk = 0; jk < terrainData.treeInstanceCount; jk++)
            //	{
            //		treePos = new Vector3((terrainData.treeInstances[jk].position.x * terrainData.size.x)+manager.iId*250, terrainData.treeInstances[jk].position.y * terrainData.size.y, (terrainData.treeInstances[jk].position.z * terrain.terrainData.size.z)+manager.jId * 250);

            //		if (Vector3.Distance(treePos, transform.position) < nearestTreeDistance)
            //		{

            //			NearestPos = treePos;
            //			nearestTreeDistance = Vector3.Distance(treePos, transform.position);

            //		}
            //	}
            //	closeBound = myCollider.ClosestPointOnBounds(NearestPos);
            //	transform.localPosition =Vector3.Lerp(transform.localPosition,(transform.position +( ( transform.position- new Vector3(closeBound.x,transform.position.y,closeBound.z) ) * Vector3.Distance(closeBound,transform.position))),0.6f);
            //////	print(Physics.ComputePenetration(myCollider, transform.localPosition, transform.rotation, overlaps[i], NearestPos, t.rotation, out Vector3 dirt, out float distt));

            //}



        }

    }
    [SerializeField] LayerMask mask; 
    private void CollisionCheck()
    {




        Ray rayRight = new Ray(transform.position + Vector3.up * 0.2f + transform.right , transform.TransformDirection(Vector3.forward) * obstacleDetectionDistance);
        Ray rayLeft = new Ray(transform.position + Vector3.up * 0.2f + transform.right , transform.TransformDirection(Vector3.forward) * obstacleDetectionDistance);

    


        if (Physics.Raycast(rayRight, obstacleDetectionDistance,mask))
        {
           
            Rotate(-1);

           
        }
        else if (Physics.Raycast(rayLeft, obstacleDetectionDistance,mask))
        {
          
            Rotate(1);

           
        }
        else


    if (Vector3.Angle(transform.forward, tempPosition - transform.position) > chaseAngle)
            {
                Rotate(ChaseAngle(transform.forward, tempPosition - transform.position, Vector3.up));
            }
            else 
            {
                Rotate(0);
            }
        
    }
    public float ChaseAngle(Vector3 forward, Vector3 targetDirection, Vector3 up)
    {
        
        float approachAngle = Vector3.Dot(Vector3.Cross(up, forward), targetDirection);

     
        if (approachAngle > 0f)
        {
            return 1f;
        }
        else if (approachAngle < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
   
    public void Rotate(float rotateDirection)
    {

        if (rotateDirection != 0)
        {
            rotationClockWise = rotateDirection * vehicleRotationSpeed ;

            transform.localEulerAngles += Vector3.up * rotationClockWise * perFrameTime;
            if (rotateDirection != -1 || rotateDirection != 1)
            {
                rotationClockWise = rotationClockWise < 0 ? -30 : 30;

            }
            else
                rotationClockWise = 0;
            wheel1.localEulerAngles = new Vector3(wheel1.localEulerAngles.x, wheel1.localEulerAngles.y+ rotationClockWise, wheel1.localEulerAngles.z);
            wheel2.localEulerAngles = new Vector3(wheel2.localEulerAngles.x,wheel2.localEulerAngles.y+ rotationClockWise, wheel2.localEulerAngles.z);
        }
    }

   
    private void OnTriggerStay(Collider other)
    {
      


        if (other.GetComponent<EnemyControl>() && other.GetComponent<EnemyControl>().reviveLeft<=0)
        {

            if (gameInfoSaver.playSound)
                soundController.PlayCrash(crash);
            other.GetComponent<EnemyControl>().health -= 1;
            other.GetComponent<EnemyControl>().reviveLeft = other.GetComponent<EnemyControl>().reviveTime;

        }
        if (other.GetComponent<VehicleMovement>() && other.GetComponent<VehicleMovement>().reviveLeft <= 0)
        {
            if (gameInfoSaver.playSound)
                soundController.PlayCrash(crash);
            other.GetComponent<VehicleMovement>().health -= 1;
            other.GetComponent<VehicleMovement>().reviveLeft = other.GetComponent<VehicleMovement>().reviveTime;
        }
    }
}

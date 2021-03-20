

using System.Collections;
using UnityEngine;

using UnityEngine.UI;

public class VehicleMovement : MonoBehaviour
{
    public Slider slider;
    public float detectAngle = 2;
    public Image healthBarImage;
 //   public float vehicleRotationSpeed = 200;
    public float obstacleDetectionDistance = 3;
    [SerializeField] float chaseAngle = 10f;
    [SerializeField]public int busterRequired;
    [SerializeField]public float forwardSpeed;
    [SerializeField]public float turnSpeed;
   public int health;
 // int realHealth;
    [SerializeField] Transform wheel1;
    [SerializeField] Transform wheel2;
    Touch touch;
    [SerializeField] LayerMask player;
    public bool lockState = false;
    public bool showCase = false;
    float screenWidth;
    //  [SerializeField] float GameController;
    Quaternion wheel1Rotation;
    Quaternion wheel2Rotation;
   public float reviveTime;
    float rotationClockWise;
    float antiRotationClockWise;
    GameInfoSaver gameInfoSaver;
    float perFrameTime;
    [SerializeField] AudioClip crash;
    SoundController soundController;
    [SerializeField] Gradient gradient;
    public Transform hitEffect;
    int hth;
    Car car;
    CarStatus carState;
    [SerializeField] AudioClip cournering;
    public AudioSource source;
    AudioSource audioSource;
    [SerializeField]public float levelAddSpeed = 1f; 
    [SerializeField]public int levelAddSteering = 1;
    [SerializeField] GameObject afterEffects;
    ParticleSystem particleSystem;
    bool playaudio;
    void OnEnable()
    {
        particleSystem = hitEffect.GetComponent<ParticleSystem>();
        gameInfoSaver = FindObjectOfType<GameInfoSaver>().GetComponent<GameInfoSaver>();
        car = gameInfoSaver.ReadCarFile(transform.name.Replace("(Clone)", "").Trim(),lockState,busterRequired, (int)((forwardSpeed - 29) / 1f), (int)((turnSpeed - 170) / 20), health - 3);
        carState = gameInfoSaver.ReadCarStatusFile(transform.name.Replace("(Clone)", "").Trim(), (int)((forwardSpeed - 29) / 1f), (int)((turnSpeed - 170) / 20), health - 3);
        rotationClockWise = wheel1.localEulerAngles.y;
        antiRotationClockWise = wheel2.localEulerAngles.y;
        screenWidth = Screen.width / 2;
        wheel1Rotation = wheel1.localRotation;
        wheel2Rotation = wheel2.localRotation;
        soundController = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundController>();
        if (!showCase)
        MaintainCarInfo();
        source = soundController.transform.GetChild(0).GetComponent<AudioSource>();
        audioSource = soundController.transform.GetChild(1).GetComponent<AudioSource>();
       
          slider.maxValue = health;
            slider.value = health;
          
           
            
        
    }

    private void MaintainCarInfo()
    {
        lockState = car.LockState();
          forwardSpeed =29+ carState.SpeedLevel()*2f + levelAddSpeed ;
        turnSpeed =(8.5f+ carState.SteeringLevel()) * 20;
        turnSpeed += levelAddSteering;
        health =3+ carState.MechanicsLevel() ;
        realHealth = health;
        hth = health;
    }

    public Color hurtFlashColor = new Color(0.5f, 0.5f, 0.5f, 1);
    public float reviveLeft;
    bool carRunAudio = true;
    private void OnDestroy()
    {
        if (!showCase)
        {


            source.Stop();
            audioSource.Stop();
        }
    }
    IEnumerator DestroyPlayer()
    {
        yield return new WaitForEndOfFrame();
        this.gameObject.SetActive(false);
    }
    public int realHealth;
    private void LateUpdate()
    {
        if (showCase) return;
        if (health == 0)
        {
            gameInfoSaver.Vibrate();
            afterEffects.SetActive(true); afterEffects.transform.position = transform.position; 

            slider.value = health;
            Time.timeScale = 0.3f;
            FindObjectOfType<MenuManager>().GetComponent<MenuManager>().ContinuePage();
            source.Stop();
            audioSource.Stop();

            StartCoroutine(DestroyPlayer());
          
        }
        if (hth < health) health = hth;
        if (health < realHealth)
        {
            UpdateSlider();
            realHealth = health;
        }
        if (reviveLeft > 0)
        {
            reviveLeft -= perFrameTime;
            if (!hitAnimation)
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

    }
    // Update is called once per frame
    void Update()
    {
        if (showCase) return;
        
        if(Time.timeScale == 0)
        {
            carRunAudio = true;
            source.Pause();
        }
        if (Time.timeScale == 1&&carRunAudio && gameInfoSaver.playSound)
        {
            source.Play();
            carRunAudio = false;
        }
        perFrameTime = Time.deltaTime;
       
       

        transform.position += transform.forward * forwardSpeed *  perFrameTime;
        //CollisionCheck();

        CheckTouchResponse();
    }
    bool hitAnimation = false;
    public void UpdateSlider()
    {
        healthBarImage.color = gradient.Evaluate(health / hth);
        slider.value = health;
    }
    private void CheckTouchResponse()
    {

        
            if (Input.touchCount > 0)
            {
            if (!audioSource.isPlaying && !playaudio && Time.timeScale==1 && gameInfoSaver.playSound)
            {
                audioSource.Play();
               
            }
            GetComponent<Animator>().Play("Skid");
            touch = Input.GetTouch(0);
                if (touch.position.x > screenWidth)
                {
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + turnSpeed * perFrameTime, 0);
               
                    wheel1.localEulerAngles = new Vector3(wheel1.localEulerAngles.x, rotationClockWise + 30, wheel1.localEulerAngles.z);
                    wheel2.localEulerAngles = new Vector3(wheel2.localEulerAngles.x, antiRotationClockWise + 30, wheel2.localEulerAngles.z);

                }
                else if (touch.position.x < screenWidth)
                {
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - turnSpeed * perFrameTime, 0);

                   // antiRotationClockWise = transform.eulerAngles.y + turnSpeed * perFrameTime;
                   
                   // antiRotationClockWise = Mathf.Clamp(antiRotationClockWise, 0, 80);
                    wheel1.localEulerAngles = new Vector3(wheel1.localEulerAngles.x, rotationClockWise - 30, wheel1.localEulerAngles.z);
                    wheel2.localEulerAngles = new Vector3(wheel2.localEulerAngles.x, antiRotationClockWise - 30, wheel2.localEulerAngles.z);
                }

            }
            else
            {
            GetComponent<Animator>().Play("Move");

            playaudio = false;
            if (audioSource.isPlaying && gameInfoSaver.playSound)
            {

                audioSource.Stop();
               
            }


            wheel1.localRotation = wheel1Rotation;
                wheel2.localRotation = wheel2Rotation;
           
        }
        



    }
  
    private void OnTriggerStay(Collider other)
    {

        if (soundController == null)
        {
            soundController = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundController>();
        }
        
        if (other.GetComponent<EnemyControl>() && other.GetComponent<EnemyControl>().reviveLeft <= 0)
        {
            if (gameInfoSaver.playSound)
                soundController.PlayCrash(crash);

            other.GetComponent<EnemyControl>().health -= 1;
            other.GetComponent<EnemyControl>().reviveLeft = other.GetComponent<EnemyControl>().reviveTime;

        }
    }
}

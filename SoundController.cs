using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
   public AudioSource audioSource;
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f);
    [SerializeField] GameObject sound;
  
    public bool vibrate = true;
    public bool playSound = true;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(sound);
        audioSource = GetComponent<AudioSource>();
    }
    public string musicTag = "Music";
    public int gamePlayed = 1;
    void Awake()
        {
            if(gamePlayed%6 == 0)
        {
            FindObjectOfType<AdsManager>().ShowInterstitialAdVideo();
        }
            if (GameObject.FindGameObjectsWithTag(musicTag).Length > 1)
            {
            SoundController[] sounds = FindObjectsOfType<SoundController>();
            foreach (SoundController g in sounds)
            {
              
                 
                        g.gamePlayed++;
            }
               
            Destroy(sound);
                     Destroy(gameObject);
           
          
            }
        }
  
    // Update is called once per frame
    public void PlayCrash(AudioClip audio)
    {
        if (playSound)
        {
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y) * Time.timeScale;
            audioSource.PlayOneShot(audio);
        }
       
    }
}

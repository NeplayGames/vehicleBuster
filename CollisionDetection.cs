using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    SoundController soundController;
    GameInfoSaver gameInfoSaver;
    [SerializeField] AudioClip crash;
    private void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundController>();
        gameInfoSaver = FindObjectOfType<GameInfoSaver>();
    }
  
    private void OnTriggerStay(Collider other)
    {
       
        if (other.GetComponent<EnemyControl>() && other.GetComponent<EnemyControl>().reviveLeft <= 0)
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

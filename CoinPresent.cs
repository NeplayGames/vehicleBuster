using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPresent : MonoBehaviour
{

    [SerializeField] AudioClip clip;
    [SerializeField] GameObject coinAnimation;
    GameController gameController;
    GameControllerForEnclisedMode gameControllerForEnclised;
    int value=2;
    private void Start()
    {
        value = UnityEngine.Random.Range(value, 4);
      
    }
   

    private void Update()
    {
        transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.World);

    }
    private void OnTriggerStay(Collider other)
    {
       
        if (other.GetComponent<VehicleMovement>())
        {
            FindObjectOfType<SoundController>().PlayCrash(clip);
            if (FindObjectOfType<MenuManager>().freeMode)
            {
                gameController = FindObjectOfType<GameController>();
                Instantiate(coinAnimation, transform.position, Quaternion.identity);
                gameController.coinCollection = gameController.coinCollection + value;
                gameController.ChangeText();
                gameController.ChangeScore(70);
            }
            else
            {
                gameControllerForEnclised = FindObjectOfType<GameControllerForEnclisedMode>();
                Instantiate(coinAnimation, transform.position, Quaternion.identity);
                gameControllerForEnclised.coinCollection = gameControllerForEnclised.coinCollection + value;
                gameControllerForEnclised.ChangeText();
                gameControllerForEnclised.ChangeScore(70);
            }
            this.gameObject.SetActive(false);

        }

    }
}

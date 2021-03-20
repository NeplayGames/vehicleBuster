using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCoins : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    GameController gameController;
    GameControllerForEnclisedMode gameControllerForEnclised;
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

                gameController.coinCollection++;
                gameController.ChangeText();
                gameController.ChangeScore(50);
            }
            else
            {
               gameControllerForEnclised = FindObjectOfType<GameControllerForEnclisedMode>();
                gameControllerForEnclised.coinCollection++;
                gameControllerForEnclised.ChangeText();
               gameControllerForEnclised.ChangeScore(50);
            }
          
           this.gameObject.SetActive(false);
        }
    }
}

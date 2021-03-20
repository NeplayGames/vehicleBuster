using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    private void Update()
    {
       transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.World);

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<VehicleMovement>())
        {
            FindObjectOfType<SoundController>().PlayCrash(clip);
            other.GetComponent<VehicleMovement>().health += 2;
            other.GetComponent<VehicleMovement>().realHealth += 2;
            FindObjectOfType<GameController>().ChangeScore(50);
            other.GetComponent<VehicleMovement>().UpdateSlider();

          this.gameObject.SetActive(false);
        }
    }

}

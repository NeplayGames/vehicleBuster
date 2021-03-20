using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPresent : MonoBehaviour
{

    [SerializeField] AudioClip clip;
    [SerializeField] GameObject present;
    //private void Update()
    //{
    //  //  transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.World);

    //}
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<VehicleMovement>())
        {
            FindObjectOfType<SoundController>().PlayCrash(clip);
            Instantiate(present, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
}

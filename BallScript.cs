using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 fakevel;
    Vector3 fakeGravity;
        void Update()
        {

            transform.position -= fakevel * Time.deltaTime;
           
            fakevel *= 0.999f;
        }

        void OnTriggerEnter(Collider other)
        {
        if (other.GetComponent<VehicleMovement>())
        {
            fakevel =new Vector3(0,0, Vector3.Dot(transform.position,other.transform.position) );
         
        }
           
          //  y = -45.68312f;

        }
   
}

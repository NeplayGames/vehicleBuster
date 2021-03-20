using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdButtonAnimation : MonoBehaviour
{
   [SerializeField] float period = 2f;
    const float tau = Mathf.PI * 2f;

    float cycle;
    float rawSign;
    // Update is called once per frame
    void Update()
    {
      cycle = Time.realtimeSinceStartup/ period;
       rawSign  = Mathf.Sin(cycle * tau);

        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y , transform.rotation.z + rawSign * 25f));
    }
}

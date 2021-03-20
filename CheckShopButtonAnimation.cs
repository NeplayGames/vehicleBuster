using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShopButtonAnimation : MonoBehaviour
{
    [SerializeField] float period = 2f;
    const float tau = Mathf.PI * 2f;

    float cycle;
    float rawSign;
    float screenHeight;
    void Start()
    {
        screenHeight = Screen.height;
    }
    // Update is called once per frame
    void Update()
    {
      
        cycle = Time.realtimeSinceStartup / period;
        rawSign = Mathf.Sin(cycle * tau);
        
        transform.GetComponent<RectTransform>().position = new Vector3(transform.position.x, transform.position.y + screenHeight* rawSign *0.0005f , transform.position.z );
    }
}

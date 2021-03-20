using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEffect : MonoBehaviour
{
    float timeElapsed;
    float lerpDuration = 0.6f;

    float startValue = 0;
    float endValue = 2;
    float valueToLerp;

    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
            timeElapsed = 0;

    }
    void Update()
    {
        if (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            transform.localScale= new Vector3(valueToLerp, valueToLerp, valueToLerp);
        }
        else
        {
            print("gone");
            this.gameObject.SetActive(false);

        }
    }
}

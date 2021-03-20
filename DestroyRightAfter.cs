using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRightAfter : MonoBehaviour
{
   [SerializeField] float time = 1f;
    void Start()
    {
        Invoke("Destroy", time);
    }

    // Update is called once per frame
    void Destroy()
    {
        this.gameObject.SetActive(false);
    }
}

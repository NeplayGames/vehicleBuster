using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region variable declaration
    Transform player;
    bool playerSet=false;
    MenuManager menuManager;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;
    VehicleMovement vehicleMovement;
    Vector3 finalPos;
    #endregion

    #region start and update methods
    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        finalPos = new Vector3(x, y, z);
    }

   
    void LateUpdate()
    {
        if(!playerSet && menuManager.startGame)
        {
            playerSet = true;
            vehicleMovement = FindObjectOfType<VehicleMovement>().GetComponent<VehicleMovement>();
            player = FindObjectOfType<VehicleMovement>().transform;
        }
        if (!playerSet) return;
        if(vehicleMovement.health>0)
            finalPos = player.position + new Vector3(x, y, z);
       

            transform.position =  finalPos;


    }
    #endregion
}

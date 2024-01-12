using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInFrontOfPlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float distanceFromCamera = 20f;
    private void Start()
    {
        playerCamera = Camera.main;
    }
    void Update()
    {
        // Set the object's position in front of the camera
        transform.position = playerCamera.transform.position + playerCamera.transform.forward * distanceFromCamera;

        // Optional: Make the object face the same direction as the camera
        //transform.rotation = playerCamera.transform.rotation;
        //Vector3 directionToCamera = playerCamera.transform.position - transform.position;

        //// Make the object's up vector point towards the camera
        //// The second argument to LookRotation is the up vector
        //transform.rotation = Quaternion.LookRotation(transform.forward, directionToCamera);
        transform.LookAt(playerCamera.transform);
        transform.Rotate(90, 0, 0);


    }
}

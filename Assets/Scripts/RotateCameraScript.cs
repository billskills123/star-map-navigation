using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraScript : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private GameObject mainCamera;
    public GameObject cameraFocusObject;

    [Header("Settings")]
    public float cameraSpeed;

    //Used to change the object the camera is currently focused on
    public void ChangeFocusObject(GameObject focusObject) {
        cameraSpeed = 30;
        cameraFocusObject = focusObject;
        mainCamera.transform.position = cameraFocusObject.transform.position;
        mainCamera.transform.position -= new Vector3(40, 0, 0);
    }

    //Makes the camera look at and spin around the focus object
    private void Update() {
        if (cameraFocusObject != null) {
            mainCamera.transform.RotateAround(cameraFocusObject.transform.position, Vector3.up, Time.deltaTime * cameraSpeed);
            mainCamera.transform.LookAt(cameraFocusObject.transform.position);
        }
    }
}
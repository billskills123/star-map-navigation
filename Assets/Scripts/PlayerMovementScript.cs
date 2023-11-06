using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour {
    [Header("Player Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera playerCamera;
    public float mouseSensitivity = 100f;
    public float playerSpeed = 10f;
    private float minimumViewDistance = 90f;
    private float xRotation = 0f;
    private Vector3 playerMoveValue;
    private bool sprinting = false;

    //Locks the player's mouse
    private void OnEnable() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Player movement and camera movement
    private void Update() {
        player.Translate(playerSpeed * Time.deltaTime * new Vector3(playerMoveValue.x, playerMoveValue.y, playerMoveValue.z)); //Moves the player based on their inputs

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Allows the player to rotate the camera within a fixed range
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, minimumViewDistance);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }

    //Called when a player presses a "Move" key
    private void OnMove(InputValue playerInputValue) {
        playerMoveValue = playerInputValue.Get<Vector3>();
    }

    //Called when the Shift key is pressed
    private void OnSprint() {
        sprinting = !sprinting; //Sets the sprinting bool to the opposite of its current value

        //If the key is held double the speed and when released half it again
        if (sprinting == true) {
            playerSpeed *= 2;
        }
        else if (sprinting == false) {
            playerSpeed /= 2;
        }
    }
}
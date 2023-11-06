using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridButtonScript : MonoBehaviour {
    [Header("Star")]
    public StarInformation buttonStar;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private ConnectStars connectStars;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject mainCamera;

    //Set up for the prefab objects
    private void Start() {
        connectStars = GameObject.Find("StarsGroup").GetComponent<ConnectStars>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCanvas = GameObject.Find("MainCanvas");
        mainCamera = GameObject.Find("MainCamera");

        gameManager.gridButtons.Add(gameObject); //Adds button to list of all grid buttons
    }

    //Change what star the button relates to
    public void ChangeStar(StarInformation star) {
        buttonStar = star;
        buttonText.text = star.name;
        gameObject.name = buttonText.text;
    }

    //Changes the selected star when clicked
    public void ChangeTargetStar() {
        connectStars.ChangeTargetStar(buttonStar); //Selects the target star
        mainCanvas.GetComponent<DrawPathScript>().endStar = buttonStar;
    }

    //Changes the start star when clicked
    public void ChangeStartStar() {
        mainCanvas.GetComponent<DrawPathScript>().startingStar = buttonStar; //Draws a path from the start star to the target star
    }

    //Focuses the camera on the selected star
    public void ChangeCameraFocus() {
        mainCamera.GetComponent<RotateCameraScript>().ChangeFocusObject(buttonStar.gameObject);
    }
}
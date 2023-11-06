using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewStarsScript : MonoBehaviour {
    [Header("Misc Objects")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text starButtonText;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private GameObject leftButton;

    [Header("Scripts")]
    [SerializeField] private RotateCameraScript rotateCameraScript;
    [SerializeField] private FadeScript interactionCanvasFadeScript;
    [SerializeField] private FadeScript viewStarsFadeScript;
    [SerializeField] private ConnectStars connectStars;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InformationScript informationScript;

    [Header("Canvases")]
    [SerializeField] private GameObject viewStarsCanvas;
    [SerializeField] private GameObject interactionCanvas;

    [Header("Misc Variables")]
    [SerializeField] private int currentViewStarIndex;

    //Sets up the screen for displaying stars
    public void ViewStars() {
        gameManager.inViewStars = true;
        Time.timeScale = 1;
        mainCamera.SetActive(true);
        player.SetActive(false);
        viewStarsCanvas.SetActive(true);

        rotateCameraScript.ChangeFocusObject(connectStars.stars[0].gameObject);
        rotateCameraScript.cameraSpeed = 30;
        starButtonText.text = connectStars.stars[0].name;

        currentViewStarIndex = 0;
        leftButton.SetActive(false);
        rightButton.SetActive(true);

        StartCoroutine(ViewStarsOpen());
    }

    //Fades the interaction menu out and fades the view stars canvas in
    private IEnumerator ViewStarsOpen() {
        interactionCanvasFadeScript.CanvasFade("Close", interactionCanvas, 2.5f);
        informationScript.DisplayInformation(connectStars.stars[currentViewStarIndex].gameObject);

        yield return new WaitUntil(() => interactionCanvas.GetComponent<CanvasGroup>().alpha == 0);
        interactionCanvas.SetActive(false);
        viewStarsFadeScript.CanvasFade("Open", viewStarsCanvas, 2.5f);
    }

    //Called when player clicks on the close button
    public void CloseViewStars() {
        StartCoroutine(ViewStarsClose());
    }

    //Closes the view stars UI and returns back to the interaction menu
    private IEnumerator ViewStarsClose() {
        viewStarsFadeScript.CanvasFade("Close", viewStarsCanvas, 2.5f);
        informationScript.EndDisplay();

        yield return new WaitUntil(() => viewStarsCanvas.GetComponent<CanvasGroup>().alpha == 0);
        viewStarsCanvas.SetActive(false);

        player.SetActive(true);
        mainCamera.SetActive(false);
        gameManager.inViewStars = false;

        interactionCanvas.SetActive(true);
        gameManager.OnInteractionMenu();
    }

    //Used for navigating to the next star in the list of stars
    public void RightButton() {
        currentViewStarIndex++;

        if (currentViewStarIndex == connectStars.stars.Length - 1) {
            rightButton.SetActive(false); //Hides the button if the player reaches the end of the list
        }
        else if (currentViewStarIndex != 0) {
            leftButton.SetActive(true); //Shows the left button if the player is not at the beginning of the list
        }

        //Focuses on the star and displays relevant information
        rotateCameraScript.ChangeFocusObject(connectStars.stars[currentViewStarIndex].gameObject);
        starButtonText.text = connectStars.stars[currentViewStarIndex].name;
        informationScript.DisplayInformation(connectStars.stars[currentViewStarIndex].gameObject);
    }

    //Used for navigating to the previous star in the list of stars
    public void LeftButton() {
        currentViewStarIndex--;

        if (currentViewStarIndex == 1) {
            leftButton.SetActive(false); //Hides the left button when the player reaches the beginning of the list
        } 
        else if (currentViewStarIndex != connectStars.stars.Length) {
            rightButton.SetActive(true); //Shows the right button if player has not reached the end of the list
        }

        //Focuses on the star and displays relevant information
        rotateCameraScript.ChangeFocusObject(connectStars.stars[currentViewStarIndex].gameObject);
        starButtonText.text = connectStars.stars[currentViewStarIndex].name;
        informationScript.DisplayInformation(connectStars.stars[currentViewStarIndex].gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonConfirmationScript : MonoBehaviour {
    [Header("Target Button Objects")]
    [SerializeField] private GameObject selectTargetButton;
    [SerializeField] private GameObject targetScrollView;
    [SerializeField] private GameObject targetStarText;

    [Header("Shared Objects")]
    [SerializeField] private GameObject selectStartButton;
    [SerializeField] private GameObject startingScrollView;
    [SerializeField] private GameObject startingStarText;

    [Header("Start Button Objects")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject infoCanvas;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject menuHintText;
    [SerializeField] private GameObject menuHintText2;

    [Header("Scripts")]
    [SerializeField] private StarSpawner starSpawner;
    [SerializeField] private ConnectStars connectStars;
    [SerializeField] private DrawPathScript drawPathScript;
    [SerializeField] private FadeScript confirmationFadeScript;
    [SerializeField] private GameManager gameManager;

    [Header("Misc Objects")]
    [SerializeField] private GameObject confirmationText;
    [SerializeField] private GameObject startStarDisplayText;
    [SerializeField] private GameObject targetStarDisplayText;

    //Used for confirming a target star has been chosen
    public void TargetButtonConfirmation() {
        //Displays the next part if a star has been selected
        if (drawPathScript.endStar != null) {
            selectTargetButton.SetActive(false);
            targetScrollView.SetActive(false);
            targetStarText.SetActive(false);

            selectStartButton.SetActive(true);
            startingScrollView.SetActive(true);
            startingStarText.SetActive(true);

            starSpawner.FillStartGrid();
            connectStars.CalculateDistances();
        }
        else if (drawPathScript.endStar == null) {
            StartCoroutine(ConfirmationTextCoroutine()); //Displays a warning to the player
        }
    }

    //Used for confirming a starting star has been chosen
    public void StartButtonConfirmation() {
        //Displays the main game if a start has been selected
        if(drawPathScript.startingStar != null) {
            mainCamera.SetActive(false);
            selectStartButton.SetActive(false);
            startingScrollView.SetActive(false);
            startingStarText.SetActive(false);

            player.SetActive(true);
            infoCanvas.SetActive(true);
            crosshair.SetActive(true);
            menuHintText.SetActive(true);
            menuHintText2.SetActive(true);

            startStarDisplayText.SetActive(true);
            startStarDisplayText.GetComponentInChildren<TMP_Text>().text += " " + drawPathScript.startingStar.name;
            targetStarDisplayText.SetActive(true);
            targetStarDisplayText.GetComponentInChildren<TMP_Text>().text += " " + drawPathScript.endStar.name;

            gameManager.inGame = true;
            drawPathScript.DrawPath();
        }
        else if (drawPathScript.startingStar == null) {
            StartCoroutine(ConfirmationTextCoroutine()); //Displays a warning to the player
        }
    }

    //Shows a warning to the player if they have not chosen a star
    private IEnumerator ConfirmationTextCoroutine() {
        confirmationText.SetActive(true);
        confirmationFadeScript.CanvasFade("Open", confirmationText, 2.5f);

        yield return new WaitUntil(() => confirmationText.GetComponent<CanvasGroup>().alpha == 1);

        yield return new WaitForSeconds(2.5f);
        confirmationFadeScript.CanvasFade("Close", confirmationText, 3.5f);

        yield return new WaitUntil(() => confirmationText.GetComponent<CanvasGroup>().alpha == 0);
        confirmationText.SetActive(false);
    }
}
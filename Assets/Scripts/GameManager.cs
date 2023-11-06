using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Event System Objects")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject unpauseButton;
    [SerializeField] private GameObject closeSettingsButton;
    [SerializeField] private GameObject closeInteractionButton;

    [Header("Canvases")]
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject settingsMenuCanvas;
    [SerializeField] private GameObject interactionMenuCanvas;
    [SerializeField] private GameObject mainCanvas;

    [Header("Fade Scripts")]
    [SerializeField] private FadeScript pauseFadeScript;
    [SerializeField] private FadeScript settingsMenuFadeScript;
    [SerializeField] private FadeScript mainCanvasFadeScript;
    [SerializeField] private FadeScript blackscreenFadeScript;
    [SerializeField] private FadeScript interactionMenuFadeScript;

    [Header("Settings Objects")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject menuHint;
    [SerializeField] private GameObject menuHint2;
    [SerializeField] private GameObject targetStarButton;
    [SerializeField] private GameObject targetStarScrollView;
    [SerializeField] private GameObject targetStarText;
    [SerializeField] private GameObject startStarButton;
    [SerializeField] private GameObject startStarScrollView;
    [SerializeField] private GameObject startStarText;
    [SerializeField] private GameObject spawnStarsButton;
    [SerializeField] private GameObject starSlider;
    [SerializeField] private GameObject numOfStarsText;
    [SerializeField] private TMP_Text hideAllLinesText;
    [SerializeField] private TMP_Text hideNormalLinesText;

    [Header("Misc Scripts")]
    [SerializeField] private RotateCameraScript rotateCameraScript;
    [SerializeField] private ConnectStars connectStars;
    [SerializeField] private DrawPathScript drawPathScript;
    [SerializeField] private StarSpawner starSpawner;

    [Header("Misc Objects")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject startStarDisplayText;
    [SerializeField] private GameObject targetStarDisplayText;

    [Header("Start Objects")]
    [SerializeField] private GameObject blackscreenCanvas;

    [Header("Misc Variables")]
    public bool inGame;
    public bool inViewStars;

    [Header("Grid Buttons List")]
    public List<GameObject> gridButtons = new List<GameObject>();

    private LineRenderer[] allLines;
    private bool linesActive;

    //Calls the start game coroutine
    private void Start() {
        StartCoroutine(StartGameCoroutine());
    }

    //Fades the black screen out and fades the main ui in. Makes scene transitions feel smoother
    private IEnumerator StartGameCoroutine() {
        blackscreenFadeScript.CanvasFade("Close", blackscreenCanvas, 2.5f);
        yield return new WaitUntil(() => blackscreenCanvas.GetComponent<CanvasGroup>().alpha == 0);
        blackscreenCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        mainCanvasFadeScript.CanvasFade("Open", mainCanvas, 2.5f);
    }

    //Function called when the 'ESC' key is pressed
    public void OnPauseMenu() {
        if (settingsMenuCanvas.activeSelf == false && interactionMenuCanvas.activeSelf == false) {
            StartCoroutine(PauseMenuOpen()); //Wont be called if the settings menu is open
        }
    }

    //Used to open the pause menu
    private IEnumerator PauseMenuOpen() {
        rotateCameraScript.cameraSpeed = 0;
        pauseCanvas.SetActive(true);
        mainCanvasFadeScript.CanvasFade("Close", mainCanvas, 3.5f);
        pauseFadeScript.CanvasFade("Open", pauseCanvas, 2.5f);

        eventSystem.SetSelectedGameObject(unpauseButton); //Selects the resume button as default
        Cursor.lockState = CursorLockMode.None;

        yield return new WaitUntil(() => pauseCanvas.GetComponent<CanvasGroup>().alpha == 1);
        mainCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    //Used to close the pause menu
    private IEnumerator PauseMenuClose() {
        Time.timeScale = 1;
        pauseFadeScript.CanvasFade("Close", pauseCanvas, 3.5f);

        mainCanvas.SetActive(true);
        mainCanvasFadeScript.CanvasFade("Open", mainCanvas, 2.5f);
        rotateCameraScript.cameraSpeed = 30;

        yield return new WaitUntil(() => pauseCanvas.GetComponent<CanvasGroup>().alpha == 0);
        pauseCanvas.SetActive(false);

        //Only locks the cursor if the crosshair is enabled (player is able to move)
        if (crosshair.activeSelf == false) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Called when closing the pause menu
    public void ResumeGame() {
        StartCoroutine(PauseMenuClose());
    }

    //Used to exit to the main menu
    public void ExitToMenu() {
        Time.timeScale = 1;
        StartCoroutine(ExitToMenuCoroutine());
        blackscreenCanvas.GetComponent<CanvasGroup>().alpha = 0;
    }

    //Exits the player back to the main menu
    private IEnumerator ExitToMenuCoroutine() {
        mainCanvasFadeScript.CanvasFade("Close", mainCanvas, 3.5f);
        yield return new WaitUntil(() => mainCanvas.GetComponent<CanvasGroup>().alpha == 0);
        mainCanvas.SetActive(false);
        blackscreenCanvas.SetActive(true);
        blackscreenFadeScript.CanvasFade("Open", blackscreenCanvas, 2.5f);
        yield return new WaitUntil(() => blackscreenCanvas.GetComponent<CanvasGroup>().alpha == 1);
        SceneManager.LoadScene("MainMenu");
    }

    //Function called when the 'M' key is pressed
    public void OnSettingsMenu() {
        if (pauseCanvas.activeSelf == false && interactionMenuCanvas.activeSelf == false && inGame == true && inViewStars == false) {
            StartCoroutine(SettingsMenuOpen()); //Wont be called if the settings menu is open
        }
    }
    
    //Used to open the settings menu
    private IEnumerator SettingsMenuOpen() {
        rotateCameraScript.cameraSpeed = 0;
        settingsMenuCanvas.SetActive(true);
        mainCanvasFadeScript.CanvasFade("Close", mainCanvas, 3.5f);
        settingsMenuFadeScript.CanvasFade("Open", settingsMenuCanvas, 2.5f);

        eventSystem.SetSelectedGameObject(closeSettingsButton); //Used to select the close button by default

        yield return new WaitUntil(() => settingsMenuCanvas.GetComponent<CanvasGroup>().alpha == 1);
        Cursor.lockState = CursorLockMode.None;
        mainCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    //Used for calling the close settings menu coroutine
    public void OnSettingsMenuClose() {
        StartCoroutine(SettingsMenuClose());
    }

    //Used for closing the settings menu
    private IEnumerator SettingsMenuClose() {
        Time.timeScale = 1;
        settingsMenuFadeScript.CanvasFade("Close", settingsMenuCanvas, 3.5f);

        mainCanvas.SetActive(true);
        mainCanvasFadeScript.CanvasFade("Open", mainCanvas, 2.5f);
        rotateCameraScript.cameraSpeed = 30;

        yield return new WaitUntil(() => settingsMenuCanvas.GetComponent<CanvasGroup>().alpha == 0);
        settingsMenuCanvas.SetActive(false);

        //Only locks the cursor if the crosshair is enabled (player is able to move)
        if (crosshair.activeSelf == false) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Finds all of the 'correct' lines and deletes them
    public void RemoveCorrectLines() {
        GameObject[] correctLines = GameObject.FindGameObjectsWithTag("CorrectLineRenderer");
        foreach (GameObject line in correctLines) {
            Destroy(line);
        }
    }

    //Hides any active lines in the scene
    public void HideAllLines() {
        allLines ??= FindObjectsOfType<LineRenderer>(); //Visual studio suggested formatting. Same as if allLines == null find objects

        foreach (LineRenderer line in allLines) { 
            if (line.CompareTag("Untagged")) { 
                line.gameObject.SetActive(!line.gameObject.activeSelf);
                linesActive = line.gameObject.activeSelf;
            }
            else if (!line.CompareTag("Untagged")) {
                line.gameObject.SetActive(!linesActive);
            }
        }

        //Show correct text display
        if (linesActive == true) {
            hideAllLinesText.text = "Hide All Lines";
        } else {
            hideAllLinesText.text = "Show All Lines";
        }
    }

    //Only shows the correct path lines
    public void HideNormalLines() {
        allLines ??= FindObjectsOfType<LineRenderer>(); //Visual studio suggested formatting. Same as if allLines == null find objects
        linesActive = !linesActive;

        foreach (LineRenderer line in allLines) {
            if (line.tag == "Untagged") {
                line.gameObject.SetActive(!line.gameObject.activeSelf); //Hides all the untagged lines (normal lines)
            }
        }

        //Show correct text display
        if (linesActive == true) {
            hideNormalLinesText.text = "Hide Normal Lines";
        } else {
            hideNormalLinesText.text = "Show Normal Lines";
        }
    }

    //Called when changing the star or end star
    public void ChangeRoutePoints(string changeType) {
        StartCoroutine(ChangeRoute(changeType));
    }

    //Used for changing either the start or end star
    private IEnumerator ChangeRoute(string changeType) {
        Time.timeScale = 1;
        settingsMenuFadeScript.CanvasFade("Close", settingsMenuCanvas, 3.5f);

        //Disables and resets the main UI
        mainCanvas.SetActive(true);
        menuHint.SetActive(false);
        menuHint2.SetActive(false);
        crosshair.SetActive(false);
        targetStarDisplayText.SetActive(false);
        targetStarDisplayText.GetComponent<TMP_Text>().text = "Target Star:";
        startStarDisplayText.SetActive(false);
        startStarDisplayText.GetComponent<TMP_Text>().text = "Start Star:";

        if (changeType == "Start") {
            startStarButton.SetActive(true);
            startStarScrollView.SetActive(true);
            startStarText.SetActive(true);
        }
        else if (changeType == "Target") {
            targetStarButton.SetActive(true);
            targetStarScrollView.SetActive(true);
            targetStarText.SetActive(true);

            foreach (StarInformation star in connectStars.stars) {
                star.starDistance = float.MaxValue; //Resets the stars distance to the target star so distances can be recalculated
            }
        }

        mainCanvasFadeScript.CanvasFade("Open", mainCanvas, 2.5f);
        rotateCameraScript.cameraSpeed = 30;

        yield return new WaitUntil(() => settingsMenuCanvas.GetComponent<CanvasGroup>().alpha == 0);
        settingsMenuCanvas.SetActive(false);

        //Only locks the cursor if the crosshair is enabled (player is able to move)
        if (crosshair.activeSelf == false) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Deletes all stars and lines
    private void DeleteAllStars() {
        GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");
        LineRenderer[] lines = FindObjectsOfType<LineRenderer>();

        foreach (LineRenderer line in lines) {
            Destroy(line); //Destroys all lines
        }

        foreach (GameObject star in stars) {
            Destroy(star); //Destroys all stars
        }

        foreach (GameObject button in gridButtons) {
            Destroy(button); //Destroys all grid buttons
        }
        gridButtons.Clear();
    }

    //Used for calling the GenerateNewStarsCoroutine
    public void GenerateNewStars() {
        StartCoroutine(GenerateNewStarsCoroutine());
    }

    //Allows the player to generate a new set of stars
    private IEnumerator GenerateNewStarsCoroutine() {
        yield return new WaitUntil(() => mainCamera.activeSelf == true);

        //'Disable' the player
        crosshair.SetActive(false);
        menuHint.SetActive(false);
        menuHint2.SetActive(false);
        targetStarDisplayText.SetActive(false);
        targetStarDisplayText.GetComponent<TMP_Text>().text = "Target Star:";
        startStarDisplayText.SetActive(false);
        startStarDisplayText.GetComponent<TMP_Text>().text = "Start Star:";

        DeleteAllStars();
        StartCoroutine(SettingsMenuClose());

        //Reset variables for generating stars
        connectStars.targetStarDisplay = null;
        connectStars.stars = null;
        rotateCameraScript.ChangeFocusObject(gameObject);
        rotateCameraScript.cameraSpeed = 5f;
        starSpawner.startGridChildren = 0;
        starSpawner.targetGridChildren = 0;
        drawPathScript.startingStar = null;
        drawPathScript.endStar = null;
        drawPathScript.starRoute = null;

        yield return new WaitUntil(() => settingsMenuCanvas.GetComponent<CanvasGroup>().alpha == 0);
        spawnStarsButton.SetActive(true);
        starSlider.SetActive(true);
        numOfStarsText.SetActive(true);
    }

    //Called by pressing 'I'
    public void OnInteractionMenu() {
        if (pauseCanvas.activeSelf == false && settingsMenuCanvas.activeSelf == false && inGame == true && inViewStars == false) {
            StartCoroutine(InteractionMenuOpen());
        }
    }

    //'Pauses' the game and opens up the interaction menu
    private IEnumerator InteractionMenuOpen() {
        rotateCameraScript.cameraSpeed = 0;
        interactionMenuCanvas.SetActive(true);
        mainCanvasFadeScript.CanvasFade("Close", mainCanvas, 3.5f);
        interactionMenuFadeScript.CanvasFade("Open", interactionMenuCanvas, 2.5f);

        eventSystem.SetSelectedGameObject(closeInteractionButton); //Used to select the close button by default

        yield return new WaitUntil(() => interactionMenuCanvas.GetComponent<CanvasGroup>().alpha == 1);
        Cursor.lockState = CursorLockMode.None;
        mainCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    //Used for calling the close settings menu coroutine
    public void OnInteractionMenuClose() {
        StartCoroutine(InteractionMenuClose());
    }

    //Used for closing the settings menu
    private IEnumerator InteractionMenuClose() {
        Time.timeScale = 1;
        interactionMenuFadeScript.CanvasFade("Close", interactionMenuCanvas, 3.5f);

        mainCanvas.SetActive(true);
        mainCanvasFadeScript.CanvasFade("Open", mainCanvas, 2.5f);
        rotateCameraScript.cameraSpeed = 30;

        yield return new WaitUntil(() => interactionMenuCanvas.GetComponent<CanvasGroup>().alpha == 0);
        interactionMenuCanvas.SetActive(false);

        //Only locks the cursor if the crosshair is enabled (player is able to move)
        if (crosshair.activeSelf == false) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
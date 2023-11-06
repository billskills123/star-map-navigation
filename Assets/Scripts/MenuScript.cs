using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private Image menu;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject blackscreenCanvas;
    [SerializeField] private FadeScript blackscreenFadeScript;

    //Set up primarily for when players return back to the menu
    private void Start() {
        StartCoroutine(LoadScene());
    }

    //Used to set up screen when loaded into the scene
    private IEnumerator LoadScene() {
        blackscreenFadeScript.CanvasFade("Close", blackscreenCanvas, 2.5f);
        yield return new WaitUntil(() => blackscreenCanvas.GetComponent<CanvasGroup>().alpha == 0);
        blackscreenCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        StartCoroutine(UILerp(menu, Vector2.zero, "Open"));
    }

    //Function to call the coroutine
    public void LoadGame() {
        StartCoroutine(UILerp(menu, Vector2.zero, "Close"));
    }

    //Lerps the menu either on or off screen
    private IEnumerator UILerp(Image uiObject, Vector2 endPosition, string lerpType) {
        //Setting up variables
        if (lerpType == "Open") {
            endPosition = new Vector2(0, 0);
        }
        else if (lerpType == "Close") {
            endPosition = new Vector2(0, 1000);
        }

        Vector2 startPosition = uiObject.rectTransform.anchoredPosition;
        float time = 0;

        //Lerp the menu
        while (time < 1.0f) {
            uiObject.rectTransform.anchoredPosition = LerpLibrary.UILerp(startPosition, endPosition, LerpLibrary.InOutBackEase(time));
            time += Time.deltaTime;
            yield return null;
        }
        uiObject.rectTransform.anchoredPosition = endPosition;

        //Fade in the black screen and then switch scenes if required
        if (lerpType == "Close") {
            mainCanvas.SetActive(false);
            blackscreenCanvas.SetActive(true);
            blackscreenFadeScript.CanvasFade("Open", blackscreenCanvas, 2.5f);
            yield return new WaitUntil(() => blackscreenCanvas.GetComponent<CanvasGroup>().alpha == 1);
            SceneManager.LoadScene("MainScene");
        }
    }

    //Used to exit the game
    public void ExitGame() {
        Application.Quit();
    }
}
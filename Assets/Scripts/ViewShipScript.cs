using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewShipScript : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private DrawPathScript drawPathScript;
    [SerializeField] private RotateCameraScript rotateCameraScript;
    [SerializeField] private GameObject spaceship;
    [SerializeField] private GameObject player;

    [Header("Settings")]
    [SerializeField] private float speed;

    //Called by the view ship button
    public void StartLerp() {
        StartCoroutine(LerpSpaceshipAlongRoute());
    }

    //Lerps the spaceship between each star and waits until it reaches the end of the line before moving to the next star
    private IEnumerator LerpSpaceshipAlongRoute() {
        for (int i = 0; i < drawPathScript.starRoute.Count; i++) {
            if (i + 1 < drawPathScript.starRoute.Count) {
                StartCoroutine(LerpSpaceship(drawPathScript.starRoute[i].transform.position, drawPathScript.starRoute[i + 1].transform.position, 1f));
                yield return new WaitUntil(() => spaceship.transform.position == drawPathScript.starRoute[i + 1].transform.position);
            }
        }
        spaceship.SetActive(false);
        player.SetActive(true);
    }

    //Lerps the spaceship between two points
    private IEnumerator LerpSpaceship(Vector3 startPosition, Vector3 endPosition, float duration) {
        float t = 0;

        while (t < duration) {
            spaceship.transform.position = LerpLibrary.PositionLerp(startPosition, endPosition, LerpLibrary.InOutEase(t));
            spaceship.transform.LookAt(endPosition);
            t += Time.deltaTime / speed;
            yield return null;
        }
        spaceship.transform.position = endPosition;
    }
}
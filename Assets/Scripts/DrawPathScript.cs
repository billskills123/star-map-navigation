using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathScript : MonoBehaviour {
    [Header("Path Settings")]
    public StarInformation startingStar;
    public StarInformation endStar;
    public List<StarInformation> starRoute;
    [SerializeField] private GameObject lineRenderer;

    //Draws a path from the start star to the end star
    public void DrawPath() {
        starRoute = new List<StarInformation>();
        var currentStar = startingStar;

        while(currentStar != endStar) {
            starRoute.Add(currentStar); //Adds the current star into the route

            GameObject newLineRenderer = Instantiate(lineRenderer, currentStar.transform);
            newLineRenderer.GetComponent<LineRenderer>().startColor = Color.green;
            newLineRenderer.GetComponent<LineRenderer>().endColor = Color.green;
            newLineRenderer.GetComponent<LineRenderer>().sortingOrder = 2;

            newLineRenderer.GetComponent<LineRenderer>().SetPosition(0, currentStar.transform.position);
            newLineRenderer.GetComponent<LineRenderer>().SetPosition(1, currentStar.shortestConnectedStar.transform.position);
            currentStar = currentStar.shortestConnectedStar;
        }
        starRoute.Add(endStar); //Adds the final star into the star route
    }
}
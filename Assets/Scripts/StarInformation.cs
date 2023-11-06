using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarInformation : MonoBehaviour {
    [Header("Connected Star Information")]
    public List<StarInformation> connectedStars;
    public StarInformation shortestConnectedStar;

    [Header("Star Information")]
    public float starDistance = float.MaxValue;
    public bool canDrawPath = false;
    [SerializeField] private GameObject lineRenderer;

    //Draw lines to the connected stars
    public void DrawConnectedStars() {
        foreach (var star in connectedStars) {
            GameObject newLineRenderer = Instantiate(lineRenderer, transform);
            newLineRenderer.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            newLineRenderer.GetComponent<LineRenderer>().SetPosition(1, star.transform.position);
            newLineRenderer.tag = "Untagged"; //Removes the correct line tag
        }
    }

    //Finds the star with the shortest distance to the target star
    public void FindShortestStarDistance() {
        shortestConnectedStar = connectedStars[0];

        foreach (var star in connectedStars) {
            if (star.starDistance < shortestConnectedStar.starDistance) {
                shortestConnectedStar = star; //Sets the connected star closest to the target
            }
        }
    }
}
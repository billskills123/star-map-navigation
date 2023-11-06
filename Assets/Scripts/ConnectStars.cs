using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ConnectStars : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private StarSpawner starSpawner;
    [SerializeField] private Slider starCount;

    [Header("Settings")]
    [SerializeField] private int counter = 0; //Should remain at 0
    public int connectionMultiplier = 2;

    [Header("Connection Information")]
    public StarInformation[] stars;
    public StarInformation targetStarDisplay;
    public static StarInformation targetStar;

    //Resests the star counter to 0
    public void ResetCounter() {
        counter = 0;
    }

    //Counter used to ensure all stars have been found before populating the grid
    public void FindStars() {
        counter++;

        if (counter == starCount.value) {
            stars = FindObjectsOfType<StarInformation>();
            starSpawner.FillTargetGrid(); //Populates the UI grid
        }
    }

    //Draws lines between all connected stars
    public void DrawStars() {
        foreach (StarInformation star in stars) {
            star.DrawConnectedStars();
        }
    }

    //Used for changing the target star
    public StarInformation ChangeTargetStar(StarInformation star) {
        return targetStar = star;
    }

    //Used for calculating the distances from stars to the target star
    public void CalculateDistances() {
        targetStarDisplay = targetStar;

        var visitedStars = new List<StarInformation>();
        var starsToVisitQueue = new Queue<StarInformation>();
        starsToVisitQueue.Enqueue(targetStar); //Add the target node into the queue

        while(starsToVisitQueue.Count > 0) {
            var currentStar = starsToVisitQueue.Dequeue(); //Gets the current star out of the queue

            if(currentStar == targetStar) {
                currentStar.starDistance = 0;
            }

            //Checks that the star hasnt been visited
            var nextStars = currentStar.connectedStars;
            var filteredStars = nextStars.Where(star => !visitedStars.Contains(star)).ToList();

            //Checks if the new distance is less and if so assigns it to the star
            foreach (var star in filteredStars) {
                var distance = CalculateStarDistance(currentStar, star);
                var newDistance = currentStar.starDistance + distance;
                star.starDistance = Math.Min(star.starDistance, newDistance);

                starsToVisitQueue.Enqueue(star);
            }

            visitedStars.Add(currentStar);
        }

        foreach (StarInformation star in stars) {
            star.FindShortestStarDistance(); //Finds the closest connected star
        }
    }

    //Used for calculating the distance on a star
    private float CalculateStarDistance(StarInformation currentStar, StarInformation star) {
        return (currentStar.transform.position - star.transform.position).magnitude;
    }

    //Used for fidning which stars are connected together
    public void ConnectNodes() {
        foreach (var star in stars) {
            //Total number of connections equals number of stars * connection multiplier
            for (int i = 0; i < connectionMultiplier; ++i) { 

                var availableConnections = stars.Where(newStar => newStar != star).ToList(); //Ensures a star cannot connected to itself
                var connection = availableConnections[Random.Range(0, availableConnections.Count)]; //Randomly picks a number of connections

                //Checks a star isnt already connected to the conncection
                if (!star.connectedStars.Contains(connection)) { 
                    star.connectedStars.Add(connection);
                    connection.connectedStars.Add(star);
                }
            }
            star.starDistance = float.MaxValue;
        }
    }
}
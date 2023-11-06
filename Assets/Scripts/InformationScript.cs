using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationScript : MonoBehaviour {
    [Header("Misc")]
    [SerializeField] private Image infoPanel;
    [SerializeField] private FadeScript fadeScript;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float preciseness = 0.98f;

    [Space(10)]
    [Header("Star Information Display")]
    [SerializeField] private TMP_Text starName;
    [SerializeField] private TMP_Text starSize;
    [SerializeField] private TMP_Text numberPlanets;
    [SerializeField] private TMP_Text commonElement;
    [SerializeField] private TMP_Text commonCompound;
    [SerializeField] private TMP_Text connectedStarsText;

    //Casts a raycast from the players camera. Detects a star and displays relevant information
    private void Update() {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hitObject)) {
            Vector3 center = hitObject.transform.position;
            Vector3 playerPos = transform.position;
            Vector3 triggerLookDirection = transform.forward;
            Vector3 triggerToPlayerDirection = (center - playerPos).normalized;

            float distance = Vector3.Distance(hitObject.transform.position, playerPos);
            float alignment = Vector3.Dot(triggerToPlayerDirection, triggerLookDirection);
            bool isLooking = alignment >= preciseness;

            //If the player is close enough and looking at a star
            if (isLooking == true && distance < 100 && hitObject.collider.CompareTag("Star")) {
                UpdateInformation(hitObject.transform.gameObject);
                fadeScript.CanvasFade("Open", infoPanel.gameObject, 2.5f);
            }
        } 
        else {
            //If the player is not looking at a star
            fadeScript.CanvasFade("Close", infoPanel.gameObject, 2.5f);
        }
    }

    //Displays information when viewing stars via the interaction menu
    public void DisplayInformation(GameObject currentStar) {
        UpdateInformation(currentStar);
        fadeScript.CanvasFade("Open", infoPanel.gameObject, 2.5f);
    }

    //Closes the information panel
    public void EndDisplay() {
        fadeScript.CanvasFade("Close", infoPanel.gameObject, 2.5f);
    }

    //Updates the information on the UI
    private void UpdateInformation(GameObject hitObject) {
        starName.text = "Star Name: " + hitObject.GetComponentInParent<StarInfoGenerator>().starName;
        starSize.text = "Star Diameter: " + hitObject.GetComponentInParent<StarInfoGenerator>().starSize + "Million KM";
        numberPlanets.text = "Number of Planets: " + hitObject.GetComponentInParent<StarInfoGenerator>().numberOfPlanets;
        commonElement.text = "Most Common Element: " + hitObject.GetComponentInParent<StarInfoGenerator>().mostCommonElement;
        commonCompound.text = "Most Common Compound: " + hitObject.GetComponentInParent<StarInfoGenerator>().mostCommonCompound;
        connectedStarsText.text = "Connected Stars: \n";

        foreach (var connectedStar in hitObject.GetComponentInParent<StarInformation>().connectedStars) {
            connectedStarsText.text += "-  " + connectedStar.name + "\n"; //Displays all connected stars names
        };
    }
}
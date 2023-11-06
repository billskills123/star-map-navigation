using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarSpawner : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private Slider starSlider;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Transform starEmpty;
    [SerializeField] private ConnectStars connectStars;
    [SerializeField] private DrawPathScript drawPathScript;
    [SerializeField] private Button targetGridButton;
    [SerializeField] private Button startGridButton;
    [SerializeField] private GameObject targetGridContent;
    [SerializeField] private GameObject startGridContent;

    [Header("Misc Variables")]
    public int targetGridChildren = 0;
    public int startGridChildren = 0;

    //Spawn in stars
    public void OnStart() {
        StartCoroutine(SpawnStars());
    }

    //Spawns in the chosen amount of stars at when the player presses start
    private IEnumerator SpawnStars() {
        for (int i = 0; i < starSlider.value; i++) {
            Vector3 starPosition = new(Random.Range(-500f, 500f), Random.Range(-500f, 500f), Random.Range(-500f, 500f));
            float randomSize = Random.Range(0.5f, 10f);

            GameObject newStar = Instantiate(starPrefab, starPosition, Quaternion.identity, starEmpty);

            newStar.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            newStar.GetComponent<Renderer>().material.SetColor("_EmissionColor", Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f));
            newStar.GetComponent<Light>().color = newStar.GetComponent<Renderer>().material.color;
            newStar.GetComponent<Light>().range = randomSize + Random.Range(5f, 15f);
            newStar.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        }
        yield return null;
        //Shows connections to other stars
        connectStars.ConnectNodes();
        connectStars.DrawStars();
    }

    //Updates the slider text to reflect chosen value
    public void UpdateSliderValue() {
        valueText.text = starSlider.value.ToString();
    }

    //Fills in the grid with all the currently spawned in stars
    public void FillTargetGrid() {
        if (targetGridChildren == 0) {
            foreach (StarInformation star in connectStars.stars) {
                Button newButton = Instantiate(targetGridButton, targetGridContent.transform);
                newButton.GetComponentInChildren<GridButtonScript>().ChangeStar(star);
                targetGridChildren++;
            }
        }
    }

    //Fills in the grid with all the currently spawned in stars
    public void FillStartGrid() {
        if (startGridChildren == 0) {
            foreach (StarInformation star in connectStars.stars) {
                if (star.name != drawPathScript.endStar.name) { //Ensures the start star cannot be the end star
                    Button newButton = Instantiate(startGridButton, startGridContent.transform);
                    newButton.GetComponentInChildren<GridButtonScript>().ChangeStar(star);
                    startGridChildren++;
                }
            }
        }
    }
}
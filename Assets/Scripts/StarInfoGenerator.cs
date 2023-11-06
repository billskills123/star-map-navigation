using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarInfoGenerator : MonoBehaviour {
    private string[] alphabet = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
    private string[] commonElement = {"Iron", "Copper", "Silver", "Mercury", "Lead", "Aluminium", "Gold", "Platinum", "Zinc", "Nickel", "Tin", "Carbon", "Hydrogen", "Oxygen", "Helium", "Lithium", "Sodium", "Calcium", "Sulphur", "Chlorine", "Nitrogen", "Fluorine", "Phosphorous", "Magnesium", "Argon", "Radon", "Bromine", "Boron"};
    private string[] commonCompounds = {"Water", "Hydrogen Peroxide", "Salt", "Carbon Dioxide", "Magnesium Oxide", "Iron Sulphide", "Ammonia", "Sulphuric Acid", "Acetic Acid", "Methane", "Nitrous Oxide", "Boric Acid", "Zinc Bromide", "Carbon Monoxide", "Carbonic Acid", "Hydrochloric Acid", "Lithium Peroxide"};
    private string starNumbers;

    [Header("Star Information")]
    public string starName;
    public float starSize;
    public int numberOfPlanets;
    public string mostCommonElement;
    public string mostCommonCompound;

    private void Start() {
        //Randomly generate between 1 and 4 letters
        for (int i = 0; i < Random.Range(1, 4); i++) {
            int randomLetter = Random.Range(0, alphabet.Length);
            starName += alphabet[randomLetter];
        }

        //Randomly generate between 1 and 4 numbers
        starNumbers += Random.Range(1, 9999).ToString();

        //Randomly generate most common element
        mostCommonElement += commonElement[Random.Range(0, commonElement.Length)];

        //Randomly generate most common compound
        mostCommonCompound += commonCompounds[Random.Range(0, commonCompounds.Length)];

        starName = starName + "-" + starNumbers; //Combine the letters and numbers to create the randomised star name e.g. ADF-32
        gameObject.name = starName;
        numberOfPlanets = Random.Range(1, 20);

        starSize = Mathf.Round(transform.localScale.x * 100.0f) / 100.0f; //Rounds the float to 2 decimal points

        GameObject.Find("StarsGroup").GetComponent<ConnectStars>().FindStars();
    }
}
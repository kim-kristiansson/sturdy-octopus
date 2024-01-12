using UnityEngine;
using System.Collections.Generic;
using TMPro; // Include the TextMeshPro namespace
using UnityEngine.UI; // Include the UI namespace for Button

public class HexagonCylinderPacker : MonoBehaviour
{
    public TMP_InputField diameterInputField;
    public TMP_InputField lengthInputField;
    public Button generateButton;
    public Cylinder cylinderPrefab;
    public Hexagon hexagonPrefab;


    private void Start()
    {
        generateButton.onClick.AddListener(OnGenerateClicked);
    }

    private void OnGenerateClicked()
    {
        float diameter = float.Parse(diameterInputField.text);
        float length = float.Parse(lengthInputField.text);

        // Check for valid input
        if (diameter > 0 && length > 0)
        {
            // Instantiate and position your cylinders here...
            GenerateCylindersInsideHexagon(diameter, length);
        }
        else
        {
            Debug.LogError("Invalid input for diameter or length");
        }
    }

    private void GenerateCylindersInsideHexagon(float diameter, float length)
    {
        // Calculate how many cylinders can fit inside the hexagon
        // This is where you'll use the logic to fit cylinders inside the hexagon

        // Example of instantiation (actual logic for positioning needs to be implemented)
        Instantiate(cylinderPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnButtons : MonoBehaviour
{
    public RubiksCube cube;

    public GameObject buttonPanel;

    private bool inverted = false;

    public void SetInverted(Toggle toggle)
    {
        inverted = toggle.isOn;
    }

    public void Turn(string rotation)
    {
        // Parse the string as a Rotation enum and add one if 'inverted' is true.
        int rot = (int)Enum.Parse(typeof(Rotation), rotation) + (inverted ? 1 : 0);

        cube.Turn((Rotation)rot);
    }

    void Update()
    {
        // Deactivate the button panel if the cube is turning to prevent multiple coroutines from running at once
        buttonPanel.SetActive(!cube.turning);
    }
}
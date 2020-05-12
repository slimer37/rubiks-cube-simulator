using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RubiksCode : MonoBehaviour
{
    public RubiksCube cube;

    public Vector3 selectedFaceNormal;

    public float functionDelay;

    public TextMeshProUGUI codingSpace;
    public TextMeshProUGUI errorSpace;

    public GameObject button;

    public Dictionary<string, string> keywords = new Dictionary<string, string>()
    {
        { "turn", "OnTurn"}
    };

    private int currentIndex = 0;
    private int currentLine = 0;

    public void OnSubmit()
    {
        button.SetActive(false);
        currentIndex = 0;
        StartCoroutine(Execute());
    }

    IEnumerator Execute()
    {
        while (currentIndex < codingSpace.text.Length - 1)
        {
            string function = GetNextWord();

            Execute(function);

            yield return new WaitForSeconds(functionDelay);
        }
        button.SetActive(true);
    }

    void Execute(string function)
    {
        if (keywords.ContainsKey(function))
        {
            Invoke(keywords[function], 0.0f);
        }
        else
        {
            errorSpace.text = $"Unrecognized word at index {currentIndex}. (Line {currentLine})";
        }
    }

    string GetNextWord()
    {
        string nextWord = string.Empty;

        for (int i = currentIndex; i < codingSpace.text.Length; i++)
        {
            if (i == codingSpace.text.Length) { break; }

            currentIndex = i;

            if (codingSpace.text[currentIndex] == ' ' || codingSpace.text[currentIndex] == '\n')
            {
                currentIndex++;

                if (codingSpace.text[currentIndex] == '\n') { currentLine++; }

                break;
            }
            else
            {
                nextWord += codingSpace.text[currentIndex];
            }
        }

        return nextWord.ToLower();
    }

    void OnTurn()
    {
        string param = GetNextWord();
        Rotation rotation = (Rotation)Enum.Parse(typeof(Rotation), param, true);

        cube.Turn(rotation);
    }
}
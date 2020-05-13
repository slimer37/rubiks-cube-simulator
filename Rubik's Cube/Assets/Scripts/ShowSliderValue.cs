using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class ShowSliderValue : MonoBehaviour
{
    public TextMeshProUGUI readingDisplay;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        readingDisplay.text = slider.value.ToString();
    }
}
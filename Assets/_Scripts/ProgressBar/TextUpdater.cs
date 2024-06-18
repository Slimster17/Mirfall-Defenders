using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    [Tooltip("Reference to the component used to display progress.")]
    [SerializeField] private TextMeshProUGUI _text;

    public void SetText(float progress) // Method to update the text with the current progress value
    {
        _text.SetText($"{(progress * 100).ToString("N2")}%");
    }
}

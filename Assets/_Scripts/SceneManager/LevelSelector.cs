using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [Tooltip("Array of level buttons.")]
    public Button[] lvlButtons;
    void Start() // Start is called before the first frame update
    {
        // Get the highest level unlocked from PlayerPrefs
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);

        // Loop through all level buttons
        for (int i = 0; i < lvlButtons.Length; i++)
        {
            // Disable the button if its level is higher than the highest unlocked level
            if (i+1>levelAt)
            {
                lvlButtons[i].interactable = false;
            }
        }
    }
}

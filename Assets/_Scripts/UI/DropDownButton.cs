using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownButton : MonoBehaviour
{

    [Tooltip("The panel that contains the button")]
    [SerializeField] private RectTransform _panel; 

    [Tooltip("The button that is used to open the drop down menu")]
    [SerializeField] private RectTransform _dropDownButton;

    private bool isHidden; // Flag to determine if the panel is hidden or not

    private void Awake()
    {
        _dropDownButton = GetComponent<RectTransform>();
    }


    public void MovePanel() // Move the panel and button to the new anchored positions
    {
        // Calculate panel and button dimensions
        float panelWidth = _panel.rect.width * _panel.transform.localScale.x;
        float buttonWidth = _dropDownButton.rect.width * _dropDownButton.transform.localScale.x;

        Debug.Log($"panel width = {panelWidth} button width = {buttonWidth}");

        // Determine the new anchored positions based on the current position
        Vector2 newPanelAnchoredPosition;
        Vector2 newButtonAnchoredPosition;

        if (isHidden)
        {
            newPanelAnchoredPosition = new Vector2(0, _panel.anchoredPosition.y);
            newButtonAnchoredPosition = new Vector2(14f + panelWidth - buttonWidth / 2, _dropDownButton.anchoredPosition.y);
        }
        else
        {
            newPanelAnchoredPosition = new Vector2(-10f -panelWidth + buttonWidth / 2, _panel.anchoredPosition.y);
            newButtonAnchoredPosition = new Vector2(-11f + buttonWidth / 2, _dropDownButton.anchoredPosition.y);
        }

        // Move the panel and button to the new anchored positions
        _panel.anchoredPosition = newPanelAnchoredPosition;
        _dropDownButton.anchoredPosition = newButtonAnchoredPosition;

        // Toggle the hidden state
        isHidden = !isHidden;
    }
}

    


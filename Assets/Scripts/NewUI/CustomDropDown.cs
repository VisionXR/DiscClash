using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class CustomDropDown : MonoBehaviour
{
    public GameObject DropDownPanel;
    public TMP_Text DropDownText;
    public List<TMP_Text> DropDownOptions;

    public UnityEvent<int> DropDownChangedEvent;

    public void OnCustomDropDownButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        if (DropDownPanel.activeSelf)
        {
            DropDownPanel.SetActive(false);
        }
        else
        {
            DropDownPanel.SetActive(true);
        }
    }

    public void OnDropDownOptionClicked(int optionIndex)
    {
        AudioManager.instance.PlayButtonClickSound();
        DropDownText.text = DropDownOptions[optionIndex].text;
        DropDownChangedEvent.Invoke(optionIndex);
        DropDownPanel.SetActive(false);
    }


}

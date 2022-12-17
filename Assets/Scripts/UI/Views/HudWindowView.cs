using Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudWindowView : APanel
{
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private Button _testButton;
    
    public void Subscribe(Action onClick)
    {
        _testButton.onClick.AddListener(onClick.Invoke);
    }
    
    public void Unsubscribe()
    {
        _testButton.onClick.RemoveAllListeners();
    }


    public void SetTestValue(string value)
    {
        _testText.text = value;
    }
}

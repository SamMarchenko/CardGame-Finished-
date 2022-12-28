using System.Collections;
using System.Collections.Generic;
using OneLine;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/CardPresets")]
public class CardPresetsDataBase : ScriptableObject
{
    [SerializeField] private CardPacksContainer CardPacksContainer;
    
    public List<CardPresets> CardPresets;

    [ContextMenu("Test")]
    public void Test()
    {
        Debug.Log("!!!!!!");
    }
}

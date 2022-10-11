using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTable : MonoBehaviour
{
    private Card[] _cards;
    [SerializeField] private List<Transform> _positions;
    public List<Transform> Positions => _positions;

    public void AddPosition(Transform position)
    {
        if (_positions.Contains(position))
        {
            Debug.Log("Этот слот уже существует");
            return;
        }
        _positions.Add(position);
    }

    public void RemovePosition(Transform position)
    {
        if (!_positions.Contains(position))
        {
            Debug.Log("Такого слота нет");
            return;
        }

        _positions.Remove(position);
    }

    public bool HasNearestFreePosition(Transform card, out Transform resultPosition)
    {
        var distance = Mathf.Infinity;
        resultPosition = null;
        foreach (var position in _positions)
        {
            var result = Vector3.Distance(card.position, position.position);
            if (result < distance)
            {
                distance = result;
                resultPosition = position;
            }
        }
        return resultPosition != null;
    }
}

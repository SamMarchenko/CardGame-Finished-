using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class DrawCardSlots : MonoBehaviour
{
    private Vector3 _size = new Vector3(70f, 1f, 100f);
    public ESlotStatus SlotStatus = ESlotStatus.Free;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, _size);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardSlots : MonoBehaviour
{
    private Vector3 _size = new Vector3(70f, 1f, 100f);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, _size);
    }
}

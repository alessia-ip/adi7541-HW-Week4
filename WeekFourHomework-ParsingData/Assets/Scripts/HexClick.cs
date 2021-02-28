using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log(this.gameObject.name);
    }
}

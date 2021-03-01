using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexClick : MonoBehaviour
{
    
    //when I click on a tile, i just want to change the current tile in the game manager to be this tile
    
    private void OnMouseDown()
    {
        Debug.Log(this.gameObject.name);
        GameManager.CurrentlySelected = this.gameObject;
    }
}

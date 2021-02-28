using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.U2D;
using UnityEngine;

public class SetTile : MonoBehaviour
{

    public string spriteName;

    private void OnEnable()
    {
        spriteName = this.gameObject.GetComponent<SpriteRenderer>().sprite.name;
    }    
    
    private void Update()
    {
        spriteName = this.gameObject.GetComponent<SpriteRenderer>().sprite.name;
    }
    
    
    
}

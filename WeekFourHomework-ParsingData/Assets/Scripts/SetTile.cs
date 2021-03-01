using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.U2D;
using UnityEngine;

public class SetTile : MonoBehaviour
{
    private float timer;
    
    public string spriteName;

    private bool tick = false;
    
    private void OnEnable()
    {
        spriteName = this.gameObject.GetComponent<SpriteRenderer>().sprite.name;
    }    
    
    private void Update()
    {
        
        //timer!
        timer += Time.deltaTime;
        
        //set sprite name based on this object's sprite
        spriteName = this.gameObject.GetComponent<SpriteRenderer>().sprite.name;

        //Debug.Log(tick + " and the mod is " + (int)timer % 10);
        
        //gold changes based on the tile types of each tile
        if ((int)timer % 10 == 0 && tick == false)
        {
            //Debug.Log("ADD GOLD");
            if (spriteName == TileTypes.cityHex.name)
            {
            
                GameManager.gold = GameManager.gold - 3;
                tick = true;
            
            } else if (spriteName == TileTypes.sciFiHex.name)
            {
            
                GameManager.gold = GameManager.gold - 1;
                tick = true;
            
            } else if (spriteName == TileTypes.marsHex.name)
            {
           
                GameManager.gold = GameManager.gold + 5;
                tick = true;
            
            }

         
        }
        //keeping track of the timer tick
        if (timer % 10 != 0)
        {
            tick = false;
        }

    }
    
    
    
}

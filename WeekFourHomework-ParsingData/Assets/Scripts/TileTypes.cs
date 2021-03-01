using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypes : MonoBehaviour
{
    private TileTypes tInstance;
    
    //all types of tiles
    public static Sprite cityHex;
    public static Sprite sciFiHex;
    public static Sprite marsHex;
    public static Sprite blankHex;

    //i wanted to set these publicly but needed them to be statically accessible oof
    public Sprite cit;
    public Sprite sci;
    public Sprite mars;
    public Sprite blank;

    //canvas!
    public GameObject canvas; 
    
    void Awake()
    {
        if (tInstance == null)
        {
            tInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //set the tiles
        cityHex = cit;
        sciFiHex = sci;
        marsHex = mars;
        blankHex = blank;

        //set the canvas as the game manager's canvas
        GameManager.can = canvas;

    }


    //all these functions are for changing tile sprites and are assigned to buttons!
    public void SetToMars()
    {
        GameManager.CurrentlySelected.GetComponent<SpriteRenderer>().sprite = marsHex;
        closeCanvas();
    }
    public void SetToCity()
    {
        GameManager.CurrentlySelected.GetComponent<SpriteRenderer>().sprite = cityHex;
        closeCanvas();
    }
    public void SetToBlank()
    {
        GameManager.CurrentlySelected.GetComponent<SpriteRenderer>().sprite = blank;
        closeCanvas();
    }
    public void SetToSciFi()
    {
        GameManager.CurrentlySelected.GetComponent<SpriteRenderer>().sprite = sciFiHex;
        closeCanvas();
    }


    //close the canvas and update the tile set
    public void closeCanvas()
    {
        GameManager.updateTiles = true;
        canvas.SetActive(false);
    }
    
    
}

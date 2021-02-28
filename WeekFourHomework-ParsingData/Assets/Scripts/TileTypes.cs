using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypes : MonoBehaviour
{
    private TileTypes tInstance;
    
    public static Sprite cityHex;
    public static Sprite sciFiHex;
    public static Sprite marsHex;
    public static Sprite blankHex;

    public Sprite cit;
    public Sprite sci;
    public Sprite mars;
    public Sprite blank;

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

        cityHex = cit;
        sciFiHex = sci;
        marsHex = mars;
        blankHex = blank;

        GameManager.can = canvas;

    }


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


    public void closeCanvas()
    {
        GameManager.updateTiles = true;
        canvas.SetActive(false);
    }
    
    
}

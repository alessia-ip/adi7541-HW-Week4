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

    }
    
    
}

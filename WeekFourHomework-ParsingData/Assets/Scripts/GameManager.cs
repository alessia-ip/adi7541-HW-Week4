using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;
using Application = UnityEngine.Application;

public class GameManager : MonoBehaviour
{
    //this is the current instance of the project
    public GameObject instance;
    private static GameObject currentlySelected;

    //what are we doing? Updating? Saving?
    public static bool updateTiles;
    private static bool save;
    
    //canvas
    public static GameObject can;

    //gold recording values!!
    public static int gold;
    public List<int> goldRecord;
    public Text goldText;
    
    //keeps track of the currently selected tiles
    public static GameObject CurrentlySelected
    {
        get
        {
            //just get the selected tile here
            return currentlySelected;
        }
        set
        {
            //if we're setting the tile:
            //set the canvas on if it's a value
            //set the canvas to off if it's null (since the canvas is for changing the tile type)
            currentlySelected = value;
            if (currentlySelected != null)
            {
                can.SetActive(true);
            }
            else
            {
                save = true;
            }
        }
    }

    //IO paths
    private static string DIR = "/Logs";
    static string FILE_NAME = "/save.txt";
    private string PATH_TO_SAVE;
    private string PATH_TO_GOLD;
    static string FILE_GOLD = "/gold.txt";

    //keeps track of the physical tiles
    //then keeps track of the sprites for each tile
    public GameObject[] tiles;
    public List<string> tileType;

    //a string of tiles we're saving to our file!
    private string tilesToSave;

    //are we writing over the tile file?
    private bool writeOver = false;

    //this text is only for the end of the game scene
    public Text endText;
    
    // This is my singleton game manager
    void Awake()
    {
        if (instance == null)
        {
            instance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        
        //THIS IS THE MAIN TILE SAVE
        PATH_TO_SAVE = Application.dataPath + DIR + FILE_NAME;

        //if the path doesn't exist, save and load function but also write over the file as new
        if (!File.Exists(PATH_TO_SAVE))
        {
            Debug.Log("Does Not Exist");
            File.Create(PATH_TO_SAVE).Dispose();
            Debug.Log("path2:" + PATH_TO_SAVE);
            writeOver = true;
            saveandload();
        }
        else
        {
            //otherwise, we're not overwriting the tiles
            //instead, we are reading the tile list
            //then we are saving those to the tile types list, and executing save and load
            Debug.Log("path2:" + PATH_TO_SAVE);
            tilesToSave = File.ReadAllText(PATH_TO_SAVE);
            String[] readTiles = tilesToSave.Split(',');
            for (int i = 0; i < readTiles.Length - 1; i++)
            {
                tileType.Add(readTiles[i]);
            }

            Debug.Log("added");
            writeOver = false; //writeover is not true here! 
            saveandload();
        }
        
        
        //This is the gold save
        PATH_TO_GOLD = Application.dataPath + DIR + FILE_GOLD;

        if (!File.Exists(PATH_TO_GOLD))
        {
            File.Create(PATH_TO_GOLD).Dispose();
        }
        else
        {
            //if gold is present, we want to find out only what the last value was in that list
            string[] goldRead = File.ReadAllText(PATH_TO_GOLD).Split(',');
            for (int i = 0; i < goldRead.Length; i++)
            {
                goldRecord.Add(Int32.Parse(goldRead[i]));
            }

            gold = goldRecord[1];
        }
        
    }

    private void Update()
    {
        //Debug.Log(gold);
        //if update tiles is true, update the  tile set
        if (updateTiles == true)
        {
            updateTileSet();
            updateTiles = false; //only do it once
        }
        
        //we only want to call save when update tiles is false
        //this is so the tiles have time to update and are correctly recorded before we save it
        if (save == true && updateTiles == false)
        {
            saveandload();
            save = false;
        }

        //update gold text at the bottom of the screen
        goldText.text = "Your current gold is: " + gold;
    }


    void updateTileSet()
    {
        //for all the tiles, get the current tile
        //find the right tile to compare the tile # to the sprite #
        //then change that tile's sprite reference in the list of saved tiles
        //then currently selected should be null 
        for (int i = 0; i < tiles.Length; i++)
        {
            Debug.Log(currentlySelected.name + ":" + tiles[i].name);
            if (currentlySelected == tiles[i])
            {
                Debug.Log(currentlySelected.GetComponent<SpriteRenderer>().sprite.name);
                tileType[i] = currentlySelected.GetComponent<SpriteRenderer>().sprite.name;
            }
        }
        CurrentlySelected = null;
    }
    
    void saveandload()
    {
        //if write over is true, we are saving all the tiles for the first time here
        if (tiles.Length == 0 && writeOver == true)
        {
            Debug.Log("WRITE OVER");
            tiles =  GameObject.FindGameObjectsWithTag("Tile");
            for (int i = 0; i < tiles.Length ; i++)
            {
                tiles[i].AddComponent<SetTile>();
                Debug.Log(tiles[i].GetComponent<SetTile>().spriteName);
                tileType.Add(tiles[i].GetComponent<SetTile>().spriteName);
                tilesToSave = tilesToSave + tileType[i] + ",";
                Debug.Log(tilesToSave);
            }
            Debug.Log("path:" + PATH_TO_SAVE);
            File.WriteAllText(PATH_TO_SAVE, tilesToSave + "");
        }
        else if (tiles.Length == 0) //othewise, if the list is empty but write over isn't true, they DO exist in the file
        {
            //we're taking all the tiles from the file and assigning them to the right objects
            tiles =  GameObject.FindGameObjectsWithTag("Tile");
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].AddComponent<SetTile>();
                if (tileType[i] == TileTypes.cityHex.name)
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.cityHex;
                } else if (tileType[i] == TileTypes.sciFiHex.name)
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.sciFiHex;
                } else if (tileType[i] == TileTypes.marsHex.name)
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.marsHex;
                }  else if (tileType[i] == TileTypes.blankHex.name)
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.blankHex;
                }
                else //this is a catch - if none of the above are true, we want to default to the blank hex in case of unsaved tiles
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.blankHex;
                }
            }
        }
        else
        {
            //this saves the game normally and records all the current tiles
            Debug.Log("save new");
            tilesToSave = "";
            for (int i = 0; i < tileType.Count; i++)
            {
                tilesToSave = tilesToSave + tileType[i] + ',';
            }
            File.WriteAllText(PATH_TO_SAVE, tilesToSave + "");
        }
    }

    public void EndGame()
    {
        int prevGold = goldRecord[goldRecord.Count - 1]; //the prev gold should be the last value
        string goldToSave = prevGold + "," + gold; //all I need to keep track of is the previous val, and the current val! We can discard the rest :) 
        Debug.Log(goldToSave);
        File.WriteAllText(PATH_TO_GOLD, goldToSave); //write it
        endText.text = "Previous Gold: " + prevGold + "\n" + "Current Gold: " + gold; //this is the end screen display of what you started and ended with
        endText.gameObject.SetActive(true);
        goldText.gameObject.SetActive(false);
        SceneManager.LoadScene(1); //load the scene with no tiles

    }
    

}

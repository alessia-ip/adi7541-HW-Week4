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
    public GameObject instance;
    private static GameObject currentlySelected;

    public static bool updateTiles;
    private static bool save;
    
    public static GameObject can;

    public static int gold;
    public List<int> goldRecord;
    public Text goldText;
    
    public static GameObject CurrentlySelected
    {
        get
        {
            return currentlySelected;
        }
        set
        {
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

    private static string DIR = "/Logs";
    static string FILE_NAME = "/save.txt";
    private string PATH_TO_SAVE;
    private string PATH_TO_GOLD;
    static string FILE_GOLD = "/gold.txt";

    public GameObject[] tiles;
    public List<string> tileType;

    private string tilesToSave;

    private bool writeOver = false;

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
            Debug.Log("path2:" + PATH_TO_SAVE);
            tilesToSave = File.ReadAllText(PATH_TO_SAVE);
            String[] readTiles = tilesToSave.Split(',');
            for (int i = 0; i < readTiles.Length - 1; i++)
            {
                tileType.Add(readTiles[i]);
            }

            Debug.Log("added");
            writeOver = false;
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
            string[] goldRead = File.ReadAllText(PATH_TO_GOLD).Split(',');
            for (int i = 0; i < goldRead.Length; i++)
            {
                goldRecord.Add(Int32.Parse(goldRead[i]));
            }
        }
        
    }

    private void Update()
    {
        //Debug.Log(gold);
        if (updateTiles == true)
        {
            updateTileSet();
            updateTiles = false;
        }
        if (save == true && updateTiles == false)
        {
            saveandload();
            save = false;
        }

        goldText.text = "Your current gold is: " + gold;
    }


    void updateTileSet()
    {
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
        else if (tiles.Length == 0)
        {
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
                else
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.blankHex;
                }
            }
        }
        else
        {
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
        int prevGold = goldRecord[goldRecord.Count - 1];
        string goldToSave = prevGold + "," + gold; //all I need to keep track of is the previous val, and the current val! We can discard the rest :) 
        Debug.Log(goldToSave);
        File.WriteAllText(PATH_TO_GOLD, goldToSave);
        endText.text = "Previous Gold: " + prevGold + "\n" + "Current Gold: " + gold;
        endText.gameObject.SetActive(true);
        goldText.gameObject.SetActive(false);
        SceneManager.LoadScene(1);

    }
    

}

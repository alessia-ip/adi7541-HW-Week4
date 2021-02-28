using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;
using Application = UnityEngine.Application;

public class GameManager : MonoBehaviour
{
    public GameObject instance;
    private static GameObject currentlySelected;

    public static bool updateTiles;
    private static bool save;
    
    public static GameObject can;
    
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

    public GameObject[] tiles;
    public List<string> tileType;

    private string tilesToSave;

    private bool writeOver = false;
    
    
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
        
    }

    private void Update()
    {
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

}

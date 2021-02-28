using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;
using Application = UnityEngine.Application;

public class GameManager : MonoBehaviour
{
    public GameObject instance;
    private static GameObject currentlySelected;
    
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
        else
        {
            tiles =  GameObject.FindGameObjectsWithTag("Tile");
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].AddComponent<SetTile>();
                if (tileType[i] == "modern_largeBuilding")
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.cityHex;
                } else if (tileType[i] == "scifi_hangar")
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.cityHex;
                } else if (tileType[i] == "mars_07")
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.marsHex;
                }  else 
                {
                    tiles[i].GetComponent<SpriteRenderer>().sprite = TileTypes.blankHex;
                }
            }
        }
 
    }

}

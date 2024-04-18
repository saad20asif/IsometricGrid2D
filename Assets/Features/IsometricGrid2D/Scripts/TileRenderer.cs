using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Newtonsoft.Json;

[System.Serializable]
public class Tile
{
    public int TileType { get; set; }
}

[System.Serializable]
public class TerrainData
{
    public List<List<Tile>> TerrainGrid { get; set; }
}

public class TileRenderer : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Array of prefabs for each tile type
    public string jsonFilePath;

    private List<List<Tile>> terrainGrid;

    void Start()
    {
        LoadJSON();
    }

    [Button("LOAD FROM JSON")]
    void LoadJSON()
    {
        try
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            Debug.Log(jsonText);
            //TerrainData terrainData1 = JsonConvert.
            // terrainData = JsonUtility.FromJson<TerrainData>(jsonText);
            TerrainData terrainData = JsonConvert.DeserializeObject<TerrainData>(jsonText);

            if (terrainData != null && terrainData.TerrainGrid != null)
            {
                terrainGrid = terrainData.TerrainGrid;
                PrintTerrainData(terrainData);
                //RenderTiles();
            }
            else
            {
                Debug.LogError("TerrainData or TerrainGrid is null.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading JSON: " + e.Message);
        }
    }

    void PrintTerrainData(TerrainData terrainData)
    {
        List<List<Tile>> grid = terrainData.TerrainGrid;
        int rows = grid.Count;

        Debug.Log("TerrainGrid:");

        foreach (var row in grid)
        {
            string rowStr = "";
            foreach (var tile in row)
            {
                rowStr += tile.TileType + " ";
            }
            Debug.Log(rowStr);
        }
    }

    void RenderTiles()
    {
        if (tilePrefabs == null || tilePrefabs.Length == 0)
        {
            Debug.LogError("Tile prefabs are not assigned.");
            return;
        }

        if (terrainGrid == null)
        {
            Debug.LogError("Terrain grid is null.");
            return;
        }

        for (int y = 0; y < terrainGrid.Count; y++)
        {
            List<Tile> row = terrainGrid[y];
            for (int x = 0; x < row.Count; x++)
            {
                int tileType = row[x].TileType;
                if (tileType >= 0 && tileType < tilePrefabs.Length)
                {
                    GameObject prefab = tilePrefabs[tileType];
                    if (prefab != null)
                    {
                        Vector3 tilePosition = IsometricToCartesian(new Vector2(x, y));
                        GameObject tile = Instantiate(prefab, tilePosition, Quaternion.identity);
                        tile.transform.SetParent(transform);
                    }
                    else
                    {
                        Debug.LogError("Prefab for tile type " + tileType + " is not assigned.");
                    }
                }
                else
                {
                    Debug.LogError("Invalid tile type: " + tileType);
                }
            }
        }
    }

    Vector3 IsometricToCartesian(Vector2 isometricCoord)
    {
        float x = isometricCoord.x - isometricCoord.y;
        float y = (isometricCoord.x + isometricCoord.y) / 2f;
        return new Vector3(x, y, 0);
    }
}

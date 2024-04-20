using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System;

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
    [SerializeField] private JsonReaderSo JsonReaderSo;
    private Transform[,] grid;

    public float xOffset = 1.0f;
    public float yOffset = 1.0f;

    private int _tileType = 0;

    [Button("LOAD FROM JSON")]
    private void LoadIt()
    {
        DestroyTiles();
        JsonReaderSo.LoadDataFromFile();
        InitializeGrid();
        RenderTiles();
    }

    [Button("DESTROY TILES")]
    private void DestroyTiles()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    private void InitializeGrid()
    {
        if (JsonReaderSo.TerrainData != null)
        {
            int rows = JsonReaderSo.TerrainData.TerrainGrid.Count;
            int cols = rows > 0 ? JsonReaderSo.TerrainData.TerrainGrid[0].Count : 0;
            grid = new Transform[rows, cols];
        }
    }

    private void RenderTiles()
    {
        if (JsonReaderSo.TerrainData != null)
        {
            for (int i = 0; i < JsonReaderSo.TerrainData.TerrainGrid.Count; i++)
            {
                List<Tile> row = JsonReaderSo.TerrainData.TerrainGrid[i];
                int cols = row.Count;

                for (int j = 0; j < cols; j++)
                {
                    Tile tile = row[j];
                    Vector3 position =
                        new Vector3(j * xOffset, i * yOffset, 0); // Adjusted position based on grid coordinates

                    string prefabName = null;
                    switch (tile.TileType)
                    {
                        case 0:
                            prefabName = "dirtPrefab";
                            _tileType = 0;
                            break;
                        case 1:
                            prefabName = "grassPrefab";
                            _tileType = 1;
                            break;
                        case 2:
                            prefabName = "stonePrefab";
                            _tileType = 2;
                            break;
                        case 3:
                            prefabName = "woodPrefab";
                            _tileType = 3;
                            break;
                        default:
                            Debug.LogWarning("Unknown tile type: " + tile.TileType);
                            break;
                    }

                    if (prefabName != null)
                    {
                        GameObject tileObject = Instantiate(Resources.Load(prefabName), transform) as GameObject;
                        tileObject.transform.localPosition = position;
                        tileObject.transform.localRotation = Quaternion.identity;
                        tileObject.name = "[" + j + " " + i + "]";
                        if (tileObject.GetComponent<TileInfo>() != null)
                        {
                            tileObject.GetComponent<TileInfo>().TileType = _tileType;
                            tileObject.GetComponent<TileInfo>().myIndex = new Vector2Int(j, i);
                        }

                        // Store the tile object in the grid array
                        grid[i, j] = tileObject.transform;
                        print(grid[i, j].name);

                    }
                }
            }
        }
    }
}

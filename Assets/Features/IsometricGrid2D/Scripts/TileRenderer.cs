using UnityEngine;
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
    [SerializeField] private JsonReaderSo JsonReaderSo;
    public GameObject dirtPrefab; // Array of prefabs for each tile type
    public GameObject grassPrefab; // Array of prefabs for each tile type
    public GameObject stonePrefab; // Array of prefabs for each tile type
    public GameObject woodPrefab; // Array of prefabs for each tile type

    private List<List<Tile>> terrainGrid;
    public float xOffset = 1.0f;
    public float yOffset = 1.0f;
    

    [Button("LOAD FROM JSON")]
    private void LoadIt()
    {
        DestroyTiles();
        JsonReaderSo.LoadDataFromFile();
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

                    GameObject prefab = null;
                    switch (tile.TileType)
                    {
                        case 0:
                            prefab = dirtPrefab;
                            break;
                        case 1:
                            prefab = grassPrefab;
                            break;
                        case 2:
                            prefab = stonePrefab;
                            break;
                        case 3:
                            prefab = woodPrefab;
                            break;
                        default:
                            Debug.LogWarning("Unknown tile type: " + tile.TileType);
                            break;
                    }

                    if (prefab != null)
                    {
                        GameObject tilee = Instantiate(prefab, transform);
                        tilee.transform.localPosition = position;
                        tilee.transform.localRotation = Quaternion.identity;
                        tilee.name = "["+j+" "+i+"]";
                    }
                }
            }
        }
    }


}

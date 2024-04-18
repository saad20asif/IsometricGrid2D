using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JsonReaderData", menuName = "ScriptableObjects/JsonReaderSO", order = 1)]
public class JsonReaderSo : JsonReaderBase<TerrainData>
{
    public TerrainData TerrainData;
    [SerializeField] private string FileName;
    public void LoadDataFromFile()
    {
        TerrainData = LoadData(FileName);

        if(TerrainData!=null)
            PrintTerrainData(TerrainData);
    }
    public void PrintTerrainData(TerrainData terrainData)
    {
        int rows = terrainData.TerrainGrid.Count-1;
        for (int i = 0; i < rows; i++)
        {
            string rowStr = "";
            List<Tile> row = terrainData.TerrainGrid[i];
            int cols = row.Count;

            for (int j = 0; j < cols; j++)
            {
                Tile tile = row[j];
                rowStr += tile.TileType + " ";
            }

            Debug.Log(rowStr);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

[CreateAssetMenu(fileName = "JsonReaderData", menuName = "ScriptableObjects/JsonReaderSO", order = 1)]
public class JsonReaderSo : JsonReaderBase<TerrainData>
{
    public TerrainData TerrainData;
    public Transform[,] Grid;
    public int Rows, Cols;
    [SerializeField] private string FileName;
    public void LoadDataFromFile()
    {
        TerrainData = LoadData(FileName);
        if (TerrainData!=null)
            PrintTerrainData(TerrainData);
    }
    public void PrintTerrainData(TerrainData terrainData)
    {
        Rows = terrainData.TerrainGrid.Count - 1;
        for (int i = 0; i < Rows; i++)
        {
            string rowStr = "";
            List<Tile> row = terrainData.TerrainGrid[i];
            Cols = row.Count;

            for (int j = 0; j < Cols; j++)
            {
                Tile tile = row[j];
                rowStr += tile.TileType + " ";
            }

            //Debug.Log(rowStr);
        }
    }
}

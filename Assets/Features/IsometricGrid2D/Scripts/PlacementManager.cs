using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TableOrientation
{
    Horizontal,
    Vertical
}
public class PlacementManager : MonoBehaviour
{
    [SerializeField] int _placementIndex = 3;
    public LayerMask LayerToRaycast; // Layer mask for the sprites
    public float HoverScaleFactor = 1.2f; // Scale factor for hover animation
    RaycastHit2D _hit;
    Transform _hoveredSprite;
    Transform _lastHoveredSprite;
    private int _defaultSortingOrder; // Default sorting order of the sprites
    [SerializeField] Transform TablesHolder;
    [SerializeField] private JsonReaderSo JsonReaderSo;
    [SerializeField] Transform shadowTable;

    public TableOrientation CurrentOrientation = TableOrientation.Horizontal;
    void Start()
    {
        // Get the default sorting order of the sprites
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            _defaultSortingOrder = spriteRenderer.sortingOrder;
        }
    }

    void Update()
    {
        SelectTile();
        CheckTablePlacement();
    }
    private void SelectTile()
    {
        _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, LayerToRaycast);

        if (_hit.collider != null)
        {
            Transform newHoveredSprite = _hit.transform;
            SetShadowTable(newHoveredSprite);
            if (newHoveredSprite != _lastHoveredSprite)
            {
                ResetLastHoveredSprite();
                SetHoveredSprite(newHoveredSprite);
            }
            _lastHoveredSprite = newHoveredSprite;
        }
        else
        {
            ResetLastHoveredSprite();
        }
    }
    private void SetShadowTable(Transform newHoveredSprite)
    {
        shadowTable.transform.SetParent(newHoveredSprite);
        shadowTable.gameObject.SetActive(true);
        ResetTransform(shadowTable);
    }

    private void ResetLastHoveredSprite()
    {
        if (_lastHoveredSprite != null)
        {
            _lastHoveredSprite.localScale = Vector3.one;
            var lastSpriteRenderer = _lastHoveredSprite.GetComponent<SpriteRenderer>();
            if (lastSpriteRenderer != null)
            {
                lastSpriteRenderer.sortingOrder = _defaultSortingOrder;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer component not found on: " + _lastHoveredSprite.name);
            }
            _lastHoveredSprite = null;
        }
    }

    private void SetHoveredSprite(Transform newHoveredSprite)
    {
        _hoveredSprite = newHoveredSprite;
        _hoveredSprite.localScale = Vector3.one * HoverScaleFactor;
        var hoveredSpriteRenderer = _hoveredSprite.GetComponent<SpriteRenderer>();
        if (hoveredSpriteRenderer != null)
        {
            hoveredSpriteRenderer.sortingOrder = _defaultSortingOrder + 1;
        }
    }

    private void CheckTablePlacement()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2Int _selectedTileIndex = _hoveredSprite.GetComponent<TileInfo>().MyIndex;
            if (CheckIfThisTileAvailable(_selectedTileIndex))
            {
                if (CurrentOrientation == TableOrientation.Horizontal)
                {
                    HorizontalPlacing(_selectedTileIndex);
                }
                else if (CurrentOrientation == TableOrientation.Vertical)
                {
                    VerticalPlacing(_selectedTileIndex);
                }
            }
        }
    }
    private void HorizontalPlacing(Vector2Int selectedTileIndex)
    {
        int _x = selectedTileIndex.x;
        int _y = selectedTileIndex.y + 1;
        Vector2Int nextTileIndex = new Vector2Int(_x, _y);

        if (IsThisTileEmpty(nextTileIndex)) // Check If next tile is available
        {
            //print("Next Tile is also avaialable : " + _hoveredSprite.GetComponent<TileInfo>().MyIndex);
            MarkTilesAsFilled(_x, _y);
            SpawnTable(selectedTileIndex, CurrentOrientation);
        }
        else
        {
            // Now check if previous tile available
            _x = selectedTileIndex.x;
            _y = selectedTileIndex.y - 1;
            Vector2Int _previosTileIndex = new Vector2Int(_x, _y);
            if (IsThisTileEmpty(_previosTileIndex))
            {
                MarkTilesAsFilled(_previosTileIndex.x, _previosTileIndex.y);
                SpawnTable(_previosTileIndex, CurrentOrientation);
            }
        }
    }
    private void VerticalPlacing(Vector2Int selectedTileIndex)
    {
        int _x = selectedTileIndex.x+1;
        int _y = selectedTileIndex.y; // Offset for vertical placement
        Vector2Int _nextTileIndex = new Vector2Int(_x, _y);

        if (IsThisTileEmpty(_nextTileIndex)) // Check if the next tile is available
        {
            MarkTilesAsFilled(_x, _y);
            SpawnTable(selectedTileIndex, TableOrientation.Vertical);
        }
        else
        {
            // Check if the previous tile is available
            _x = selectedTileIndex.x - 1; // Adjust for previous tile
            Vector2Int _previousTileIndex = new Vector2Int(_x, _y);
            if (IsThisTileEmpty(_previousTileIndex))
            {
                MarkTilesAsFilled(_previousTileIndex.x, _previousTileIndex.y);
                SpawnTable(_previousTileIndex, TableOrientation.Vertical);
            }
        }
    }
    private void MarkTilesAsFilled(int x, int y)
    {
        _hoveredSprite.GetComponent<TileInfo>().Filled = true;
        JsonReaderSo.Grid[x, y].GetComponent<TileInfo>().Filled = true;
    }
    private bool CheckIfThisTileAvailable(Vector2Int index)
    {
        if (JsonReaderSo.Grid!= null)
        {
            if (JsonReaderSo.Grid[index.x,index.y].GetComponent<TileInfo>() != null)
            {
                if(JsonReaderSo.Grid[index.x, index.y].GetComponent<TileInfo>().TileType == _placementIndex)
                {
                    if (JsonReaderSo.Grid[index.x, index.y].GetComponent<TileInfo>().Filled == false)
                    {
                        return true;
                    }
                }
            }
        }
        print($"This tile {index} is not available!");
        return false;
    }
    private bool IsThisTileEmpty(Vector2Int currentIndex)
    {
        //print("Rows : " + JsonReaderSo.Rows + "  Cols : " + JsonReaderSo.Cols);
        //print("currentIndex : " + currentIndex);
        if(currentIndex.y < JsonReaderSo.Cols && currentIndex.y>=0)
        {
            return CheckIfThisTileAvailable(currentIndex);
        }
        print("Next Tile is not available!");
        return false;
    }
    private void SpawnTable(Vector2Int index, TableOrientation orientation)
    {
        string _prefabName = (orientation == TableOrientation.Horizontal) ? "HorizontalTable" : "VericalTable";
        GameObject table = Instantiate(Resources.Load(_prefabName), JsonReaderSo.Grid[index.x, index.y]) as GameObject;
    }
    private void ResetTransform(Transform objectToReset)
    {
        shadowTable.transform.localPosition = Vector3.zero;
        shadowTable.localScale = Vector3.one;
        shadowTable.localEulerAngles = Vector3.zero;
    }

}

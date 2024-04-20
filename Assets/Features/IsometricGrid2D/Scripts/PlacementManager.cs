using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] int placementIndex = 3;
    public LayerMask SpriteLayer; // Layer mask for the sprites
    public float HoverScaleFactor = 1.2f; // Scale factor for hover animation
    int _selectedSortingOrder = 1; // Sorting order for the selected tile
    RaycastHit2D _hit;
    Transform _hoveredSprite;
    SpriteRenderer _hoveredSpriteRenderer;
    SpriteRenderer _lastSpriteRenderer;
    private Transform _lastHoveredSprite;
    private int _defaultSortingOrder; // Default sorting order of the sprites
    [SerializeField] Transform TablesHolder;
    [SerializeField] private JsonReaderSo JsonReaderSo;


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
        HoverOverGrid();
        CheckTablePlacement();
    }
    private void HoverOverGrid()
    {
        // Cast a ray from the mouse position to detect the sprites
        _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, SpriteLayer);

        // If it hits something...
        if (_hit.collider != null)
        {
            _hoveredSprite = _hit.transform;
            // Check if the hovered sprite is different from the last one
            if (_hoveredSprite != _lastHoveredSprite)
            {
                // Reset scale of the last hovered sprite
                if (_lastHoveredSprite != null)
                {
                    _lastHoveredSprite.localScale = Vector3.one;
                    // Reset the sorting order of the last hovered sprite
                    SpriteRenderer lastSpriteRenderer = _lastHoveredSprite.GetComponent<SpriteRenderer>();
                    if (lastSpriteRenderer != null)
                    {
                        lastSpriteRenderer.sortingOrder = _defaultSortingOrder;
                    }
                }

                // Scale up the hovered sprite
                _hoveredSprite.localScale = Vector3.one * HoverScaleFactor;

                // Set the sorting order of the hovered sprite
                _hoveredSpriteRenderer = _hoveredSprite.GetComponent<SpriteRenderer>();
                if (_hoveredSpriteRenderer != null)
                {
                    _hoveredSpriteRenderer.sortingOrder = _selectedSortingOrder;
                }

                // Update last hovered sprite
                _lastHoveredSprite = _hoveredSprite;
            }
        }
        else
        {
            // Reset scale and sorting order of the last hovered sprite when not hovering over any sprite
            if (_lastHoveredSprite != null)
            {
                _lastHoveredSprite.localScale = Vector3.one;
                _lastSpriteRenderer = _lastHoveredSprite.GetComponent<SpriteRenderer>();
                if (_lastSpriteRenderer != null)
                {
                    _lastSpriteRenderer.sortingOrder = _defaultSortingOrder;
                }
                else
                {
                    Debug.LogWarning("SpriteRenderer component not found on: " + _lastHoveredSprite.name);
                }
                _lastHoveredSprite = null;
            }
        }
    }
    private void CheckTablePlacement()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(CheckIfThisTileAvailable(_hoveredSprite.GetComponent<TileInfo>().MyIndex))
            {
                int x = _hoveredSprite.GetComponent<TileInfo>().MyIndex.x;
                int y = _hoveredSprite.GetComponent<TileInfo>().MyIndex.y + 1;
                Vector2Int nextTileIndex = new Vector2Int(x, y);

                if (IsThisTileEmpty(nextTileIndex)) // Check If next tile is available
                {
                    //print("Next Tile is also avaialable : " + _hoveredSprite.GetComponent<TileInfo>().MyIndex);
                    _hoveredSprite.GetComponent<TileInfo>().Filled = true;
                    JsonReaderSo.Grid[x, y - 1].GetComponent<TileInfo>().Filled = true;
                    SpawnTable(_hoveredSprite.GetComponent<TileInfo>().MyIndex);
                }
                else
                {
                    // Now check if previous tile available
                    x = _hoveredSprite.GetComponent<TileInfo>().MyIndex.x;
                    y = _hoveredSprite.GetComponent<TileInfo>().MyIndex.y - 1;
                    Vector2Int previosTileIndex = new Vector2Int(x, y);
                    if (IsThisTileEmpty(previosTileIndex))
                    {
                        _hoveredSprite.GetComponent<TileInfo>().Filled = true;
                        JsonReaderSo.Grid[previosTileIndex.x, previosTileIndex.y].GetComponent<TileInfo>().Filled = true;
                        SpawnTable(previosTileIndex);
                    }
                }
            }
        }
    }
    private bool CheckIfThisTileAvailable(Vector2Int index)
    {
        if (JsonReaderSo.Grid!= null)
        {
            if (JsonReaderSo.Grid[index.x,index.y].GetComponent<TileInfo>() != null)
            {
                if(JsonReaderSo.Grid[index.x, index.y].GetComponent<TileInfo>().TileType == placementIndex)
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
    private void SpawnTable(Vector2Int index)
    {
        GameObject table = Instantiate(Resources.Load("HorizontalTable"),JsonReaderSo.Grid[index.x, index.y]) as GameObject;
    }
}

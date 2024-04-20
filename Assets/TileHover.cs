using System.Collections.Generic;
using UnityEngine;

public class TileHover : MonoBehaviour
{
    public LayerMask SpriteLayer; // Layer mask for the sprites
    public float HoverScaleFactor = 1.2f; // Scale factor for hover animation
    int _selectedSortingOrder = 1; // Sorting order for the selected tile
    RaycastHit2D _hit;
    Transform _hoveredSprite;
    SpriteRenderer _hoveredSpriteRenderer;
    SpriteRenderer _lastSpriteRenderer;
    private Transform _lastHoveredSprite;
    private int _defaultSortingOrder; // Default sorting order of the sprites

    
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
}

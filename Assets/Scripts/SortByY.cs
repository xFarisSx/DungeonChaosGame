using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SortByY : MonoBehaviour
{
    public Grid grid;
    public float sortingOffset = 0.00f;

    private void Start()
    {
        if (grid == null)
            grid = GetComponent<Grid>();

        SortTilesByY();
    }

    private void SortTilesByY()
    {
        // Get the Tilemap component from the Grid
        var tilemap = grid.GetComponentInChildren<Tilemap>();

        // Iterate through each tile in the tilemap
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            var tile = tilemap.GetTile(position);

            // Check if the position has a tile
            if (tile != null)
            {
                // Create a GameObject for the tile
                var tileGameObject = new GameObject("Tile");
                tileGameObject.transform.SetParent(tilemap.transform);
                tileGameObject.transform.position = tilemap.CellToWorld(position);

                // Add a SpriteRenderer component to the GameObject
                var spriteRenderer = tileGameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = tilemap.GetSprite(position);
                spriteRenderer.sortingLayerName = "Tiles";

                // Calculate the sorting order based on Y position
                var sortingOrder = Mathf.RoundToInt(position.y * 100f) * -1;
                spriteRenderer.sortingOrder = sortingOrder;

                // Adjust the sorting order using the offset
                spriteRenderer.color = new Color(1f, 1f, 1f, sortingOrder * sortingOffset);
            }
        }
    }
}

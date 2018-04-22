using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOverlay : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Highlight(Tile tile, Color color)
    {
        transform.position = tile.transform.position;
        spriteRenderer.color = color;
        spriteRenderer.enabled = true;
    }

    public void Disable()
    {
        if (spriteRenderer)
        {
            spriteRenderer.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileUpdater : MonoBehaviour
{
    void Update()
    {
        Tile tile = GetComponent<Tile>();
        if(tile)
        {
            tile.SetType(tile.type);
            tile.RecalculateEdges();
        }
    }
}

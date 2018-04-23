using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUpdater : MonoBehaviour
{
    [ContextMenu("UpdateAllTiles")]
    public void UpdateTiles()
    {
        Tile[] tiles = GetComponentsInChildren<Tile>();
        foreach (Tile t in tiles)
        {
            t.Settle();
            t.SetType(t.type, true);
        }
        foreach (Tile t in tiles)
        {
            t.RecalculateEdges();
        }
    }
}

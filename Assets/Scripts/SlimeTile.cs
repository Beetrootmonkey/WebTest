using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTile : MonoBehaviour
{
    Tile tile;

    void Awake()
    {
        tile = GetComponent<Tile>();
    }

    public void Spread()
    {
        if(tile && enabled)
        {
            List<Tile> n = tile.GetNeighbours();
            while(n.Count > 0)
            {
                int r = Random.Range(0, n.Count);
                Tile t = n[r];
                if (t.type != Tile.TileType.SLIME)
                {
                    t.SetType(Tile.TileType.SLIME);
                    return;
                } else
                {
                    n.RemoveAt(r);
                }
            }
            enabled = false;
            //tile.Remove();
        }
    }
}

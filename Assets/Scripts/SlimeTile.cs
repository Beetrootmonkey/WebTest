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
        if(tile)
        {
            Tile[] neighbours = tile.GetNeighbours();
            List<Tile> n = new List<Tile>(neighbours);
            do
            {
                int r = Random.Range(0, n.Capacity);
                Tile t = neighbours[r];
                if (t.type != Tile.TileType.SLIME)
                {
                    t.setType(Tile.TileType.SLIME);
                    break;
                } else
                {
                    n.Remove(t);
                }
            } while (n.Capacity > 0);
            

        }
    }
}

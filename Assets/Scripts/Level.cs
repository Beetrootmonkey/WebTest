using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public float slimeTiles = 0.001f;
    private List<Tile> tiles = new List<Tile>();

    // Use this for initialization
    void Awake()
    {
        Tile[] tiles = GetComponentsInChildren<Tile>();
        StartCoroutine(InitTiles(tiles));
    }

    private IEnumerator InitTiles(Tile[] tiles)
    {
        foreach (Tile t in tiles)
        {
            t.OnAwake();
        }
        foreach (Tile t in tiles)
        {
            t.OnStart();
        }
        SpawnSlime();
        yield break;
    }    

    private void SpawnSlime()
    {
        for(int i = 0; i < slimeTiles * tiles.Count; i++)
        {
            GetRandomTile().SetType(Tile.TileType.SLIME);
        }
    }

    private Tile GetRandomTile()
    {
        Tile tile = null;
        int count = tiles.Count;
        if(count > 0)
        {
            tile = tiles[Random.Range(0, count)];
        }
        
        return tile;
    }



}

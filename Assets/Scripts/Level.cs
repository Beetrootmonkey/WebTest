using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private Tile[,] tiles = null;
    private int arrWidth = 0;
    private int arrHeight = 0;
    private float width = 0;
    private float height = 0;
    // Use this for initialization
    void Awake()
    {
        OrderTiles(FindObjectsOfType<Tile>());
        SpawnSlime();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OrderTiles(Tile[] tArr)
    {
        if (tArr == null)
        {
            return;
        }

        tiles = new Tile[1000, 1000];

        foreach (Tile t in tArr)
        {
            settleTile(t);
            placeTile(t);
        }
    }

    //void Awake()
    //{
    //    tiles = new Tile[height * width];

    //    for (int y = 0, i = 0; y < height; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            CreateTile(x, y, i);
    //            settleTile(tiles[i]);
    //            placeTile(tiles[i]);
    //            i++;
    //        }
    //    }
    //}

    void settleTile(Tile tile)
    {
        float fx = tile.transform.position.x / 1.5f / HexMetrics.outerRadius;
        float fy = tile.transform.position.y / 2 / HexMetrics.innerRadius + fx / 2 - fx * 0.5f;
        int x = (int)fx;
        int y = (int)fy;
        arrWidth = Mathf.Max(x, arrWidth);
        arrHeight = Mathf.Max(y, arrHeight);
        width = Mathf.Max(tile.transform.position.x, width);
        height = Mathf.Max(tile.transform.position.y, height);

        tile.transform.position = new Vector2(x, y);
        tiles[x, y] = tile;
    }

    void placeTile(Tile tile)
    {
        int x = (int)(tile.transform.position.x);
        int y = (int)(tile.transform.position.y);

        Vector3 position;
        position.x = x * HexMetrics.outerRadius * 1.5f;
        position.y = (y + x * 0.5f - x / 2) * HexMetrics.innerRadius * 2;
        position.z = 0f;
        tile.transform.position = position;
    }

    private void SpawnSlime()
    {
        for(int i = 0; i < 0; i++)
        {
            Tile tile = GetRandomTile();
            if(tile)
            {
                tile.SetType(Tile.TileType.SLIME);
            }
        }
    }

    private Tile GetRandomTile()
    {
        Tile tile = null;
        int tries = 10;
        do
        {
            int x = Random.Range(0, arrWidth);
            int y = Random.Range(0, arrHeight);
            tile = GetTileAt(x, y);
            tries--;
        } while (tile == null && tries > 0);
        
        return tile;
    }

    private Tile GetTileAt(int x, int y)
    {
        try
        {
            return tiles[x, y];
        }
        catch (System.IndexOutOfRangeException)
        {
            return null;
        }
    }



}

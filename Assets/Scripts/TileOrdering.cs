using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOrdering : MonoBehaviour {
    Tile[] tiles = null;
    public int width = 6;
    public int height = 6;
    // Use this for initialization
    void Awake () {
        tiles = FindObjectsOfType<Tile>();
        
        OrderTiles();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OrderTiles()
    {
        if(tiles == null)
        {
            return;
        }

        foreach(Tile t in tiles)
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

        tile.transform.position = new Vector2(x, y);
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

    //void CreateTile(int x, int y, int i)
    //{
    //    Vector3 position;
    //    position.x = x * HexMetrics.outerRadius * 1.5f;
    //    position.y = (y + x * 0.5f - x / 2) * HexMetrics.innerRadius * 2;
    //    position.z = 0f;

    //    Tile cell = tiles[i] = Instantiate<Tile>(tilePrefab);
    //    cell.transform.SetParent(transform, false);
    //    cell.transform.localPosition = position;
    //}

}

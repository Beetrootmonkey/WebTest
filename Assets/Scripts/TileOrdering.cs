using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOrdering : MonoBehaviour {
    Tile[] tiles = null;
    public int width = 6;
    public int height = 6;

    public Tile tilePrefab;
    // Use this for initialization
    void Start () {
        //tiles = GetComponentsInChildren<Tile>();
        //OrderTiles();
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
            float x = t.transform.position.x;
            float y = t.transform.position.y;
            t.transform.position = new Vector2(x, y);
        }
    }

    void Awake()
    {
        tiles = new Tile[height * width];

        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateTile(x, y, i++);
            }
        }
    }

    void CreateTile(int x, int y, int i)
    {
        Vector3 position;
        position.x = x * HexMetrics.outerRadius * 1.5f;
        position.y = (y + x * 0.5f - x / 2) * HexMetrics.innerRadius * 2;
        position.z = 0f;

        Tile cell = tiles[i] = Instantiate<Tile>(tilePrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public TileType type = TileType.GRASS;
    private TileType oldType = TileType.GRASS;
    public TileEdge[] edges = new TileEdge[6];
    private static Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>();

    private static Player player;

    public enum TileType
    {
        GRASS,
        FOREST,
        VILLAGE,
        SLIME,
        WATER,
        MOUNTAIN,
        PATH,
        NONE
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!player)
        {
            player = FindObjectOfType<Player>();
        }

        if (type == TileType.SLIME)
        {
            SlimeTile slimeTile = GetComponent<SlimeTile>();
            if (!slimeTile)
            {
                gameObject.AddComponent<SlimeTile>();
            }
        }

        SetType(type, true);
    }

    // Use this for initialization
    void Start()
    {
        RecalculateEdges();
        RecalculateNeighbouringEdges();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RecalculateNeighbouringEdges()
    {
        List<Tile> neighbours = GetNeighbours();
        foreach (Tile t in neighbours)
        {
            t.RecalculateEdges();
        }
    }

    public void RecalculateEdges()
    {
        foreach (TileEdge e in edges)
        {
            SpriteRenderer renderer = e.gameObject.GetComponent<SpriteRenderer>();
            if (renderer)
            {
                renderer.enabled = true;
            }
        }
        List<Tile> neighbours = GetNeighbours();
        foreach (Tile t in neighbours)
        {
            if (t.type == type)
            {
                int index = CalculateIndexOfDirection(t);
                SpriteRenderer renderer = edges[index].gameObject.GetComponent<SpriteRenderer>();
                if (renderer)
                {
                    if (renderer.enabled)
                    {

                        renderer.enabled = false;
                    }
                    else
                    {

                        //Debug.Log("Fatal: Tried to disable edge that was already disabled! - " + index);
                    }
                }
                else
                {
                    Debug.Log("Fatal: Couldn't find renderer of edge!");
                }
            }
        }
    }

    public static Tile GetFocusedTile()
    {
        Tile t = null;

        Camera cam = FindObjectOfType<Camera>();
        if (cam && player.floor)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 offset = (mousePos - player.floor.transform.position).normalized * HexMetrics.innerRadius * 2;
            offset.z = 0;
            if (offset.magnitude == 0)
            {
                // Mouse is exactly over player pos
                return null;
            }
            Vector2 pos = player.floor.transform.position + offset;
            t = FindTile(pos);
        }

        return t;
    }

    public bool IsNeighbour(Tile tile)
    {
        List<Tile> neighbours = GetNeighbours();
        foreach (Tile t in neighbours)
        {
            if (t == tile)
            {
                return true;
            }
        }
        return false;
    }

    public List<Tile> GetNeighbours()
    {
        Collider2D ownCollider = GetComponent<Collider2D>();
        if (!ownCollider)
        {
            Debug.Log("Fatal: Collider of Tile missing!");
            return null;
        }

        List<Tile> neighbours = new List<Tile>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, HexMetrics.innerRadius * 2);
        foreach (Collider2D c in colliders)
        {
            if (c != ownCollider)
            {
                Tile tile = c.GetComponent<Tile>();
                if (tile)
                {
                    neighbours.Add(tile);
                }
            }
        }
        return neighbours;
    }

    public int GetTimeLost()
    {
        switch (type)
        {
            case TileType.FOREST: return 3;
            case TileType.GRASS: return 2;
            case TileType.MOUNTAIN: return 10;
            case TileType.SLIME: return 1;
            case TileType.PATH: return 1;
            case TileType.VILLAGE: return 4;
            case TileType.WATER: return 10;
            default: return 1;
        }
    }

    public static Tile FindTile(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, HexMetrics.innerRadius * 0.5f);
        Tile tile = null;
        foreach (Collider2D c in colliders)
        {
            Tile t = c.GetComponent<Tile>();
            if (t)
            {
                if (tile)
                {
                    float distance = Utils.CalculateDistance(t.transform.position, position);
                    if (distance < Utils.CalculateDistance(tile.transform.position, position))
                    {
                        tile = t;
                    }
                }
                else
                {
                    tile = t;
                }
            }
        }

        return tile;
    }

    public void SetType(TileType type, bool forceUpdate = false)
    {
        if (type == oldType && !forceUpdate)
        {
            return;
        }
        oldType = type;
        Sprite[] sprites = GetSprites(type);
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (!spriteRenderer)
        {
            Debug.Log("Fatal: Missing tile's SpriteRenderer!");
            return;
        }
        if (sprites.Length < 1)
        {
            Debug.Log("Fatal: Couldn't find Sprites!");
            return;
        }

        // Ist NICHT slime, aber wird slime
        if (type == TileType.SLIME && this.type != TileType.SLIME)
        {
            gameObject.AddComponent<SlimeTile>();
        }
        else if (type != TileType.SLIME && this.type == TileType.SLIME) // Ist slime, und verliert diesen state
        {
            SlimeTile slimeTile = GetComponent<SlimeTile>();
            if (slimeTile)
            {
                Destroy(slimeTile);
            }
        }
        this.type = type;
        spriteRenderer.sprite = sprites[0];
        for (int i = 1; i < sprites.Length; i++)
        {
            SpriteRenderer r = edges[i - 1].GetComponent<SpriteRenderer>();
            if (r && sprites[i])
            {
                r.sprite = sprites[i];
            }
        }
        RecalculateEdges();
        RecalculateNeighbouringEdges();
        return;
    }

    private static Sprite[] GetSprites(TileType type)
    {
        string defEdgePath = "Sprites/Terrain/EdgeDetection/edge_";
        string mainPath = "Sprites/Terrain/";
        string edgePath = mainPath + "";
        switch (type)
        {
            case TileType.FOREST:
                mainPath += "forestTile1";
                break;
            case TileType.GRASS:
                mainPath += "Grass/grassTile" +
                    (Random.Range(1, 20) == 1 ? "2"
                    : Random.Range(1, 5) == 1 ? "3" : "1");
                break;
            case TileType.MOUNTAIN:
                mainPath += "mountainTile1";
                break;
            case TileType.SLIME:
                mainPath += "Slime/slimeTile" + Random.Range(1, 4);
                edgePath += "Slime/edge_";
                break;
            case TileType.PATH:
                mainPath += "Path/pathTile" + Random.Range(1, 4);
                edgePath += "Path/edge_";
                break;
            case TileType.VILLAGE:
                mainPath += "villageTile1";
                break;
            case TileType.WATER:
                mainPath += "Water/waterTile1";
                edgePath += "Water/edge_";
                break;
            default:
                mainPath += "Template";
                break;
        }
        Sprite[] sprites = new Sprite[7];
        if (!spriteMap.TryGetValue(mainPath, out sprites[0]))
        {
            sprites[0] = Resources.Load<Sprite>(mainPath);
            if (sprites[0])
            {
                spriteMap.Add(mainPath, sprites[0]);
            }
        }
        //spriteMap.Clear();
        for (int i = 1; i < sprites.Length; i++)
        {
            string path = edgePath + (i - 1);
            //if (!spriteMap.TryGetValue(path, out sprites[i]))
            //{
            sprites[i] = Resources.Load<Sprite>(path);
            if (sprites[i])
            {
                //spriteMap.Add(path, sprites[i]);
            }
            else
            {
                path = defEdgePath + (i - 1);
                sprites[i] = Resources.Load<Sprite>(path);
                if (sprites[i])
                {
                    //spriteMap.Add(path, sprites[i]);
                }
            }
            //}
        }

        return sprites;
    }

    public int CalculateIndexOfDirection(Tile tile)
    {
        return Utils.CalculateIndexOfDirection(transform.position, tile.transform.position);
    }

    public void Remove()
    {
        type = TileType.NONE;
        RecalculateNeighbouringEdges();
        Destroy(gameObject);
    }
}

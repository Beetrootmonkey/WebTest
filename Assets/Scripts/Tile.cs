using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public TileType type = TileType.GRASS;
    private static Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>();

    private static Player player;

    public enum TileType
    {
        GRASS,
        FOREST,
        STREET,
        VILLAGE,
        SLIME,
        WATER,
        MOUNTAIN
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
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Tile GetFocusedTile()
    {
        Tile t = null;

        Camera cam = FindObjectOfType<Camera>();
        if (cam)
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
            case TileType.STREET: return 1;
            case TileType.VILLAGE: return 4;
            case TileType.WATER: return 10;
            default: return 1;
        }
    }

    public static Tile FindTile(Vector2 position)
    {
        Collider2D collider = Physics2D.OverlapCircle(position, 0.01f);
        if (!collider)
        {
            return null;
        }
        return collider.GetComponent<Tile>();
    }

    public void SetType(TileType type)
    {
        Sprite sprite = GetSprite(type);
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (!spriteRenderer)
        {
            Debug.Log("Fatal: Missing tile's SpriteRenderer!");
            return;
        }
        if (!sprite)
        {
            Debug.Log("Fatal: Couldn't find Sprite!");
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
        spriteRenderer.sprite = sprite;
        return;
    }

    private static Sprite GetSprite(TileType type)
    {
        string path = "Sprites/Terrain/";
        switch (type)
        {
            case TileType.FOREST:
                path += "forestTile1";
                break;
            case TileType.GRASS:
                path += "grassTile1";
                break;
            case TileType.MOUNTAIN:
                path += "Template";
                break;
            case TileType.SLIME:
                path += "slimeTile1";
                break;
            case TileType.STREET:
                path += "streetTile1";
                break;
            case TileType.VILLAGE:
                path += "villageTile1";
                break;
            case TileType.WATER:
                path += "Template";
                break;
            default:
                path += "Template";
                break;
        }
        Sprite sprite = null;
        if (!spriteMap.TryGetValue(path, out sprite))
        {
            sprite = Resources.Load<Sprite>(path);
        }
        return sprite;
    }
}

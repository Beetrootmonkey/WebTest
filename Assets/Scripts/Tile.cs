using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    private TileOverlay mouseOverlay;
    private SpriteRenderer mouseOverlaySpriteRenderer;
    private static Tile playerGround;
    public TileType type = TileType.GRASS;

    private Player player;

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
        mouseOverlay = FindObjectOfType<TileOverlay>();
        if (mouseOverlay)
        {
            mouseOverlaySpriteRenderer = mouseOverlay.gameObject.GetComponent<SpriteRenderer>();
        }

        player = FindObjectOfType<Player>();
        playerGround = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        if (mouseOverlaySpriteRenderer && IsReachable() == Reachable.IN_RANGE)
        {
            mouseOverlay.transform.position = transform.position;
            mouseOverlaySpriteRenderer.enabled = true;
        }
    }

    void OnMouseExit()
    {
        if (mouseOverlaySpriteRenderer && IsReachable() == Reachable.IN_RANGE)
        {
            mouseOverlaySpriteRenderer.enabled = false;
        }
    }

    void OnMouseDown()
    {
        if(IsReachable() == Reachable.IN_RANGE)
        {
            if(player.SpendTime(GetTimeLost()))
            {
                MovePlayerHere();
            }
        }
    }

    enum Reachable
    {
        NOT_IN_RANGE,
        IN_RANGE,
        STANDING_ON
    }

    Reachable IsReachable()
    {
        if(!playerGround)
        {
            Debug.Log("Player position not registered!");
            return Reachable.NOT_IN_RANGE;
        }
        Collider2D ownCollider = GetComponent<Collider2D>();
        if (!ownCollider)
        {
            Debug.Log("Fatal: Collider of Tile under mouse missing!");
            return Reachable.NOT_IN_RANGE;
        }
        Collider2D playerCollider = playerGround.GetComponent<Collider2D>();
        if (!playerCollider)
        {
            Debug.Log("Fatal: Collider of Tile under player missing!");
            return Reachable.NOT_IN_RANGE;
        }

        if(ownCollider == playerCollider)
        {
            return Reachable.STANDING_ON;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerGround.transform.position, HexMetrics.innerRadius * 2);
        foreach (Collider2D c in colliders)
        {
            if (c == ownCollider)
            {
                return Reachable.IN_RANGE;
            }
        }

        return Reachable.NOT_IN_RANGE;
    }
    
    public static void SetPlayerGround(Tile tile)
    {
        playerGround = tile;
    }

    public int GetTimeLost()
    {
        switch(type)
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

    public void MovePlayerHere()
    {
        player.transform.position = transform.position;
        playerGround = this;
        mouseOverlaySpriteRenderer.enabled = false;
    }
}

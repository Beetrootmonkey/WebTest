using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private TileOverlay focusedTileOverlay;
    private SpriteRenderer spriteRenderer;
    private MenuController menu;
    public Tile floor;
    private const int timeMax = 8;
    private int timeLeft = timeMax;
    private bool slimeAIRunning;
    public Sprite[] sprites = new Sprite[6];

    void Awake()
    {
        focusedTileOverlay = FindObjectOfType<TileOverlay>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        menu = FindObjectOfType<MenuController>();
    }

    // Use this for initialization
    void Start () {
        // Register the tile at the players floor
        ForceMove(transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        if(menu.IsActive()) {
            return;
        }

        if (focusedTileOverlay)
        {
            Tile t = Tile.GetFocusedTile();
            if (t && !UIHover.IsMouseOverUI())
            {
                Face(t);
                focusedTileOverlay.Highlight(t);
            }
            else
            {
                focusedTileOverlay.Disable();
            }
        }

        if(Input.GetMouseButtonDown(0) && !UIHover.IsMouseOverUI())
        {
            Tile t = Tile.GetFocusedTile();

            if (t)
            {
                TryMove(t);
            }
        }
    }

    private void Face(Tile tile)
    {
        Vector2 tilePos = tile.transform.position;
        Vector2 pos = transform.position;
        Vector2 offset = (tilePos - pos);
        float angle = Vector2.SignedAngle(Vector2.right, offset);
        angle += 360;
        angle += 90;
        angle %= 360;
        angle /= 60;

        int dir = 6 - (int)angle;
        dir %= 6;
        Debug.Log(dir);
        spriteRenderer.sprite = sprites[dir];

    }

    public int GetTimeLeft()
    {
        return timeLeft;
    }

    public bool SpendTime(int amount)
    {
        if(timeLeft > amount)
        {
            timeLeft -= amount;
            return true;
        } else if(timeLeft == amount)
        {
            timeLeft = 0;
            DepleteTime();
            return true;
        }
        return false;
    }

    public void DepleteTime()
    {
        StartCoroutine(RunSlimeAI());
    }

    private IEnumerator RunSlimeAI()
    {
        if (!slimeAIRunning)
        {
            slimeAIRunning = true;
            timeLeft = 0;
            StartCoroutine(SlimeSpreader.SpreadSlime());
            yield return new WaitUntil(() => !SlimeSpreader.slimeSpreading);
            timeLeft = timeMax;
            slimeAIRunning = false;
        }
        yield break;
    }

    public void ForceMove(Tile tile)
    {
        if(tile)
        {
            transform.position = tile.transform.position;
            floor = tile;
        }
    }

    public void ForceMove(Vector2 position)
    {
        Tile t = Tile.FindTile(position);
        if(t)
        {
            ForceMove(t);
        }
    }

    public void TryMove(Tile tile)
    {
        if (tile)
        {
            if (SpendTime(tile.GetTimeLost()))
            {
                ForceMove(tile);
            }
        }
    }

    public void TryMove(Vector2 position)
    {
        Tile t = Tile.FindTile(position);
        if (t)
        {
            TryMove(t);
        }
    }



}

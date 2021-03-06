﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private TileOverlay focusedTileOverlay;
    private SpriteRenderer spriteRenderer;
    private MenuController menu;
    private AudioSource source;
    public Tile floor;
    private bool moving;
    public float speed;
    private int maxFootSteps = 1;
    private int footStepCounter = 1;
    private const int timeMax = 8;
    private int timeLeft = timeMax;
    private bool slimeAIRunning;
    public Sprite[] sprites = new Sprite[6];
    public bool dead = false;

    void Awake()
    {
        focusedTileOverlay = FindObjectOfType<TileOverlay>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        menu = FindObjectOfType<MenuController>();
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        // Register the tile at the players floor
        Teleport(transform.position);
        floor.type = Tile.TileType.GRASS;
    }
	
	// Update is called once per frame
	void Update () {
        if(floor.type == Tile.TileType.SLIME)
        {
            Kill();
        }
        else if(floor.type == Tile.TileType.VILLAGE)
        {
            Win();
        }
        if(menu && menu.IsActive() || dead) {
            return;
        }

        if (focusedTileOverlay)
        {
            Tile t = Tile.GetFocusedTile();
            if (t && !UIHover.IsMouseOverUI() && !moving && timeLeft > 0)
            {
                Face(t);
                Color color = TestSpendTime(t.GetTimeLost()) ? Color.white : Color.red;
                focusedTileOverlay.Highlight(t, color);
            }
            else
            {
                focusedTileOverlay.Disable();
            }
        }

        if(Input.GetMouseButtonDown(0) && !UIHover.IsMouseOverUI() && !moving)
        {
            Tile t = Tile.GetFocusedTile();

            if (t)
            {
                TryMove(t);
            }
        }

        if(moving)
        {
            float dist = (floor.transform.position - transform.position).magnitude;
            if(dist > 0.05)
            {
                if (source && !source.isPlaying && footStepCounter > 0)
                {
                    source.Play();
                    footStepCounter--;
                }
                transform.position = Vector2.Lerp(transform.position, floor.transform.position, speed);
                
            } else
            {
                footStepCounter = maxFootSteps;
                moving = false;
                transform.position = floor.transform.position;
            }
            
        }

    }

    private void Face(Tile tile)
    {
        int index = Utils.CalculateIndexOfDirection(transform.position, tile.transform.position);
        spriteRenderer.sprite = sprites[index];
    }

    public int GetTimeLeft()
    {
        return timeLeft;
    }

    public bool SpendTime(int amount)
    {
        bool subtracted = false;
        if(TestSpendTime(amount))
        {
            timeLeft -= amount;
            subtracted = true;
        }

        if(timeLeft <= 0)
        {
            DepleteTime();
        }

        return subtracted;
    }

    public bool TestSpendTime(int amount)
    {
        if (timeLeft >= amount)
        {
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
            moving = true;
            //transform.position = tile.transform.position;
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

    public void Teleport(Tile tile)
    {
        if (tile)
        {
            transform.position = tile.transform.position;
            floor = tile;
        }
    }

    public void Teleport(Vector2 position)
    {
        Tile t = Tile.FindTile(position);
        if (t)
        {
            Teleport(t);
        }
    }

    public void Win()
    {
        StartCoroutine(ActuallyWin());
    }

    public void Kill()
    {
        StartCoroutine(ActuallyKill());
    }

    private IEnumerator ActuallyKill()
    {
        if(!dead)
        {
            dead = true;
            DeathManager d = FindObjectOfType<DeathManager>();
            if (d)
            {
                d.FadeIn(2);
            }
            yield return new WaitForSeconds(4);
            SceneLoader loader = FindObjectOfType<SceneLoader>();
            if(loader)
            {
                loader.Load("menu");
            }
        }
        yield break;
    }

    private IEnumerator ActuallyWin()
    {
        if (!dead)
        {
            dead = true;
            WinManager w = FindObjectOfType<WinManager>();
            if (w)
            {
                w.FadeIn(2);
            }
            yield return new WaitForSeconds(4);
            SceneLoader loader = FindObjectOfType<SceneLoader>();
            if (loader)
            {
                loader.Load("menu");
            }
        }
        yield break;
    }



}

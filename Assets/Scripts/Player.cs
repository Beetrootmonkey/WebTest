using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private const int timeMax = 8;
    private int timeLeft = timeMax;
    // Use this for initialization
    void Start () {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, HexMetrics.innerRadius * 0.5f);
        Tile tile = collider.GetComponent<Tile>();
        if (tile)
        {
            Tile.SetPlayerGround(tile);
            transform.position = tile.transform.position;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
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
            DepleteTime();
        }
        return false;
    }

    public void DepleteTime()
    {
        timeLeft = 0;
        SlimeTile[] slime = FindObjectsOfType<SlimeTile>();
        foreach(SlimeTile s in slime)
        {
            s.Spread();
        }
        timeLeft = timeMax;
    }
}

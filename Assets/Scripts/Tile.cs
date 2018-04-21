using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    private TileOverlay mouseOverlay;
    private SpriteRenderer mouseOverlaySpriteRenderer;

    private Player player;

    void Awake()
    {
        mouseOverlay = FindObjectOfType<TileOverlay>();
        if (mouseOverlay)
        {
            mouseOverlaySpriteRenderer = mouseOverlay.gameObject.GetComponent<SpriteRenderer>();
        }

        player = FindObjectOfType<Player>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        if (mouseOverlaySpriteRenderer)
        {
            mouseOverlay.transform.position = transform.position;
            mouseOverlaySpriteRenderer.enabled = true;
        }
    }

    void OnMouseExit()
    {
        if (mouseOverlaySpriteRenderer)
        {
            mouseOverlaySpriteRenderer.enabled = false;
        }
    }

    void OnMouseDown()
    {
        if(player)
        {
            player.transform.position = transform.position;
        }
    }
}

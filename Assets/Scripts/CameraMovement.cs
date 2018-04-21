using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private Player player;
    public float speed = 0.3f;

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    void Update () {
        if(player)
        {
            Vector3 pos = player.transform.position;
            pos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, pos, speed);
        }
	}
}

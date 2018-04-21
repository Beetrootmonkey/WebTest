using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public Transform[] watches = new Transform[8];
    private Image[] images = new Image[8];
    private Player player;
    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Player>();
        for (int i = 0; i < watches.Length; i++)
        {
            images[i] = watches[i].GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < watches.Length; i++)
        {
            if (images[i])
            {
                if (i < player.GetTimeLeft())
                {
                    images[i].enabled = true;
                }
                else
                {
                    images[i].enabled = false;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{

    private Image[] images;
    private float speed = 0f;
    private AudioSource source;

    void Awake()
    {
        images = GetComponentsInChildren<Image>();
        source = GetComponent<AudioSource>();
        SetActive(false);
    }

    void Update()
    {
        foreach (Image img in images)
        {
            if (img.enabled)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + speed * Time.deltaTime);
            }
        }
    }

    private void SetActive(bool value)
    {
        foreach (Image img in images)
        {
            if (value)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
            }

            img.enabled = value;
        }

    }

    public void FadeIn(float seconds)
    {
        source.Play();
        SetActive(true);
        speed = 1 / seconds;
    }
}

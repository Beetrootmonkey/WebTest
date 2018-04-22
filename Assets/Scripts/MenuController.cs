using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private bool active = false;
    // Use this for initialization
    void Start()
    {
        SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleActive();
        }
    }

    public void SetActive(bool value)
    {
        active = value;
        Image[] imgs = GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            img.enabled = value;
        }
    }

    public void ToggleActive()
    {
        SetActive(!active);
    }

    public bool IsActive()
    {
        return active;
    }
}

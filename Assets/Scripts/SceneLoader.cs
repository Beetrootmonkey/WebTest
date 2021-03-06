﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public void Load(string scene)
    {
        StartCoroutine(LoadYourAsyncScene(scene));
    }

    private IEnumerator LoadYourAsyncScene(string scene)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        MenuController menu = FindObjectOfType<MenuController>();
        if(menu)
        {
            menu.SetActive(false);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/" + scene);

        yield return new WaitUntil(() => asyncLoad.isDone);
        yield break;
    }

    public void Exit()
    {
        Application.Quit();
    }
}

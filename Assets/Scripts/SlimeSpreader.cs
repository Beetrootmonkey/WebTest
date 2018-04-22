using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpreader : MonoBehaviour{

    public static float chance = 0.5f;
    public static float delay = 0.5f;
    public static bool slimeSpreading;
    public static IEnumerator SpreadSlime()
    {
        if(!slimeSpreading)
        {
            slimeSpreading = true;
            yield return new WaitForSeconds(0.1f);
            SlimeTile[] slime = FindObjectsOfType<SlimeTile>();
            List<SlimeTile> tiles = new List<SlimeTile>(slime);
            tiles = tiles.FindAll(t => t.enabled && Random.Range(0f, 0.99999999f) < chance);
            int counter = 0;
            foreach (SlimeTile t in tiles)
            {
                t.Spread();
                counter++;
                if(counter >= tiles.Count * 0.1f)
                {
                    counter = 0;
                    yield return new WaitForSeconds(delay * 0.1f);
                }
            }
            yield return new WaitForSeconds(0.1f);
            slimeSpreading = false;
        }
        yield break;
    }
}

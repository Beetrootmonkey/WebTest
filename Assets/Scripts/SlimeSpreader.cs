using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpreader : MonoBehaviour{

    public static float chance = 0.5f;
    public static bool slimeSpreading;
    public static IEnumerator SpreadSlime()
    {
        if(!slimeSpreading)
        {
            slimeSpreading = true;
            yield return new WaitForSeconds(0.1f);
            SlimeTile[] slime = FindObjectsOfType<SlimeTile>();
            foreach (SlimeTile s in slime)
            {
                if (s.enabled && Random.Range(0f, 0.99999999f) < chance)
                {
                    s.Spread();
                    yield return new WaitForSeconds(0.01f);
                }
            }
            slimeSpreading = false;
        }
        yield break;
    }
}

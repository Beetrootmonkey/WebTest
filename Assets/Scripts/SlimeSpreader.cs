using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpreader : MonoBehaviour
{

    public static float chance = 0.5f;
    public static float delay = 0.5f;
    public static float parts = 10;
    public static bool slimeSpreading;
    public static IEnumerator SpreadSlime()
    {
        if (!slimeSpreading)
        {
            slimeSpreading = true;
            //yield return new WaitForSeconds(0.1f);

            AudioClip clip = Resources.Load<AudioClip>("Audio/Sounds/Footstep3");
            if (!clip)
            {
                Debug.Log("Couldn't find Audio Clip!");
            }

            SlimeTile[] slime = FindObjectsOfType<SlimeTile>();
            List<SlimeTile> tiles = new List<SlimeTile>(slime);
            tiles = tiles.FindAll(t => t.enabled && Random.Range(0f, 0.99999999f) < chance);
            int counter = 0;
            foreach (SlimeTile t in tiles)
            {
                t.Spread();
                counter++;
                if (counter >= tiles.Count / parts)
                {
                    counter = 0;
                    
                    AudioSource source = t.gameObject.AddComponent<AudioSource>();
                    if (source)
                    {
                        source.pitch = Random.Range(0.8f, 1.25f);
                        source.volume = 0.5f;
                        source.PlayOneShot(clip, 1f);
                        Destroy(source, 1);
                    }
                    yield return new WaitForSeconds(delay / parts);
                }
            }
            yield return new WaitForSeconds(0.1f);
            slimeSpreading = false;
        }
        yield break;
    }
}

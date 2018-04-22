using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    public static int CalculateIndexOfDirection(Vector2 position1, Vector2 position2)
    {
        Vector2 offset = (position2 - position1);
        float angle = Vector2.SignedAngle(Vector2.right, offset);
        angle += 360;
        angle += 90;
        angle %= 360;
        angle = Mathf.Round(angle);
        angle /= 60;

        int dir = 6 - (int)angle;
        dir %= 6;
        return dir;
    }

    public static float CalculateDistance(Vector2 position1, Vector2 position2)
    {
        return (position2 - position1).magnitude;
    }
}

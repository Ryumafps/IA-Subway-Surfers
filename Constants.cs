using System.Collections.Generic;

public static class Constants
{
    public static readonly float playerPosX = -5.5f;
    public static readonly float enemyStartX = 10f;
    public static Dictionary<int, float> posYForPosition = new Dictionary<int, float>
    {
        { 1,
        -1.71f
    },
        { 2, -0.36f},
        { 3 , 0.89f}
    };
}
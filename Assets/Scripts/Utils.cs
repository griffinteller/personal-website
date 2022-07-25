using UnityEngine;

public static class Utils
{
    public static Color ColorFromHex(uint hex)
    {
        return new Color(
            ((hex >> 24) & 0xFF) / (float) 0xFF, 
            ((hex >> 16) & 0xFF) / (float) 0xFF, 
            ((hex >> 8) & 0xFF) / (float) 0xFF, 
            (hex & 0xFF) / (float) 0xFF);
    }
}
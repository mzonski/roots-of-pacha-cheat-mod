using UnityEngine;

namespace CheatMod.Core.UI;

public static class CheatUIStyles
{
    public static readonly GUIStyle Label = new(GUI.skin.label)
    {
        fontSize = 16,
        fontStyle = FontStyle.Bold,
        alignment = TextAnchor.MiddleLeft,
        padding = new RectOffset(16, 0, 0, 0),
        normal = { textColor = Color.white }
    };

    public static readonly GUIStyle Toggle = new(GUI.skin.toggle)
    {
        fontSize = 16,
        fontStyle = FontStyle.Bold,
        stretchWidth = true
    };

    public static readonly GUIStyle ListItemButton = new(GUI.skin.button)
    {
        normal =
        {
            textColor = Color.white,
            background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 1f))
        },
        fontSize = 16,
        padding = new RectOffset(16, 16, 8, 8),
        margin = new RectOffset(0, 0, 8, 8),
        active =
        {
            background = MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 1f))
        },
        hover =
        {
            background = MakeTex(2, 2, new Color(0.4f, 0.4f, 0.4f, 1f))
        },
        focused =
        {
            background = MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 1f))
        }
    };

    private static Texture2D MakeTex(int width, int height, Color color)
    {
        var pixels = new Color[width * height];

        for (var i = 0; i < pixels.Length; i++) pixels[i] = color;

        var result = new Texture2D(width, height);
        result.SetPixels(pixels);
        result.Apply();

        return result;
    }
}
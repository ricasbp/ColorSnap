using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalColors
{
    public static float[] red = { 0.986f, 0.027f, 0.60f, 0.5f };

    public static float[] green = { 0.25f, 0.375f, 0.60f, 0.5f };

    public static float[] blue = { 0.613f, 0.68f, 0.60f, 0.5f };

    public static float[] cyan = { 0.472f, 0.555f, 0.60f, 0.5f };

    public static float[] magenta = { 0.805f, 0.875f, 0.55f, 0.5f };

    public static float[] yellow = { 0.120f, 0.180f, 0.60f, 0.5f };

    public static float[] purple = { 0.715f, 0.777f, 0.55f, 0.5f };

    public static float[] orange = { 0.055f, 0.111f, 0.65f, 0.5f };

    public static float[][] colorsValue = { red, green, blue, cyan, magenta, yellow, purple, orange };
    public static string[] colorsName = { "red", "green", "blue", "cyan", "magenta", "yellow","purple", "orange" };

    public static float minSat = 0.23f;

    public static float minVal = 0.1f;

    public static float maxVal = 0.75f;

    public static Color trueRed = new Color(1f,0,0);

    public static Color trueGreen = new Color(0, 1f, 0);

    public static Color trueBlue = new Color(0, 0, 1f);

    public static Color trueCyan = new Color(0, 1f, 1f);

    public static Color trueMagenta = new Color(1f, 0, 1f);

    public static Color trueYellow = new Color(1f, 1f, 0);

    public static Color truePurple = new Color(0.5f, 0, 1f);

    public static Color trueOrange = new Color(1f, 0.5f, 0);

    public static Color[] trueColors = { trueRed, trueGreen, trueBlue, trueCyan, trueMagenta, trueYellow, truePurple , trueOrange};

    public static Color findTrueColor(Color c)
    {
        float h, s, v;
        Color.RGBToHSV(c, out h, out s, out v);

        for (int i = 0; i < colorsValue.Length; i++)
        {
            if (colorsValue[i][1] < colorsValue[i][0])
            {
                if (((h > colorsValue[i][0] && h < 1f) || (h > 0f && h < colorsValue[i][1])) && s > colorsValue[i][2] && v > colorsValue[i][3])
                {
                    return trueColors[i];
                }
            }
            else
            {
                if (h > colorsValue[i][0] && h < colorsValue[i][1] && s > colorsValue[i][2] && v > colorsValue[i][3])
                {
                    return trueColors[i];
                }
            }
        }
        return Color.black;
    }

    public static int findTrueColori(Color c)
    {
        float h, s, v;
        Color.RGBToHSV(c, out h, out s, out v);

        for (int i = 0; i < colorsValue.Length; i++)
        {
            if (colorsValue[i][1] < colorsValue[i][0])
            {
                if (((h > colorsValue[i][0] && h <= 1f) || (h >= 0f && h < colorsValue[i][1])) && s > colorsValue[i][2] && v > colorsValue[i][3])
                {
                    return i;
                }
            }
            else
            {
                if (h > colorsValue[i][0] && h < colorsValue[i][1] && s > colorsValue[i][2] && v > colorsValue[i][3])
                {
                    return i;
                }
            }
        }
        return -1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HSVMixer : MonoBehaviour
{

    public GameObject[] colors = new GameObject[2];


    public GameObject mixButton;

    public GameObject mixedColor;

    private int inputedColors;

    private Color[] color = new Color[2];

    // Start is called before the first frame update
    void Start()
    {
        inputedColors = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mixedColor == null)
        {
            Debug.Log("gone");
        }
    }

    public void mixColor()
    {
        float h1, s1, v1, h2, s2, v2;
        Color.RGBToHSV(color[0], out h1, out s1, out v1);
        Color.RGBToHSV(color[1], out h2, out s2, out v2);
        Color mixed;

        Color[] cs = { color[0], color[1] };
        float[] ss = { s1, s2 };
        float[] hs = { h1, h2 };
        float[] vs = { v1, v2 };

        for(int i = 0; i < cs.Length; i++)
        {
            if(cs[i] == Color.black)
            {
                hs[i] = hs[cs.Length - 1 - i];
                ss[i] = 1f;
                vs[i] = vs[cs.Length - 1 - i];
            }
            else if (cs[i] == Color.white)
            {
                hs[i] = hs[cs.Length - 1 - i];
                ss[i] = ss[cs.Length - 1 - i];
                vs[i] = 1f;
            }
            else if (ss[i] < GlobalColors.minSat)
            {
                hs[i] = hs[cs.Length - 1 - i];
            }

        }

        float hueDifference = Mathf.Abs(hs[0] - hs[1]);

        float middleHue;
        if (1f - hueDifference < hueDifference) {
            Debug.Log(hueDifference);
            Debug.Log("correct");
            middleHue = (hs[0] > hs[1]) ? (hs[0] + (1f- hueDifference) / 2f) % 1f : (hs[1] + (1f - hueDifference)/ 2f) % 1f;
        }
        else
        {
            middleHue = (hs[0] < hs[1]) ? hs[0] + hueDifference / 2f : hs[1] + hueDifference / 2f;
        }

        mixed = Color.HSVToRGB(middleHue, (ss[0] + ss[1]) / 2f, (vs[0] + vs[1]) / 2f);
        
        
        
        mixedColor.SetActive(true);
        mixedColor.GetComponentInChildren<Image>().color = mixed;
        mixButton.SetActive(false);
        foreach (GameObject c in colors)
        {
            
            c.SetActive(false);
        }
        inputedColors = 0;
    }

    public void inputColor(Color inputC)
    {
        
        if (inputedColors < 2)
        {
            GameObject curColor = colors[inputedColors];
            curColor.SetActive(true);
            curColor.GetComponentInChildren<Image>().color = inputC;
            color[inputedColors] = inputC;
            inputedColors += 1;
            if (inputedColors == 2 && !mixButton.activeSelf)
            {
                mixButton.SetActive(true);
                mixedColor.SetActive(false);
            }
        }
    }

    
}

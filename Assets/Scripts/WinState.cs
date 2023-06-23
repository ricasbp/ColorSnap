using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinState : MonoBehaviour
{
    GameObject panel;
    TextMeshProUGUI text;

    GameObject button;

    GameObject parent;
    WinCheck pCheck;

    // Start is called before the first frame update
    void Start()
    {
        panel = transform.GetChild(0).gameObject;
        text = panel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        button = transform.GetChild(1).gameObject;
        parent = transform.parent.gameObject;
        pCheck = parent.GetComponent<WinCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCheck()
    {
        button.SetActive(false);
        panel.SetActive(true);
        if (pCheck.CheckWin()) {
            
            text.text = "Correct!";
        }
        else
        {
            text.text = "Wrong!";
        }
        StartCoroutine(ShowForSeconds());
    }

    IEnumerator ShowForSeconds()
    {
        yield return new WaitForSeconds(2f);

        button.SetActive(true);
        panel.SetActive(false);
    }

}

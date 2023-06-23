using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowHide2 : MonoBehaviour
{
    public GameObject instructions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHideInst()
    {
        TextMeshProUGUI text = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        if (instructions.activeSelf)
        {
            instructions.SetActive(false);
            text.text = "Show HSV";
        }
        else
        {
            instructions.SetActive(true);
            text.text = "Hide HSV";
        }
        
    }

    
}

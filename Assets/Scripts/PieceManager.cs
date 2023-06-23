using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{

    [SerializeField]
    public GameObject[] inPieces;

    [SerializeField]
    public GameObject[] outPieces;

    public static GameObject[] sInPieces;

    public static GameObject[] sOutPieces;


    private void Awake()
    {
        sInPieces = inPieces;
        sOutPieces = outPieces;
    }
    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

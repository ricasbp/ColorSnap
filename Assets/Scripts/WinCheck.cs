using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class WinCheck : MonoBehaviour
{
    [SerializeField]
    GameObject[] inPiecesSlots;

    [SerializeField]
    GameObject[] outPiecesSlots;

    SelectInPiece[] inPiecesScript = new SelectInPiece[4];

    SelectOutPiece[] outPiecesScript = new SelectOutPiece[4];

    int[] inPiecesPos = new int[4];
    int[] outPiecesPos = new int[4];

    Transform centerPiece;

    int[] angles = { 0, 90, 180, 270, 360};


    // Start is called before the first frame update
    void Start()
    {
        centerPiece = transform.GetChild(0);
        for (int i = 0; i < 4; i++)
        {
            inPiecesScript[i] = inPiecesSlots[i].GetComponent<SelectInPiece>();
            outPiecesScript[i] = outPiecesSlots[i].GetComponent<SelectOutPiece>();
            if (inPiecesScript[i] == null)
            {
                Debug.Log("inPieces " + i);
            }
            else if (outPiecesScript[i] == null)
            {
                Debug.Log("outPieces " + i);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckWin()
    {
        
        
        for (int i = 0; i < 4; i++)
        {
            inPiecesPos[i] = inPiecesScript[i].piecePos;
            outPiecesPos[i] = outPiecesScript[i].piecePos;
        }
        Debug.Log("Check");

        bool areEqual = inPiecesPos.OrderBy(x => x).SequenceEqual(outPiecesPos.OrderBy(x => x));
        if (!areEqual)
        {
            return false;
        }
        Debug.Log("Contains");
        

        int separation = -1;
        bool present = true;
        for(int i = 0; i < inPiecesPos.Length; i++)
        {
            if (inPiecesPos[i] == outPiecesPos[0])
            {
                present = true;
                for (int j = 0; j < outPiecesPos.Length; j++)
                {
                    if (outPiecesPos[j] != inPiecesPos[(i + j) % 4])
                    {
                        present = false;
                        break;

                    }
                }
                if (present)
                {
                    separation = i;
                    break;
                }
            }
            
        }
        if (separation == -1)
        {
            return false;
        }
        Debug.Log("inOrder");

        
        float cRotation = centerPiece.localEulerAngles.y;

        float[] subtractedRotations = angles.Select(x => Mathf.Abs(x - cRotation)).ToArray();
        
        float minimumValue = subtractedRotations.Min();
        Debug.Log(minimumValue);
        int correctedAngle = angles[Array.IndexOf(subtractedRotations, minimumValue)% 4];
        Debug.Log(correctedAngle / 90);
        Debug.Log(separation);
        if (correctedAngle / 90 != (4 - separation) % 4)
        {
            return false;
        }

        Debug.Log("Correct");
        return true;

    }
}

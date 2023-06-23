using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectInPiece : MonoBehaviour
{

    Camera arCamera;

    GameObject[] inPieces;

    GameObject piece;

    Material material;
    Renderer mrenderer;

    public int piecePos;

    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;
        inPieces = PieceManager.sInPieces;
        piecePos = -1;
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    public void selectPiece(Color c)
    {
        if (piece != null)
        {
            Destroy(piece);
            piece = null;
        }
        
        int tColor = GlobalColors.findTrueColori(c);
        
        
        if (tColor != -1)
        {
            piecePos = tColor;
            piece = Instantiate(inPieces[tColor], transform.position, transform.rotation,transform);
            material = piece.GetComponent<Renderer>().material;
            material.color = c;


            int correction = tColor / 2;
            piece.transform.localRotation = Quaternion.Euler (0f, 0f, 90f * correction);

        }
    }
}

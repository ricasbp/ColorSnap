using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOutPiece : MonoBehaviour
{
    Camera arCamera;

    GameObject[] outPieces;

    GameObject piece;

    Material material;

    Renderer mrenderer;

    public int piecePos;

    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;
        outPieces = PieceManager.sOutPieces;

        List<int> numbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> selectedNumbers = new List<int>();
        int randomNumber;

        
        int randomIndex = Random.Range(0, numbers.Count);
        randomNumber = numbers[randomIndex];

        
        piecePos = randomNumber;

        piece = Instantiate(outPieces[randomNumber], transform.position, transform.rotation, transform);
        material = piece.GetComponent<Renderer>().material;
        material.color = GlobalColors.trueColors[randomNumber];


        int correction = randomNumber / 2;
        piece.transform.localRotation = Quaternion.Euler(0f, 0f, 90f * correction);
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
            piece = Instantiate(outPieces[tColor], transform.position, transform.rotation, transform);
            material = piece.GetComponent<Renderer>().material;
            material.color = c;


            int correction = tColor / 2;
            piece.transform.localRotation = Quaternion.Euler(0f, 0f, 90f * correction);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    Camera arCamera;

    private float inputRotation = 0f;

    [SerializeField]
    private float rotationspeed;

    private Vector2 posInit;

    private Vector2 posFinal;

    private bool posUpdate = false;

    private bool isOnScreen = false;

    private bool draggedStarted = false;


    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;

        posInit = new Vector2(0f, 0f);
        posFinal = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        

        //Debug.Log("Touches " + Input.touchCount);
        if (Input.touchCount == 0)
            //inputRotation = inputRotation * 0.90f;
            return;

        Touch touch = Input.GetTouch(0);

        int layerMask = LayerMask.GetMask("MainPiece");

        Ray ray = arCamera.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,10f,layerMask))
        {
            if (touch.phase == TouchPhase.Began)
            {
                posUpdate = false;

                Debug.Log(transform.localRotation.eulerAngles);
            

                updatePos(touch.position);

                draggedStarted = true;
            

            }
            else if(draggedStarted)
            {
                updatePos(touch.position);

            }
            
        }
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            
            draggedStarted = false;
        }

        inputRotation = (posFinal - posInit).x * rotationspeed;
        if (inputRotation != 0)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, -inputRotation);
            transform.rotation *= rotation;
            posInit = posFinal;
            posUpdate = false;
        }
    }
    private void FixedUpdate()
    {
        
        
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;
    }

    void updatePos(Vector2 touchpos)
    {
        if (!posUpdate)
        {
            posInit = touchpos;
            posFinal = touchpos;
            posUpdate = true;
        }
        else
        {
            posFinal = touchpos;
            posUpdate = true;
        }
    }
}

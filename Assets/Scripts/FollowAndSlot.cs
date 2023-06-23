using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FollowAndSlot : MonoBehaviour
{
    private Image image;
    private Color OriginalColor;
    private bool IsStoringColor = false;
    private Color StoredColor;



    [SerializeField]
    public GameObject prefab;


    private GameObject colorInst;


    private Camera arCamera;

    private Transform cameraTransform;

    private bool pickedUp;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;

        image = GetComponent<Image>();
        


        arCamera = Camera.main;

        pickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;
        Touch touch = Input.GetTouch(0);
        if (colorInst != null && pickedUp)
        {
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {

                colorInst.transform.position = touch.position;

            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                Slot pSlot = findSlot(touch);
                if (pSlot != null)
                {
                    pSlot.OnDrop(image.color);
                    transform.parent.gameObject.SetActive(false);
                }

                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the hit object is a virtual object
                    GameObject virtualObject = hit.transform.gameObject;
                    if (virtualObject != null)
                    {
                        if (virtualObject.name == "HSV")
                        {
                            HSVMixer hsv = virtualObject.GetComponent<HSVMixer>();
                            hsv.inputColor(image.color);
                            transform.parent.gameObject.SetActive(false);
                        }

                        // The raycast hit a virtual object
                        // Do something with the virtual object
                    }


                }
                Destroy(colorInst);
                Debug.Log("Destroyed");
                pickedUp = false;
                colorInst = null;
            }
        }
    }

    private void LateUpdate()
    {
        // Face the camera by setting the canvas rotation
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
            cameraTransform.rotation * Vector3.up);
    }

    public void StartDrag(PointerEventData eventData)
    {
        colorInst = Instantiate(prefab, eventData.position, Quaternion.identity);
        colorInst.GetComponent<Image>().color = image.color;
        colorInst.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        Transform t = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        colorInst.transform.SetParent(t);
        Debug.Log("picked up");
        pickedUp = true;
    }

    private Slot findSlot(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult obj in results)
        {
            Debug.Log(obj.gameObject);
            if (obj.gameObject.GetComponent<Slot>() != null)
            {
                Slot slot = obj.gameObject.GetComponent<Slot>();
                return slot;
            }
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    private Image image;
    private Color OriginalColor;
    private bool IsStoringColor = false;
    private Color StoredColor;


    [SerializeField]
    public GameObject prefab;


    private GameObject colorInst;

    private RectTransform rTransform;


    private Camera arCamera;

    public GameObject snapText;

    public AudioSource SnapSound;

    private bool pickedUp;
    private void Start()
    {
        image = GetComponent<Image>();
        OriginalColor = Color.HSVToRGB(0, 0, 0.3f);

        OriginalColor.a = 0.7f;
        StoredColor = OriginalColor;

        rTransform = GetComponent<RectTransform>();

        arCamera = Camera.main;

        pickedUp = false;
    }

    public void OnDrop(Color draggedColor)
    {
        float ht, st, vt;
        Color.RGBToHSV(draggedColor, out ht, out st, out vt);
        Debug.Log(ht + " " + st + " " + vt);
        
        Color pColor = GlobalColors.findTrueColor(draggedColor);
        if (pColor == Color.black)
        {
            StoredColor = draggedColor;
        }
        else
        {
            StartCoroutine(Snap());
            Debug.Log("True Color");
            StoredColor = pColor;
        }
        if (vt < GlobalColors.minVal)
        {
            StoredColor = Color.black;
        }
        else if (st < GlobalColors.minSat)
        {
            if (vt > GlobalColors.maxVal)
            {
                StoredColor = Color.white;
            }
        }
        image.color = StoredColor;
        IsStoringColor = true;
    }

    IEnumerator Snap()
    {
        snapText.SetActive(true);
        SnapSound.Play();
        yield return new WaitForSeconds(0.5f);
        snapText.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Optional: Implement the OnDrop method if you need additional functionality
    }

    

    public Vector3 getCenter()
    {
        Vector2 center = rTransform.rect.center;
        return rTransform.TransformPoint(center);
    }
    private void Update()
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
                if (pSlot != null && !GameObject.ReferenceEquals(pSlot.gameObject, gameObject))
                {
                    
                    pSlot.OnDrop(StoredColor);
                    StoredColor = OriginalColor;
                    image.color = StoredColor;
                }
                else
                {
                    int hsvlm = LayerMask.GetMask("HSV");
                    int cplm = LayerMask.GetMask("ColorPieces");
                    Ray ray = arCamera.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit,10f,hsvlm))
                    {
                        
                        
                       HSVMixer hsv = hit.transform.gameObject.GetComponent<HSVMixer>();
                       hsv.inputColor(StoredColor);
                       StoredColor = OriginalColor;
                        

                        


                    }
                    else if (Physics.Raycast(ray, out hit, 10f, cplm))
                    {
                        SelectInPiece ip = hit.transform.gameObject.GetComponent<SelectInPiece>();
                        ip.selectPiece(StoredColor);
                        StoredColor = OriginalColor;
                    }
                    else
                    {
                        image.color = StoredColor;
                    }
                }
                
                Destroy(colorInst);
                Debug.Log("Destroyed");
                pickedUp = false;
                colorInst = null;
            }
        }
        
    }

    public void StartDrag(PointerEventData eventData)
    {
        if (StoredColor == OriginalColor)
            return;
        colorInst = Instantiate(prefab, eventData.position, transform.rotation);
        colorInst.GetComponent<Image>().color = StoredColor;
        colorInst.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        colorInst.transform.SetParent(transform);
        image.color = OriginalColor;
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
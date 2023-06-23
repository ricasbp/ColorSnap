using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class DraggableElement : MonoBehaviour
{

    private RectTransform draggableTransform;
    private CanvasGroup canvasGroup;

    private Camera arCamera;

    private Image image;

    private bool beginDrag = false;

    private void Awake()
    {
        draggableTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = true;
        canvasGroup.alpha = 0;
    }
    
  


    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;

        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Slot slot = findSlot(touch);
            if (slot != null)
            {
                PointerEventData ed = new PointerEventData(EventSystem.current);
                ed.position = new Vector2(touch.position.x, touch.position.y);
                slot.StartDrag(ed);
                return;
            }
            FollowAndSlot fSlot = findFSlot(touch);
            if (fSlot != null)
            {
                PointerEventData ed = new PointerEventData(EventSystem.current);
                ed.position = new Vector2(touch.position.x, touch.position.y);
                fSlot.StartDrag(ed);
                return;
            }
            if (findUI(touch))
            {

                return;
            }
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f))
            {
                
                return;

            }
            


            draggableTransform.position = touch.position;

            Debug.Log("Drag Started");
            // Read pixels from the screen and apply them to the texture
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();
            Color backgroundColor = texture.GetPixel(Mathf.RoundToInt(touch.position.x), Mathf.RoundToInt(touch.position.y));
            image.color = backgroundColor;
            canvasGroup.alpha = 1;
            beginDrag = true;

        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            if (!beginDrag)
                return;
            draggableTransform.position = touch.position;
        }
        else
        {
            if (!beginDrag)
                return;
            Slot pSlot = findSlot(touch);
            if (pSlot != null)
            {
                pSlot.OnDrop(image.color);
            }

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            Debug.Log("Drag Ended");
            beginDrag = false;
        }
        
    }

   private Slot findSlot(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult obj in results)
        {
            if (obj.gameObject.GetComponent<Slot>() != null)
            {
                Slot slot = obj.gameObject.GetComponent<Slot>();
                return slot;
            }
        }

        return null;
    }

    private FollowAndSlot findFSlot(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult obj in results)
        {
            if (obj.gameObject.GetComponent<FollowAndSlot>() != null)
            {
                FollowAndSlot slot = obj.gameObject.GetComponent<FollowAndSlot>();
                return slot;
            }
        }

        return null;
    }

    private bool findUI(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult obj in results)
        {
            if (obj.gameObject.name == "Instruction" || obj.gameObject.name == "InstructionB")
            {
                
                return true;
            }
        }

        return false;
    }
}

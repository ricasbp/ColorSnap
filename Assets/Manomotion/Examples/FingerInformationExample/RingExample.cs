using UnityEngine;
using TMPro;
using UnityEngine.Android;

/// <summary>
/// This will place out a ring on the ring finger using the information from the FingerInfoGizmo
/// </summary>
public class RingExample : MonoBehaviour
{
    /// <summary>
    /// The finger gizmo contains the finger information.
    /// </summary>
    private FingerInfoGizmo fingerInfoGizmo;

    /// <summary>
    /// Ring parts for only display half of the ring at a time.
    /// </summary>
    public GameObject[] ringPrefabParts;

    /// <summary>
    /// Used to set wich part of the ring prefabs to show.
    /// </summary>
    private bool isFront = true;

    /// <summary>
    /// Gesture inforamtion.
    /// </summary>
    private GestureInfo gestureInfo;

    /// <summary>
    /// Palmside.
    /// </summary>
    private HandSide palm = HandSide.Palmside;

    /// <summary>
    /// The gameobject contaning the image of the outlined hand. 
    /// </summary>
    [SerializeField]
    private GameObject outlineImage;

    /// <summary>
    /// The UI text for the current finger to be shown.
    /// </summary>
    public TMP_Text changeFingerText;

    /// <summary>
    /// The string that will be used on the toggle finger button.
    /// </summary>
    private string buttonText = "Toggle finger for ring. Current finger: ";

    private void Start()
    {
        ///Sets the screen orienation to portrait mode.
        Screen.orientation = ScreenOrientation.Portrait;

        if (fingerInfoGizmo == null)
        {
            try
            {
                fingerInfoGizmo = GameObject.Find("TryOnManager").GetComponent<FingerInfoGizmo>();
            }
            catch 
            {
                Debug.Log("Cant find 'TryOnManager' GameObject");
            }
        }

        SetManoMotionSettings();
        SetSelectFingerButtonText();
        GameObject.Find("Finger").SetActive(false);
    }

    private void SetManoMotionSettings()
    {
        ManomotionManager.Instance.ShouldRunFingerInfo(true);
        ManomotionManager.Instance.ShouldCalculateGestures(true);
        int ringFingerIndexDefault = 4;
        ManomotionManager.Instance.ToggleFingerInfoFinger(ringFingerIndexDefault);
    }

    void Update()
    {
        ///Updates the gestureinfo
        gestureInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;

        ///While open hand gesture is performed the ring should show.
        if (gestureInfo.mano_class == ManoClass.GRAB_GESTURE)
        {
            fingerInfoGizmo.ShowFingerInformation();
            ShowRing();
        }

        ///if not open hand gesture is performed the ring dont show.
        else
        {
            DontShowRing();
        }
    }

    private void ShowRing()
    {
        float centerPosition = 0.5f;

        ///Gets the position between the 2 finger points from the finger gizmo.
        Vector3 ringPlacement = Vector3.Lerp(fingerInfoGizmo.LeftFingerPoint3DPosition, fingerInfoGizmo.RightFingerPoint3DPosition, centerPosition);

        ///Place the ring at the ring placement position.
        transform.position = ringPlacement;

        ///Sets the rotation of the ring in relation the the finger position when hand is rotated 
        transform.LookAt(fingerInfoGizmo.LeftFingerPoint3DPosition);

        ///Scale the ring with the width from the 2 finger points and multiplyed by a scaleModifier.
        transform.localScale = new Vector3(fingerInfoGizmo.WidthBetweenFingerPoints, fingerInfoGizmo.WidthBetweenFingerPoints, fingerInfoGizmo.WidthBetweenFingerPoints);

        ///When Palm is showing the scale gets inverted to show the back of the ring.
        if (gestureInfo.hand_side == palm)
        {
            ActivateRingParts(!isFront);
            transform.localScale = new Vector3(-transform.localScale.x, -transform.localScale.y, -transform.localScale.z);
        }
        else
        {
            ActivateRingParts(isFront);
        }

        ///Disables the outline image.
        outlineImage.SetActive(false);
    }

    /// <summary>
    /// Activate or deactive parts of the ring depengind on wich half should show.
    /// </summary>
    /// <param name="front">bool to set the values.</param>
    private void ActivateRingParts(bool front)
    {
        ringPrefabParts[0].SetActive(front);
        ringPrefabParts[1].SetActive(front);
        ringPrefabParts[2].SetActive(!front);
    }

    /// <summary>
    /// Enabled the outline image and move the ring to -Vector3.one so its not visable.
    /// </summary>
    private void DontShowRing()
    {
        outlineImage.SetActive(true);
        transform.position = -Vector3.one;
    }

    /// current finger index, 4 default for ring finger;
    private int currentFingerIndex = 4;

    /// <summary>
    /// Toggles the finger that should use the ring and also updated the UI text.
    /// </summary>
    public void ToggleFingerForRing()
    {

        if (currentFingerIndex < 5)
        {
            currentFingerIndex++;
        }
        else
        {
            currentFingerIndex = 0;
        }

        ManomotionManager.Instance.ToggleFingerInfoFinger(currentFingerIndex);
        SetSelectFingerButtonText();
    }


    /// <summary>
    /// Sets the button text on the UI to match current finger info.
    /// </summary>
    private void SetSelectFingerButtonText()
    {
        switch (currentFingerIndex)
        {
            case 0:
                changeFingerText.text = buttonText + "None";
                break;
            case 1:
                changeFingerText.text = buttonText + "Thumb";
                break;
            case 2:
                changeFingerText.text = buttonText + "Index";
                break;
            case 3:
                changeFingerText.text = buttonText + "Middle";
                break;
            case 4:
                changeFingerText.text = buttonText + "Ring";
                break;
            case 5:
                changeFingerText.text = buttonText + "Pinky";
                break;
            default:
                break;
        }
    }
}

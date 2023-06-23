using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the visualization of the wrist width and position.
/// </summary>
public class WristInfoGizmo : MonoBehaviour
{
    /// <summary>
    /// Wrist info, gets the information from ManoMotionManager.
    /// </summary>
    private WristInfo wristInfo;

    /// <summary>
    /// The distance between the two wrist points.
    /// </summary>
    private float _widthBetweenWristPoints;

    [SerializeField]
    private GameObject wristInformationPrefab;

    /// <summary>
    /// The left wrist point gameobject.
    /// </summary>
    private GameObject leftWrist3D;

    /// <summary>
    /// The right wrist point gameobject.
    /// </summary>
    private GameObject rightWrist3D;

    /// <summary>
    /// The getter for the distance between the 2 Vector3 positions.
    /// </summary>
    public float WidthBetweenWristPoints
    {
        get
        {
            return _widthBetweenWristPoints;
        }
    }

    /// <summary>
    /// The getter for the left wrist position.
    /// </summary>
    public Vector3 LeftWristPoint3DPosition
    {
        get
        {
            return leftWrist3D.transform.position;
        }
    }

    /// <summary>
    /// The getter for the right wrist postion.
    /// </summary>
    public Vector3 RightWristPoint3DPosition
    {
        get
        {
            return rightWrist3D.transform.position;
        }
    }

    /// <summary>
    /// If left or right point is not set from inspector, it will find the LeftWristSphere and RightWristSphere game objects.
    /// </summary>
    private void Start()
    {
        wristInformationPrefab = Instantiate(wristInformationPrefab);
        wristInformationPrefab.name = "Wrist";

        if (leftWrist3D == null || rightWrist3D == null)
        {
            leftWrist3D = GameObject.Find("LeftWristSphere");
            rightWrist3D = GameObject.Find("RightWristSphere");
        }
    }

    /// <summary>
    /// If SDK should run wrist information ShowWristInformation will calculate the normalized values to fit the hands position.
    /// if no hand is detected the left and right sphere will be set to position (-1,-1,-1)
    /// </summary>
    public void ShowWristInformation()
    {
        wristInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.wristInfo;

        _widthBetweenWristPoints = (Vector3.Distance(wristInfo.left_point, wristInfo.right_point));

        leftWrist3D.transform.position = ManoUtils.Instance.CalculateNewPositionSkeletonPosition(new Vector3(wristInfo.left_point.x, wristInfo.left_point.y, ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[0].z), 1.5f);
        rightWrist3D.transform.position = ManoUtils.Instance.CalculateNewPositionSkeletonPosition(new Vector3(wristInfo.right_point.x, wristInfo.right_point.y, ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[0].z), 1.5f);

        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class == ManoClass.NO_HAND)
        {
            ActivateWristGizmos(false);
        }
        else
        {
            ActivateWristGizmos(true);
        }
    }

    private void ActivateWristGizmos(bool status)
    {
        rightWrist3D.SetActive(status);
        leftWrist3D.SetActive(status);
    }
}

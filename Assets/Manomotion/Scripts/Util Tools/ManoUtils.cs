using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

/// <summary>
/// Tools that can be used to get information adapted to the screen size.
/// </summary>
public class ManoUtils : MonoBehaviour
{
    #region  Event
    public static Action OnOrientationChanged;
    #endregion

    public SupportedOrientation currentOrientation;

    #region Singleton
    private static ManoUtils instance;
    public static ManoUtils Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    private Vector3 correction_ratio = Vector3.one;

    protected void Awake()
    {
        if (instance == null)
            instance = this;
        if (!cam)
            cam = Camera.main;
    }

    [SerializeField]
    private Camera cam;

    /// <summary>
    /// Calculates the new position in relation to the screen.
    /// </summary>
    /// <param name="Point">Requires a Vector3 point</param>
    /// <param name="depth">Requires the float value of depth</param>
    /// <returns></returns>
    public Vector3 CalculateScreenPosition(Vector3 Point, float depth)
    {
        if (!cam)
        {
            cam = Camera.main;
        }

        Vector3 correct_point = Point - Vector3.one * 0.5f;
        correct_point.Scale(correction_ratio);
        correct_point = correct_point + Vector3.one * 0.5f;
        correct_point = new Vector3(Mathf.Clamp(correct_point.x, 0, 1), Mathf.Clamp(correct_point.y, 0, 1), Mathf.Clamp(correct_point.z, 0, 1));
        return cam.ViewportToScreenPoint(correct_point + Vector3.forward * depth);
    }

    /// <summary>
    /// Calculates the new position in relation to the main camera.
    /// </summary>
    /// <param name="Point">Requires a Vector3 point</param>
    /// <param name="depth">Requires the float value of depth</param>
    /// <returns></returns>
    public Vector3 CalculateNewPosition(Vector3 Point, float depth)
    {
        if (!cam)
        {
            cam = Camera.main;
        }

        Vector3 correct_point = Point - Vector3.one * 0.5f;
        correct_point.Scale(correction_ratio);
        correct_point = correct_point + Vector3.one * 0.5f;
        correct_point = new Vector3(Mathf.Clamp(correct_point.x, 0, 1), Mathf.Clamp(correct_point.y, 0, 1), Mathf.Clamp(correct_point.z, 0, 1));
        return cam.ViewportToWorldPoint(correct_point + Vector3.forward);
    }

    /// <summary>
    /// Calculates the new position in relation to the main camera, for the skleleton Joitns with clamped depth if -1 and 1.
    /// </summary>
    /// <param name="Point">Requires a Vector3 point</param>
    /// <param name="depth">Requires the float value of depth</param>
    /// <returns></returns>
    public Vector3 CalculateNewPositionSkeletonPosition(Vector3 Point, float depth)
    {
        if (!cam)
        {
            cam = Camera.main;
        }

        Vector3 correct_point = Point - Vector3.one * 0.5f;
        correct_point.Scale(correction_ratio);
        correct_point = correct_point + Vector3.one * 0.5f;
        correct_point = new Vector3(Mathf.Clamp(correct_point.x, 0, 1), Mathf.Clamp(correct_point.y, 0, 1), Mathf.Clamp(correct_point.z, -1, 1));
        return cam.ViewportToWorldPoint(correct_point + Vector3.forward * depth);
    }

    /// <summary>
    /// Calculates the new position with depth in relation to the main camera.
    /// </summary>
    /// <param name="Point">Requires a Vector3 point</param>
    /// <param name="depth">Requires the float value of depth</param>
    /// <returns></returns>
    public Vector3 CalculateNewPositionDepth(Vector3 Point, float depth)
    {
        if (!cam)
        {
            cam = Camera.main;
        }

        Vector3 correct_point = Point - Vector3.one * 0.5f;
        correct_point.Scale(correction_ratio);
        correct_point = correct_point + Vector3.one * 0.5f;
        correct_point = new Vector3(Mathf.Clamp(correct_point.x, 0, 1), Mathf.Clamp(correct_point.y, 0, 1), Mathf.Clamp(correct_point.z, 0, 1));
        return cam.ViewportToWorldPoint(correct_point + Vector3.forward * depth);
    }

    /// <summary>
    /// Adjust the transform in the received mesh renderer to fit the screen without stretching
    /// </summary>
    /// <param name="mesh_renderer"></param>
    internal void AjustBorders(MeshRenderer mesh_renderer, Session session)
    {
        float ratio = CalculateRatio(mesh_renderer, session);
        float size = CalculateSize(mesh_renderer, session, ratio);

        AdjustMeshScale(mesh_renderer, session, ratio, size);
        CalculateCorrectionPoint(mesh_renderer, session, ratio, size);
    }

    /// <summary>
    /// Calculates the current ratio depending on the device orientation
    /// </summary>
    /// <param name="mesh_renderer">Requiers a mesh_renderer</param>
    /// <param name="session">Requiers a Session for orientation</param>
    /// <returns></returns>
    internal float CalculateRatio(MeshRenderer mesh_renderer, Session session)
    {
        float ratio = 1;
        switch (session.orientation)
        {
            case SupportedOrientation.FACE_DOWN:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
            case SupportedOrientation.FACE_UP:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
            case SupportedOrientation.PORTRAIT:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
            case SupportedOrientation.PORTRAIT_UPSIDE_DOWN:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
            case SupportedOrientation.LANDSCAPE_LEFT:
                ratio = (float)ManomotionManager.Instance.Width / ManomotionManager.Instance.Height;
                break;
            case SupportedOrientation.LANDSCAPE_RIGHT:
                ratio = (float)ManomotionManager.Instance.Width / ManomotionManager.Instance.Height;
                break;
            case SupportedOrientation.PORTRAIT_FRONT_FACING:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
            case SupportedOrientation.PORTRAIT_UPSIDE_DOWN_FRONT_FACING:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
            case SupportedOrientation.LANDSCAPE_LEFT_FRONT_FACING:
                ratio = (float)ManomotionManager.Instance.Width / ManomotionManager.Instance.Height;
                break;
            case SupportedOrientation.LANDSCAPE_RIGHT_FRONT_FACING:
                ratio = (float)ManomotionManager.Instance.Width / ManomotionManager.Instance.Height;
                break;
            default:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
        }

        return ratio;
    }

    /// <summary>
    /// Gets the size for the AdjustBorders method.
    /// </summary>
    /// <param name="mesh_renderer">Requiers a mesh_renderer</param>
    /// <param name="session">Requiers a Session for orientation</param>
    /// <param name="ratio">Requiers a ratio</param>
    /// <returns></returns>
    internal float CalculateSize(MeshRenderer mesh_renderer, Session session, float ratio)
    {
        if (!cam)
            cam = Camera.main;
        float size = 1;
        float height = 2.0f * Mathf.Tan(0.5f * cam.fieldOfView * Mathf.Deg2Rad) * mesh_renderer.transform.localPosition.z;
        float width;

        switch (session.orientation)
        {
            case SupportedOrientation.FACE_DOWN:
                size = height;
                break;
            case SupportedOrientation.FACE_UP:
                size = height;
                break;
            case SupportedOrientation.PORTRAIT:
                size = height;
                break;
            case SupportedOrientation.PORTRAIT_UPSIDE_DOWN:
                size = height;
                break;
            case SupportedOrientation.LANDSCAPE_LEFT:
                width = height * Screen.width / Screen.height;
                size = width / ratio;
                break;
            case SupportedOrientation.LANDSCAPE_RIGHT:
                width = height * Screen.width / Screen.height;
                size = width / ratio;
                break;
            case SupportedOrientation.PORTRAIT_FRONT_FACING:
                size = height;
                break;
            case SupportedOrientation.PORTRAIT_UPSIDE_DOWN_FRONT_FACING:
                size = height;
                break;
            case SupportedOrientation.LANDSCAPE_LEFT_FRONT_FACING:
                width = height * Screen.width / Screen.height;
                size = width / ratio;
                break;
            case SupportedOrientation.LANDSCAPE_RIGHT_FRONT_FACING:
                width = height * Screen.width / Screen.height;
                size = width / ratio;
                break;
            default:
                width = height * Screen.width / Screen.height;
                size = width / ratio;
                break;
        }
        return size;
    }

    /// <summary>
    /// Adjust the scale of the mesh render.
    /// </summary>
    /// <param name="mesh_renderer">Requiers a mesh_render</param>
    /// <param name="session">Requiers a Session</param>
    /// <param name="ratio">Requiers a ratio</param>
    /// <param name="size">Requiers a size</param>
    internal void AdjustMeshScale(MeshRenderer mesh_renderer, Session session, float ratio, float size)
    {
        switch (session.orientation)
        {
            case SupportedOrientation.FACE_DOWN:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
            case SupportedOrientation.FACE_UP:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
            case SupportedOrientation.PORTRAIT:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
            case SupportedOrientation.PORTRAIT_UPSIDE_DOWN:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
            case SupportedOrientation.LANDSCAPE_LEFT:
                mesh_renderer.transform.localScale = new Vector3(size * ratio, size, 0f);
                break;
            case SupportedOrientation.LANDSCAPE_RIGHT:
                mesh_renderer.transform.localScale = new Vector3(size * ratio, size, 0f);
                break;
            case SupportedOrientation.PORTRAIT_FRONT_FACING:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
            case SupportedOrientation.PORTRAIT_UPSIDE_DOWN_FRONT_FACING:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
            case SupportedOrientation.LANDSCAPE_LEFT_FRONT_FACING:
                mesh_renderer.transform.localScale = new Vector3(size * ratio, size, 0f);
                break;
            case SupportedOrientation.LANDSCAPE_RIGHT_FRONT_FACING:
                mesh_renderer.transform.localScale = new Vector3(size * ratio, size, 0f);
                break;
            default:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
        }
    }

    /// <summary>
    /// Calculate a correction point depending on the orientation.
    /// </summary>
    /// <param name="mesh_renderer">Requiers a mesh_render</param>
    /// <param name="session">Requiers a Session</param>
    /// <param name="ratio">Requiers a ratio</param>
    /// <param name="size">Requiers a size</param>
    internal void CalculateCorrectionPoint(MeshRenderer mesh_renderer, Session session, float ratio, float size)
    {
        Vector3 screen_ratio;
        Vector3 image_ratio;
        switch (session.orientation)
        {
            case SupportedOrientation.FACE_DOWN:
                screen_ratio = new Vector3(((float)Screen.height / Screen.width), 1, 1);
                image_ratio = new Vector3(ratio, 1, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case SupportedOrientation.FACE_UP:
                screen_ratio = new Vector3(((float)Screen.height / Screen.width), 1, 1);
                image_ratio = new Vector3(ratio, 1, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case SupportedOrientation.PORTRAIT:
                screen_ratio = new Vector3(((float)Screen.height / Screen.width), 1, 1);
                image_ratio = new Vector3(ratio, 1, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case SupportedOrientation.PORTRAIT_UPSIDE_DOWN:
                screen_ratio = new Vector3(((float)Screen.height / Screen.width), 1, 1);
                image_ratio = new Vector3(ratio, 1, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case SupportedOrientation.LANDSCAPE_LEFT:
                screen_ratio = new Vector3(1, 1 / ((float)Screen.height / Screen.width), 1);
                image_ratio = new Vector3(1, 1 / ratio, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case SupportedOrientation.LANDSCAPE_RIGHT:
                screen_ratio = new Vector3(1, 1 / ((float)Screen.height / Screen.width), 1);
                image_ratio = new Vector3(1, 1 / ratio, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case SupportedOrientation.PORTRAIT_FRONT_FACING:
                screen_ratio = new Vector3(((float)Screen.height / Screen.width), 1, 1);
                image_ratio = new Vector3(ratio, 1, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case SupportedOrientation.PORTRAIT_UPSIDE_DOWN_FRONT_FACING:
                screen_ratio = new Vector3(((float)Screen.height / Screen.width), 1, 1);
                image_ratio = new Vector3(ratio, 1, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case SupportedOrientation.LANDSCAPE_LEFT_FRONT_FACING:
                screen_ratio = new Vector3(1, 1 / ((float)Screen.height / Screen.width), 1);
                image_ratio = new Vector3(1, 1 / ratio, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case SupportedOrientation.LANDSCAPE_RIGHT_FRONT_FACING:
                screen_ratio = new Vector3(1, 1 / ((float)Screen.height / Screen.width), 1);
                image_ratio = new Vector3(1, 1 / ratio, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            default:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
        }
    }

    private void Start()
    {
        currentOrientation = SupportedOrientation.LANDSCAPE_LEFT;
        if (OnOrientationChanged != null)
        {
            OnOrientationChanged();
        }
    }

    void Update()
    {
        CheckForScreenOrientationChange();
    }

    /// <summary>
    /// Checks for changes on the orientation of the device.
    /// </summary>
    void CheckForScreenOrientationChange()
    {
#if UNITY_STANDALONE
        //currentOrientation = SupportedOrientation.LANDSCAPE_LEFT_FRONT_FACING;

        if (OnOrientationChanged != null)
        {
            OnOrientationChanged();
        }
#else
        if ((int)Input.deviceOrientation != (int)SupportedOrientation.FACE_DOWN && (int)Input.deviceOrientation != (int)SupportedOrientation.FACE_UP && (int)Input.deviceOrientation != (int)SupportedOrientation.UNKNOWN)
        {
            if ((int)currentOrientation != (int)Input.deviceOrientation)
            {
                int currentInputOrietation = (int)Input.deviceOrientation;

                currentOrientation = (SupportedOrientation)currentInputOrietation;
                if (OnOrientationChanged != null)
                {
                    OnOrientationChanged();
                }
            }

        }
#endif
    }

    /// <summary>
    /// Retrieve the absolute values of a Vector3
    /// </summary>
    /// <returns>The abs.</returns>
    /// <param name="vector">Requires a Vector3 value.</param>
    Vector3 VectorAbs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    /// <summary>
    /// Properly orients a MeshRenderer in order to be displayed properly
    /// </summary>
    /// <param name="meshRenderer">Mesh renderer.</param>
    public void OrientMeshRenderer(MeshRenderer meshRenderer)
    {
        if (ManomotionManager.Instance.Manomotion_Session.add_on == AddOn.DEFAULT)
        {
            switch (ManomotionManager.Instance.Manomotion_Session.orientation)
            {
                case SupportedOrientation.PORTRAIT:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                case SupportedOrientation.PORTRAIT_UPSIDE_DOWN:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                case SupportedOrientation.LANDSCAPE_LEFT:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case SupportedOrientation.LANDSCAPE_RIGHT:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    break;
                case SupportedOrientation.FACE_DOWN:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                case SupportedOrientation.FACE_UP:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                case SupportedOrientation.UNKNOWN:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                case SupportedOrientation.PORTRAIT_FRONT_FACING:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                case SupportedOrientation.PORTRAIT_UPSIDE_DOWN_FRONT_FACING:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                case SupportedOrientation.LANDSCAPE_LEFT_FRONT_FACING:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case SupportedOrientation.LANDSCAPE_RIGHT_FRONT_FACING:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    break;
                default:
                    break;
            }
        }
        else
        {
            meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

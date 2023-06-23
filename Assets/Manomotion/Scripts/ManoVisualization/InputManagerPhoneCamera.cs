using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Gets the images from the phone camera and send it to the ManoMotionManager.
/// </summary>
public class InputManagerPhoneCamera : InputManagerBaseClass
{
    #region consts

    const int STARTING_WIDTH = 640;
    const int STARTING_HEIGHT = 480;

    #endregion

    #region  #Variables
    /// The phone cameras.
    private WebCamDevice[] webCamDevices;
    private WebCamTexture backFacingCamera;
    public ManoMotionFrame currentManoMotionFrame;
    public ManoVisualization manoVisualization;

    #endregion

    private void Awake()
    {
        ForceApplicationPermissions();
    }

    private void Start()
    {
        InitializeManoMotionFrame();
    }

    /// <summary>
    /// Initializes the ManoMotion Frame and lets the subscribers of the event know of its information.
    /// </summary>
    private void InitializeManoMotionFrame()
    {
        currentManoMotionFrame = new ManoMotionFrame();
        ResizeManoMotionFrameResolution(STARTING_WIDTH, STARTING_HEIGHT);
        currentManoMotionFrame.orientation = Input.deviceOrientation;

        if (OnFrameInitialized != null)
        {
            OnFrameInitialized(currentManoMotionFrame);
            Debug.Log("Initialized input parameters");
        }
        else
        {
            Debug.LogWarning("No one subscribed to OnFrameInitialized");
        }
    }

    /// <summary>
    /// Gets the camera frame pixel colors.
    /// </summary>
    protected void GetCameraFrameInformation()
    {
        if (!backFacingCamera)
        {
            Debug.LogError("No device camera available");
            return;
        }
        if (backFacingCamera.GetPixels32().Length < 300)
        {
            Debug.LogWarning("The frame from the camera is too small. Pixel array length:  " + backFacingCamera.GetPixels32().Length);
            return;
        }

        if (currentManoMotionFrame.pixels.Length != backFacingCamera.GetPixels32().Length)
        {
            ResizeManoMotionFrameResolution(backFacingCamera.width, backFacingCamera.height);
            return;
        }

        currentManoMotionFrame.pixels = backFacingCamera.GetPixels32();
        currentManoMotionFrame.texture.SetPixels32(backFacingCamera.GetPixels32());
        currentManoMotionFrame.texture.Apply();
        currentManoMotionFrame.orientation = Input.deviceOrientation;

        if (OnFrameUpdated != null)
        {
            OnFrameUpdated(currentManoMotionFrame);
        }
    }

    /// <summary>
    /// Sets the resolution of the currentManoMotion frame that is passed to the subscribers that want to make use of the input camera feed.
    /// </summary>
    /// <param name="newWidth">Requires a width value.</param>
    /// <param name="newHeight">Requires a height value.</param>
    protected void ResizeManoMotionFrameResolution(int newWidth, int newHeight)
    {
        Debug.Log("Called ResizeManomotionFrame");
        currentManoMotionFrame.width = newWidth;
        currentManoMotionFrame.height = newHeight;
        currentManoMotionFrame.pixels = new Color32[newWidth * newHeight];
        currentManoMotionFrame.texture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, true);
        currentManoMotionFrame.texture.Apply();

        if (OnFrameResized != null)
        {
            OnFrameResized(currentManoMotionFrame);
        }
    }

    void Update()
    {
        GetCameraFrameInformation();
    }

    /// <summary>
    /// Start the camera when enabled.
    /// </summary>
    private void OnEnable()
    {
        if (backFacingCamera)
        {
            if (!backFacingCamera.isPlaying)
            {
                backFacingCamera.Play();
            }
        }
        else
        {
            Debug.LogError("I dont have a backfacing Camera");
        }
    }

    /// <summary>
    /// Stops the camera when disabled.
    /// </summary>
    private void OnDisable()
    {
        if (backFacingCamera && !backFacingCamera.isPlaying)
        {
            backFacingCamera.Stop();
        }
    }

    #region Application on Background

    bool isPaused = false;

    /// <summary>
    /// Stops processing when application is put to background.
    /// </summary>
    /// <param name="hasFocus">If application is running or is in background</param>
    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
        if (isPaused)
        {
            ManomotionManager.Instance.StopProcessing();
        }
    }

    /// <summary>
    /// Stops the processing if application is paused.
    /// </summary>
    /// <param name="pauseStatus">If application is paused or not</param>
    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
        if (isPaused)
        {
            ManomotionManager.Instance.StopProcessing();
        }
    }

    #endregion
}

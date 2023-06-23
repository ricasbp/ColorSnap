using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CameraSystem
{
    public class InputManagerAdjustable : InputManagerBaseClass
    {
        #region consts

        const int STARTING_WIDTH = 640;
        const int STARTING_HEIGHT = 480;

        #endregion

        #region  Event

        #endregion

        #region  #Variables

        private WebCamDevice[] webCamDevices;
        private WebCamTexture currentPlayingCamera;
        private int currentIndex;
        public ManoMotionFrame currentManoMotionFrame;

#if !UNITY_STANDALONE
        [HideInInspector]
#endif
        public bool isFrontFacingSceneario;

        #endregion
        private void HandleNewCameraDeviceSelected(int deviceNumber)
        {
            Debug.Log("Handling a new Camera " + deviceNumber);
            currentIndex = deviceNumber;
            StopCurrentPlayingCamera();
            AssignNewCurrentCamera(deviceNumber);
            StartCurrentCamera();

            isFrontFacing = WebCamTexture.devices[currentIndex].isFrontFacing;
            if (OnChangeCamera != null)
            {
                OnChangeCamera();
            }
        }

        /// <summary>
        /// Stop all cameras that might be playing.
        /// </summary>
        private void StopCurrentPlayingCamera()
        {
            if (currentPlayingCamera)
            {
                currentPlayingCamera.Stop();
            }
            currentPlayingCamera = null;
        }

        private void AssignNewCurrentCamera(int cameraNumber)
        {
            currentPlayingCamera = new WebCamTexture(WebCamTexture.devices[cameraNumber].name, STARTING_WIDTH, STARTING_HEIGHT);
        }

        private void StartCurrentCamera()
        {
            try
            {
                currentPlayingCamera.Play();
            }
            catch (System.Exception ex)
            {

            }
        }

        private void Awake()
        {
            ForceApplicationPermissions();
            CameraSelector.OnCameraDeviceNumberSelected += HandleNewCameraDeviceSelected;
        }

        private void Start()
        {
#if !UNITY_STANDALONE
            isFrontFacingSceneario = true;
#endif
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
            if (!currentPlayingCamera)
            {
                Debug.LogWarning("No device camera available");
                HandleNewCameraDeviceSelected(0);
                return;
            }

            if (currentPlayingCamera.GetPixels32().Length < 300)
            {
                Debug.LogWarning("The frame from the camera is too small. Pixel array length:  " + currentPlayingCamera.GetPixels32().Length);
                return;
            }

            if (currentManoMotionFrame.pixels.Length != currentPlayingCamera.GetPixels32().Length)
            {
                ResizeManoMotionFrameResolution(currentPlayingCamera.width, currentPlayingCamera.height);
                return;
            }

            currentManoMotionFrame.pixels = currentPlayingCamera.GetPixels32();
            currentManoMotionFrame.texture.SetPixels32(currentPlayingCamera.GetPixels32());
            currentManoMotionFrame.texture.Apply();

            //Flip the texture if using front facing to mach the image to the device.
            if (WebCamTexture.devices[currentIndex].isFrontFacing && isFrontFacingSceneario)
            {
#if UNITY_ANDROID || UNITY_STANDALONE

                FlipTextureHorizontal(ref currentManoMotionFrame.texture);
#endif
#if UNITY_IOS
                FlipTextureVertical(ref currentManoMotionFrame.texture);
#endif

                currentManoMotionFrame.texture.Apply();
            }

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
            try
            {
                GetCameraFrameInformation();
            }
            catch
            {
                Debug.Log("Cant get camera information");
            }
        }

        /// <summary>
        /// Flips the texture Horizontaly
        /// </summary>
        /// <param name="original">the ref to the texture to filp</param>
        public void FlipTextureHorizontal(ref Texture2D original)
        {
            int textureWidth = original.width;
            int textureHeight = original.height;

            {
                Color[] colorArray = original.GetPixels();

                for (int j = 0; j < textureHeight; j++)
                {
                    int rowStart = 0;
                    int rowEnd = textureWidth - 1;

                    while (rowStart < rowEnd)
                    {
                        Color hold = colorArray[(j * textureWidth) + (rowStart)];
                        colorArray[(j * textureWidth) + (rowStart)] = colorArray[(j * textureWidth) + (rowEnd)];
                        colorArray[(j * textureWidth) + (rowEnd)] = hold;
                        rowStart++;
                        rowEnd--;
                    }
                }

                original.SetPixels(colorArray);
                original.Apply();
            }
        }

        /// <summary>
        /// Flips the texture Verticaly.
        /// </summary>
        /// <param name="orignal">The ref to the texture to flip</param>
        public void FlipTextureVertical(ref Texture2D orignal)
        {
            int width = orignal.width;
            int height = orignal.height;
            Color[] pixels = orignal.GetPixels();
            Color[] pixelsFlipped = orignal.GetPixels();
            for (int i = 0; i < height; i++)
            {
                Array.Copy(pixels, i * width, pixelsFlipped, (height - i - 1) * width, width);
            }
            orignal.SetPixels(pixelsFlipped);
            orignal.Apply();
        }

        /// <summary>
        /// Start the camera when enabled.
        /// </summary>
        private void OnEnable()
        {
            if (currentPlayingCamera)
            {
                if (!currentPlayingCamera.isPlaying)
                {
                    currentPlayingCamera.Play();
                }
            }
            else
            {
                Debug.LogWarning("I dont have a backfacing Camera");
            }
        }

        /// <summary>
        /// Stops the camera when disabled.
        /// </summary>
        private void OnDisable()
        {
            if (currentPlayingCamera && !currentPlayingCamera.isPlaying)
            {
                currentPlayingCamera.Stop();
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
}

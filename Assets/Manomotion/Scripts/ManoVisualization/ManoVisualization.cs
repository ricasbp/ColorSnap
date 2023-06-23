using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the background mesh and displays the camera iamge.
/// </summary>
[AddComponentMenu("ManoMotion/ManoVisualization")]
public class ManoVisualization : MonoBehaviour
{
    private int handsSupportedByLicence = 1;

    #region variables

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject manomotionGenericLayerPrefab;

    [SerializeField]
    private bool _showBackgroundLayer;

    private MeshRenderer _backgroundMeshRenderer;

    #endregion

    #region Properties
    
    public bool ShowBackgroundLayer
    {
        get { return _showBackgroundLayer; }

        set { _showBackgroundLayer = value; }
    }

    #endregion

    #region Initializing Components

    /// <summary>
    /// Creates the meshes needed by the different Manomotion Layers and also parents them to the scene's Main Camera
    /// </summary>
    private void InstantiateManomotionMeshes()
    {
        int backgroundInitialDepth = 10;
        GameObject background = Instantiate(manomotionGenericLayerPrefab);
        background.transform.name = "Background";
        background.transform.SetParent(cam.transform);
        _backgroundMeshRenderer = background.GetComponent<MeshRenderer>();
        _backgroundMeshRenderer.transform.localPosition = new Vector3(0, 0, backgroundInitialDepth);
    }

    void Start()
    {
        if (!cam)
            cam = Camera.main;

        SetHandsSupportedByLicence();
        InstantiateManomotionMeshes();

        ManomotionManager.OnManoMotionFrameProcessed += HandleVisualizationOfUpdatedFrame;
    }

    /// <summary>
    /// Set the maximum number of hands that can be simultaneously detected by Manomotion Manager based on the licence, only 1 hand is supporetd.
    /// </summary>
    void SetHandsSupportedByLicence()
    {
        handsSupportedByLicence = 1;
    }

    #endregion

    /// <summary>
    /// Update the visualization after the manomotion frame is processed by the SDK.
    /// </summary>
    void HandleVisualizationOfUpdatedFrame()
    {
        if (!cam)
            cam = Camera.main;

        for (int handIndex = 0; handIndex < handsSupportedByLicence; handIndex++)
        {
            Warning warning = ManomotionManager.Instance.Hand_infos[handIndex].hand_info.warning;
            TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[handIndex].hand_info.tracking_info;
        }

        DisplayBackground(ManomotionManager.Instance.Visualization_info.rgb_image, _backgroundMeshRenderer);
    }

    /// <summary>
    /// Projects the texture received from the camera as the background
    /// </summary>
    /// <param name="backgroundTexture">Requires the texture captured from the camera</param>
    /// <param name="backgroundMeshRenderer">Requires the MeshRenderer that the texture will be displayed</param>
    void DisplayBackground(Texture2D backgroundTexture, MeshRenderer backgroundMeshRenderer)
    {
        if (!backgroundMeshRenderer)
        {
            return;
        }

        backgroundMeshRenderer.enabled = _showBackgroundLayer;

        if (_showBackgroundLayer)
        {
            ManoUtils.Instance.OrientMeshRenderer(backgroundMeshRenderer);
            backgroundMeshRenderer.material.mainTexture = backgroundTexture;
            ManoUtils.Instance.AjustBorders(backgroundMeshRenderer, ManomotionManager.Instance.Manomotion_Session);
        }
    }

    /// <summary>
    /// Toggles the visibility of the given gameobject
    /// </summary>
    /// <param name="givenObject">Requires a gameObject</param>
    private void ToggleObjectVisibility(GameObject givenObject)
    {
        givenObject.SetActive(!givenObject.activeInHierarchy);
    }
}
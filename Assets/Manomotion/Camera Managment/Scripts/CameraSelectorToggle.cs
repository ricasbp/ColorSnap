using CameraSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toggle the Camera selction dropdown in the UI.
/// </summary>
public class CameraSelectorToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraSelector;

    public void Start()
    {
        if (cameraSelector == null)
        {
            cameraSelector = GetComponentInChildren<CameraSelector>().gameObject;
        }
    }

    /// <summary>
    /// Toggle if Camera Selector should be active or not.
    /// </summary>
    public void ToggleCameraSelector()
    {
        cameraSelector.SetActive(!cameraSelector.activeInHierarchy);
    }
}

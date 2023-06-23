using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        // Face the camera by setting the canvas rotation
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
            cameraTransform.rotation * Vector3.up);
    }
}

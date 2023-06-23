using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace CameraSystem
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class CameraSelector : MonoBehaviour
    {
        public static Action<int> OnCameraDeviceNumberSelected;
        private TMP_Dropdown dropDownMenu;

        /// <summary>
        /// Adds the available device cameras and add them to drop down selector.
        /// </summary>
        void Start()
        {
            dropDownMenu = this.GetComponent<TMP_Dropdown>();
            List<string> deviceNames = new List<string>();

            for (int i = 0; i < WebCamTexture.devices.Length; i++)
            {
                if (WebCamTexture.devices[i].isFrontFacing)
                {
                    deviceNames.Add("Front Facing " + WebCamTexture.devices[i].name);
                }
                else
                {
                    deviceNames.Add("Back Facing " + WebCamTexture.devices[i].name);
                }
            }

            dropDownMenu.AddOptions(deviceNames);

            HandleInputData();

            this.gameObject.SetActive(false);
        }


        public void HandleInputData()
        {
            if (OnCameraDeviceNumberSelected != null)
            {
                OnCameraDeviceNumberSelected(dropDownMenu.value);
                Debug.Log("HandleValue Changed " + dropDownMenu.value);
            }
        }
    }
}

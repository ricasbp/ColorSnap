using System;
using UnityEngine;
using UnityEngine.Android;

/// <summary>
/// Base class for the input manager 
/// </summary>
public abstract class InputManagerBaseClass : MonoBehaviour
{
	public static Action<ManoMotionFrame> OnFrameUpdated;
	public static Action<ManoMotionFrame> OnFrameInitialized;
	public static Action<ManoMotionFrame> OnFrameResized;
	public static Action<DeviceOrientation> OnOrientationChanged;
	public static Action<AddOn> OnAddonSet;
	public static Action OnChangeCamera;

	protected int rezMinValue;
	protected int rezMaxValue;

	[HideInInspector]
	public bool isFrontFacing;


	protected virtual void ResizeCurrentFrameTexture(int width, int height) { }

	/// <summary>
	/// Forces the application to ask for camera permissions and external storage read and writte.
	/// </summary>
	protected virtual void ForceApplicationPermissions()
	{

#if UNITY_ANDROID
		/* Since 2018.3, Unity doesn't automatically handle permissions on Android, so as soon as
            * the menu is displayed, ask for camera permissions. */
		if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
		{
			Permission.RequestUserPermission(Permission.Camera);
		}
		if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
		{
			Permission.RequestUserPermission(Permission.ExternalStorageWrite);
		}
		if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
		{
			Permission.RequestUserPermission(Permission.ExternalStorageRead);
		}
#endif
	}

	protected virtual void UpdateFrame<T>(T parameter) { }
	protected virtual void UpdateFrame() { }
}



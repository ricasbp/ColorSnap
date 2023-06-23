using UnityEngine;

/// <summary>
/// Toggle the visualization of the background mesh.
/// </summary>
public class ToggleVisualizationValues : MonoBehaviour
{
	private ManoVisualization _manoVisualization;

	private void Start()
	{
		_manoVisualization = GetComponent<ManoVisualization>();
	}

	/// <summary>
	/// Toggles the boolean of showing the background layer.
	/// </summary>
	public void ToggleShowBackgroundLayer()
	{
		_manoVisualization.ShowBackgroundLayer = !_manoVisualization.ShowBackgroundLayer;
	}
}

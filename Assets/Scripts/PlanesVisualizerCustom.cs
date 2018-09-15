using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.XR.MagicLeap;

/// <summary>
/// Manages plane rendering based on plane detection from Planes component.
/// </summary>
[RequireComponent(typeof(PlanesCustom))]
public class PlanesVisualizerCustom : MonoBehaviour
{
	#region Public Variables
	[Tooltip("Object prefab to use for plane visualization.")]
	public GameObject PlaneVisualPrefab;

	// List of all the planes being rendered
	private List<GameObject> _planeCache;
	private List<uint> _planeFlags;
	#endregion

	#region Unity Methods
	/// <summary>
	/// Initializes all variables and makes sure needed components exist
	/// </summary>
	void Awake()
	{
		if (PlaneVisualPrefab == null)
		{
			Debug.LogError("Error PlanesVisualizer.PlaneVisualPrefab is not set, disabling script.");
			enabled = false;
			return;
		}

		MeshRenderer planeRenderer = PlaneVisualPrefab.GetComponent<MeshRenderer>();
		if (planeRenderer == null)
		{
			Debug.LogError("Error PlanesVisualizer MeshRenderer component not found, disabling script.");
			enabled = false;
			return;
		}

		_planeCache = new List<GameObject>();
		_planeFlags = new List<uint>();
	}

	/// <summary>
	/// Destroys all planes instances created
	/// </summary>
	void OnDestroy()
	{
		_planeCache.ForEach((GameObject go) => GameObject.Destroy(go));
		_planeCache.Clear();
		_planeFlags.Clear();
	}
	#endregion

	#region Public Functions
	/// <summary>
	/// Updates planes and creates new planes based on detected planes.
	///
	/// This function reuses previously allocated memory to convert all planes
	/// to the new ones by changing their transforms, it allocates new objects
	/// if the current result ammount is bigger than the ones already stored.
	/// </summary>
	/// <param name="p">The planes component</param>
	public void OnPlanesUpdate(MLWorldPlane[] planes)
	{
		int index = planes.Length > 0 ? planes.Length - 1 : 0;
		for (int i = index; i < _planeCache.Count; ++i)
		{
			_planeCache[i].SetActive(false);
		}

		for (int i = 0; i < planes.Length; ++i)
		{
			GameObject planeVisual;
			if (i < _planeCache.Count)
			{
				planeVisual = _planeCache[i];
				planeVisual.SetActive(true);
			}
			else
			{
				planeVisual = Instantiate(PlaneVisualPrefab);
				_planeCache.Add(planeVisual);
				_planeFlags.Add(0);
			}

			planeVisual.transform.position = planes[i].Center;
			planeVisual.transform.rotation = planes[i].Rotation;
			planeVisual.transform.localScale = new Vector3(planes[i].Width, planes[i].Height, 1f);

			_planeFlags[i] = planes[i].Flags;
		}

		RefreshAllPlaneMaterials();
	}
	#endregion

	#region Private Functions
	/// <summary>
	/// Refresh all the plane materials
	/// </summary>
	private void RefreshAllPlaneMaterials()
	{
		for (int i = 0; i < _planeCache.Count; ++i)
		{
			if (!_planeCache[i].activeSelf)
			{
				continue;
			}

			Renderer planeRenderer = _planeCache[i].GetComponent<Renderer>();
		}
	}
	#endregion
}
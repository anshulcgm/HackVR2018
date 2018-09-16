using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class EyeTracking : MonoBehaviour {
    #region Public Variables
    public GameObject Camera;
    public Material FocusedMaterial, NonFocusedMaterial;
    #endregion

    #region Private Variables 
    private Vector3 _heading;
    #endregion

    #region Unity Methods
    private void Start()
    {
        MLEyes.Start();
        transform.position = Camera.transform.position + Camera.transform.forward * 2.0f;
    }

    private void OnDisable()
    {
        MLEyes.Stop();
    }

    private void Update()
    {
        if (MLEyes.IsStarted)
        {
            RaycastHit hit;
            _heading = MLEyes.FixationPoint - Camera.transform.position;

            //Get the MeshRenderer Component 
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            //Get the Assigned Material 
            Material material = meshRenderer.sharedMaterial;

            if(Physics.Raycast(Camera.transform.position, _heading, out hit, 10.0f))
            {
                meshRenderer.material = FocusedMaterial;
            }
            else
            {
                meshRenderer.material = NonFocusedMaterial;
            }
        }
    }
    #endregion
}

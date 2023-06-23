using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARManager : MonoBehaviour
{
    [SerializeField]
    private GameObject anchorPrefab;

    ARAnchorManager arAnchorManager;

    List<Vector3> anchorList = new List<Vector3>();

    [SerializeField]
    TextMeshProUGUI text;

    GameObject arCamera;

    GameObject planeObject;
    Mesh mesh;
    MeshFilter meshFilter;


    // Start is called before the first frame update
    void Start()
    {
        arAnchorManager = GetComponent<ARAnchorManager>();
        arCamera = gameObject.transform.GetChild(0).gameObject;
        planeObject = new GameObject("Plane");
        meshFilter = planeObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = planeObject.AddComponent<MeshRenderer>();
        mesh = new Mesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && anchorList.Count < 4)
        {
            Debug.Log("click");
            GameObject anchorObject = Instantiate(anchorPrefab, arCamera.transform.position + arCamera.transform.forward , Quaternion.identity);
            anchorObject.AddComponent<ARAnchor>();
            anchorList.Add(anchorObject.transform.position);
            /*Vector3 anchorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0,  Camera.main.nearClipPlane));
            Debug.Log("pos");
            GameObject anchorObject = Instantiate(anchorPrefab, anchorPos, Quaternion.identity);
            Debug.Log("instance");
            anchorObject.AddComponent<ARAnchor>();
            Debug.Log("anchor");
            
            Debug.Log("add");*/
            if (anchorList.Count == 4)
            {
                Instantiate(planeObject, gameObject.transform.position, Quaternion.identity);
            }
        }
        

        if(anchorList.Count == 4)
        {
            
            mesh.vertices = anchorList.ToArray();
            mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
        }
    }
}

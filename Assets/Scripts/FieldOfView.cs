using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public Mesh mesh;

    public float totalFov = 90f;
    public  int numberOfFovSegments = 50;
    Vector3 fovOrigin = Vector3.zero;
    float currentFovAngle = 0f;
    float fovAngleIncrease = 0f;
    public float fovDistance = 5f;
    Vector3[] fovVertices;
    Vector2[] fovUvs;
    int[] fovTriangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        fovAngleIncrease = totalFov / numberOfFovSegments;

        fovVertices = new Vector3[numberOfFovSegments + 2];
        fovUvs = new Vector2[fovVertices.Length];
        fovTriangles = new int[numberOfFovSegments * 3];

        fovVertices[0] = fovOrigin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= numberOfFovSegments; i++)
        {
            Vector3 currentVertex;
            RaycastHit2D currentRaycastHit = Physics2D.Raycast(fovOrigin, GameController.GetPositionFromAngle(currentFovAngle + (totalFov / 2f)), fovDistance);
            if (currentRaycastHit.collider == null)   // Raycast doesn't hit.
            {
                //Debug.Log("No hit.");
                currentVertex = fovOrigin + GameController.GetPositionFromAngle(currentFovAngle + (totalFov / 2)) * fovDistance;  // +(total/2) centers the fov.
            }
            else    // Raycast hits, put vertex point where the raycast hits.
            {
                //Debug.Log("Hit.");
                currentVertex = currentRaycastHit.point;
            }

            //currentVertex = fovOrigin + GameController.GetPositionFromAngle(currentFovAngle + (totalFov / 2)) * fovDistance;  // +(total/2) centers the fov.

            fovVertices[vertexIndex] = currentVertex;

            if (i > 0)
            {
                // Work in groups of three.
                fovTriangles[triangleIndex] = 0;    // Start on fovOrigin.
                fovTriangles[triangleIndex + 1] = vertexIndex - 1;  // Link to previous vertex.
                fovTriangles[triangleIndex + 2] = vertexIndex;  // Link to current vertex
                triangleIndex += 3;
            }

            vertexIndex++;

            currentFovAngle -= fovAngleIncrease;

            mesh.vertices = fovVertices;
            mesh.uv = fovUvs;
            mesh.triangles = fovTriangles;
        }
    }
}

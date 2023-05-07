using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CreatePlane : MonoBehaviour
{
    public Vector2Int resolution;
    public Texture2D heightMap;
    public float polygonSize;
    public float elevationFactor;
    private Mesh mesh;
    private Vector3[] vertices;

    private void Awake()
    {
        if(heightMap == null)
        {
            heightMap = new Texture2D(1, 1);
        }

        Generate();
    }

    private void Generate()
    {
        vertices = new Vector3[(resolution.x + 1) * (resolution.y + 1)];

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Grid";

        float Ratiox = (float)heightMap.width / (resolution.x);
        float Ratioy = (float)heightMap.height / (resolution.y);

        for (int i = 0, y = 0; y <= resolution.y; y++)
        {
            for (int x = 0; x <= resolution.x; x++, i++)
            {
                Color color = heightMap.GetPixel((int)(Ratiox * x), (int)(Ratioy * y));
                vertices[i] = new Vector3(((x * polygonSize)), ((y * polygonSize)), color.r * elevationFactor);
            }
        }

        mesh.vertices = vertices;

        int[] triangles = new int[resolution.x * resolution.y * 6];

        int triangleIndex = 0;
        int vertexIndex = 0;

        for (int y = 0; y < resolution.y; y++, vertexIndex++)
        {
            for (int x = 0; x < resolution.x; x++, triangleIndex += 6, vertexIndex++)
            {
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + resolution.x + 1;
                triangles[triangleIndex + 2] = vertexIndex + 1;
                triangles[triangleIndex + 5] = vertexIndex + resolution.x + 2;
                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + resolution.x + 1;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}

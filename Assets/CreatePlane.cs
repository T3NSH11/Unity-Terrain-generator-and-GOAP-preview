using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlane : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    public Vector2Int Resolution;
    public Texture2D HeightMap;
    public float PolygonSize;
    public float ElevationFactor;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Grid";

        float Ratiox = (float)HeightMap.width / ((float)Resolution.x + 1);
        float Ratioy = (float)HeightMap.height / ((float)Resolution.y + 1);


        vertices = new Vector3[(Resolution.x + 1) * (Resolution.y + 1)];
        //Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0, y = 0; y <= Resolution.y; y++)
        {
            for (int x = 0; x <= Resolution.x; x++, i++)
            {
                Color color = HeightMap.GetPixel((int)Ratiox * x, (int)Ratioy * y);
                vertices[i] = new Vector3(((x * PolygonSize) + x), ((y * PolygonSize) + y), color.r * ElevationFactor);
                //uv[i] = new Vector3(((x * PolygonSize) + x) / Resolution.x, ((y * PolygonSize) + y) / Resolution.y);
            }
        }
        mesh.vertices = vertices;

        int[] triangles = new int[Resolution.x * Resolution.y * 6];
        for (int TIndex = 0, VIndex = 0, y = 0; y < Resolution.y; y++, VIndex++)
        {
            for (int x = 0; x < Resolution.x; x++, TIndex += 6, VIndex++)
            {
                triangles[TIndex] = VIndex;
                triangles[TIndex + 1] = VIndex + Resolution.x + 1;
                triangles[TIndex + 2] = VIndex + 1;
                triangles[TIndex + 3] = VIndex + 1;
                triangles[TIndex + 4] = VIndex + Resolution.x + 1;
                triangles[TIndex + 5] = VIndex + Resolution.x + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}

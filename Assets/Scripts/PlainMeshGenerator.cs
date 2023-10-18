using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlainMeshGenerator : MonoBehaviour
{
    [SerializeField] private MeshFilter _target;

    [SerializeField] private int _сonsistency;

    private Mesh _mesh;

    private void Reset() {
        if (!_target) _target = GetComponent<MeshFilter>();
    }

    [ContextMenu("Regenerate")]
    private void GenerateMesh()
    {
        _mesh = new Mesh();
        _mesh.name = "Plane";

        var widthSegments = _сonsistency;
        var lengthSegments = _сonsistency;

        int numVertices = (widthSegments + 1) * (lengthSegments + 1);
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];

        int[] triangles = new int[widthSegments * lengthSegments * 6];

        for (int i = 0, y = 0; y <= lengthSegments; y++)
        {
            for (int x = 0; x <= widthSegments; x++, i++)
            {
                vertices[i] = new Vector3((float)x / widthSegments, 0, (float)y / lengthSegments);
                uv[i] = new Vector2((float)x / widthSegments, (float)y / lengthSegments);
            }
        }

        for (int ti = 0, vi = 0, y = 0; y < lengthSegments; y++, vi++)
        {
            for (int x = 0; x < widthSegments; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + widthSegments + 1;
                triangles[ti + 5] = vi + widthSegments + 2;
            }
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        _target.mesh = _mesh;
    }
}
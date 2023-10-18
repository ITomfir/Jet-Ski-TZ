using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMeshUpdater : MonoBehaviour {
    [SerializeField] private List<MeshFilter> _meshFilters;

    private void Update() {
        foreach (var meshFilters in _meshFilters) {
            var mesh = meshFilters.mesh;
            Vector3[] vectors = mesh.vertices;

            for (int i = 0; i < vectors.Length; i++) {
                var vertexPosition = meshFilters.transform.position + (vectors[i] * meshFilters.transform.localScale.z);
                vectors[i].y = Sea.instance.GetWaveHeight(vertexPosition);
            }

            mesh.vertices = vectors;
            mesh.RecalculateNormals();
        }
    }
}

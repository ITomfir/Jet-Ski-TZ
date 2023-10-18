using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterFootprint : MonoBehaviour {
    [SerializeField] private float _speed;
    [SerializeField] private int _size;
    [SerializeField] private Transform _targetCenter;
    [SerializeField] private Transform _trailStart;
    [SerializeField] private AnimationCurve _trailPower;
    [SerializeField] private AnimationCurve _trailOffses;

    private List<Vector3> _trail = new List<Vector3>();
    private Dictionary<int, Vector3[]> _trailPositionMap = new Dictionary<int, Vector3[]>();
    
    private MeshFilter _meshFilter;

    private void Awake() {
        _meshFilter = GetComponent<MeshFilter>();
        
        for (int i = 0; i < _size; i++) {
            var point = new Vector3(0, 0, transform.lossyScale.z / _size * i / transform.lossyScale.z);
            
            _trail.Add(point);

            var pointsMap = new List<Vector3>();

            for (int t = 0; t < _size; t++) {
                pointsMap.Add(point + Vector3.right * CalculateXByVertexIndex(t)); 
            }

            _trailPositionMap.Add(i, pointsMap.ToArray());
        }
    } 

    private void Update () {
        Vector3[] points = _trail.ToArray();

        for (int i = 0; i < points.Length; i++) {
            var targetPosition = -(_targetCenter.position - _trailStart.position).x * _trailOffses.Evaluate(points[i].z);
            points[i].x = Mathf.Lerp(points[i].x, targetPosition, _trailPower.Evaluate(points[i].z) * _speed * Time.deltaTime);

            var pointsPosMap = _trailPositionMap[i];

            for (int vi = 0; vi < pointsPosMap.Length; vi++) {
                pointsPosMap[vi].x = points[i].x + CalculateXByVertexIndex(vi);
            }

            _trailPositionMap[i] = pointsPosMap;
        }

        var vertices = _meshFilter.mesh.vertices;
        foreach (var item in _trailPositionMap) {
            for (int i = 0; i < _size; i++) {
                vertices[i + _size * item.Key] = item.Value[i];
            }
        }

        _meshFilter.mesh.vertices = vertices;

        _trail = new List<Vector3>();
        _trail.AddRange(points);
    }

    private float CalculateXByVertexIndex (int index) {
        return (transform.lossyScale.x / _size * index - transform.lossyScale.x / 2) / transform.lossyScale.x;
    }
}

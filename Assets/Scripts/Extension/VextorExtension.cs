using System.Linq;
using UnityEngine;

public static class VextorExtension {
    public static Vector2 ToV2XZ (this Vector3 vector) {
        return new Vector2 (vector.x, vector.z);
    }

    public static Vector3 ToV3XZ (this Vector2 vector) {
        return new Vector3 (vector.x, 0, vector.y);
    }
}
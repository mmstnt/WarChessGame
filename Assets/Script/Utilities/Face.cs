using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Face
{
    public List<Vector3> vertices { get; private set; }
    public List<Vector2> uvs { get; private set; }
    public List<int> triangles { get; private set; }

    public Face(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles)
    {
        this.vertices = vertices;
        this.uvs = uvs;
        this.triangles = triangles;
    }
}

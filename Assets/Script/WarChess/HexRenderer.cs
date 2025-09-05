using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class HexRenderer : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;

    private List<Face> faces;

    public Material material;
    public float innerSize;
    public float outerSize;
    public float height;
    public bool isFlatTopped;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();

        mesh = new Mesh();
        mesh.name = "Hex";

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshRenderer.material = material;
    }

    public void OnEnable()
    {
        drawMesh();    
    }

    public void OnValidate()
    {
        if (Application.isPlaying) 
        {
            drawMesh();
        }
    }

    public void setMaterial(Material material) 
    {
        meshRenderer.material = material;
    }

    public void setColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void drawMesh() 
    {
        drawFaces();
        combineFaces();
    }

    private void drawFaces() 
    {
        faces = new List<Face>();

        for(int point = 0; point < 6; point++) 
        {
            faces.Add(createFace(innerSize, outerSize, height / 2f, height / 2f, point));
        }

        for(int point = 0; point < 6; point++) 
        {
            faces.Add(createFace(innerSize, outerSize, -height / 2f, -height / 2f, point, true));
        }

        for(int point = 0; point < 6; point++) 
        {
            faces.Add(createFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
        }

        for (int point = 0; point < 6; point++)
        {
            faces.Add(createFace(innerSize, innerSize, height / 2f, -height / 2f, point, false));
        }
    }

    private void combineFaces() 
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>(); 

        for (int i = 0; i < faces.Count; i++) 
        {
            vertices.AddRange(faces[i].vertices);
            uvs.AddRange(faces[i].uvs);

            int offset = (4 * i);
            foreach (int triangle in faces[i].triangles) 
            {
                tris.Add(triangle + offset);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
    }

    private Face createFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false) 
    {
        Vector3 pointA = getPoint(innerRad, heightB, point);
        Vector3 pointB = getPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = getPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = getPoint(outerRad, heightA, point);

        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        List<int> triangles = new List<int> { 0, 1, 2, 2, 3, 0 };
        if (reverse) 
        {
            vertices.Reverse();
        }

        return new Face(vertices, uvs, triangles);
    }

    protected Vector3 getPoint(float size,float height,int index) 
    {
        float angleDeg = isFlatTopped ? 60 * index : 60 * index - 30;
        float angleRad = Mathf.PI / 180f * angleDeg;
        return new Vector3(size * Mathf.Cos(angleRad), height, size * Mathf.Sin(angleRad));
    }
}

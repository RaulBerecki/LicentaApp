using UnityEngine;

public class AI_Testing : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateWhitePyramid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateWhitePyramid()
    {
        GameObject pyramid = new GameObject("WhitePyramid");
        MeshFilter meshFilter = pyramid.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = pyramid.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[]
        {
        new Vector3(0, 1, 0),     // top
        new Vector3(-1, 0, -1),   // base front-left
        new Vector3(1, 0, -1),    // base front-right
        new Vector3(1, 0, 1),     // base back-right
        new Vector3(-1, 0, 1)     // base back-left
        };

        int[] triangles = new int[]
        {
        0, 1, 2, // front face
        0, 2, 3, // right face
        0, 3, 4, // back face
        0, 4, 1, // left face
        1, 4, 3, // base triangle 1
        1, 3, 2  // base triangle 2
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        Material material = new Material(Shader.Find("Unlit/Color"));
        material.color = Color.white;
        meshRenderer.material = material;
    }


}

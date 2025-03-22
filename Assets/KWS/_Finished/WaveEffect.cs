using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField] private Vector3[] vertices;
    public float waveSpeed = 1f;
    public float waveHeight = 0.5f;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    private void Update()
    {
        if (mesh == null) return;

        for (int i = 0; i < vertices.Length; i++)
        {
            float wave = Mathf.Sin(Time.time * waveSpeed * vertices[i].x + vertices[i].z) * waveHeight;
            vertices[i].y = wave;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}

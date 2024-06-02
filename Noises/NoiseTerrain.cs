using UnityEngine;

namespace Noises
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public abstract class NoiseTerrain : MonoBehaviour
    {
        public int size = 256;

        protected abstract float GenerateNoise(float x, float z); 
    
        private void Awake()
        {
            var meshFilter = GetComponent<MeshFilter>();

            meshFilter.mesh = GenerateMesh();
        }

        private Mesh GenerateMesh()
        {
            var mesh = new Mesh();

            var vertices = new Vector3[(size + 1) * (size + 1)];
            var triangles = new int[size * size * 6];
            var uvs = new Vector2[(size + 1) * (size + 1)];

            var vertexIndex = 0;
            var triangleIndex = 0;

            for (var z = 0; z <= size; z++)
            {
                for (var x = 0; x <= size; x++)
                {
                    var posY = GenerateNoise(x, z);

                    vertices[vertexIndex] = new Vector3(x, posY, z);
                    uvs[vertexIndex] = new Vector2(x / (float)size, z / (float)size);

                    if (x != size && z != size)
                    {
                        triangles[triangleIndex] = vertexIndex;
                        triangles[triangleIndex+1] = vertexIndex + size + 1;
                        triangles[triangleIndex+2] = vertexIndex + 1;
                        triangles[triangleIndex+3] = vertexIndex + 1;
                        triangles[triangleIndex+4] = vertexIndex + size + 1;
                        triangles[triangleIndex+5] = vertexIndex + size + 2;

                        triangleIndex += 6;
                    }

                    vertexIndex++;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
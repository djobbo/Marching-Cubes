using UnityEngine;
using System.Collections.Generic;

public class MarchingMeshGenerator : MonoBehaviour
{
    public MarchingChunkGenerator chunkGen;

    public Material m_material;
    public float maxHeight = 16f;
    public float groundLevel = 8f;
    public HeightMap[] heightMaps;

    public TerrainDecorationProperties[] decoration;
    public List<TerrainDecoration> decorationObjects;

    public int seed = 0;

    List<GameObject> meshes = new List<GameObject>();

    //Neighbours
    public MarchingMeshGenerator topChunk, bottomChunk, leftChunk, rightChunk, forwardChunk, backChunk;

    public Vector3Int position;

    //The size of voxel array.
    public int width = 8;
    public int height = 8;
    public int length = 8;

    public float cubeSize = 1f;

    float[] voxels;
    bool meshModifiedThisFrame = false;

    public AnimationCurve heightCurve;

    public void Setup(MarchingChunkGenerator _chunkGen, Vector3Int _pos, int _chunkSize, float _cubeSize)
    {
        chunkGen = _chunkGen;
        position = _pos;

        width = height = length = _chunkSize + 1;
        cubeSize = _cubeSize;

        transform.name = "Chunk " + position;

        FillMap();
        GenerateMesh();
    }

    public void GenerateMesh()
    {        
        //Set the mode used to create the mesh.
        //Cubes is faster and creates less verts, tetrahedrons is slower and creates more verts but better represents the mesh surface.
        Marching marching = new MarchingCubes();

        //Surface is the value that represents the surface of mesh
        //For example the perlin noise has a range of -1 to 1 so the mid point is where we want the surface to cut through.
        //The target value does not have to be the mid point it can be any value with in the range.
        marching.Surface = -.25f;

        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();

        //The mesh produced is not optimal. There is one vert for each index.
        //Would need to weld vertices for better quality mesh.
        marching.Generate(voxels, width, height, length, verts, indices);

        //A mesh in unity can only be made up of 65000 verts.
        //Need to split the verts between multiple meshes.

        int maxVertsPerMesh = 21000; //must be divisible by 3, ie 3 verts == 1 triangle
        int numMeshes = verts.Count / maxVertsPerMesh + 1;

        for (int i = 0; i < numMeshes; i++)
        {

            List<Vector3> splitVerts = new List<Vector3>();
            List<int> splitIndices = new List<int>();

            for (int j = 0; j < maxVertsPerMesh; j++)
            {
                int idx = i * maxVertsPerMesh + j;

                if (idx < verts.Count)
                {
                    splitVerts.Add(verts[idx]);
                    splitIndices.Add(j);
                }
            }

            if (splitVerts.Count == 0) continue;

            GameObject go;

            if (i < meshes.Count) {
                go = meshes[i];
                
            }
            else {
                go = new GameObject("Mesh");
                go.transform.parent = transform;
                go.layer = gameObject.layer;
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                go.AddComponent<MeshCollider>();
                meshes.Add(go);
            }

            Mesh mesh = new Mesh();
            mesh.SetVertices(splitVerts);
            mesh.SetTriangles(splitIndices, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            go.GetComponent<Renderer>().material = m_material;
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.GetComponent<MeshCollider>().sharedMesh = mesh;
            go.transform.localPosition = new Vector3(-width / 2, -height / 2, -length / 2);
            go.transform.tag = "Chunk";
            go.transform.localScale = Vector3.one;
        }

        for (int i = numMeshes; i < meshes.Count; i++)
        {
            meshes[i].SetActive(false);
        }

        for (int i = 0; i < decorationObjects.Count; i++) {
            var decObj = decorationObjects[i];
            if (decObj != null)
                decObj.Stick();
        }

    }

    void FillMap() {
        INoise perlin = new PerlinNoise(seed, 2.0f);
        FractalNoise fractal = new FractalNoise(perlin, 3, 2.0f);

        voxels = new float[width * height * length];

        //Fill voxels with values. Im using perlin noise but any method to create voxels will work.
        for (int y = 0; y < height; y++)
        {
            var posY = y + position.y * (width - 1.0f);

            for (int x = 0; x < width; x++)
            {
                var posX = x + position.x * (width - 1.0f);
                for (int z = 0; z < length; z++)
                {
                    var posZ = z + position.z * (width - 1.0f);

                    float fx = (x + position.x * (width - 1.0f)) / (width - 1.0f) / 24f;
                    float fy = (y + position.y * (width - 1.0f)) / (height - 1.0f) / 16f;
                    float fz = (z + position.z * (width - 1.0f)) / (length - 1.0f) / 24f;

                    //var perl = fractal.Sample3D(fx, fy, fz);
                    
                    int idx = x + y * width + z * width * height;

                    float density = 0f;

                    for (int i = 0; i < heightMaps.Length; i++)
                    {
                        var heightMap = heightMaps[i].map;
                        var scale = heightMaps[i].scale;
                        density += (heightMap.GetPixel(
                            Mathf.RoundToInt(posX * scale) % heightMap.width,
                            Mathf.RoundToInt(posZ * scale) % heightMap.height
                            ).r * 2 - 1) * heightMaps[i].opacity;
                    }

                    density = Mathf.Clamp(-posY / maxHeight * (1f + density), posY == 0 ? -.2f : -1f, 1f);
                    voxels[idx] = density;

                    /*foreach (var decoProperties in decoration) {
                        if ((y + position.y * width) >= decoProperties.minHeight && (y + position.y * width) <= decoProperties.maxHeight
                            && density >= decoProperties.minDensity && density <= decoProperties.maxDensity && Random.Range(0f, 1f) <= decoProperties.probability) {
                            var objPos = (new Vector3(x, y, z) + position * width) * cubeSize - Vector3.one * width/2f * cubeSize;
                            var decoObj = Instantiate(decoProperties.prefab, objPos, Quaternion.Euler(0, Random.Range(0f, 360f), 0), transform);
                            decoObj.Setup(decoProperties.stickDirection);
                            decorationObjects.Add(decoObj);
                        }
                    }*/
                }
            }
        }
    }

    void LateUpdate() {
        meshModifiedThisFrame = false;
    }

    public void Deform(Vector3 point, float brushGravity, float brushRadius, bool dig, AnimationCurve deformCurve = null)
    {
        if (meshModifiedThisFrame) return;
        meshModifiedThisFrame = true;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < length; z++)
                {
                    var pos = new Vector3(x, y, z) + new Vector3(-width / 2, -height / 2, -length / 2) + position * (width - 1);
                    pos *= cubeSize;
                    var dist = Vector3.Distance(point, pos);
                    if (dist <= brushRadius)
                    {
                        var idx = x + y * width + z * width * height;
                        //voxels[idx] = Mathf.Clamp(voxels[idx] + (dig ? -1 : 1) * brushGravity * (deformCurve != null ? deformCurve.Evaluate(dist / brushGravity) : 1) * Time.deltaTime, -1f, 1f);
                        voxels[idx] = Mathf.Lerp(voxels[idx], (dig ? ((y + position.y * width) == 0 ? -.2f : -1f) : 1f), brushGravity * Time.deltaTime * (deformCurve != null ? deformCurve.Evaluate(dist / brushRadius) : 1));
                        
                        if (x == 0 && leftChunk)
                            leftChunk.Deform(point, brushGravity, brushRadius, dig, deformCurve);
                        else if (x == (width - 1) && rightChunk) {
                            rightChunk.Deform(point, brushGravity, brushRadius, dig, deformCurve);
                        }
                        if (y == 0 && bottomChunk)
                            bottomChunk.Deform(point, brushGravity, brushRadius, dig, deformCurve);
                        else if (y == (width - 1) && topChunk)
                            topChunk.Deform(point, brushGravity, brushRadius, dig, deformCurve);
                        if (z == 0 && backChunk)
                            backChunk.Deform(point, brushGravity, brushRadius, dig, deformCurve);
                        else if (z == (width - 1) && forwardChunk)
                            forwardChunk.Deform(point, brushGravity, brushRadius, dig, deformCurve);
                    }
                } 
            }
        }

        GenerateMesh();
    }

    public void SetNeighbours(
        MarchingMeshGenerator _topChunk,
        MarchingMeshGenerator _bottomChunk,
        MarchingMeshGenerator _leftChunk,
        MarchingMeshGenerator _rightChunk,
        MarchingMeshGenerator _forwardChunk,
        MarchingMeshGenerator _backChunk)
    {
        topChunk = _topChunk;
        bottomChunk = _bottomChunk;
        leftChunk = _leftChunk;
        rightChunk = _rightChunk;
        forwardChunk = _forwardChunk;
        backChunk = _backChunk;
    }

    public float PerlinNoise3D(float x, float y, float z)
    {
        float XY = Mathf.PerlinNoise(x, y);
        float YZ = Mathf.PerlinNoise(y, z);
        float XZ = Mathf.PerlinNoise(x, z);

        float YX = Mathf.PerlinNoise(y, x);
        float ZY = Mathf.PerlinNoise(z, y);
        float ZX = Mathf.PerlinNoise(z, x);

        return (XY + YZ + XZ + YX + ZY + ZX) / 6f;
    }

}

[System.Serializable]
public class HeightMap {
    public Texture2D map;
    public float scale;
    public float opacity;
}

[System.Serializable]
public class TerrainDecorationProperties {

    public TerrainDecoration prefab;

    public Vector3 stickDirection;

    [Range(-1f, 1f)]
    public float minDensity;
    [Range(-1f, 1f)]
    public float maxDensity;
    [Range(0f, 1f)]
    public float probability;

    public float minHeight;
    public float maxHeight;
}
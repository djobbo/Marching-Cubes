using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class Chunk : MonoBehaviour {

    public ChunkGenerator chunkGen;
    public Chunk topChunk, bottomChunk, leftChunk, rightChunk, forwardChunk, backChunk;

    public MeshGenerator meshGen;
    public Vector3Int position;
    public ChunkVertice[,,] map;

    public int chunkSize;
    public float cubeSize;

    public Mesh mesh;
    public MeshFilter mf;
    public MeshCollider mc;
    
    public List<Vector3Int> verticesStartPosition;

    bool meshModifiedThisFrame = false;

    public ChunkColorPalette palette;

    public void Setup(ChunkGenerator _chunkGen, Vector3Int _pos, int _chunkSize, float _cubeSize)
    {

        chunkGen = _chunkGen;
        position = _pos;

        chunkSize = _chunkSize;
        cubeSize = _cubeSize;

        transform.name = "Chunk " + position;
        map = new ChunkVertice[chunkSize + 1, chunkSize + 1, chunkSize + 1];

        meshGen.chunkGen = chunkGen;

        mesh = new Mesh();
        mf.mesh = mesh;
        mc.sharedMesh = mesh;
    }

    private void LateUpdate()
    {
        meshModifiedThisFrame = false;
    }

    public void GenerateMesh()
    {
        meshGen.GenerateMesh(map, palette.colors);
        UpdateCollider();
    }

    public void UpdateCollider()
    {
        mc.sharedMesh = mesh;
    }

    public void SetNeighbours(Chunk _topChunk, Chunk _bottomChunk, Chunk _leftChunk, Chunk _rightChunk, Chunk _forwardChunk, Chunk _backChunk)
    {
        topChunk = _topChunk;
        bottomChunk = _bottomChunk;
        leftChunk = _leftChunk;
        rightChunk = _rightChunk;
        forwardChunk = _forwardChunk;
        backChunk = _backChunk;
    }

    public void Deform(Vector3 point, Vector3 normal, float brushGravity, float brushDensity, float brushRadius, bool dig)
    {
        if (meshModifiedThisFrame) return;
        meshModifiedThisFrame = true;

        for (int x = 0; x < chunkSize + 1; x++)
        {
            for (int y = 0; y < chunkSize + 1; y++)
            {
                for (int z = 0; z < chunkSize + 1; z++)
                {
                    var pos = (map[x, y, z].startPosition + position * chunkSize) * cubeSize;
                    var dist = Vector3.Distance(point, pos);
                    if (dist <= brushRadius)
                    {
                        //map[x, y, z].startPosition += -normal * (dig ? 1 : -1) * brushForce * Time.deltaTime * Mathf.InverseLerp(brushRadius, 0, dist);
                        var interpolation = Mathf.Pow(Mathf.InverseLerp(brushRadius, 0, dist), 2);

                        var vertice = map[x, y, z];
                        var density = vertice.density;
                        var deltaDist = Vector3.Distance(vertice.position, vertice.startPosition) * cubeSize;

                        if (/*(density > 1 && deltaDist > 0.8f) || (density < 0 && deltaDist > 0.8f)*/deltaDist > 0.8f)
                        {
                            map[x, y, z].activated = (dig ? 0 : 1);
                            //map[x, y, z].density = 0.5f;
                            map[x, y, z].position = vertice.startPosition;
                        }
                        else
                        {
                            //map[x, y, z].density = Mathf.Clamp(density + (dig ? -1 : 1) * brushDensity * interpolation * Time.deltaTime, 0.01f, 0.99f);
                            map[x, y, z].position += (dig ? 1 : -1) * normal * brushGravity * interpolation * Time.deltaTime;
                        }

                        //var deltaDist = Vector3.Distance(map[x, y, z].position, map[x, y, z].startPosition);
                        //bool reset = map[x, y, z].density >= 10f;

                        //if (reset)
                        //{
                        //    if (dig)
                        //    {
                        //        if (map[x, y, z].activated == 1)
                        //        {
                        //            map[x, y, z].activated = 0;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (map[x, y, z].activated == 0)
                        //        {
                        //            map[x, y, z].activated = 1;
                        //        }
                        //    }
                        //    map[x, y, z].density = 1f;
                        //}

                        if (x == 0 && leftChunk)
                            leftChunk.Deform(point, normal, brushGravity, brushDensity, brushRadius, dig);
                        else if (x == chunkSize && rightChunk)
                            rightChunk.Deform(point, normal, brushGravity, brushDensity, brushRadius, dig);
                        if (y == 0 && bottomChunk)
                            bottomChunk.Deform(point, normal, brushGravity, brushDensity, brushRadius, dig);
                        else if (y == chunkSize && topChunk)
                            topChunk.Deform(point, normal, brushGravity, brushDensity, brushRadius, dig);
                        if (z == 0 && backChunk)
                            backChunk.Deform(point, normal, brushGravity, brushDensity, brushRadius, dig);
                        else if (z == chunkSize && forwardChunk)
                            forwardChunk.Deform(point, normal, brushGravity, brushDensity, brushRadius, dig);

                    }
                }
            }
        }

        GenerateMesh();
    }

}

public class ChunkVertice {
    public int activated;
    public Vector3 startPosition;
    public Vector3 position;
    public int colorID = 0;
    public float density;

    public Vector3 worldPosition;

    public ChunkVertice(int _activated, Vector3 _startPos, float _density, int _colorID)
    {
        activated = _activated;
        startPosition = _startPos;
        position = _startPos;
        colorID = _colorID;
        density = _density;
    }
}
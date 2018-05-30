using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingChunkGenerator : MonoBehaviour {

    public static MarchingChunkGenerator singleton;

    public float worldRadius = 32f;
    public Vector3Int mapSize = new Vector3Int(4, 2, 4);
    public Vector3Int mapOffset = new Vector3Int(-2, -1, -2);
    public int chunkSize = 4;
    public float cubeSize = 0.5f;

    Dictionary<Vector3Int, MarchingMeshGenerator> chunks;
    public MarchingMeshGenerator chunkPrefab;

    public GravityBody rock;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        chunks = new Dictionary<Vector3Int, MarchingMeshGenerator>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    var pos = new Vector3Int(x, y, z) + mapOffset;
                    var chunk = Instantiate(chunkPrefab, cubeSize * (Vector3)pos * chunkSize, Quaternion.identity, transform);
                    chunk.transform.localScale = Vector3.one * cubeSize;

                    chunks.Add(pos, chunk);

                    chunk.Setup(this, pos, chunkSize, cubeSize);
                }
            }
        }

        for (int x = mapSize.x - 1; x >= 0; x--)
        {
            for (int y = mapSize.y - 1; y >= 0; y--)
            {
                for (int z = mapSize.z - 1; z >= 0; z--)
                {
                    var pos = new Vector3Int(x, y, z) + mapOffset;
                    Vector3Int topPos = pos + Vector3Int.up,
                                btmPos = pos + Vector3Int.down,
                                lftPos = pos + Vector3Int.left,
                                rgtPos = pos + Vector3Int.right,
                                frdPos = pos + new Vector3Int(0, 0, 1),
                                bckPos = pos + new Vector3Int(0, 0, -1);

                    MarchingMeshGenerator topChunk = chunks.ContainsKey(topPos) ? chunks[topPos] : null,
                        bottomChunk = chunks.ContainsKey(btmPos) ? chunks[btmPos] : null,
                        leftChunk = chunks.ContainsKey(lftPos) ? chunks[lftPos] : null,
                        rightChunk = chunks.ContainsKey(rgtPos) ? chunks[rgtPos] : null,
                        forwardChunk = chunks.ContainsKey(frdPos) ? chunks[frdPos] : null,
                        backChunk = chunks.ContainsKey(bckPos) ? chunks[bckPos] : null;

                    chunks[pos].SetNeighbours(topChunk, bottomChunk, leftChunk, rightChunk, forwardChunk, backChunk);
                    chunks[pos].GenerateMesh();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour {

    public static ChunkGenerator singleton;

    public float worldRadius = 32f;
    public Vector3Int mapSize = new Vector3Int(4, 2, 4);
    public Vector3Int mapOffset = new Vector3Int(-2, -1, -2);
    public int chunkSize = 4;
    public float cubeSize = 0.5f;

    Dictionary<Vector3Int, Chunk> chunks;
    public Chunk chunkPrefab;

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
        chunks = new Dictionary<Vector3Int, Chunk>();
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

                    FillMap(chunk);
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

                    Chunk topChunk = chunks.ContainsKey(topPos) ? chunks[topPos] : null,
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

    void FillMap(Chunk chunk)
    {
        var map = chunk.map;
        var worldPos = (Vector3)chunk.position * chunkSize;

        for (int x = 0; x < chunkSize+1; x++)
        {
            for (int y = 0; y < chunkSize+1; y++)
            {
                for (int z = 0; z < chunkSize+1; z++)
                {
                    var currentPos = new Vector3(x, y, z);

                    var currentWorldPos = currentPos + worldPos;
                    currentWorldPos = currentWorldPos.normalized * Mathf.Floor(currentWorldPos.magnitude); //Cleaning mesh error :(

                    //map[x, y, z] = y <= Mathf.PerlinNoise(x/50f, z/50f)*chunkSize ? 1 : 0;
                    var perl = PerlinNoise3D((currentWorldPos.x) / 8f + 0.8f, (currentWorldPos.y) / 16f + 0.2f, (currentWorldPos.z) / 12f + 0.4f);
                    var mag = currentWorldPos.magnitude;
                    var color = mag >= 14f ? Random.Range(3, 5) : mag >= 12f ? Random.Range(1, 5) : mag >= 8 ? Random.Range(1, 3) : 0;

                    var fill = (/*mag <= worldRadius && perl >= 0.4f && */Mathf.PerlinNoise((x + chunk.position.x * chunkSize) / 12f, (z + chunk.position.z * chunkSize) / 12f) * 12f  + 12f >= y + chunk.position.y * chunkSize);
                    var density = Mathf.Abs(2 * perl - 1);
                    map[x, y, z] = new ChunkVertice(fill ? 1 : 0, currentPos, .5f, color);

                    //if (!fill && mag <= worldRadius + 1 && Random.Range(0, 250) == 0)
                    //{
                    //    Instantiate(rock, currentWorldPos * cubeSize, Quaternion.identity, chunk.transform).planet = GetComponent<GravityAttractor>();
                    //}
                }
            }
        }
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

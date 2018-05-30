using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Chunk))]
public class MeshGenerator : MonoBehaviour {

    public ChunkGenerator chunkGen;

    public Chunk chunk;
    public CubeGrid cubeGrid;

    List<Vector3> vertices;
    List<int>[] triangles;

    Vector3[] flatShadedVertices;
    int[] flatShadedTriangles;

    public bool cubeGridGenerated = false;

    public int colorCount;

    public void GenerateMesh(ChunkVertice[,,] map, List<Color> _colors)
    {
        if (vertices == null)
            vertices = new List<Vector3>();
        else vertices.Clear();

        //if (triangles == null)
        colorCount = _colors.Count;
        triangles = new List<int>[colorCount];
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = new List<int>();
        }
        //else triangles.Clear();

        if (chunk.verticesStartPosition == null)
            chunk.verticesStartPosition = new List<Vector3Int>();
        else chunk.verticesStartPosition.Clear();

        if (!cubeGridGenerated)
            cubeGrid = new CubeGrid(chunk, map);

        for (int x = 0; x < cubeGrid.cubes.GetLength(0); x++)
        {
            for (int y = 0; y < cubeGrid.cubes.GetLength(1); y++)
            {
                for (int z = 0; z < cubeGrid.cubes.GetLength(2); z++)
                {
                    TriangulateCube(cubeGrid.cubes[x, y, z]);
                }
            }
        }

        chunk.mesh.Clear();
        chunk.mesh.SetVertices(vertices);
        chunk.mesh.subMeshCount = triangles.Length;
        for (int i = 0; i < triangles.Length; i++)
        {
            chunk.mesh.SetTriangles(triangles[i], i);
        }
        
        //FlatShading();
        chunk.mesh.RecalculateBounds();
        chunk.mesh.RecalculateNormals();
    }

    void TriangulateCube(Cube cube)
    {
        switch (cube.configuration)
        {
            default:
                break;

            case 0:
                break;

            case 1:
                CreateTriangle(cube.e0, cube.e8, cube.e3);
                break;

            case 2:
                CreateTriangle(cube.e0, cube.e1, cube.e9);
                break;

            case 3:
                CreateTriangle(cube.e1, cube.e8, cube.e3);
                CreateTriangle(cube.e9, cube.e8, cube.e1);
                break;

            case 4:
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                break;

            case 5:
                CreateTriangle(cube.e0, cube.e8, cube.e3);
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                break;

            case 6:
                CreateTriangle(cube.e9, cube.e2, cube.e10);
                CreateTriangle(cube.e0, cube.e2, cube.e9);
                break;

            case 7:
                CreateTriangle(cube.e2, cube.e8, cube.e3);
                CreateTriangle(cube.e2, cube.e10, cube.e8);
                CreateTriangle(cube.e10, cube.e9, cube.e8);
                break;

            case 8:
                CreateTriangle(cube.e3, cube.e11, cube.e2);
                break;

            case 9:
                CreateTriangle(cube.e0, cube.e11, cube.e2);
                CreateTriangle(cube.e8, cube.e11, cube.e0);
                break;

            case 10:
                CreateTriangle(cube.e1, cube.e9, cube.e0);
                CreateTriangle(cube.e2, cube.e3, cube.e11);
                break;

            case 11:
                CreateTriangle(cube.e1, cube.e11, cube.e2);
                CreateTriangle(cube.e1, cube.e9, cube.e11);
                CreateTriangle(cube.e9, cube.e8, cube.e11);
                break;

            case 12:
                CreateTriangle(cube.e3, cube.e10, cube.e1);
                CreateTriangle(cube.e11, cube.e10, cube.e3);
                break;

            case 13:
                CreateTriangle(cube.e0, cube.e10, cube.e1);
                CreateTriangle(cube.e0, cube.e8, cube.e10);
                CreateTriangle(cube.e8, cube.e11, cube.e10);
                break;

            case 14:
                CreateTriangle(cube.e3, cube.e9, cube.e0);
                CreateTriangle(cube.e3, cube.e11, cube.e9);
                CreateTriangle(cube.e11, cube.e10, cube.e9);
                break;

            case 15:
                CreateTriangle(cube.e9, cube.e8, cube.e10);
                CreateTriangle(cube.e10, cube.e8, cube.e11);
                break;

            case 16:
                CreateTriangle(cube.e4, cube.e7, cube.e8);
                break;

            case 17:
                CreateTriangle(cube.e4, cube.e3, cube.e0);
                CreateTriangle(cube.e7, cube.e3, cube.e4);
                break;

            case 18:
                CreateTriangle(cube.e0, cube.e1, cube.e9);
                CreateTriangle(cube.e8, cube.e4, cube.e7);
                break;

            case 19:
                CreateTriangle(cube.e4, cube.e1, cube.e9);
                CreateTriangle(cube.e4, cube.e7, cube.e1);
                CreateTriangle(cube.e7, cube.e3, cube.e1);
                break;

            case 20:
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                CreateTriangle(cube.e8, cube.e4, cube.e7);
                break;

            case 21:
                CreateTriangle(cube.e3, cube.e4, cube.e7);
                CreateTriangle(cube.e3, cube.e0, cube.e4);
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                break;

            case 22:
                CreateTriangle(cube.e9, cube.e2, cube.e10);
                CreateTriangle(cube.e9, cube.e0, cube.e2);
                CreateTriangle(cube.e8, cube.e4, cube.e7);
                break;

            case 23:
                CreateTriangle(cube.e2, cube.e10, cube.e9);
                CreateTriangle(cube.e2, cube.e9, cube.e7);
                CreateTriangle(cube.e2, cube.e7, cube.e3);
                CreateTriangle(cube.e7, cube.e9, cube.e4);
                break;

            case 24:
                CreateTriangle(cube.e8, cube.e4, cube.e7);
                CreateTriangle(cube.e3, cube.e11, cube.e2);
                break;

            case 25:
                CreateTriangle(cube.e11, cube.e4, cube.e7);
                CreateTriangle(cube.e11, cube.e2, cube.e4);
                CreateTriangle(cube.e2, cube.e0, cube.e4);
                break;

            case 26:
                CreateTriangle(cube.e9, cube.e0, cube.e1);
                CreateTriangle(cube.e8, cube.e4, cube.e7);
                CreateTriangle(cube.e2, cube.e3, cube.e11);
                break;

            case 27:
                CreateTriangle(cube.e4, cube.e7, cube.e11);
                CreateTriangle(cube.e9, cube.e4, cube.e11);
                CreateTriangle(cube.e9, cube.e11, cube.e2);
                CreateTriangle(cube.e9, cube.e2, cube.e1);
                break;

            case 28:
                CreateTriangle(cube.e3, cube.e10, cube.e1);
                CreateTriangle(cube.e3, cube.e11, cube.e10);
                CreateTriangle(cube.e7, cube.e8, cube.e4);
                break;

            case 29:
                CreateTriangle(cube.e1, cube.e11, cube.e10);
                CreateTriangle(cube.e1, cube.e4, cube.e11);
                CreateTriangle(cube.e1, cube.e0, cube.e4);
                CreateTriangle(cube.e7, cube.e11, cube.e4);
                break;

            case 30:
                CreateTriangle(cube.e4, cube.e7, cube.e8);
                CreateTriangle(cube.e9, cube.e0, cube.e11);
                CreateTriangle(cube.e9, cube.e11, cube.e10);
                CreateTriangle(cube.e11, cube.e0, cube.e3);
                break;

            case 31:
                CreateTriangle(cube.e4, cube.e7, cube.e11);
                CreateTriangle(cube.e4, cube.e11, cube.e9);
                CreateTriangle(cube.e9, cube.e11, cube.e10);
                break;

            case 32:
                CreateTriangle(cube.e9, cube.e5, cube.e4);
                break;

            case 33:
                CreateTriangle(cube.e9, cube.e5, cube.e4);
                CreateTriangle(cube.e0, cube.e8, cube.e3);
                break;

            case 34:
                CreateTriangle(cube.e0, cube.e5, cube.e4);
                CreateTriangle(cube.e1, cube.e5, cube.e0);
                break;

            case 35:
                CreateTriangle(cube.e8, cube.e5, cube.e4);
                CreateTriangle(cube.e8, cube.e3, cube.e5);
                CreateTriangle(cube.e3, cube.e1, cube.e5);
                break;

            case 36:
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                CreateTriangle(cube.e9, cube.e5, cube.e4);
                break;

            case 37:
                CreateTriangle(cube.e3, cube.e0, cube.e8);
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                CreateTriangle(cube.e4, cube.e9, cube.e5);
                break;

            case 38:
                CreateTriangle(cube.e5, cube.e2, cube.e10);
                CreateTriangle(cube.e5, cube.e4, cube.e2);
                CreateTriangle(cube.e4, cube.e0, cube.e2);
                break;

            case 39:
                CreateTriangle(cube.e2, cube.e10, cube.e5);
                CreateTriangle(cube.e3, cube.e2, cube.e5);
                CreateTriangle(cube.e3, cube.e5, cube.e4);
                CreateTriangle(cube.e3, cube.e4, cube.e8);
                break;

            case 40:
                CreateTriangle(cube.e9, cube.e5, cube.e4);
                CreateTriangle(cube.e2, cube.e3, cube.e11);
                break;

            case 41:
                CreateTriangle(cube.e0, cube.e11, cube.e2);
                CreateTriangle(cube.e0, cube.e8, cube.e11);
                CreateTriangle(cube.e4, cube.e9, cube.e5);
                break;

            case 42:
                CreateTriangle(cube.e0, cube.e5, cube.e4);
                CreateTriangle(cube.e0, cube.e1, cube.e5);
                CreateTriangle(cube.e2, cube.e3, cube.e11);
                break;

            case 43:
                CreateTriangle(cube.e2, cube.e1, cube.e5);
                CreateTriangle(cube.e2, cube.e5, cube.e8);
                CreateTriangle(cube.e2, cube.e8, cube.e11);
                CreateTriangle(cube.e4, cube.e8, cube.e5);
                break;

            case 44:
                CreateTriangle(cube.e10, cube.e3, cube.e11);
                CreateTriangle(cube.e10, cube.e1, cube.e3);
                CreateTriangle(cube.e9, cube.e5, cube.e4);
                break;

            case 45:
                CreateTriangle(cube.e4, cube.e9, cube.e5);
                CreateTriangle(cube.e0, cube.e8, cube.e1);
                CreateTriangle(cube.e8, cube.e10, cube.e1);
                CreateTriangle(cube.e8, cube.e11, cube.e10);
                break;

            case 46:
                CreateTriangle(cube.e5, cube.e4, cube.e0);
                CreateTriangle(cube.e5, cube.e0, cube.e11);
                CreateTriangle(cube.e5, cube.e11, cube.e10);
                CreateTriangle(cube.e11, cube.e0, cube.e3);
                break;

            case 47:
                CreateTriangle(cube.e5, cube.e4, cube.e8);
                CreateTriangle(cube.e5, cube.e8, cube.e10);
                CreateTriangle(cube.e10, cube.e8, cube.e11);
                break;

            case 48:
                CreateTriangle(cube.e9, cube.e7, cube.e8);
                CreateTriangle(cube.e5, cube.e7, cube.e9);
                break;

            case 49:
                CreateTriangle(cube.e9, cube.e3, cube.e0);
                CreateTriangle(cube.e9, cube.e5, cube.e3);
                CreateTriangle(cube.e5, cube.e7, cube.e3);
                break;

            case 50:
                CreateTriangle(cube.e0, cube.e7, cube.e8);
                CreateTriangle(cube.e0, cube.e1, cube.e7);
                CreateTriangle(cube.e1, cube.e5, cube.e7);
                break;

            case 51:
                CreateTriangle(cube.e1, cube.e5, cube.e3);
                CreateTriangle(cube.e3, cube.e5, cube.e7);
                break;

            case 52:
                CreateTriangle(cube.e9, cube.e7, cube.e8);
                CreateTriangle(cube.e9, cube.e5, cube.e7);
                CreateTriangle(cube.e10, cube.e1, cube.e2);
                break;

            case 53:
                CreateTriangle(cube.e10, cube.e1, cube.e2);
                CreateTriangle(cube.e9, cube.e5, cube.e0);
                CreateTriangle(cube.e5, cube.e3, cube.e0);
                CreateTriangle(cube.e5, cube.e7, cube.e3);
                break;

            case 54:
                CreateTriangle(cube.e8, cube.e0, cube.e2);
                CreateTriangle(cube.e8, cube.e2, cube.e5);
                CreateTriangle(cube.e8, cube.e5, cube.e7);
                CreateTriangle(cube.e10, cube.e5, cube.e2);
                break;

            case 55:
                CreateTriangle(cube.e2, cube.e10, cube.e5);
                CreateTriangle(cube.e2, cube.e5, cube.e3);
                CreateTriangle(cube.e3, cube.e5, cube.e7);
                break;

            case 56:
                CreateTriangle(cube.e7, cube.e9, cube.e5);
                CreateTriangle(cube.e7, cube.e8, cube.e9);
                CreateTriangle(cube.e3, cube.e11, cube.e2);
                break;

            case 57:
                CreateTriangle(cube.e9, cube.e5, cube.e7);
                CreateTriangle(cube.e9, cube.e7, cube.e2);
                CreateTriangle(cube.e9, cube.e2, cube.e0);
                CreateTriangle(cube.e2, cube.e7, cube.e11);
                break;

            case 58:
                CreateTriangle(cube.e2, cube.e3, cube.e11);
                CreateTriangle(cube.e0, cube.e1, cube.e8);
                CreateTriangle(cube.e1, cube.e7, cube.e8);
                CreateTriangle(cube.e1, cube.e5, cube.e7);
                break;

            case 59:
                CreateTriangle(cube.e11, cube.e2, cube.e1);
                CreateTriangle(cube.e11, cube.e1, cube.e7);
                CreateTriangle(cube.e7, cube.e1, cube.e5);
                break;

            case 60:
                CreateTriangle(cube.e9, cube.e5, cube.e8);
                CreateTriangle(cube.e8, cube.e5, cube.e7);
                CreateTriangle(cube.e10, cube.e1, cube.e3);
                CreateTriangle(cube.e10, cube.e3, cube.e11);
                break;

            case 61:
                CreateTriangle(cube.e5, cube.e7, cube.e0);
                CreateTriangle(cube.e5, cube.e0, cube.e9);
                CreateTriangle(cube.e7, cube.e11, cube.e0);
                CreateTriangle(cube.e1, cube.e0, cube.e10);
                break;

            case 62:
                CreateTriangle(cube.e11, cube.e10, cube.e0);
                CreateTriangle(cube.e11, cube.e0, cube.e3);
                CreateTriangle(cube.e10, cube.e5, cube.e0);
                CreateTriangle(cube.e8, cube.e0, cube.e7);
                break;

            case 63:
                CreateTriangle(cube.e11, cube.e10, cube.e5);
                CreateTriangle(cube.e7, cube.e11, cube.e5);
                break;

            case 64:
                CreateTriangle(cube.e10, cube.e6, cube.e5);
                break;

            case 65:
                CreateTriangle(cube.e0, cube.e8, cube.e3);
                CreateTriangle(cube.e5, cube.e10, cube.e6);
                break;

            case 66:
                CreateTriangle(cube.e9, cube.e0, cube.e1);
                CreateTriangle(cube.e5, cube.e10, cube.e6);
                break;

            case 67:
                CreateTriangle(cube.e1, cube.e8, cube.e3);
                CreateTriangle(cube.e1, cube.e9, cube.e8);
                CreateTriangle(cube.e5, cube.e10, cube.e6);
                break;

            case 68:
                CreateTriangle(cube.e1, cube.e6, cube.e5);
                CreateTriangle(cube.e2, cube.e6, cube.e1);
                break;

            case 69:
                CreateTriangle(cube.e1, cube.e6, cube.e5);
                CreateTriangle(cube.e1, cube.e2, cube.e6);
                CreateTriangle(cube.e3, cube.e0, cube.e8);
                break;

            case 70:
                CreateTriangle(cube.e9, cube.e6, cube.e5);
                CreateTriangle(cube.e9, cube.e0, cube.e6);
                CreateTriangle(cube.e0, cube.e2, cube.e6);
                break;

            case 71:
                CreateTriangle(cube.e5, cube.e9, cube.e8);
                CreateTriangle(cube.e5, cube.e8, cube.e2);
                CreateTriangle(cube.e5, cube.e2, cube.e6);
                CreateTriangle(cube.e3, cube.e2, cube.e8);
                break;

            case 72:
                CreateTriangle(cube.e2, cube.e3, cube.e11);
                CreateTriangle(cube.e10, cube.e6, cube.e5);
                break;

            case 73:
                CreateTriangle(cube.e11, cube.e0, cube.e8);
                CreateTriangle(cube.e11, cube.e2, cube.e0);
                CreateTriangle(cube.e10, cube.e6, cube.e5);
                break;

            case 74:
                CreateTriangle(cube.e0, cube.e1, cube.e9);
                CreateTriangle(cube.e2, cube.e3, cube.e11);
                CreateTriangle(cube.e5, cube.e10, cube.e6);
                break;

            case 75:
                CreateTriangle(cube.e5, cube.e10, cube.e6);
                CreateTriangle(cube.e1, cube.e9, cube.e2);
                CreateTriangle(cube.e9, cube.e11, cube.e2);
                CreateTriangle(cube.e9, cube.e8, cube.e11);
                break;

            case 76:
                CreateTriangle(cube.e6, cube.e3, cube.e11);
                CreateTriangle(cube.e6, cube.e5, cube.e3);
                CreateTriangle(cube.e5, cube.e1, cube.e3);
                break;

            case 77:
                CreateTriangle(cube.e0, cube.e8, cube.e11);
                CreateTriangle(cube.e0, cube.e11, cube.e5);
                CreateTriangle(cube.e0, cube.e5, cube.e1);
                CreateTriangle(cube.e5, cube.e11, cube.e6);
                break;

            case 78:
                CreateTriangle(cube.e3, cube.e11, cube.e6);
                CreateTriangle(cube.e0, cube.e3, cube.e6);
                CreateTriangle(cube.e0, cube.e6, cube.e5);
                CreateTriangle(cube.e0, cube.e5, cube.e9);
                break;

            case 79:
                CreateTriangle(cube.e6, cube.e5, cube.e9);
                CreateTriangle(cube.e6, cube.e9, cube.e11);
                CreateTriangle(cube.e11, cube.e9, cube.e8);
                break;

            case 80:
                CreateTriangle(cube.e5, cube.e10, cube.e6);
                CreateTriangle(cube.e4, cube.e7, cube.e8);
                break;

            case 81:
                CreateTriangle(cube.e4, cube.e3, cube.e0);
                CreateTriangle(cube.e4, cube.e7, cube.e3);
                CreateTriangle(cube.e6, cube.e5, cube.e10);
                break;

            case 82:
                CreateTriangle(cube.e1, cube.e9, cube.e0);
                CreateTriangle(cube.e5, cube.e10, cube.e6);
                CreateTriangle(cube.e8, cube.e4, cube.e7);
                break;

            case 83:
                CreateTriangle(cube.e10, cube.e6, cube.e5);
                CreateTriangle(cube.e1, cube.e9, cube.e7);
                CreateTriangle(cube.e1, cube.e7, cube.e3);
                CreateTriangle(cube.e7, cube.e9, cube.e4);
                break;

            case 84:
                CreateTriangle(cube.e6, cube.e1, cube.e2);
                CreateTriangle(cube.e6, cube.e5, cube.e1);
                CreateTriangle(cube.e4, cube.e7, cube.e8);
                break;

            case 85:
                CreateTriangle(cube.e1, cube.e2, cube.e5);
                CreateTriangle(cube.e5, cube.e2, cube.e6);
                CreateTriangle(cube.e3, cube.e0, cube.e4);
                CreateTriangle(cube.e3, cube.e4, cube.e7);
                break;

            case 86:
                CreateTriangle(cube.e8, cube.e4, cube.e7);
                CreateTriangle(cube.e9, cube.e0, cube.e5);
                CreateTriangle(cube.e0, cube.e6, cube.e5);
                CreateTriangle(cube.e0, cube.e2, cube.e6);
                break;

            case 87:
                CreateTriangle(cube.e7, cube.e3, cube.e9);
                CreateTriangle(cube.e7, cube.e9, cube.e4);
                CreateTriangle(cube.e3, cube.e2, cube.e9);
                CreateTriangle(cube.e5, cube.e9, cube.e6);
                break;

            case 88:
                CreateTriangle(cube.e3, cube.e11, cube.e2);
                CreateTriangle(cube.e7, cube.e8, cube.e4);
                CreateTriangle(cube.e10, cube.e6, cube.e5);
                break;

            case 89:
                CreateTriangle(cube.e5, cube.e10, cube.e6);
                CreateTriangle(cube.e4, cube.e7, cube.e2);
                CreateTriangle(cube.e4, cube.e2, cube.e0);
                CreateTriangle(cube.e2, cube.e7, cube.e11);
                break;

            case 90:
                CreateTriangle(cube.e0, cube.e1, cube.e9);
                CreateTriangle(cube.e4, cube.e7, cube.e8);
                CreateTriangle(cube.e2, cube.e3, cube.e11);
                CreateTriangle(cube.e5, cube.e10, cube.e6);
                break;

            case 91:
                CreateTriangle(cube.e9, cube.e2, cube.e1);
                CreateTriangle(cube.e9, cube.e11, cube.e2);
                CreateTriangle(cube.e9, cube.e4, cube.e11);
                CreateTriangle(cube.e7, cube.e11, cube.e4);
                break;

            case 92:
                CreateTriangle(cube.e8, cube.e4, cube.e7);
                CreateTriangle(cube.e3, cube.e11, cube.e5);
                CreateTriangle(cube.e3, cube.e5, cube.e1);
                CreateTriangle(cube.e5, cube.e11, cube.e6);
                break;

            case 93:
                CreateTriangle(cube.e5, cube.e1, cube.e11);
                CreateTriangle(cube.e5, cube.e11, cube.e6);
                CreateTriangle(cube.e1, cube.e0, cube.e11);
                CreateTriangle(cube.e7, cube.e11, cube.e4);
                break;

            case 94:
                CreateTriangle(cube.e0, cube.e5, cube.e9);
                CreateTriangle(cube.e0, cube.e6, cube.e5);
                CreateTriangle(cube.e0, cube.e3, cube.e6);
                CreateTriangle(cube.e11, cube.e6, cube.e3);
                break;

            case 95:
                CreateTriangle(cube.e6, cube.e5, cube.e9);
                CreateTriangle(cube.e6, cube.e9, cube.e11);
                CreateTriangle(cube.e4, cube.e7, cube.e9);
                CreateTriangle(cube.e7, cube.e11, cube.e9);
                break;

            case 96:
                CreateTriangle(cube.e10, cube.e4, cube.e9);
                CreateTriangle(cube.e6, cube.e4, cube.e10);
                break;

            case 97:
                CreateTriangle(cube.e4, cube.e10, cube.e6);
                CreateTriangle(cube.e4, cube.e9, cube.e10);
                CreateTriangle(cube.e0, cube.e8, cube.e3);
                break;

            case 98:
                CreateTriangle(cube.e10, cube.e0, cube.e1);
                CreateTriangle(cube.e10, cube.e6, cube.e0);
                CreateTriangle(cube.e6, cube.e4, cube.e0);
                break;

            case 99:
                CreateTriangle(cube.e8, cube.e3, cube.e1);
                CreateTriangle(cube.e8, cube.e1, cube.e6);
                CreateTriangle(cube.e8, cube.e6, cube.e4);
                CreateTriangle(cube.e6, cube.e1, cube.e10);
                break;

            case 100:
                CreateTriangle(cube.e1, cube.e4, cube.e9);
                CreateTriangle(cube.e1, cube.e2, cube.e4);
                CreateTriangle(cube.e2, cube.e6, cube.e4);
                break;

            case 101:
                CreateTriangle(cube.e3, cube.e0, cube.e8);
                CreateTriangle(cube.e1, cube.e2, cube.e9);
                CreateTriangle(cube.e2, cube.e4, cube.e9);
                CreateTriangle(cube.e2, cube.e6, cube.e4);
                break;

            case 102:
                CreateTriangle(cube.e0, cube.e2, cube.e4);
                CreateTriangle(cube.e4, cube.e2, cube.e6);
                break;

            case 103:
                CreateTriangle(cube.e8, cube.e3, cube.e2);
                CreateTriangle(cube.e8, cube.e2, cube.e4);
                CreateTriangle(cube.e4, cube.e2, cube.e6);
                break;

            case 104:
                CreateTriangle(cube.e10, cube.e4, cube.e9);
                CreateTriangle(cube.e10, cube.e6, cube.e4);
                CreateTriangle(cube.e11, cube.e2, cube.e3);
                break;

            case 105:
                CreateTriangle(cube.e0, cube.e8, cube.e2);
                CreateTriangle(cube.e2, cube.e8, cube.e11);
                CreateTriangle(cube.e4, cube.e9, cube.e10);
                CreateTriangle(cube.e4, cube.e10, cube.e6);
                break;

            case 106:
                CreateTriangle(cube.e3, cube.e11, cube.e2);
                CreateTriangle(cube.e0, cube.e1, cube.e6);
                CreateTriangle(cube.e0, cube.e6, cube.e4);
                CreateTriangle(cube.e6, cube.e1, cube.e10);
                break;

            case 107:
                CreateTriangle(cube.e6, cube.e4, cube.e1);
                CreateTriangle(cube.e6, cube.e1, cube.e10);
                CreateTriangle(cube.e4, cube.e8, cube.e1);
                CreateTriangle(cube.e2, cube.e1, cube.e11);
                break;

            case 108:
                CreateTriangle(cube.e9, cube.e6, cube.e4);
                CreateTriangle(cube.e9, cube.e3, cube.e6);
                CreateTriangle(cube.e9, cube.e1, cube.e3);
                CreateTriangle(cube.e11, cube.e6, cube.e3);
                break;

            case 109:
                CreateTriangle(cube.e8, cube.e11, cube.e1);
                CreateTriangle(cube.e8, cube.e1, cube.e0);
                CreateTriangle(cube.e11, cube.e6, cube.e1);
                CreateTriangle(cube.e9, cube.e1, cube.e4);
                break;

            case 110:
                CreateTriangle(cube.e3, cube.e11, cube.e6);
                CreateTriangle(cube.e3, cube.e6, cube.e0);
                CreateTriangle(cube.e0, cube.e6, cube.e4);
                break;

            case 111:
                CreateTriangle(cube.e6, cube.e4, cube.e8);
                CreateTriangle(cube.e11, cube.e6, cube.e8);
                break;

            case 112:
                CreateTriangle(cube.e7, cube.e10, cube.e6);
                CreateTriangle(cube.e7, cube.e8, cube.e10);
                CreateTriangle(cube.e8, cube.e9, cube.e10);
                break;

            case 113:
                CreateTriangle(cube.e0, cube.e7, cube.e3);
                CreateTriangle(cube.e0, cube.e10, cube.e7);
                CreateTriangle(cube.e0, cube.e9, cube.e10);
                CreateTriangle(cube.e6, cube.e7, cube.e10);
                break;

            case 114:
                CreateTriangle(cube.e10, cube.e6, cube.e7);
                CreateTriangle(cube.e1, cube.e10, cube.e7);
                CreateTriangle(cube.e1, cube.e7, cube.e8);
                CreateTriangle(cube.e1, cube.e8, cube.e0);
                break;

            case 115:
                CreateTriangle(cube.e10, cube.e6, cube.e7);
                CreateTriangle(cube.e10, cube.e7, cube.e1);
                CreateTriangle(cube.e1, cube.e7, cube.e3);
                break;

            case 116:
                CreateTriangle(cube.e1, cube.e2, cube.e6);
                CreateTriangle(cube.e1, cube.e6, cube.e8);
                CreateTriangle(cube.e1, cube.e8, cube.e9);
                CreateTriangle(cube.e8, cube.e6, cube.e7);
                break;

            case 117:
                CreateTriangle(cube.e2, cube.e6, cube.e9);
                CreateTriangle(cube.e2, cube.e9, cube.e1);
                CreateTriangle(cube.e6, cube.e7, cube.e9);
                CreateTriangle(cube.e0, cube.e9, cube.e3);
                break;

            case 118:
                CreateTriangle(cube.e7, cube.e8, cube.e0);
                CreateTriangle(cube.e7, cube.e0, cube.e6);
                CreateTriangle(cube.e6, cube.e0, cube.e2);
                break;

            case 119:
                CreateTriangle(cube.e7, cube.e3, cube.e2);
                CreateTriangle(cube.e6, cube.e7, cube.e2);
                break;

            case 120:
                CreateTriangle(cube.e2, cube.e3, cube.e11);
                CreateTriangle(cube.e10, cube.e6, cube.e8);
                CreateTriangle(cube.e10, cube.e8, cube.e9);
                CreateTriangle(cube.e8, cube.e6, cube.e7);
                break;

            case 121:
                CreateTriangle(cube.e2, cube.e0, cube.e7);
                CreateTriangle(cube.e2, cube.e7, cube.e11);
                CreateTriangle(cube.e0, cube.e9, cube.e7);
                CreateTriangle(cube.e6, cube.e7, cube.e10);
                break;

            case 122:
                CreateTriangle(cube.e1, cube.e8, cube.e0);
                CreateTriangle(cube.e1, cube.e7, cube.e8);
                CreateTriangle(cube.e1, cube.e10, cube.e7);
                CreateTriangle(cube.e6, cube.e7, cube.e10);
                break;

            case 123:
                CreateTriangle(cube.e11, cube.e2, cube.e1);
                CreateTriangle(cube.e11, cube.e1, cube.e7);
                CreateTriangle(cube.e10, cube.e6, cube.e1);
                CreateTriangle(cube.e6, cube.e7, cube.e1);
                break;

            case 124:
                CreateTriangle(cube.e8, cube.e9, cube.e6);
                CreateTriangle(cube.e8, cube.e6, cube.e7);
                CreateTriangle(cube.e9, cube.e1, cube.e6);
                CreateTriangle(cube.e11, cube.e6, cube.e3);
                break;

            case 125:
                CreateTriangle(cube.e0, cube.e9, cube.e1);
                CreateTriangle(cube.e11, cube.e6, cube.e7);
                break;

            case 126:
                CreateTriangle(cube.e7, cube.e8, cube.e0);
                CreateTriangle(cube.e7, cube.e0, cube.e6);
                CreateTriangle(cube.e3, cube.e11, cube.e0);
                CreateTriangle(cube.e11, cube.e6, cube.e0);
                break;

            case 127:
                CreateTriangle(cube.e7, cube.e11, cube.e6);
                break;

            case 128:
                CreateTriangle(cube.e7, cube.e6, cube.e11);
                break;

            case 129:
                CreateTriangle(cube.e3, cube.e0, cube.e8);
                CreateTriangle(cube.e11, cube.e7, cube.e6);
                break;

            case 130:
                CreateTriangle(cube.e0, cube.e1, cube.e9);
                CreateTriangle(cube.e11, cube.e7, cube.e6);
                break;

            case 131:
                CreateTriangle(cube.e8, cube.e1, cube.e9);
                CreateTriangle(cube.e8, cube.e3, cube.e1);
                CreateTriangle(cube.e11, cube.e7, cube.e6);
                break;

            case 132:
                CreateTriangle(cube.e10, cube.e1, cube.e2);
                CreateTriangle(cube.e6, cube.e11, cube.e7);
                break;

            case 133:
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                CreateTriangle(cube.e3, cube.e0, cube.e8);
                CreateTriangle(cube.e6, cube.e11, cube.e7);
                break;

            case 134:
                CreateTriangle(cube.e2, cube.e9, cube.e0);
                CreateTriangle(cube.e2, cube.e10, cube.e9);
                CreateTriangle(cube.e6, cube.e11, cube.e7);
                break;

            case 135:
                CreateTriangle(cube.e6, cube.e11, cube.e7);
                CreateTriangle(cube.e2, cube.e10, cube.e3);
                CreateTriangle(cube.e10, cube.e8, cube.e3);
                CreateTriangle(cube.e10, cube.e9, cube.e8);
                break;

            case 136:
                CreateTriangle(cube.e7, cube.e2, cube.e3);
                CreateTriangle(cube.e6, cube.e2, cube.e7);
                break;

            case 137:
                CreateTriangle(cube.e7, cube.e0, cube.e8);
                CreateTriangle(cube.e7, cube.e6, cube.e0);
                CreateTriangle(cube.e6, cube.e2, cube.e0);
                break;

            case 138:
                CreateTriangle(cube.e2, cube.e7, cube.e6);
                CreateTriangle(cube.e2, cube.e3, cube.e7);
                CreateTriangle(cube.e0, cube.e1, cube.e9);
                break;

            case 139:
                CreateTriangle(cube.e1, cube.e6, cube.e2);
                CreateTriangle(cube.e1, cube.e8, cube.e6);
                CreateTriangle(cube.e1, cube.e9, cube.e8);
                CreateTriangle(cube.e8, cube.e7, cube.e6);
                break;

            case 140:
                CreateTriangle(cube.e10, cube.e7, cube.e6);
                CreateTriangle(cube.e10, cube.e1, cube.e7);
                CreateTriangle(cube.e1, cube.e3, cube.e7);
                break;

            case 141:
                CreateTriangle(cube.e10, cube.e7, cube.e6);
                CreateTriangle(cube.e1, cube.e7, cube.e10);
                CreateTriangle(cube.e1, cube.e8, cube.e7);
                CreateTriangle(cube.e1, cube.e0, cube.e8);
                break;

            case 142:
                CreateTriangle(cube.e0, cube.e3, cube.e7);
                CreateTriangle(cube.e0, cube.e7, cube.e10);
                CreateTriangle(cube.e0, cube.e10, cube.e9);
                CreateTriangle(cube.e6, cube.e10, cube.e7);
                break;

            case 143:
                CreateTriangle(cube.e7, cube.e6, cube.e10);
                CreateTriangle(cube.e7, cube.e10, cube.e8);
                CreateTriangle(cube.e8, cube.e10, cube.e9);
                break;

            case 144:
                CreateTriangle(cube.e6, cube.e8, cube.e4);
                CreateTriangle(cube.e11, cube.e8, cube.e6);
                break;

            case 145:
                CreateTriangle(cube.e3, cube.e6, cube.e11);
                CreateTriangle(cube.e3, cube.e0, cube.e6);
                CreateTriangle(cube.e0, cube.e4, cube.e6);
                break;

            case 146:
                CreateTriangle(cube.e8, cube.e6, cube.e11);
                CreateTriangle(cube.e8, cube.e4, cube.e6);
                CreateTriangle(cube.e9, cube.e0, cube.e1);
                break;

            case 147:
                CreateTriangle(cube.e9, cube.e4, cube.e6);
                CreateTriangle(cube.e9, cube.e6, cube.e3);
                CreateTriangle(cube.e9, cube.e3, cube.e1);
                CreateTriangle(cube.e11, cube.e3, cube.e6);
                break;

            case 148:
                CreateTriangle(cube.e6, cube.e8, cube.e4);
                CreateTriangle(cube.e6, cube.e11, cube.e8);
                CreateTriangle(cube.e2, cube.e10, cube.e1);
                break;

            case 149:
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                CreateTriangle(cube.e3, cube.e0, cube.e11);
                CreateTriangle(cube.e0, cube.e6, cube.e11);
                CreateTriangle(cube.e0, cube.e4, cube.e6);
                break;

            case 150:
                CreateTriangle(cube.e4, cube.e11, cube.e8);
                CreateTriangle(cube.e4, cube.e6, cube.e11);
                CreateTriangle(cube.e0, cube.e2, cube.e9);
                CreateTriangle(cube.e2, cube.e10, cube.e9);
                break;

            case 151:
                CreateTriangle(cube.e10, cube.e9, cube.e3);
                CreateTriangle(cube.e10, cube.e3, cube.e2);
                CreateTriangle(cube.e9, cube.e4, cube.e3);
                CreateTriangle(cube.e11, cube.e3, cube.e6);
                break;

            case 152:
                CreateTriangle(cube.e8, cube.e2, cube.e3);
                CreateTriangle(cube.e8, cube.e4, cube.e2);
                CreateTriangle(cube.e4, cube.e6, cube.e2);
                break;

            case 153:
                CreateTriangle(cube.e0, cube.e4, cube.e2);
                CreateTriangle(cube.e4, cube.e6, cube.e2);
                break;

            case 154:
                CreateTriangle(cube.e1, cube.e9, cube.e0);
                CreateTriangle(cube.e2, cube.e3, cube.e4);
                CreateTriangle(cube.e2, cube.e4, cube.e6);
                CreateTriangle(cube.e4, cube.e3, cube.e8);
                break;

            case 155:
                CreateTriangle(cube.e1, cube.e9, cube.e4);
                CreateTriangle(cube.e1, cube.e4, cube.e2);
                CreateTriangle(cube.e2, cube.e4, cube.e6);
                break;

            case 156:
                CreateTriangle(cube.e8, cube.e1, cube.e3);
                CreateTriangle(cube.e8, cube.e6, cube.e1);
                CreateTriangle(cube.e8, cube.e4, cube.e6);
                CreateTriangle(cube.e6, cube.e10, cube.e1);
                break;

            case 157:
                CreateTriangle(cube.e10, cube.e1, cube.e0);
                CreateTriangle(cube.e10, cube.e0, cube.e6);
                CreateTriangle(cube.e6, cube.e0, cube.e4);
                break;

            case 158:
                CreateTriangle(cube.e4, cube.e6, cube.e3);
                CreateTriangle(cube.e4, cube.e3, cube.e8);
                CreateTriangle(cube.e6, cube.e10, cube.e3);
                CreateTriangle(cube.e0, cube.e3, cube.e9);
                break;

            case 159:
                CreateTriangle(cube.e10, cube.e9, cube.e4);
                CreateTriangle(cube.e6, cube.e10, cube.e4);
                break;

            case 160:
                CreateTriangle(cube.e4, cube.e9, cube.e5);
                CreateTriangle(cube.e7, cube.e6, cube.e11);
                break;

            case 161:
                CreateTriangle(cube.e0, cube.e8, cube.e3);
                CreateTriangle(cube.e4, cube.e9, cube.e5);
                CreateTriangle(cube.e11, cube.e7, cube.e6);
                break;

            case 162:
                CreateTriangle(cube.e5, cube.e0, cube.e1);
                CreateTriangle(cube.e5, cube.e4, cube.e0);
                CreateTriangle(cube.e7, cube.e6, cube.e11);
                break;

            case 163:
                CreateTriangle(cube.e11, cube.e7, cube.e6);
                CreateTriangle(cube.e8, cube.e3, cube.e4);
                CreateTriangle(cube.e3, cube.e5, cube.e4);
                CreateTriangle(cube.e3, cube.e1, cube.e5);
                break;

            case 164:
                CreateTriangle(cube.e9, cube.e5, cube.e4);
                CreateTriangle(cube.e10, cube.e1, cube.e2);
                CreateTriangle(cube.e7, cube.e6, cube.e11);
                break;

            case 165:
                CreateTriangle(cube.e6, cube.e11, cube.e7);
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                CreateTriangle(cube.e0, cube.e8, cube.e3);
                CreateTriangle(cube.e4, cube.e9, cube.e5);
                break;

            case 166:
                CreateTriangle(cube.e7, cube.e6, cube.e11);
                CreateTriangle(cube.e5, cube.e4, cube.e10);
                CreateTriangle(cube.e4, cube.e2, cube.e10);
                CreateTriangle(cube.e4, cube.e0, cube.e2);
                break;

            case 167:
                CreateTriangle(cube.e3, cube.e4, cube.e8);
                CreateTriangle(cube.e3, cube.e5, cube.e4);
                CreateTriangle(cube.e3, cube.e2, cube.e5);
                CreateTriangle(cube.e10, cube.e5, cube.e2);
                break;

            case 168:
                CreateTriangle(cube.e7, cube.e2, cube.e3);
                CreateTriangle(cube.e7, cube.e6, cube.e2);
                CreateTriangle(cube.e5, cube.e4, cube.e9);
                break;

            case 169:
                CreateTriangle(cube.e9, cube.e5, cube.e4);
                CreateTriangle(cube.e0, cube.e8, cube.e6);
                CreateTriangle(cube.e0, cube.e6, cube.e2);
                CreateTriangle(cube.e6, cube.e8, cube.e7);
                break;

            case 170:
                CreateTriangle(cube.e3, cube.e6, cube.e2);
                CreateTriangle(cube.e3, cube.e7, cube.e6);
                CreateTriangle(cube.e1, cube.e5, cube.e0);
                CreateTriangle(cube.e5, cube.e4, cube.e0);
                break;

            case 171:
                CreateTriangle(cube.e6, cube.e2, cube.e8);
                CreateTriangle(cube.e6, cube.e8, cube.e7);
                CreateTriangle(cube.e2, cube.e1, cube.e8);
                CreateTriangle(cube.e4, cube.e8, cube.e5);
                break;

            case 172:
                CreateTriangle(cube.e9, cube.e5, cube.e4);
                CreateTriangle(cube.e10, cube.e1, cube.e6);
                CreateTriangle(cube.e1, cube.e7, cube.e6);
                CreateTriangle(cube.e1, cube.e3, cube.e7);
                break;

            case 173:
                CreateTriangle(cube.e1, cube.e6, cube.e10);
                CreateTriangle(cube.e1, cube.e7, cube.e6);
                CreateTriangle(cube.e1, cube.e0, cube.e7);
                CreateTriangle(cube.e8, cube.e7, cube.e0);
                break;

            case 174:
                CreateTriangle(cube.e4, cube.e0, cube.e10);
                CreateTriangle(cube.e4, cube.e10, cube.e5);
                CreateTriangle(cube.e0, cube.e3, cube.e10);
                CreateTriangle(cube.e6, cube.e10, cube.e7);
                break;

            case 175:
                CreateTriangle(cube.e7, cube.e6, cube.e10);
                CreateTriangle(cube.e7, cube.e10, cube.e8);
                CreateTriangle(cube.e5, cube.e4, cube.e10);
                CreateTriangle(cube.e4, cube.e8, cube.e10);
                break;

            case 176:
                CreateTriangle(cube.e6, cube.e9, cube.e5);
                CreateTriangle(cube.e6, cube.e11, cube.e9);
                CreateTriangle(cube.e11, cube.e8, cube.e9);
                break;

            case 177:
                CreateTriangle(cube.e3, cube.e6, cube.e11);
                CreateTriangle(cube.e0, cube.e6, cube.e3);
                CreateTriangle(cube.e0, cube.e5, cube.e6);
                CreateTriangle(cube.e0, cube.e9, cube.e5);
                break;

            case 178:
                CreateTriangle(cube.e0, cube.e11, cube.e8);
                CreateTriangle(cube.e0, cube.e5, cube.e11);
                CreateTriangle(cube.e0, cube.e1, cube.e5);
                CreateTriangle(cube.e5, cube.e6, cube.e11);
                break;

            case 179:
                CreateTriangle(cube.e6, cube.e11, cube.e3);
                CreateTriangle(cube.e6, cube.e3, cube.e5);
                CreateTriangle(cube.e5, cube.e3, cube.e1);
                break;

            case 180:
                CreateTriangle(cube.e1, cube.e2, cube.e10);
                CreateTriangle(cube.e9, cube.e5, cube.e11);
                CreateTriangle(cube.e9, cube.e11, cube.e8);
                CreateTriangle(cube.e11, cube.e5, cube.e6);
                break;

            case 181:
                CreateTriangle(cube.e0, cube.e11, cube.e3);
                CreateTriangle(cube.e0, cube.e6, cube.e11);
                CreateTriangle(cube.e0, cube.e9, cube.e6);
                CreateTriangle(cube.e5, cube.e6, cube.e9);
                break;

            case 182:
                CreateTriangle(cube.e11, cube.e8, cube.e5);
                CreateTriangle(cube.e11, cube.e5, cube.e6);
                CreateTriangle(cube.e8, cube.e0, cube.e5);
                CreateTriangle(cube.e10, cube.e5, cube.e2);
                break;

            case 183:
                CreateTriangle(cube.e6, cube.e11, cube.e3);
                CreateTriangle(cube.e6, cube.e3, cube.e5);
                CreateTriangle(cube.e2, cube.e10, cube.e3);
                CreateTriangle(cube.e10, cube.e5, cube.e3);
                break;

            case 184:
                CreateTriangle(cube.e5, cube.e8, cube.e9);
                CreateTriangle(cube.e5, cube.e2, cube.e8);
                CreateTriangle(cube.e5, cube.e6, cube.e2);
                CreateTriangle(cube.e3, cube.e8, cube.e2);
                break;

            case 185:
                CreateTriangle(cube.e9, cube.e5, cube.e6);
                CreateTriangle(cube.e9, cube.e6, cube.e0);
                CreateTriangle(cube.e0, cube.e6, cube.e2);
                break;

            case 186:
                CreateTriangle(cube.e1, cube.e5, cube.e8);
                CreateTriangle(cube.e1, cube.e8, cube.e0);
                CreateTriangle(cube.e5, cube.e6, cube.e8);
                CreateTriangle(cube.e3, cube.e8, cube.e2);
                break;

            case 187:
                CreateTriangle(cube.e1, cube.e5, cube.e6);
                CreateTriangle(cube.e2, cube.e1, cube.e6);
                break;

            case 188:
                CreateTriangle(cube.e1, cube.e3, cube.e6);
                CreateTriangle(cube.e1, cube.e6, cube.e10);
                CreateTriangle(cube.e3, cube.e8, cube.e6);
                CreateTriangle(cube.e5, cube.e6, cube.e9);
                break;

            case 189:
                CreateTriangle(cube.e10, cube.e1, cube.e0);
                CreateTriangle(cube.e10, cube.e0, cube.e6);
                CreateTriangle(cube.e9, cube.e5, cube.e0);
                CreateTriangle(cube.e5, cube.e6, cube.e0);
                break;

            case 190:
                CreateTriangle(cube.e0, cube.e3, cube.e8);
                CreateTriangle(cube.e5, cube.e6, cube.e10);
                break;

            case 191:
                CreateTriangle(cube.e10, cube.e5, cube.e6);
                break;

            case 192:
                CreateTriangle(cube.e11, cube.e5, cube.e10);
                CreateTriangle(cube.e7, cube.e5, cube.e11);
                break;

            case 193:
                CreateTriangle(cube.e11, cube.e5, cube.e10);
                CreateTriangle(cube.e11, cube.e7, cube.e5);
                CreateTriangle(cube.e8, cube.e3, cube.e0);
                break;

            case 194:
                CreateTriangle(cube.e5, cube.e11, cube.e7);
                CreateTriangle(cube.e5, cube.e10, cube.e11);
                CreateTriangle(cube.e1, cube.e9, cube.e0);
                break;

            case 195:
                CreateTriangle(cube.e10, cube.e7, cube.e5);
                CreateTriangle(cube.e10, cube.e11, cube.e7);
                CreateTriangle(cube.e9, cube.e8, cube.e1);
                CreateTriangle(cube.e8, cube.e3, cube.e1);
                break;

            case 196:
                CreateTriangle(cube.e11, cube.e1, cube.e2);
                CreateTriangle(cube.e11, cube.e7, cube.e1);
                CreateTriangle(cube.e7, cube.e5, cube.e1);
                break;

            case 197:
                CreateTriangle(cube.e0, cube.e8, cube.e3);
                CreateTriangle(cube.e1, cube.e2, cube.e7);
                CreateTriangle(cube.e1, cube.e7, cube.e5);
                CreateTriangle(cube.e7, cube.e2, cube.e11);
                break;

            case 198:
                CreateTriangle(cube.e9, cube.e7, cube.e5);
                CreateTriangle(cube.e9, cube.e2, cube.e7);
                CreateTriangle(cube.e9, cube.e0, cube.e2);
                CreateTriangle(cube.e2, cube.e11, cube.e7);
                break;

            case 199:
                CreateTriangle(cube.e7, cube.e5, cube.e2);
                CreateTriangle(cube.e7, cube.e2, cube.e11);
                CreateTriangle(cube.e5, cube.e9, cube.e2);
                CreateTriangle(cube.e3, cube.e2, cube.e8);
                break;

            case 200:
                CreateTriangle(cube.e2, cube.e5, cube.e10);
                CreateTriangle(cube.e2, cube.e3, cube.e5);
                CreateTriangle(cube.e3, cube.e7, cube.e5);
                break;

            case 201:
                CreateTriangle(cube.e8, cube.e2, cube.e0);
                CreateTriangle(cube.e8, cube.e5, cube.e2);
                CreateTriangle(cube.e8, cube.e7, cube.e5);
                CreateTriangle(cube.e10, cube.e2, cube.e5);
                break;

            case 202:
                CreateTriangle(cube.e9, cube.e0, cube.e1);
                CreateTriangle(cube.e5, cube.e10, cube.e3);
                CreateTriangle(cube.e5, cube.e3, cube.e7);
                CreateTriangle(cube.e3, cube.e10, cube.e2);
                break;

            case 203:
                CreateTriangle(cube.e9, cube.e8, cube.e2);
                CreateTriangle(cube.e9, cube.e2, cube.e1);
                CreateTriangle(cube.e8, cube.e7, cube.e2);
                CreateTriangle(cube.e10, cube.e2, cube.e5);
                break;

            case 204:
                CreateTriangle(cube.e1, cube.e3, cube.e5);
                CreateTriangle(cube.e3, cube.e7, cube.e5);
                break;

            case 205:
                CreateTriangle(cube.e0, cube.e8, cube.e7);
                CreateTriangle(cube.e0, cube.e7, cube.e1);
                CreateTriangle(cube.e1, cube.e7, cube.e5);
                break;

            case 206:
                CreateTriangle(cube.e9, cube.e0, cube.e3);
                CreateTriangle(cube.e9, cube.e3, cube.e5);
                CreateTriangle(cube.e5, cube.e3, cube.e7);
                break;

            case 207:
                CreateTriangle(cube.e9, cube.e8, cube.e7);
                CreateTriangle(cube.e5, cube.e9, cube.e7);
                break;

            case 208:
                CreateTriangle(cube.e5, cube.e8, cube.e4);
                CreateTriangle(cube.e5, cube.e10, cube.e8);
                CreateTriangle(cube.e10, cube.e11, cube.e8);
                break;

            case 209:
                CreateTriangle(cube.e5, cube.e0, cube.e4);
                CreateTriangle(cube.e5, cube.e11, cube.e0);
                CreateTriangle(cube.e5, cube.e10, cube.e11);
                CreateTriangle(cube.e11, cube.e3, cube.e0);
                break;

            case 210:
                CreateTriangle(cube.e0, cube.e1, cube.e9);
                CreateTriangle(cube.e8, cube.e4, cube.e10);
                CreateTriangle(cube.e8, cube.e10, cube.e11);
                CreateTriangle(cube.e10, cube.e4, cube.e5);
                break;

            case 211:
                CreateTriangle(cube.e10, cube.e11, cube.e4);
                CreateTriangle(cube.e10, cube.e4, cube.e5);
                CreateTriangle(cube.e11, cube.e3, cube.e4);
                CreateTriangle(cube.e9, cube.e4, cube.e1);
                break;

            case 212:
                CreateTriangle(cube.e2, cube.e5, cube.e1);
                CreateTriangle(cube.e2, cube.e8, cube.e5);
                CreateTriangle(cube.e2, cube.e11, cube.e8);
                CreateTriangle(cube.e4, cube.e5, cube.e8);
                break;

            case 213:
                CreateTriangle(cube.e0, cube.e4, cube.e11);
                CreateTriangle(cube.e0, cube.e11, cube.e3);
                CreateTriangle(cube.e4, cube.e5, cube.e11);
                CreateTriangle(cube.e2, cube.e11, cube.e1);
                break;

            case 214:
                CreateTriangle(cube.e0, cube.e2, cube.e5);
                CreateTriangle(cube.e0, cube.e5, cube.e9);
                CreateTriangle(cube.e2, cube.e11, cube.e5);
                CreateTriangle(cube.e4, cube.e5, cube.e8);
                break;

            case 215:
                CreateTriangle(cube.e9, cube.e4, cube.e5);
                CreateTriangle(cube.e2, cube.e11, cube.e3);
                break;

            case 216:
                CreateTriangle(cube.e2, cube.e5, cube.e10);
                CreateTriangle(cube.e3, cube.e5, cube.e2);
                CreateTriangle(cube.e3, cube.e4, cube.e5);
                CreateTriangle(cube.e3, cube.e8, cube.e4);
                break;

            case 217:
                CreateTriangle(cube.e5, cube.e10, cube.e2);
                CreateTriangle(cube.e5, cube.e2, cube.e4);
                CreateTriangle(cube.e4, cube.e2, cube.e0);
                break;

            case 218:
                CreateTriangle(cube.e3, cube.e10, cube.e2);
                CreateTriangle(cube.e3, cube.e5, cube.e10);
                CreateTriangle(cube.e3, cube.e8, cube.e5);
                CreateTriangle(cube.e4, cube.e5, cube.e8);
                break;

            case 219:
                CreateTriangle(cube.e5, cube.e10, cube.e2);
                CreateTriangle(cube.e5, cube.e2, cube.e4);
                CreateTriangle(cube.e1, cube.e9, cube.e2);
                CreateTriangle(cube.e9, cube.e4, cube.e2);
                break;

            case 220:
                CreateTriangle(cube.e8, cube.e4, cube.e5);
                CreateTriangle(cube.e8, cube.e5, cube.e3);
                CreateTriangle(cube.e3, cube.e5, cube.e1);
                break;

            case 221:
                CreateTriangle(cube.e0, cube.e4, cube.e5);
                CreateTriangle(cube.e1, cube.e0, cube.e5);
                break;

            case 222:
                CreateTriangle(cube.e8, cube.e4, cube.e5);
                CreateTriangle(cube.e8, cube.e5, cube.e3);
                CreateTriangle(cube.e9, cube.e0, cube.e5);
                CreateTriangle(cube.e0, cube.e3, cube.e5);
                break;

            case 223:
                CreateTriangle(cube.e9, cube.e4, cube.e5);
                break;

            case 224:
                CreateTriangle(cube.e4, cube.e11, cube.e7);
                CreateTriangle(cube.e4, cube.e9, cube.e11);
                CreateTriangle(cube.e9, cube.e10, cube.e11);
                break;

            case 225:
                CreateTriangle(cube.e0, cube.e8, cube.e3);
                CreateTriangle(cube.e4, cube.e9, cube.e7);
                CreateTriangle(cube.e9, cube.e11, cube.e7);
                CreateTriangle(cube.e9, cube.e10, cube.e11);
                break;

            case 226:
                CreateTriangle(cube.e1, cube.e10, cube.e11);
                CreateTriangle(cube.e1, cube.e11, cube.e4);
                CreateTriangle(cube.e1, cube.e4, cube.e0);
                CreateTriangle(cube.e7, cube.e4, cube.e11);
                break;

            case 227:
                CreateTriangle(cube.e3, cube.e1, cube.e4);
                CreateTriangle(cube.e3, cube.e4, cube.e8);
                CreateTriangle(cube.e1, cube.e10, cube.e4);
                CreateTriangle(cube.e7, cube.e4, cube.e11);
                break;

            case 228:
                CreateTriangle(cube.e4, cube.e11, cube.e7);
                CreateTriangle(cube.e9, cube.e11, cube.e4);
                CreateTriangle(cube.e9, cube.e2, cube.e11);
                CreateTriangle(cube.e9, cube.e1, cube.e2);
                break;

            case 229:
                CreateTriangle(cube.e9, cube.e7, cube.e4);
                CreateTriangle(cube.e9, cube.e11, cube.e7);
                CreateTriangle(cube.e9, cube.e1, cube.e11);
                CreateTriangle(cube.e2, cube.e11, cube.e1);
                break;

            case 230:
                CreateTriangle(cube.e11, cube.e7, cube.e4);
                CreateTriangle(cube.e11, cube.e4, cube.e2);
                CreateTriangle(cube.e2, cube.e4, cube.e0);
                break;

            case 231:
                CreateTriangle(cube.e11, cube.e7, cube.e4);
                CreateTriangle(cube.e11, cube.e4, cube.e2);
                CreateTriangle(cube.e8, cube.e3, cube.e4);
                CreateTriangle(cube.e3, cube.e2, cube.e4);
                break;

            case 232:
                CreateTriangle(cube.e2, cube.e9, cube.e10);
                CreateTriangle(cube.e2, cube.e7, cube.e9);
                CreateTriangle(cube.e2, cube.e3, cube.e7);
                CreateTriangle(cube.e7, cube.e4, cube.e9);
                break;

            case 233:
                CreateTriangle(cube.e9, cube.e10, cube.e7);
                CreateTriangle(cube.e9, cube.e7, cube.e4);
                CreateTriangle(cube.e10, cube.e2, cube.e7);
                CreateTriangle(cube.e8, cube.e7, cube.e0);
                break;

            case 234:
                CreateTriangle(cube.e3, cube.e7, cube.e10);
                CreateTriangle(cube.e3, cube.e10, cube.e2);
                CreateTriangle(cube.e7, cube.e4, cube.e10);
                CreateTriangle(cube.e1, cube.e10, cube.e0);
                break;

            case 235:
                CreateTriangle(cube.e1, cube.e10, cube.e2);
                CreateTriangle(cube.e8, cube.e7, cube.e4);
                break;

            case 236:
                CreateTriangle(cube.e4, cube.e9, cube.e1);
                CreateTriangle(cube.e4, cube.e1, cube.e7);
                CreateTriangle(cube.e7, cube.e1, cube.e3);
                break;

            case 237:
                CreateTriangle(cube.e4, cube.e9, cube.e1);
                CreateTriangle(cube.e4, cube.e1, cube.e7);
                CreateTriangle(cube.e0, cube.e8, cube.e1);
                CreateTriangle(cube.e8, cube.e7, cube.e1);
                break;

            case 238:
                CreateTriangle(cube.e4, cube.e0, cube.e3);
                CreateTriangle(cube.e7, cube.e4, cube.e3);
                break;

            case 239:
                CreateTriangle(cube.e4, cube.e8, cube.e7);
                break;

            case 240:
                CreateTriangle(cube.e9, cube.e10, cube.e8);
                CreateTriangle(cube.e10, cube.e11, cube.e8);
                break;

            case 241:
                CreateTriangle(cube.e3, cube.e0, cube.e9);
                CreateTriangle(cube.e3, cube.e9, cube.e11);
                CreateTriangle(cube.e11, cube.e9, cube.e10);
                break;

            case 242:
                CreateTriangle(cube.e0, cube.e1, cube.e10);
                CreateTriangle(cube.e0, cube.e10, cube.e8);
                CreateTriangle(cube.e8, cube.e10, cube.e11);
                break;

            case 243:
                CreateTriangle(cube.e3, cube.e1, cube.e10);
                CreateTriangle(cube.e11, cube.e3, cube.e10);
                break;

            case 244:
                CreateTriangle(cube.e1, cube.e2, cube.e11);
                CreateTriangle(cube.e1, cube.e11, cube.e9);
                CreateTriangle(cube.e9, cube.e11, cube.e8);
                break;

            case 245:
                CreateTriangle(cube.e3, cube.e0, cube.e9);
                CreateTriangle(cube.e3, cube.e9, cube.e11);
                CreateTriangle(cube.e1, cube.e2, cube.e9);
                CreateTriangle(cube.e2, cube.e11, cube.e9);
                break;

            case 246:
                CreateTriangle(cube.e0, cube.e2, cube.e11);
                CreateTriangle(cube.e8, cube.e0, cube.e11);
                break;

            case 247:
                CreateTriangle(cube.e3, cube.e2, cube.e11);
                break;

            case 248:
                CreateTriangle(cube.e2, cube.e3, cube.e8);
                CreateTriangle(cube.e2, cube.e8, cube.e10);
                CreateTriangle(cube.e10, cube.e8, cube.e9);
                break;

            case 249:
                CreateTriangle(cube.e9, cube.e10, cube.e2);
                CreateTriangle(cube.e0, cube.e9, cube.e2);
                break;

            case 250:
                CreateTriangle(cube.e2, cube.e3, cube.e8);
                CreateTriangle(cube.e2, cube.e8, cube.e10);
                CreateTriangle(cube.e0, cube.e1, cube.e8);
                CreateTriangle(cube.e1, cube.e10, cube.e8);
                break;

            case 251:
                CreateTriangle(cube.e1, cube.e10, cube.e2);
                break;

            case 252:
                CreateTriangle(cube.e1, cube.e3, cube.e8);
                CreateTriangle(cube.e9, cube.e1, cube.e8);
                break;

            case 253:
                CreateTriangle(cube.e0, cube.e9, cube.e1);
                break;

            case 254:
                CreateTriangle(cube.e0, cube.e3, cube.e8);
                break;

            case 255:
                break;
        }
    }

    void AssignVertices(params Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            //if (points[i].vertexIndex == -1) // Toggle low poly
            //{
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].startPos);
                //chunk.verticesStartPosition.Add(points[i].startPos);
            //}
        }
    }

    void CreateTriangle(Node a, Node b, Node c)
    {
        AssignVertices(a, b, c);
        var rdmInt = Random.Range(0, 3);
        //var colorID = rdmInt == 0 ? a.colorID : rdmInt == 1 ? b.colorID : c.colorID;
        var colorID = a.colorID;
        if (colorID >= colorCount) colorID = 0;
        triangles[colorID].Add(a.vertexIndex);
        triangles[colorID].Add(b.vertexIndex);
        triangles[colorID].Add(c.vertexIndex);
    }

    //private void OnDrawGizmos()
    //{
    //    if (cubeGrid != null)
    //    {
    //        for (int x = 0; x < cubeGrid.cubes.GetLength(0); x++)
    //        {
    //            for (int y = 0; y < cubeGrid.cubes.GetLength(1); y++)
    //            {
    //                for (int z = 0; z < cubeGrid.cubes.GetLength(2); z++)
    //                {
    //                    Gizmos.color = cubeGrid.cubes[x, y, z].v0.active ? Color.black : Color.white;
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].v0.position, Vector3.one * .4f);

    //                    Gizmos.color = cubeGrid.cubes[x, y, z].v1.active ? Color.black : Color.white;
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].v1.position, Vector3.one * .4f);

    //                    Gizmos.color = cubeGrid.cubes[x, y, z].v2.active ? Color.black : Color.white;
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].v2.position, Vector3.one * .4f);

    //                    Gizmos.color = cubeGrid.cubes[x, y, z].v3.active ? Color.black : Color.white;
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].v3.position, Vector3.one * .4f);

    //                    Gizmos.color = cubeGrid.cubes[x, y, z].v4.active ? Color.black : Color.white;
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].v4.position, Vector3.one * .4f);

    //                    Gizmos.color = cubeGrid.cubes[x, y, z].v5.active ? Color.black : Color.white;
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].v5.position, Vector3.one * .4f);

    //                    Gizmos.color = cubeGrid.cubes[x, y, z].v6.active ? Color.black : Color.white;
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].v6.position, Vector3.one * .4f);

    //                    Gizmos.color = cubeGrid.cubes[x, y, z].v7.active ? Color.black : Color.white;
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].v7.position, Vector3.one * .4f);

    //                    Gizmos.color = Color.gray;
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e0.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e1.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e2.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e3.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e4.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e5.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e6.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e7.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e8.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e9.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e10.position, Vector3.one * .15f);
    //                    Gizmos.DrawCube(cubeGrid.cubes[x, y, z].e11.position, Vector3.one * .15f);
    //                }
    //            }
    //        }
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    if (cubeGrid != null)
    //    {
    //        Gizmos.color = Color.black;
    //        for (int x = 0; x < cubeGrid.cubes.GetLength(0); x++)
    //        {
    //            for (int y = 0; y < cubeGrid.cubes.GetLength(1); y++)
    //            {
    //                for (int z = 0; z < cubeGrid.cubes.GetLength(2); z++)
    //                {
    //                    if (cubeGrid.cubes[x, y, z].v0.active)
    //                        Gizmos.DrawCube(cubeGrid.cubes[x, y, z].v0.position, Vector3.one * .4f);
    //                }
    //            }
    //        }
    //    }
    //}

    public class CubeGrid
    {
        public Cube[,,] cubes;

        public CubeGrid(Chunk chunk, ChunkVertice[,,] map)
        {
            int nodeCount = map.GetLength(0);

            ControlNode[,,] controlNodes = new ControlNode[nodeCount, nodeCount, nodeCount];

            for (int x = 0; x < nodeCount; x++)
            {
                for (int y = 0; y < nodeCount; y++)
                {
                    for (int z = 0; z < nodeCount; z++)
                    {
                        //Vector3 pos = new Vector3(-chunkSize / 2 + x + 1 / 2f, -chunkSize / 2 + y + 1 / 2f, -chunkSize / 2 + z + 1 / 2f);

                        var topNodePos =
                            y != nodeCount - 1 ?
                                Vector3.Normalize(map[x, y + 1, z].position - map[x, y, z].position) * (map[x, y, z].density / (map[x, y + 1, z].density + map[x, y, z].density)) :
                                (
                                    chunk.topChunk ? Vector3.Normalize((chunk.topChunk.map[x, 0, z].position) + (chunk.position + Vector3.up) * chunk.chunkSize - map[x, y, z].position + chunk.position * chunk.chunkSize)
                                    * (map[x, y, z].density / (chunk.topChunk.map[x, 0, z].density + map[x, y, z].density)) :
                                    Vector3.up * 0.5f
                                );

                        var rightNodePos =
                            x != nodeCount - 1 ?
                                Vector3.Normalize(map[x + 1, y, z].position - map[x, y, z].position) /** (map[x, y, z].density / (map[x + 1, y, z].density + map[x, y, z].density))*/ :
                                (
                                    chunk.rightChunk ? Vector3.Normalize((chunk.rightChunk.map[0, y, z].position) + (chunk.position + Vector3.right) * chunk.chunkSize - map[x, y, z].position + chunk.position * chunk.chunkSize)
                                    /** (map[x, y, z].density / (chunk.rightChunk.map[0, y, z].density + map[x, y, z].density))*/ :
                                    Vector3.right * 0.5f
                                );

                        var forwardNodePos =
                            z != nodeCount - 1 ?
                                Vector3.Normalize(map[x, y, z + 1].position - map[x, y, z].position) /** (map[x, y, z].density / (map[x, y, z + 1].density + map[x, y, z].density))*/ :
                                (
                                    chunk.forwardChunk ? Vector3.Normalize((chunk.forwardChunk.map[x, y, 0].position) + (chunk.position + Vector3.forward) * chunk.chunkSize - map[x, y, z].position + chunk.position * chunk.chunkSize)
                                    /** (map[x, y, z].density / (chunk.forwardChunk.map[x, y, 0].density + map[x, y, z].density))*/ :
                                    Vector3.forward * 0.5f
                                );

                        controlNodes[x, y, z] = new ControlNode(
                            map[x, y, z].startPosition,
                            map[x, y, z].activated == 1,
                            //y != nodeCount - 1 ? Vector3.Normalize(map[x, y + 1, z].startPosition - map[x, y, z].startPosition) * 0.5f : (chunk.topChunk ? Vector3.Normalize(chunk.topChunk.map[x, 0, z].startPosition - map[x, y, z].startPosition) * 0.5f : Vector3.up * 0.5f),
                            //x != nodeCount - 1 ? Vector3.Normalize(map[x + 1, y, z].startPosition - map[x, y, z].startPosition) * 0.5f : (chunk.rightChunk ? Vector3.Normalize(chunk.rightChunk.map[0, y, z].startPosition - map[x, y, z].startPosition) * 0.5f : Vector3.right * 0.5f),
                            //z != nodeCount - 1 ? Vector3.Normalize(map[x, y, z + 1].position - map[x, y, z].position) * 0.5f : (chunk.forwardChunk ? Vector3.Normalize(chunk.forwardChunk.map[x, y, 0].position - map[x, y, z].position) * 0.5f : Vector3.forward * 0.5f),
                            topNodePos,
                            //rightNodePos,
                            //forwardNodePos,
                            Vector3.right * .5f, Vector3.forward * .5f,
                            map[x, y, z].colorID
                            );
                    }
                }
            }

            cubes = new Cube[nodeCount - 1, nodeCount - 1, nodeCount - 1];
            for (int x = 0; x < nodeCount - 1; x++)
            {
                for (int y = 0; y < nodeCount - 1; y++)
                {
                    for (int z = 0; z < nodeCount - 1; z++)
                    {
                        if (x < nodeCount - 1 && y < nodeCount - 1 && z < nodeCount - 1)
                        {
                            cubes[x, y, z] = new Cube(
                                controlNodes[x, y, z],
                                controlNodes[x + 1, y, z],
                                controlNodes[x + 1, y, z + 1],
                                controlNodes[x, y, z + 1],
                                controlNodes[x, y + 1, z],
                                controlNodes[x + 1, y + 1, z],
                                controlNodes[x + 1, y + 1, z + 1],
                                controlNodes[x, y + 1, z + 1]
                                );
                        }
                    }
                }
            }
        }
    }

    public class Cube
    {
        public ControlNode v0, v1, v2, v3, v4, v5, v6, v7;
        public Node e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11;
        public int configuration;
        public int rotation;

        public Cube (ControlNode _v0, ControlNode _v1, ControlNode _v2, ControlNode _v3, ControlNode _v4, ControlNode _v5, ControlNode _v6, ControlNode _v7)
        {
            v0 = _v0;
            v1 = _v1;
            v2 = _v2;
            v3 = _v3;
            v4 = _v4;
            v5 = _v5;
            v6 = _v6;
            v7 = _v7;

            e0 = v0.right;
            e1 = v1.forward;
            e2 = v3.right;
            e3 = v0.forward;
            e4 = v4.right;
            e5 = v5.forward;
            e6 = v7.right;
            e7 = v4.forward;
            e8 = v0.up;
            e9 = v1.up;
            e10 = v2.up;
            e11 = v3.up;

            if (_v0.active) configuration += 1;
            if (_v1.active) configuration += 2;
            if (_v2.active) configuration += 4;
            if (_v3.active) configuration += 8;
            if (_v4.active) configuration += 16;
            if (_v5.active) configuration += 32;
            if (_v6.active) configuration += 64;
            if (_v7.active) configuration += 128;

        }
    }

    public class Node
    {
        public int colorID;

        public Vector3 startPos;
        public int vertexIndex = -1;

        public Node(Vector3 _startPos, int _colorID)
        {
            startPos = _startPos;
            colorID = _colorID;
        }
    }

    public class ControlNode : Node
    {
        public bool active;
        public Node up, right, forward;

        public ControlNode(Vector3 _startPos, bool _active, Vector3 _upNodePos, Vector3 _rightNodePos, Vector3 _forwardNodePos, int _colorID) : base(_startPos, _colorID)
        {
            active = _active;
            up = new Node(_startPos + /*Vector3.up / 2f*/ _upNodePos, _colorID);
            right = new Node(_startPos + /*Vector3.right / 2f*/ _rightNodePos, _colorID);
            forward = new Node(_startPos + /*Vector3.forward / 2f*/ _forwardNodePos, _colorID);
        }
    }
}

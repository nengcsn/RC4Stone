using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTriangle
{
    //public List<TriMesh> Group;
    //Index of 3 vertices of the triangle mesh
    public int[] VerticeIndexes;
    //vertices of the triangle mesh
    public Vector3[] Vertices;
    public Vector3 TriangleNormal;
    public Vector3 CentrePoint;

    public MeshTriangle(int[] _verticeIndexes, Vector3 _meshNormal, Vector3[] _vertices)
    {
        VerticeIndexes = _verticeIndexes;
        Vertices = _vertices;
        TriangleNormal = _meshNormal;

        CentrePoint = (_vertices[0] + _vertices[1] + _vertices[2]) / 3;

    }
    /*
    public TriMesh(List<TriMesh> group, int[] verticeIndexes, Vector3[] vertices, Vector3 meshNormal)
    {
        Group = group;
        this.verticeIndexes = verticeIndexes;
        this.vertices = vertices;
        this.meshNormal = meshNormal;
    }*/
}

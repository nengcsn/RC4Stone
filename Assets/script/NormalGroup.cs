using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGroup
{
    public Vector3 GroupNormal;
    public Vector3 GroupCentrePoint;
    List<MeshTriangle> _triangleMeshGroup;

    public NormalGroup(MeshTriangle initialTriangle)
    {
        GroupNormal = initialTriangle.TriangleNormal;
        GroupCentrePoint = initialTriangle.CentrePoint;

        _triangleMeshGroup.Add(initialTriangle);

    }

    public void AddNormal(MeshTriangle newTriangle)
    {
        _triangleMeshGroup.Add(newTriangle);
        GetAverage();
    }

    public void GetAverage()
    {
        //Calculates the average normal of all the triangles in _trianleMeshGroup
    }
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGroupBase
{
    List<Pose> normals;
    Pose _groupNormal;
    

    public List<Vector3> GetMeshNormals(Mesh stone, Vector3 firstNormal)
    {
        //take a random face in the mesh
        // Get its normal
        // make a new normal group with the face normal

        //loop untill entire mesh is checked
        //Get next face using Breadth First Search
        //if angle between new face normal and group normal < tolerance
        //add the face to the normal group
        //get the average normal of all faces in this normal group
        //if angle between new face normal and group normal > tolerance
        //Make a new normal group
        //Set next face as checked


        List<Vector3> normalGroup = new List<Vector3>();
        float tolerance = 0.1f;
        Vector3 randomNormal = stone.normals[0];
        normalGroup.Add(firstNormal);
        Vector3 groupNormal = randomNormal;

        for (int i = 0; i < stone.normals.Length; i++)
        {
            if (Vector3.Angle(groupNormal, stone.normals[i]) < tolerance)
            {
                normalGroup.Add(stone.normals[i]);
                //groupNormal = normalGroup.GetAverage();
            }
            else
            {
                normalGroup = new List<Vector3>();
            }
        }

        return normalGroup;
    }
}
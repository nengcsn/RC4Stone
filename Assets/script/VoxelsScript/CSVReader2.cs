using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CSVReader2
{

    public static Vector3 ReadVoxelsVector(int pointNumber)
    {

        string fileContent = Resources.Load<TextAsset>("Data/activate_voxels_vector").text;

        string[] lines = fileContent.Split('\n');
        string[] activateVoxelsVectorX = lines.Select(l => l.Split(',')[0]).ToArray();
        string[] activateVoxelsVectorY = lines.Select(l => l.Split(',')[1]).ToArray();
        string[] activateVoxelsVectorZ = lines.Select(l => l.Split(',')[2]).ToArray();
        float voxelX = float.Parse(activateVoxelsVectorX[pointNumber]);
        float voxelY = float.Parse(activateVoxelsVectorY[pointNumber]);
        float voxelZ = float.Parse(activateVoxelsVectorZ[pointNumber]);




        return new Vector3(voxelX, voxelY, voxelZ);

        

    }
}
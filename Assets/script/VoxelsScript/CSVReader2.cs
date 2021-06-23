using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CSVReader2
{

    public static List<Vector3Int> ReadVoxels()
    {
        List<Vector3Int> result = new List<Vector3Int>();
        string fileContent = Resources.Load<TextAsset>("Data/activate_voxels_vector").text;

        string[] lines = fileContent.Split('\n');
        string[] activateVoxelsVectorX = lines.Select(l => l.Split(',')[0]).ToArray();
        string[] activateVoxelsVectorY = lines.Select(l => l.Split(',')[2]).ToArray();
        string[] activateVoxelsVectorZ = lines.Select(l => l.Split(',')[1]).ToArray();

        for (int i = 0; i < lines.Length; i++)
        {
            int voxelX = int.Parse(activateVoxelsVectorX[i]);
            int voxelY = int.Parse(activateVoxelsVectorY[i]);
            int voxelZ = int.Parse(activateVoxelsVectorZ[i]);
            result.Add(new Vector3Int(voxelX, voxelY, voxelZ));
        }

        return result;
    }
}
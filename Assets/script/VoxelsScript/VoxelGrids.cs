using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGrid : MonoBehaviour
{
    //02 Create the properties of the VoxelGrid
    public Vector3Int GridSize;
    public Voxel2[,,] Voxels;
    public Vector3 Origin;
    //02.1 VoxelSize with custom 
    public float VoxelSize { get; private set; }

    //04 Create the basic VoxelGrid constructor
    /// <summary>
    /// Constructor for a basic <see cref="VoxelGrid"/>
    /// </summary>
    /// <param name="size">Size of the grid</param>
    /// <param name="origin">Origin of the grid</param>
    /// <param name="voxelSize">The size of each <see cref="Voxel2"/></param>
    public VoxelGrid(Vector3Int size, Vector3 origin, float voxelSize)
    {
        GridSize = size;
        Origin = origin;
        VoxelSize = voxelSize;

        //05 Create the cubeDummy prefab in Unity
        var cubePrefab = Resources.Load<GameObject>("Prefabs/cubaby");

        //06 Initiate the Voxel array
        Voxels = new Voxel2[GridSize.x, GridSize.y, GridSize.z];

        //07 Populate the array with the new Voxels
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int z = 0; z < GridSize.z; z++)
                {
                    //C# allows functions to be broken down in lines
                    Voxels[x, y, z] = new Voxel2(
                        new Vector3Int(x, y, z),
                        this,
                        cubePrefab);
                }
            }
        }
    }
   
}

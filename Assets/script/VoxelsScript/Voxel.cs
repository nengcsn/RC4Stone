using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoxelState { Dead, Available, Occupied }

public class Voxel
{
    //01 Create properties of the voxel
    public Vector3Int Index;
    public GameObject VoxelGO;
    public VoxelGrid _voxelGrid;
    protected float _size;


    private VoxelState _status = VoxelState.Available;
    public VoxelState Status
    {
        get
        {
            return _status;
        }
        set
        {
            VoxelGO.SetActive(value == VoxelState.Available);
            _status = value;
        }
    }
    //03 Create the constructor of the basic voxel
    /// <summary>
    /// Creates a regular voxel on a voxel grid
    /// </summary>
    /// <param name="index">The index of the Voxel</param>
    /// <param name="voxelgrid">The <see cref="VoxelGrid"/> this <see cref="Voxel"/> is attached to</param>
    /// <param name="voxelGameObject">The <see cref="GameObject"/> used on the Voxel</param>
    public Voxel(Vector3Int index, VoxelGrid voxelgrid, GameObject voxelGameObject, Transform parent = null)
    {
        Index = index;
        _voxelGrid = voxelgrid;
        _size = _voxelGrid.VoxelSize;
        VoxelGO = GameObject.Instantiate(voxelGameObject, 
            (_voxelGrid.Origin + Index) * _size, Quaternion.identity);

        VoxelGO.transform.localScale *= _voxelGrid.VoxelSize*0.9f;
        VoxelGO.name = $"Voxel_{Index.x}_{Index.y}_{Index.z}";
        if (parent != null) VoxelGO.transform.parent = parent;

        VoxelGO.GetComponent<VoxelTrigger>().Voxel = this;
        
        Status = VoxelState.Dead;
    }

    //24 Create the generic constructor for Voxel
    /// <summary>
    /// Generic constructor, alllows the use of inheritance
    /// </summary>
    public Voxel () { }


    public Voxel[] GetNeighbours()
    {
        List<Vector3Int> directions = new List<Vector3Int>()
        {
            new Vector3Int(-1,0,0),
            new Vector3Int(1,0,0),
            new Vector3Int(0,-1,0),
            new Vector3Int(0,1,0),
            new Vector3Int(0,0,-1),
            new Vector3Int(0,0,1)
        };


        Voxel[] neighbours = new Voxel[6];

        for (int i = 0; i < 6; i++)
        {
            if (Util.CheckBounds(_voxelGrid, Index + directions[i]))
            {
                neighbours[i] = _voxelGrid.GetVoxel(Index + directions[i]);
            }
        } 
        //if (neighbours.Count < 1) Debug.Log("No neighbours found");

        return neighbours;
    }

}

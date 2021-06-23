using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum VoxelState { Dead,Alive}


public class Voxel2
{
    //01 Create properties of the voxel
    public Vector3Int Index;
    protected GameObject _voxelGO;
    protected VoxelGrid _voxelGrid;
    protected float _size;

    private VoxelState _status = VoxelState.Alive;
    public VoxelState Status
    {
        get
        {
            return _status;
        }
        set
        {
            _voxelGO.SetActive(value == VoxelState.Alive);
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
    public Voxel2(Vector3Int index, VoxelGrid voxelgrid, GameObject voxelGameObject)
    {
        Index = index;
        _voxelGrid = voxelgrid;
        _size = _voxelGrid.VoxelSize;
        _voxelGO = GameObject.Instantiate(voxelGameObject, (_voxelGrid.Origin + Index) * _size, Quaternion.identity);

        _voxelGO.transform.localScale *= _voxelGrid.VoxelSize*0.9f;
        _voxelGO.name = $"Voxel_{Index.x}_{Index.y}_{Index.z}";
    }

    //24 Create the generic constructor for Voxel
    /// <summary>
    /// Generic constructor, alllows the use of inheritance
    /// </summary>
    public Voxel2 () { }
}

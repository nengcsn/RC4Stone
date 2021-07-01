using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTrigger : MonoBehaviour
{
    public Stone Stone;
    //public bool Count = false;
    //private int _collisionAmount = 0;
    //private List<Collider> _collided = new List<Collider>();

    //private void OnCollisionEnter(Collision col)
    //{
    //    if (!_collided.Contains(col.collider) 
    //        && col.collider.tag == "Voxel"
    //        && col.gameObject.GetComponent<VoxelTrigger>().Voxel.Status == VoxelState.Available)
    //    { 
    //        _collided.Add(col.collider);
    //    }
    //}

    //private void OnCollisionStay(Collision col)
    //{
    //    if (!_collided.Contains(col.collider)
    //        && col.collider.tag == "Voxel"
    //        && col.gameObject.GetComponent<VoxelTrigger>().Voxel.Status == VoxelState.Available)
    //    {
    //        _collided.Add(col.collider);
    //    }
    //}



    //public void ClearCollisions()
    //{
    //    _collided = new List<Collider>();
    //}

    //public int CountVoxels()
    //{
    //    return _collided.Count;
    //}
}

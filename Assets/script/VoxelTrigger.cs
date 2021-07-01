using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTrigger : MonoBehaviour
{
    public Voxel Voxel;
    bool _isColliding;

    private void Update()
    {
        //if (_isColliding)
        //{
        //    if (Voxel.Status != VoxelState.Occupied)
        //    {
        //        Voxel.Status = VoxelState.Occupied;
        //    }
        //}
        //else if (Voxel.Status != VoxelState.Available)
        //{
        //    Voxel.Status = VoxelState.Available;
        //}
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Stone")
        {
            var stone = col.gameObject.GetComponent<StoneTrigger>().Stone;
            stone.AddOccupiedVoxel(Voxel);
            //Voxel.Status = VoxelState.Occupied;

        }
   
    }
    
    //private void OnCollisionExit(Collision col)
    //{
    //    Debug.Log("Exiting");
    //    Voxel.Status = VoxelState.Available;
    //}

}

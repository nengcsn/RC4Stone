using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class StoneAgent : Agent
{
    #region Fields and Properties

    // 24.1 The voxel where the agent is currently
    private Voxel _voxelLocation;


    // 24.3 The VoxelGrid the agent is navigating
    private VoxelGrid _voxelGrid;

    // 24.4 The environment the agent belongs to
    private MLEnvironment _environment;

    // 24.5 The normalized position the agent is currently at
    private Vector3 _normalizedIndex;


    // 24.7 Training booleans
    public bool Training;
    private bool _frozen;


    //Stone Setup
    //private List<Stone> Stones;
    //protected GameObject PrefabStone;
    public Material TransparentMat;
    public Material BrickMat;
    //private BoxCollider StoneCol;

    #endregion

    #region Unity standard methods

    // 25 Create Awake method
    private void Awake()
    {
        // 26 Read the environment from the hierarchy
        _environment = transform.parent.gameObject.GetComponent<MLEnvironment>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P");
            var stone = _environment.GetUnplacedStones().First();
            stone.MoveStartToPosition(_voxelLocation.Index);
        }
    }

    #endregion

    #region MLAgents methods

    public override void OnEpisodeBegin()
    {
        var availableVoxels = _environment.VoxelGrid.GetVoxels().Where(v => v.Status == VoxelState.Available);

        // Start the agent in a random availble voxel
        _voxelLocation = availableVoxels.First();

        if (Training)
        {
            _frozen = false;
            

            

        }
        else
        {

            _frozen = true;

        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        // Update the available voxels list

        // Action to move the agent through the voxel grid
        // Action to select a stone to place
        // Action to move a stone to a position                            ---> Reward or penalise
        //   Check how many voxels the stone intersects
        //   Occupy the voxels that you have set
        // Action to set the stone's orientation (in increments of 15deg)  ---> Reward or penalise
        //   Check how many voxels the stone intersects
        //   Occupy the voxels that you have set
        // Action to remove stone

        // Check if a target percentage of voxels have been occupied
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        // The current normalised position of the agent 
        // How many stones have been placed
        // How many stones are left to be placed
        // How many voxels are occupied / percentage
        // And whatever else you can think to check
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P");
            var stone = _environment.GetUnplacedStones().First();
            stone.MoveStartToPosition(_voxelLocation.Index);
        }
    }

    #endregion

    #region Private methods



    #endregion

    #region Public methods


    #endregion
}

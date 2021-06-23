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

    // 24.2 The component at the current location
    private Component _component;

    // 24.3 The VoxelGrid the agent is navigating
    private VoxelGrid _voxelGrid;

    // 24.4 The environment the agent belongs to
    private EnvironmentManager _environment;

    // 24.5 The normalized position the agent is currently at
    private Vector3 _normalizedIndex;

    // 24.6 The target void ratio
    private float _voidRatioThreshold;

    // 24.7 Training booleans
    public bool Training;
    private bool _frozen;


    //Stone Setup
    public GameObject[] StoneLib;
    protected GameObject PrefabStone;
    public Material TransparentMat;
    public Material BrickMat;
    private BoxCollider StoneCol;

    #endregion

    #region Unity standard methods

    // 25 Create Awake method
    private void Awake()
    {
        // 26 Read the environment from the hierarchy
        _environment = transform.parent.gameObject.GetComponent<EnvironmentManager>();
    }

    #endregion

    #region MLAgents methods

    public override void OnEpisodeBegin()
    {

        if (Training)
        {
            // 30 Unfreeze agent
            _frozen = false;

            // 32 Reset the environment
            //_environment.ResetEnvironment();

            //// 33 Find a new random position
            //int x = Random.Range(0, _voxelGrid.GridSize.x);s
            //int y = Random.Range(0, _voxelGrid.GridSize.y);
            //int z = Random.Range(0, _voxelGrid.GridSize.z);

            // 38 Move the agent to new random voxel
            //GoToVoxel(new Vector3Int(x, y, z));
        }
        else
        {
            // 39 Freeze the agent
            _frozen = true;
            // 40 Move the agent to the origin voxel
            //GoToVoxel(new Vector3Int(0, 0, 0));
        }
    }

    #endregion

    #region Private methods



    #endregion

    #region Public methods
    //// 34 Create the method to move voxel to an index
    ///// <summary>
    ///// Move the agent to an index
    ///// </summary>
    ///// <param name="index">The target index position</param>
    //public void GoToVoxel(Vector3Int index)
    //{
    //    // 35 Get the target voxel
    //    var voxel = _voxelGrid.Voxels[index.x, index.y, index.z];
    //    _voxelLocation = voxel;

    //    // 36 Move agent game object to target position
    //    transform.localPosition = voxel.Index;

    //    // 37 Get the component at the target position -> create Method
    //    _component = _environment.GetComponentAtVoxel(_voxelLocation);
    //}

    #endregion
}

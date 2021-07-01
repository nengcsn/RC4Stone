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

    //  The voxel where the agent is currently
    private Voxel _voxelLocation;


    //  The VoxelGrid the agent is navigating
    private VoxelGrid _voxelGrid;

    // The environment the agent belongs to
    private MLEnvironment _environment;

    // The normalized position the agent is currently at
    private Vector3 _normalizedIndex;

    // 01.3 The array that contains all the components of the environment
    Component[,,] _components;

    //  Training booleans
    public bool Training;
    private bool _frozen;


    //Stone Setup
    private List<Stone> Stones;
    protected GameObject PrefabStone;
    public Material TransparentMat;
    public Material BrickMat;
    private MeshCollider StoneCol;
    protected GameObject CurrentStone;
    #endregion

    #region Unity standard methods

    //  Create Awake method
    private void Awake()
    {
        // 26 Read the environment from the hierarchy
        _environment = transform.parent.gameObject.GetComponent<MLEnvironment>();
        //PrefabStone = Stones;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P");
            var stone = _environment.GetUnplacedStones().First();
            stone.MoveStartToPosition(_voxelLocation.Index);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("E");
            var stone = _environment.GetUnplacedStones().First();
            PrefabStone.transform.Rotate(Vector3.up, 5);//Rotate stone?
        }
    }

    #endregion

    #region MLAgents methods

    public override void OnEpisodeBegin()
    {
        var availableVoxels = _environment.VoxelGrid.GetVoxels().Where(v => v.Status == VoxelState.Available);

        // 28 Get the voxel grid from the environment
        _voxelGrid = _environment.VoxelGrid;

        // Start the agent in a random availble voxel
        _voxelLocation = availableVoxels.First();

        if (Training)
        {
            _frozen = false;

            // 33 Find a new random position
            int x = Random.Range(0, _voxelGrid.GridSize.x);
            int y = Random.Range(0, _voxelGrid.GridSize.y);
            int z = Random.Range(0, _voxelGrid.GridSize.z);


            Vector3Int position = new Vector3Int(x, y, z);

            //GoToVoxel(position);

        }
        else
        {

            _frozen = true;

        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var act = actions.DiscreteActions;

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

        //Action 

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;
        var dirToGoForwardAction = act[0];
        var rotateDirAction = act[1];
        var dirToGoSideAction = act[2];

        if (dirToGoForwardAction == 1)
            dirToGo = 1f * transform.forward;
        else if (dirToGoForwardAction == 2)
            dirToGo = -1f * transform.forward;
        if (rotateDirAction == 1)
            rotateDir = transform.up * -1f;
        else if (rotateDirAction == 2)
            rotateDir = transform.up * 1f;
        if (dirToGoSideAction == 1)
            dirToGo = -0.6f * transform.right;
        else if (dirToGoSideAction == 2)
            dirToGo = 0.6f * transform.right;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(CurrentStone.transform.localPosition);


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
        else if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("E");
            //var stone = _environment.GetUnplacedStones().First();
            //var CurrentStone = _environment.GetUnplacedStones().First();
            //PrefabStone.transform.Rotate(Vector3.up, 5);//Rotate the prefab
            //}
        }

        #endregion

        #region Private methods

        // 44 Create the method to move the agent
        /// <summary>
        /// Attempt to move the based on an integer action
        /// </summary>
        /// <param name="action">The action</param>
        /// <returns>The success of the attempt</returns>
        //    private bool MoveAgent(int action)
        //{
        //    // 45 Create the vector and assign it's value based on the action input
        //    Vector3Int direction;

        //    // 46 Set direction based on action input
        //    if (action == 0) return true;
        //    else if (action == 1) direction = new Vector3Int(1, 0, 0);
        //    else if (action == 2) direction = new Vector3Int(-1, 0, 0);
        //    else if (action == 3) direction = new Vector3Int(0, 1, 0);
        //    else if (action == 4) direction = new Vector3Int(0, -1, 0);
        //    else if (action == 5) direction = new Vector3Int(0, 0, 1);
        //    else direction = new Vector3Int(0, 0, -1);

        //    // 47 Check if the resulting action keeps the agent within the grid
        //    Vector3Int destination = _voxelLocation.Index + direction;
        //    if (destination.x < 0 || destination.x >= _voxelGrid.GridSize.x ||
        //        destination.y < 0 || destination.y >= _voxelGrid.GridSize.y ||
        //        destination.z < 0 || destination.z >= _voxelGrid.GridSize.z)
        //    {
        //        // 48 Return false if action was invalid
        //        return false;
        //    }

        //    //// 49 Move the agent to the destination
        //    //GoToVoxel(destination);

        //    return true;
    }
    #endregion

}
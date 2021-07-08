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
    private Vector3 _normalisedIndex;

    bool _placedStone = false;

    Vector3[] _directions = new Vector3[]// array of vectors of directions 
    {
        new Vector3(1,0,0),
        new Vector3(1,0.5f,0),
        new Vector3(0.5f,1,0),
        new Vector3(0,1,0),
        new Vector3(-0.5f,1,0),
        new Vector3(-1,0.5f,0),
        new Vector3(-1,0,0),
        new Vector3(-0.5f,-1,0),
        new Vector3(-1,-0.5f,0),
        new Vector3(0,-1,0),
        new Vector3(1,-0.5f,0),
        new Vector3(0.5f,-1,0),

        new Vector3(0,0,1),
        new Vector3(0,0.5f,1),
        new Vector3(0,1,0.5f),
        new Vector3(0,0.5f,-1),
        new Vector3(0,1,-0.5f), 
        new Vector3(0,0,-1),
        new Vector3(0,-0.5f,-1),
        new Vector3(0,-1,-0.5f),
        new Vector3(0,-0.5f,1),
        new Vector3(0,-1,0.5f),

        new Vector3(0.5f,0,1),
        new Vector3(1,0,0.5f),
        new Vector3(0.5f,0,-1),
        new Vector3(1,0,-0.5f),
        new Vector3(-0.5f,0,-1),
        new Vector3(-1,0,-0.5f),
        new Vector3(-0.5f,0,1),
        new Vector3(-1,0,0.5f),
    };

    //  Training booleans
    public bool Training;
    private bool _frozen;

    //Stone Setup
    //private List<Stone> Stones;
    //private GameObject PrefabStone;
    //public Material TransparentMat;
    //public Material BrickMat;
    //private MeshCollider StoneCol;
    private Stone _currentStone;
    #endregion

    #region Unity standard methods

    //  Create Awake method
    private void Start()
    {
        // 26 Read the environment from the hierarchy
        _environment = transform.parent.gameObject.GetComponent<MLEnvironment>();
        _voxelGrid = _environment.VoxelGrid;
        //PrefabStone = Stones;
    }

    private void FixedUpdate()
    {
        if (_placedStone)
        {
            _currentStone.OccupyVoxels();
            Debug.Log(_environment.GetOccupiedRatio());
            _placedStone = false;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlaceAndOrientStone(Vector3.up);
            _placedStone = true;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GetStoneByLength(Random.Range(0.5f, 3.5f));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateStone(_normalisedIndex);
        }

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Debug.Log("P");
        //    var stone = _environment.GetUnplacedStones().First();
        //    stone.MoveStartToPosition(_voxelLocation.Index);
        //}
        //else if (Input.GetKey(KeyCode.E))
        //{
        //    Debug.Log("E");
        //    var stone = _environment.GetUnplacedStones().First();
        //    //PrefabStone.transform.Rotate(Vector3.up, 5);//Rotate stone?
        //}
    }

    #endregion

    #region MLAgents methods

    public override void OnEpisodeBegin()
    {
        // Start the agent in a random availble voxel
        //_voxelLocation = availableVoxels.First();

        if (Training)
        {
            _frozen = false;

            _currentStone = _environment.Stones[0];

            var availableVoxels = _voxelGrid.GetVoxels().Where(v => v.Status == VoxelState.Available).ToList();
            var randomVoxel = availableVoxels[Random.Range(0, availableVoxels.Count)];
            Vector3Int position = randomVoxel.Index;

            GoToVoxel(position);
        }
        else
        {
            _frozen = true;
            GoToVoxel(Vector3Int.zero);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var act = actions.DiscreteActions;

        base.OnActionReceived(actions);

        // Update the available voxels list

        // Action to move the agent through the voxel grid v


        // Action to select a stone to place v
        // Action to move a stone to a position  v                          ---> Reward or penalise
        //   Check how many voxels the stone intersects v
        //   Occupy the voxels that you have set v
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
        //sensor.AddObservation(_currentStone.transform.localPosition);


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

        
    }
    
    #region Private methods

    public void GoToVoxel(Vector3Int index)
    {
        _voxelLocation = _voxelGrid.GetVoxel(index);
        SetNormalisedIndex();
        transform.localPosition = new Vector3(index.x * _voxelGrid.VoxelSize,
            index.y * _voxelGrid.VoxelSize,
            index.z * _voxelGrid.VoxelSize);
    }

    private void SetNormalisedIndex()
    {
        _normalisedIndex = new Vector3(
            _voxelLocation.Index.x / _voxelGrid.GridSize.x - 1,
            _voxelLocation.Index.y / _voxelGrid.GridSize.y - 1,
            _voxelLocation.Index.z / _voxelGrid.GridSize.z - 1);
    }


    /// <summary>
    /// Attempt to move the based on an integer action
    /// </summary>
    /// <param name="action">The action</param>
    /// <returns>The success of the attempt</returns>
    private bool MoveAgent(int action)
    {
        // 45 Create the vector and assign it's value based on the action input
        Vector3Int direction;

        // 46 Set direction based on action input
        if (action == 0) return true;
        else if (action == 1) direction = new Vector3Int(1, 0, 0);
        else if (action == 2) direction = new Vector3Int(-1, 0, 0);
        else if (action == 3) direction = new Vector3Int(0, 1, 0);
        else if (action == 4) direction = new Vector3Int(0, -1, 0);
        else if (action == 5) direction = new Vector3Int(0, 0, 1);
        else direction = new Vector3Int(0, 0, -1);

        // 47 Check if the resulting action keeps the agent within the grid
        Vector3Int destination = _voxelLocation.Index + direction;
        if (!Util.CheckBounds(_voxelGrid, destination) 
            || _voxelGrid.GetVoxel(destination).Status != VoxelState.Available)
        {
            return false;
        }

        GoToVoxel(destination);

        return true;
    }

    private void PlaceAndOrientStone(Vector3 orientation)
    {
        //_currentStone.ClearCollisions();
        _currentStone.MoveStartToVoxel(_voxelLocation);
        _currentStone.OrientNormal(orientation);
        _currentStone.State = StoneState.Placed;
       
    }

    private void RotateStone(Vector3 _directions)
    {
        ///该变石头方向调用上面单子里的任意一个方向
        ////_currentStone.ClearCollisions();
        //_currentStone.OrientNormal(_directions);
        _currentStone.OrientNormal(transform.position + _directions);


    }

    //private float CheckCollisions()
    //{ 
    //    //int col = _currentStone.CountCollisions();
    //    Debug.Log(col);
    //    return 0;
    //}

    /// <summary>
    /// Gets a stone that has a length that is closer to a length
    /// </summary>
    /// <param name="len"></param>
    /// <returns></returns>
    private bool GetStoneByLength(float len)
    {
        float dif = float.MaxValue;
        Stone stone = null;
        foreach (float realLength in _environment.StonesLengths.Keys)
        {
            var unplacedStones = _environment.StonesLengths[realLength].Where(s => s.State == StoneState.NotPlaced);
            if (unplacedStones.Count() > 0)
            {
                float currentDif = Mathf.Abs(len - realLength);
                if (currentDif < dif)
                {
                    dif = currentDif;
                    stone = unplacedStones.First();
                    break;
                }
            }  
        }

        if (stone != null)
        {
            _currentStone = stone;
            return true;
        }
        
        return false;
    }

    

    #endregion

}
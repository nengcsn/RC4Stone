using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class StoneAgent : Agent
{
    /// <summary>
    /// The agent has 3 discrete action branches
    /// 0: Movment
    /// 1: Rotation
    /// 2: Stone selection
    ///     Either remove a stone or add a stone from the available library
    /// </summary>


    #region Fields and Properties

    //  The voxel where the agent is currently
    private Voxel _voxelLocation;

    //  The VoxelGrid the agent is navigating
    private VoxelGrid _voxelGrid;

    // The environment the agent belongs to
    private MLEnvironment _environment;

    // The normalized position the agent is currently at
    private Vector3 _normalisedIndex;

    private float _voidRatioThreshold;

    bool _placedStone = false;

    Vector3[] _directions = new Vector3[30]// array of vectors of directions 
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
            // Place the current stone in the current voxel
            PlaceCurrentStone();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GetStoneByLength(Random.Range(0.5f, 3.5f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RotateStone(2);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RotateStone(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RotateStone(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RotateStone(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            RotateStone(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            RotateStone(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            RotateStone(8);
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
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveAgent(1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveAgent(2);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveAgent(3);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveAgent(4);
        if (Input.GetKeyDown(KeyCode.W)) MoveAgent(5);
    }

    #endregion

    #region MLAgents methods

    public override void OnEpisodeBegin()
    {
        // 29 Read the target initial void ratio
        _voidRatioThreshold = Academy.Instance.EnvironmentParameters.GetWithDefault("void_ratio", 0.055f);

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

        //try to add reward to Action 
        if (!_frozen)
        {
            int movementAction = actions.DiscreteActions[0];//Set the correct discrete action branch for movement

            if (MoveAgent(movementAction))
            {
                // 50 If action was valid, add reward
                AddReward(0.0001f);
            }
            else
            {
                // 51 Otherwise, apply penalty
                AddReward(-0.0001f);
            }

            int rotationAction = actions.DiscreteActions[1];

            if (RotateStone(rotationAction))
            {
                // 50 If action was valid, add reward
                AddReward(0.0001f);
            }
            else
            {
                // 51 Otherwise, apply penalty
                AddReward(-0.0001f);
            }

            int placeRemoveStoneAction = actions.DiscreteActions[2];
            if (placeRemoveStoneAction == 1)
            {
                if (PlaceLongestStone())
                {
                    // 50 If action was valid, add reward
                    AddReward(0.0001f);
                }
                else
                {
                    // 51 Otherwise, apply penalty
                    AddReward(-0.0001f);
                }
            }
            else if (placeRemoveStoneAction == 2)
            {
                Stone parentStone = null;
                foreach (Stone stone in _environment.GetPlacedStones())
                {
                    if (Util.IsPointInCollider(stone.GoStoneMesh.GetComponent<MeshCollider>(), _voxelLocation.VoxelGO.transform.position))
                    {
                        parentStone = stone;
                    }
                }

                if (parentStone == null)
                    AddReward(-0.0001f);
                else if (parentStone.ResetStone(_environment.PlatePositions[parentStone]))
                {
                    AddReward(0.0001f);
                }
                else
                {
                    AddReward(-0.0001f);
                }

            }



            //Check if the grid is sattisfied and finish the training episode
            if (_environment.GetOccupiedRatio() <= _voidRatioThreshold)
            {

                Debug.Log($"Succeeded with {_voidRatioThreshold}");
                AddReward(1f);
                EndEpisode();
            }


        }

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
        // Normalized index of the agent [3 Observations]
        _normalisedIndex = new Vector3(
            _voxelLocation.Index.x / _voxelGrid.GridSize.x - 1,
            _voxelLocation.Index.y / _voxelGrid.GridSize.y - 1,
            _voxelLocation.Index.z / _voxelGrid.GridSize.z - 1);
        sensor.AddObservation(_normalisedIndex);

        //  Existance of face neighbours and its state (occupied or not) [6 Observations]
        Voxel[] neighbours = _voxelLocation.GetNeighbours();
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] != null)
            {
                //  If neighbour voxel is occupied
                if (neighbours[i].Status != VoxelState.Available) sensor.AddObservation(1);
                // If neighbour voxel is not occupied
                else sensor.AddObservation(2);
            }
            //  If neighbour voxel does not exist
            else sensor.AddObservation(0);

        }

        if (_voxelLocation.Status != VoxelState.Available)
        {
            sensor.AddObservation(0);
        }
        else sensor.AddObservation(1);

        //  Ratio of voids [1 Observation]
        sensor.AddObservation(_environment.GetOccupiedRatio());
        // How many stones have been placed
        // How many stones are left to be placed
        // How many voxels are occupied / percentage
        // And whatever else you can think to check
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Debug.Log("P");
        //    var stone = _environment.GetUnplacedStones().First();
        //    stone.MoveStartToPosition(_voxelLocation.Index);
        //}
        //else if (Input.GetKey(KeyCode.E))
        //{
        //    Debug.Log("E");


        //    //var stone = _environment.GetUnplacedStones().First();
        //    //var CurrentStone = _environment.GetUnplacedStones().First();
        //    //PrefabStone.transform.Rotate(Vector3.up, 5);//Rotate the prefab
        //    //}
        //}
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0;

        if (Input.GetKeyDown(KeyCode.P))
        {
            // Place the current stone in the current voxel
            PlaceCurrentStone();
            discreteActions[0] = 1;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GetStoneByLength(Random.Range(0.5f, 3.5f));
            discreteActions[0] = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RotateStone(2);
            discreteActions[0] = 3;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RotateStone(3);
            discreteActions[0] = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RotateStone(4);
            discreteActions[0] = 5;

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RotateStone(5);
            discreteActions[0] = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            RotateStone(6);
            discreteActions[0] = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            RotateStone(7);
            discreteActions[0] = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            RotateStone(8);
            discreteActions[0] = 9;
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveAgent(1);
            discreteActions[0] = 10;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveAgent(2);
            discreteActions[0] = 11;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveAgent(3);
            discreteActions[0] = 12;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveAgent(4);
            discreteActions[0] = 13;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveAgent(5);
            discreteActions[0] = 14;
        }

    }
    #endregion

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

    private void PlaceCurrentStone()
    {
        _currentStone.ClearOccupied();
        _currentStone.MoveStartToVoxel(_voxelLocation);
        _currentStone.State = StoneState.Placed;

        _placedStone = true;
    }

    private bool RotateStone(int index)
    {
        _currentStone.ClearOccupied();
        _placedStone = true;

        if (index < _directions.Length)
        {
            _currentStone.OrientNormal(_directions[index]);
            return true;
        }
        else if (index == 30) return true;
        else return false;
    }

    //private float CheckCollisions()
    //{ 
    //    //int col = _currentStone.CountCollisions();
    //    Debug.Log(col);
    //    return 0;
    //}

    private bool PlaceLongestStone()
    {
        Dictionary<float, List<Stone>> availableStoneLengths = new Dictionary<float, List<Stone>>();
        foreach (var length in _environment.StonesLengths.Keys)
        {
            var availableStones = _environment.StonesLengths[length].Where(s => s.State == StoneState.NotPlaced).ToList();
            if (availableStones.Count > 0) availableStoneLengths.Add(length, availableStones);
        }
        Stone stone = availableStoneLengths[_environment.StonesLengths.Keys.Max()].First();

        if (stone == null) return false;

        _currentStone = stone;
        PlaceCurrentStone();
        return true;
    }





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

    public void UnfreezeAgent()
    {
        _frozen = false;
    }



    #endregion

}
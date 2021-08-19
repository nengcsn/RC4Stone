using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class MLEnvironment : MonoBehaviour
{
    public VoxelGrid VoxelGrid { get; private set; }
    public List<Stone> Stones { get; private set; }
    private int _stoneDuplicates = 3;
    public Dictionary<Stone, Vector3> PlatePositions;

    private int _startingCount;

    public Dictionary<float, List<Stone>> StonesLengths { get; private set; }

    [SerializeField]
    Text _voidRatio;

    StoneAgent _agent;
    // 01.4 The selected component
    Voxel _selected;



    #region Unity Standard methods
    void Awake()
    {
        Stones = new List<Stone>();
        PlatePositions = new Dictionary<Stone, Vector3>();

        //  Get the Agent from the hierarchy
        _agent = GameObject.FindWithTag("Agent").GetComponent<StoneAgent>();

        CreateStones();
        CreateGridFromFile("Data/activate_voxels_vector");

        Debug.Log(GetVoidRatio());
        Debug.Log(_startingCount);

        OrganiseStones();
        StoreLengths();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectVoxel();
        }
        if (_voidRatio != null)
        {
            _voidRatio.text = $"Current Void Ratio: {GetVoidRatio().ToString("F2")}";
        }

    }
    #endregion

    #region Public Methods

    public IEnumerable<Stone> GetUnplacedStones()
    {
        return Stones.Where(s => s.State == StoneState.NotPlaced);
    }

    public IEnumerable<Stone> GetPlacedStones()
    {
        return Stones.Where(s => s.State != StoneState.NotPlaced);
    }

    public void Restart()
    {
        //CreateGridFromFile("Data/activate_voxels_vector");
        VoxelGrid.Restart();
        foreach (var stone in Stones)
        {
            var pos = PlatePositions[stone];
            stone.MoveStartToPosition(pos);
            stone.OrientNormal(Vector3.forward);
        }
    }

    #endregion

    #region Private Methods

    private void CreateStones()
    {
        var files = Resources.LoadAll<TextAsset>("SavedStones");
        foreach (var file in files)
        {
            for (int i = 0; i < _stoneDuplicates; i++)
            {
                Stone stone = JsonConvert.DeserializeObject<Stone>(file.text);
                stone.CreateLoadedStone();
                Stones.Add(stone);
                stone.SetParent(transform);
            }
        }
    }

    private void CreateGridFromFile(string file)
    {
        string[] lines = Resources.Load<TextAsset>(file).text.Split('\n');
        List<Vector3Int> activeVectors = CSVReader2.ReadVoxels();

        int xSize = activeVectors.Max(v => v.x) + 1;
        int ySize = activeVectors.Max(v => v.y) + 1;
        int zSize = activeVectors.Max(v => v.z) + 1;
        Vector3Int gridSize = new Vector3Int(xSize, ySize, zSize);

        VoxelGrid = new VoxelGrid(gridSize, transform.position, 1f, parent: transform);

        foreach (var index in activeVectors)
        {
            VoxelGrid.ActivateVoxel(index);
        }
        _startingCount = VoxelGrid.GetVoxels().Count(v => v.Status == VoxelState.Available);
    }

    private void OrganiseStones()
    {
        int rowCount = Mathf.CeilToInt(Stones.Count / 5f);

        int i = 0;
        float step = 3;
        Vector3 plateOrigin = new Vector3(-(8 * step), 0, 0);
        for (int x = 0; x < rowCount; x++)
        {
            for (int z = 0; z < 50; z++)
            {
                if (i < Stones.Count)
                {
                    var stone = Stones[i];
                    var pos = plateOrigin + new Vector3(x * step, 0, (z * step) + 1);
                    stone.MoveStartToPosition(pos);
                    stone.OrientNormal(Vector3.forward);
                    PlatePositions.Add(stone, pos);
                    i++;
                }
            }
        }
    }

    private void StoreLengths()
    {
        StonesLengths = new Dictionary<float, List<Stone>>();
        foreach (Stone stone in Stones)
        {
            if (StonesLengths.ContainsKey(stone.Length))
            {
                var stones = StonesLengths[stone.Length];
                stones.Add(stone);
            }
            else
            {
                List<Stone> stones = new List<Stone>();
                stones.Add(stone);
                StonesLengths.Add(stone.Length, stones);
            }

        }
    }

    #endregion





    //19 Create the method to select component by clicking
    /// <summary>
    /// Select a component and assign Agent position with mouse click
    /// </summary>
    public void SelectVoxel()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.CompareTag("Voxel"))
            {
                // 20 Assign clicked component to the selected variable
                _selected = objectHit.GetComponent<VoxelTrigger>().Voxel;

                _agent.GoToVoxel(_selected.Index);
            }
        }
        else
        {
            _selected = null;
        }
    }

    public float GetVoidRatio()//occcupied voxel ratio
    {
        float current = VoxelGrid.GetVoxels().Count(v => v.Status == VoxelState.Available) * 1f;
        return current / _startingCount;
    }

}

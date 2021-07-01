using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class MLEnvironment : MonoBehaviour
{
    public VoxelGrid VoxelGrid;
    public List<Stone> Stones;

    StoneAgent _agent;
    // 01.4 The selected component
    Component _selected;



    #region Unity Standard methods
    // Start is called before the first frame update
    void Start()
    {

        Stones = new List<Stone>();

        //  Get the Agent from the hierarchy
        _agent = transform.Find("StoneAgent").GetComponent<StoneAgent>();

        var files = Resources.LoadAll<TextAsset>("SavedStones");
        foreach (var file in files)
        {
            Stone stone = JsonConvert.DeserializeObject<Stone>(file.text);
            stone.CreateLoadedStone();
            Stones.Add(stone);
        }


        

        string[] lines = Resources.Load<TextAsset>("Data/activate_voxels_vector").text.Split('\n');
        List<Vector3Int> activeVectors = CSVReader2.ReadVoxels();

        int xSize = activeVectors.Max(v => v.x) + 1;
        int ySize = activeVectors.Max(v => v.y) + 1;
        int zSize = activeVectors.Max(v => v.z) + 1;
        Vector3Int gridSize = new Vector3Int(xSize, ySize, zSize);
        
        VoxelGrid = new VoxelGrid(gridSize, transform.position, 1f);

        foreach (var index in activeVectors)
        {
            VoxelGrid.ActivateVoxel(index);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

     #region Public Methods
    
    public IEnumerable<Stone> GetUnplacedStones()
    {
        return Stones.Where(s => s.State == StoneState.NotPlaced);
    }

    //// 37.1 Create method to the component at a given voxel
    ///// <summary>
    ///// Get the component located at a <see cref="Voxel"/>
    ///// </summary>
    ///// <param name="voxel"></param>
    ///// <returns>The <see cref="Component"/> at the voxel</returns>
    //public Component GetComponentAtVoxel(Voxel voxel)
    //{
    //    return _components[voxel.Index.x, voxel.Index.y, voxel.Index.z];
    //}

    #endregion

    //19 Create the method to select component by clicking
    /// <summary>
    /// Select a component and assign Agent position with mouse click
    /// </summary>
    private void SelectComponent()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.CompareTag("Component"))
            {
                // 20 Assign clicked component to the selected variable
                _selected = objectHit.GetComponent<Component>();

                // 75 Set the position of the agent at the clicked voxel
                var pos = objectHit.transform.localPosition;
                Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
                //_agent.GoToVoxel(posInt);
            }
        }
        else
        {
            _selected = null;
        }
    }

}

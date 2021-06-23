using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class MLEnvironment : MonoBehaviour
{
    public VoxelGrid VoxelGrid;
    public List<Stone> Stones;

    #region Unity Standard methods
    // Start is called before the first frame update
    void Start()
    {

        Stones = new List<Stone>();

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

    #endregion
}

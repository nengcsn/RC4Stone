using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public enum StoneState { Placed, NotPlaced };
public class Stone : IEquatable<Stone>
{
    GameObject _goStoneMesh;
    
    [JsonProperty]
    float[] _localPosition;

    public string PrefabName;

    GameObject _goStoneParent;
    
    [JsonProperty]
    float[] _parentPosition;

    Vector3 _centerOfGravity;

    Vector3 _stoneNormal;
    [JsonProperty]
    float[] _normalArray;
    //List<Voxel> _voxels;
    public float Length { get; private set; }

    public List<NormalGroup> StoneNormals { get; private set; }
    public List<MeshTriangle> TriangleMesh { get; private set; }

    public float Weight { get; private set; }
    public float VoxelSize { get; private set; }
    
    private GameObject[,,] _voxels;//We don't want to store this, we only generate the voxels to get the normal and mass of the stone and then we forget about them
    //The start of the normal becomes the anchor point for your stone
    //store the start and endpoint of the normal in a gameobjects as children of the stone object as references.
    
    
    Material _material;

    //public Vector3 Normal;
    public float NormalTollerance;

    private LineRenderer _normalRenderer;

    [JsonIgnore]
    public GameObject NormalStart { get; private set; }
    
    [JsonProperty]
    float[] _normalStart;

    [JsonIgnore]
    public GameObject NormalEnd { get; private set; }
    
    [JsonProperty]
    float[] _normalEnd;

    [JsonIgnore]
    public bool IsEndUp => NormalEnd.transform.localPosition.y > NormalStart.transform.localPosition.y;


    List<Stone> neighbours;

    List<SpringJoint> joints;

    public StoneState State;
    internal object transform;

    // Store the voxels the stone is touching
    private List<Voxel> _currentOccupied;


    public Stone(GameObject goStone)
    {
        VoxelSize = 0.1f;
        _goStoneMesh = goStone;
        State = StoneState.NotPlaced;
    }

    #region Voxelization
    public void VoxeliseMesh()
    {
        //Get bounds of the mesh
        //divide bounds into voxelsize
        //create voxelgrid in the bounds of the mesh
        //Check which voxels are inside the mesh
        //set the voxels active

        //Make a variable that store stoneMesh.bounds
        Mesh stoneMesh = _goStoneMesh.GetComponent<MeshFilter>().mesh;
        Vector3 centerPoint = stoneMesh.bounds.center;

        int gridX = Mathf.CeilToInt(stoneMesh.bounds.size.x / VoxelSize);
        int gridY = Mathf.CeilToInt(stoneMesh.bounds.size.y / VoxelSize);
        int gridZ = Mathf.CeilToInt(stoneMesh.bounds.size.z / VoxelSize);

        //float r = Mathf.Min(extents.x, extents.y, extents.z) / 10;
        _voxels = new GameObject[gridX, gridY, gridZ];
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                for (int z = 0; z < gridZ; z++)
                {
                    Vector3 localPosition = stoneMesh.bounds.min + (new Vector3(x, y, z) * VoxelSize);
                    if (Util.IsPointInCollider(_goStoneMesh.GetComponent<MeshCollider>(), _goStoneMesh.transform.position + _goStoneMesh.transform.TransformVector(localPosition)))
                    {
                        GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        voxel.transform.SetParent(_goStoneMesh.transform);
                        voxel.transform.localEulerAngles = Vector3.zero;
                        voxel.transform.localPosition = localPosition;
                        voxel.transform.localScale = Vector3.one * VoxelSize;
                        GameObject.Destroy(voxel.GetComponent<Collider>());
                        voxel.transform.GetComponent<MeshRenderer>().enabled = false;

                        _voxels[x, y, z] = voxel;
                    }
                }
            }
        }

        GetStoneNormal();
        CreateParent();
        VisualiseStoneNormal();
    }

    public void GetStoneNormal()
    {
        List<GameObject> voxelList = new List<GameObject>();

        for (int x = 0; x < _voxels.GetLength(0); x++)
            for (int y = 0; y < _voxels.GetLength(1); y++)
                for (int z = 0; z < _voxels.GetLength(2); z++)
                {
                    if (_voxels[x, y, z] != null) voxelList.Add(_voxels[x, y, z]);
                }

        //Vector3 longestLine = Vector3.zero;
        _stoneNormal = Vector3.zero;

        for (int i = 0; i < voxelList.Count; i++)
        {
            for (int j = 0; j < voxelList.Count; j++)
            {
                //Vector3 line = voxelList[i].transform.position - voxelList[j].transform.position;
                Vector3 line = voxelList[i].transform.position - voxelList[j].transform.position;

                if (line.magnitude > Length)
                {
                    Length = line.magnitude;
                    _stoneNormal = line.normalized;
                    NormalStart = voxelList[i];
                    NormalEnd = voxelList[j];
                    if (Vector3.Distance(NormalStart.transform.localPosition, Vector3.zero)
                        > Vector3.Distance(NormalEnd.transform.localPosition, Vector3.zero))
                    {
                        NormalStart = voxelList[j];
                        NormalEnd = voxelList[i];
                        //_stoneNormal = (NormalEnd.transform.localPosition - NormalStart.transform.localPosition).normalized;
                        _stoneNormal = -_stoneNormal;
                    }
                }
            }
        }
        _stoneNormal = -_stoneNormal;
    }

    public void VisualiseStoneNormal()
    {
        if (_normalRenderer == null)
        {
            _normalRenderer = _goStoneMesh.AddComponent<LineRenderer>();
        }

        _normalRenderer.positionCount = 2;
        _normalRenderer.SetPosition(0, NormalStart.transform.localPosition);
        _normalRenderer.SetPosition(1, NormalEnd.transform.localPosition);
        _normalRenderer.startWidth = 0.1f;
        _normalRenderer.endWidth = 0.1f;

        if (_material == null)
            _material = _goStoneMesh.GetComponent<MeshRenderer>().material;
        _goStoneMesh.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Material/Transparent");

    }


    public void OrientNormal(Vector3 normalTarget)
    {
        Quaternion rotation = Util.RotateFromTo(_stoneNormal, normalTarget);
        _goStoneParent.transform.localRotation = rotation;
        _stoneNormal = NormalEnd.transform.localPosition - NormalStart.transform.localPosition;
        //GetStoneNormal();
        //CreateParrent(); //refactor code to work with children/parrent
    }

    public void SetRotation(Quaternion rotation)
    {
        _goStoneParent.transform.localRotation = rotation;
        GetStoneNormal();
    }

    public Quaternion GetRotation()
    {
        return _goStoneParent.transform.localRotation;
    }

    public void MoveStartToPosition(Vector3 target)
    {
        //Move start point to target
        _goStoneParent.transform.position = target;
        //VisualiseStoneNormal();
    }

    public void MoveStartToVoxel(Voxel voxel)
    {
        ClearOccupied();
        _goStoneParent.transform.localPosition = voxel.Index;
    }

    private void NormalCorrect()
    {
        RaycastHit hit;
        Vector3 origin = NormalStart.transform.position + (_stoneNormal * VoxelSize * 1.5f);
        var orGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        orGo.transform.position = origin;
        orGo.transform.localScale = Vector3.one * 0.05f;
        orGo.transform.parent = _goStoneParent.transform;
        if (Physics.Raycast(origin, _stoneNormal, out hit, VoxelSize))
        {
            Debug.Log($"{_goStoneParent.transform.name} is right");
        }
    }

    #endregion


    void CreateParent()
    {
        GameObject parent = new GameObject($"{ _goStoneMesh.name }_parent");
        parent.transform.position = NormalStart.transform.position;
        parent.name = PrefabName + "(Parent)";
        //parent.transform.rotation = Quaternion.FromToRotation(parent.transform.up, _stoneNormal) * parent.transform.rotation;
        _goStoneMesh.transform.SetParent(parent.transform);
        _goStoneParent = parent;
        _goStoneParent.tag = "Stone";
    }

    public void WriteStoneToJson()
    {
        _localPosition = _goStoneMesh.transform.localPosition.AsArray();
        PrefabName = _goStoneMesh.name;
        _parentPosition = _goStoneParent.transform.position.AsArray();
        _normalArray = _stoneNormal.AsArray();
        _normalStart = NormalStart.transform.localPosition.AsArray();
        _normalEnd = NormalEnd.transform.localPosition.AsArray();
        try
        {
            string content = JsonConvert.SerializeObject(this);
            string folder = Path.Combine(Application.dataPath, "Resources/SavedStones");
            string file = Path.Combine(folder, this.PrefabName + ".json");

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            System.IO.File.WriteAllText(file, content);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }
        
    }

    public void CreateLoadedStone()
    {
        State = StoneState.NotPlaced;
        _stoneNormal = _normalArray.AsVector();

        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Stones/{PrefabName}");
        _goStoneMesh = GameObject.Instantiate(prefab);
        
        //var empty = new GameObject();
        NormalStart = new GameObject();
        NormalStart.transform.parent = _goStoneMesh.transform;
        NormalStart.transform.localPosition = _normalStart.AsVector();
        NormalStart.transform.name = "Start";

        NormalEnd = new GameObject();
        NormalEnd.transform.parent = _goStoneMesh.transform;
        NormalEnd.transform.localPosition = _normalEnd.AsVector();
        NormalEnd.transform.name = "End";

        CreateParent();
        _goStoneMesh.transform.localPosition = _localPosition.AsVector();
        _goStoneMesh.GetComponent<StoneTrigger>().Stone = this;
    }

    public void SetParent(Transform parent)
    {
        _goStoneParent.transform.parent = parent;
    }

    //public int CountCollisions()
    //{
    //    return _goStoneMesh.GetComponent<StoneTrigger>().CountVoxels();
    //}

    //public void ClearCollisions()
    //{
    //    _goStoneMesh.GetComponent<StoneTrigger>().ClearCollisions();
    //}

    public void ClearOccupied()
    {
        if (_currentOccupied != null)
        {
            foreach (Voxel voxel in _currentOccupied)
            {
                voxel.Status = VoxelState.Available;
            }
        }
        _currentOccupied = new List<Voxel>();
    }

    public void AddOccupiedVoxel(Voxel v)
    {
        _currentOccupied.Add(v);
    }

    public void OccupyVoxels()
    {
        foreach (Voxel voxel in _currentOccupied)
        {
            voxel.Status = VoxelState.Occupied;
        }
    }

    #region Equality checks

    public bool Equals(Stone other)
    {
        return (other != null) && (GetHashCode() == other.GetHashCode());
    }


    public override int GetHashCode()
    {
        return _goStoneParent.GetHashCode();
    }

    #endregion
}

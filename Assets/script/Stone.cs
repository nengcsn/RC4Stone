using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Stone
{
    GameObject _goStoneMesh;
    GameObject _goStoneTransform;
    Vector3 _centerOfGravity;
    Vector3 _stoneNormal;
    //List<Voxel> _voxels;
    public float Length { get; private set; }
    List<NormalGroup> _stoneNormals;
    List<MeshTriangle> _triangleMesh;
    float _weight;
    float _voxelSize = 0.1f;
    private GameObject[,,] _voxels;//We don't want to store this, we only generate the voxels to get the normal and mass of the stone and then we forget about them
    //The start of the normal becomes the anchor point for your stone
    //store the start and endpoint of the normal in a gameobjects as children of the stone object as references.
    Material _material;

    //public Vector3 Normal;
    public float NormalTollerance;

    private LineRenderer _normalRenderer;


    public GameObject NormalStart { get; private set; }
    public GameObject NormalEnd { get; private set; }

    public bool IsEndUp => NormalEnd.transform.localPosition.y > NormalStart.transform.localPosition.y;

    List<Stone> neighbours;
    List<SpringJoint> joints;

    public Stone(GameObject goStone)
    {
        _goStoneMesh = goStone;
    }
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

        int gridX = Mathf.CeilToInt(stoneMesh.bounds.size.x / _voxelSize);
        int gridY = Mathf.CeilToInt(stoneMesh.bounds.size.y / _voxelSize);
        int gridZ = Mathf.CeilToInt(stoneMesh.bounds.size.z / _voxelSize);

        //float r = Mathf.Min(extents.x, extents.y, extents.z) / 10;
        _voxels = new GameObject[gridX, gridY, gridZ];
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                for (int z = 0; z < gridZ; z++)
                {
                    Vector3 localPosition = stoneMesh.bounds.min + (new Vector3(x, y, z) * _voxelSize);
                    if (Util.IsPointInCollider(_goStoneMesh.GetComponent<MeshCollider>(), _goStoneMesh.transform.position + _goStoneMesh.transform.TransformVector(localPosition)))
                    {
                        GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        voxel.transform.SetParent(_goStoneMesh.transform);
                        voxel.transform.localEulerAngles = Vector3.zero;
                        voxel.transform.localPosition = localPosition;
                        voxel.transform.localScale = Vector3.one * _voxelSize;
                        GameObject.Destroy(voxel.GetComponent<Collider>());
                        voxel.transform.GetComponent<MeshRenderer>().enabled = false;

                        _voxels[x, y, z] = voxel;
                    }
                }
            }
        }
        //_goStone.GetComponent<MeshRenderer>().enabled = false;



        GetStoneNormal();
        CreateParent();
        //NormalCorrect();
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
        _normalRenderer.SetPosition(0, NormalStart.transform.position);
        _normalRenderer.SetPosition(1, NormalEnd.transform.position);
        _normalRenderer.startWidth = 0.1f;
        _normalRenderer.endWidth = 0.1f;

        if (_material == null)
            _material = _goStoneMesh.GetComponent<MeshRenderer>().material;
        _goStoneMesh.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Material/Transparent");

    }


    public void OrientNormal(Vector3 normalTarget)
    {
        Quaternion rotation = Util.RotateFromTo(_stoneNormal, normalTarget);
        _goStoneTransform.transform.localRotation = rotation;
        _stoneNormal = normalTarget * Length;
        GetStoneNormal();
        //CreateParrent(); //refactor code to work with children/parrent
    }

    public void SetRotation(Quaternion rotation)
    {
        _goStoneTransform.transform.localRotation = rotation;
        GetStoneNormal();
    }

    public Quaternion GetRotation()
    {
        return _goStoneTransform.transform.localRotation;
    }

    public void MoveStartToPosition(Vector3 target)
    {
        //Move start point to target
        _goStoneTransform.transform.position = target;
        VisualiseStoneNormal();

    }

    private void NormalCorrect()
    {
        RaycastHit hit;
        Vector3 origin = NormalStart.transform.position + (_stoneNormal * _voxelSize * 1.5f);
        var orGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        orGo.transform.position = origin;
        orGo.transform.localScale = Vector3.one * 0.05f;
        orGo.transform.parent = _goStoneTransform.transform;
        if (Physics.Raycast(origin, _stoneNormal, out hit, _voxelSize))
        {
            Debug.Log($"{_goStoneTransform.transform.name} is right");
        }
    }


    void CreateParent()
    {
        GameObject parent = new GameObject($"{ _goStoneMesh.name }_parent");
        parent.transform.position = NormalStart.transform.position;
        //parent.transform.rotation = Quaternion.FromToRotation(parent.transform.up, _stoneNormal) * parent.transform.rotation;
        _goStoneMesh.transform.SetParent(parent.transform);
        _goStoneTransform = parent;
    }
}

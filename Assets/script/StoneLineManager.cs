using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoneLineManager : MonoBehaviour
{
    //Voxelise the mesh
    Stone[] Stones;
    public GameObject LineStart;
    public GameObject LineEnd;
    public LineRenderer LineRenderer;
    private Vector3 _targetNormal;


    void Start()
    {
        //Find all the stones in my project and create stone objects
        Stones = GameObject.FindGameObjectsWithTag("Stone").Select(s => new Stone(s)).ToArray();
        _targetNormal = LineEnd.transform.position - LineStart.transform.position;
        LineRenderer.positionCount = 2;
        LineRenderer.SetPosition(0, LineStart.transform.position);
        LineRenderer.SetPosition(1, LineEnd.transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 targetPosition = LineStart.transform.position;
            //Rotate stones according to the longest directions and voxelise them
            for (int i = 0; i < Stones.Length; i++)
            {
                //Stones[i].PlaceStoneByLongestDirection();
                Stones[i].VoxeliseMesh();//变网格
                Stones[i].OrientNormal(_targetNormal);//旋转
                Stones[i].MoveStartToPosition(targetPosition);
                Debug.Log(Stones[i].NormalStart.transform.position);
                Debug.Log(Stones[i].NormalEnd.transform.position);
                targetPosition = Stones[i].NormalStart.transform.position;
            }
        }
    }
}
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoneLineManager : MonoBehaviour
{
    //Voxelise the mesh
    Stone[] Stones;
    public GameObject LineStart;
    public GameObject LineEnd;
    public LineRenderer LineRenderer;
    private Vector3 _targetNormal;


    void Start()
    {
        //Find all the stones in my project and create stone objects
        Stones = GameObject.FindGameObjectsWithTag("Stone").Select(s => new Stone(s)).ToArray();
        _targetNormal = LineEnd.transform.position - LineStart.transform.position;
        LineRenderer.positionCount = 2;
        LineRenderer.SetPosition(0, LineStart.transform.position);
        LineRenderer.SetPosition(1, LineEnd.transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 targetPosition = LineStart.transform.position;
            //Rotate stones according to the longest directions and voxelise them
            for (int i = 0; i < Stones.Length; i++)
            {
                //Stones[i].PlaceStoneByLongestDirection();
                Stones[i].VoxeliseMesh();
                Stones[i].OrientNormal(_targetNormal);
                Stones[i].MoveStartToPosition(targetPosition);
                targetPosition = Stones[i].NormalEnd.transform.position;//修改为Normalstart 头尾相连
            }
        }
    }
}*/
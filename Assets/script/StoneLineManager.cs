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
    public GameObject LineStart1;
    public GameObject LineEnd1;
    public LineRenderer LineRenderer;
    public LineRenderer LineRenderer1;
    private Vector3 _targetNormal;
    private Vector3 _targetNormal2;


    void Start()
    {
        //Find all the stones in my project and create stone objects
        Stones = GameObject.FindGameObjectsWithTag("Stone").Select(s => new Stone(s)).ToArray();
        _targetNormal = LineEnd.transform.position - LineStart.transform.position;
        _targetNormal2 = LineEnd1.transform.position - LineStart1.transform.position;
        LineRenderer.positionCount = 2;
        LineRenderer.SetPosition(0, LineStart.transform.position);
        LineRenderer.SetPosition(1, LineEnd.transform.position);
        LineRenderer1.positionCount = 2;
        LineRenderer1.SetPosition(0, LineStart1.transform.position);
        LineRenderer1.SetPosition(1, LineEnd1.transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 targetPosition = LineStart.transform.position;
            //Rotate stones according to the longest directions and voxelise them
            for (int i = 0; i < Stones.Length; i++)
            {
                if (i < Stones.Length / 2)
                {

                    //Stones[i].PlaceStoneByLongestDirection();
                    Stones[i].VoxeliseMesh();
                    Stones[i].OrientNormal(_targetNormal);
                    Stones[i].MoveStartToPosition(targetPosition);
                    targetPosition = Stones[i].NormalStart.transform.position;
                    Debug.Log(Stones[i].NormalStart.transform.position);
                    Debug.Log(Stones[i].NormalEnd.transform.position);
                }
                else
                {
                    targetPosition = LineStart1.transform.position;
                    Stones[i].VoxeliseMesh();
                    Stones[i].OrientNormal(_targetNormal);
                    Stones[i].MoveStartToPosition(targetPosition);
                    targetPosition = Stones[i].NormalStart.transform.position;
                }
            }
        }
    }
}


//if(NormalStart.tranform.position-LineEnd.transform.position>(0,0,0))



/*
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
                targetPosition = Stones[i].NormalEnd.transform.position;
            }
        }
    }
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoneLineManager : MonoBehaviour
{
    //Voxelise the mesh
    Stone[] _stones;
    List<GameObject> _lines;
    List<Vector3> _targetNormals;
    //public GameObject LineStart;
    //public GameObject LineEnd;
    //public GameObject LineStart1;
    //public GameObject LineEnd1;
    //public LineRenderer LineRenderer;
    //public LineRenderer LineRenderer1;
    //private Vector3 _targetNormal;
    //private Vector3 _targetNormal2;


    void Start()
    {
        //Find all the stones in my project and create stone objects
        _stones = GameObject.FindGameObjectsWithTag("Stone").Select(s => new Stone(s)).ToArray();
        foreach (var stone in _stones) stone.VoxeliseMesh();

        _lines = GameObject.FindGameObjectsWithTag("Line").ToList();
        _targetNormals = new List<Vector3>();
        foreach (var line in _lines)
        {
            var startPoint = line.transform.Find("Start");
            var endPoint = line.transform.Find("End");
            var renderer = line.transform.Find("Renderer").GetComponent<LineRenderer>();
            renderer.positionCount = 2;
            renderer.SetPosition(0, startPoint.position);
            renderer.SetPosition(1, endPoint.position);
            Vector3 newNormal = endPoint.position - startPoint.position;
            _targetNormals.Add(newNormal);
        }




        //_targetNormal = LineEnd.transform.position - LineStart.transform.position;
        //_targetNormal2 = LineEnd1.transform.position - LineStart1.transform.position;
        //LineRenderer.positionCount = 2;
        //LineRenderer.SetPosition(0, LineStart.transform.position);
        //LineRenderer.SetPosition(1, LineEnd.transform.position);
        //LineRenderer1.positionCount = 2;
        //LineRenderer1.SetPosition(0, LineStart1.transform.position);
        //LineRenderer1.SetPosition(1, LineEnd1.transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlaceStones());
            //Queue<Stone> availableStones = new Queue<Stone>(_stones);
            //for (int i = 0; i < _lines.Count; i++)
            //{
            //    var line = _lines[i];
            //    var length = _targetNormals[i].magnitude;
            //    float stonesLength = 0;
            //    Vector3 startPosition = line.transform.Find("Start").transform.position;
            //    while (stonesLength < length && availableStones.Count > 0)
            //    {
            //        var nextStone = availableStones.Dequeue();
            //        stonesLength += nextStone.Length;
            //        nextStone.OrientNormal(_targetNormals[i]);
            //        nextStone.MoveStartToPosition(startPosition);
            //        startPosition = nextStone.NormalStart.transform.position;

            //    }

            //}

            
        }
    }

    IEnumerator PlaceStones()
    {
        Queue<Stone> availableStones = new Queue<Stone>(_stones);
        for (int i = 0; i < _lines.Count; i++)
        {
            var line = _lines[i];
            var length = _targetNormals[i].magnitude;
            float stonesLength = 0;
            Vector3 startPosition = line.transform.Find("Start").transform.position;
            while (stonesLength < length && availableStones.Count > 0)
            {
                var nextStone = availableStones.Dequeue();
                stonesLength += nextStone.Length;
                nextStone.OrientNormal(_targetNormals[i]);
                nextStone.MoveStartToPosition(startPosition);
                startPosition = nextStone.NormalStart.transform.position;
                yield return new WaitForSeconds(0.5f);
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
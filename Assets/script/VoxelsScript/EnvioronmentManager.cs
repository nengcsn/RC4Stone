using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvioronmentManager : MonoBehaviour
{
    public VoxelGrid _voxelGrid;
    // Start is called before the first frame update
    void Start()
    {
        _voxelGrid = new VoxelGrid(new Vector3Int(15,16,15), transform.position,1f);

        string[] lines = Resources.Load<TextAsset>("Data/activate_voxels_vector").text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            var test = CSVReader2.ReadVoxelsVector(i);


            print("Active Vovel" + i + " is" + test);



            Instantiate(Resources.Load<GameObject>("Prefabs/cubeAlive"), test, Quaternion.identity);



        }



    }

    //// 31 Create method to reset the environment
    ///// <summary>
    ///// Resets the environment by setting all components' state as 0
    ///// </summary>
    //public void ResetEnvironment()
    //{
    //    foreach (var component in _components)
    //    {
    //        component.ChangeState(0);
    //    }
    //}


    // Update is called once per frame
    void Update()
    {
        
    }
}

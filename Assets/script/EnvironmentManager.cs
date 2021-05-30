using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnvironmentManager : MonoBehaviour
{


    //public GameObject prefabStart = Resources.Load<GameObject>("Prefabs/pointstart");
    //public GameObject prefabEnd = Resources.Load<GameObject>("Prefabs/pointend");


    // Start is called before the first frame update
    void Start()
    {
        string[] lines = Resources.Load<TextAsset>("Data/start_points").text.Split('\n');
        for (int i =0; i<lines.Length; i++)
        {
            var testStart = CSVReader.ReadStartPoints(i);
            var testEnd = CSVReader.ReadEndPoints(i);

            print("startpoint"+i+" is" + testStart);
            print("endpoint" +i+ " is" + testEnd);



            Instantiate(Resources.Load<GameObject>("Prefabs/pointstart"), testStart, Quaternion.identity);
            Instantiate(Resources.Load<GameObject>("Prefabs/pointend"), testEnd, Quaternion.identity);
        }
        


        /*
        var test1 = CSVReader.ReadStartPoints(2);
        print("startpoint is"+test1);
        var test2 = CSVReader.ReadEndPoints(2);
        print("endpoint is"+test2);
        */

    }
}

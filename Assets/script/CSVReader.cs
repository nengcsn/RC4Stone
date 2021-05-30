using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CSVReader
{
    
    public static Vector3 ReadStartPoints(int pointNumber)
    {

        string fileContent = Resources.Load<TextAsset>("Data/start_points").text;

        string[] lines = fileContent.Split('\n');
        string[] allStartPointsX = lines.Select(l => l.Split(',')[0]).ToArray();
        string[] allStartPointsY = lines.Select(l => l.Split(',')[1]).ToArray();
        string[] allStartPointsZ = lines.Select(l => l.Split(',')[2]).ToArray();
        float startX = float.Parse(allStartPointsX[pointNumber]);
        float startY = float.Parse(allStartPointsY[pointNumber]);
        float startZ = float.Parse(allStartPointsZ[pointNumber]);



        var lineStart = GameObject.Find("pointstart");
        lineStart.transform.position= new Vector3(startX, startY, startZ);
        return lineStart.transform.position;


        
    }

    public static Vector3 ReadEndPoints(int pointNumber)
    {

        string fileContent = Resources.Load<TextAsset>("Data/end_points").text;

        string[] lines = fileContent.Split('\n');
        string[] allEndPointsX = lines.Select(l => l.Split(',')[0]).ToArray();
        string[] allEndPointsY = lines.Select(l => l.Split(',')[1]).ToArray();
        string[] allEndPointsZ = lines.Select(l => l.Split(',')[2]).ToArray();
        float endX = float.Parse(allEndPointsX[pointNumber]);
        float endY = float.Parse(allEndPointsY[pointNumber]);
        float endZ = float.Parse(allEndPointsZ[pointNumber]);



        var lineEnd = GameObject.Find("pointend");
        lineEnd.transform.position = new Vector3(endX, endY, endZ);
        return lineEnd.transform.position;

    }
}

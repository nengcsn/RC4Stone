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
        lineStart.transform.position = new Vector3(startX, startY, startZ);
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

    public static List<Line> ReadLines(string startPoints, string endPoints)
    {
        List<Line> result = new List<Line>();
        
        string startContent = Resources.Load<TextAsset>($"Data/{startPoints}").text;
        string endContent = Resources.Load<TextAsset>($"Data/{endPoints}").text;

        string[] startCoords = startContent.Split('\n');
        string[] allStartPointsX = startCoords.Select(l => l.Split(',')[0]).ToArray();
        string[] allStartPointsY = startCoords.Select(l => l.Split(',')[2]).ToArray();
        string[] allStartPointsZ = startCoords.Select(l => l.Split(',')[1]).ToArray();

        string[] endCoords = endContent.Split('\n');
        string[] allEndPointsX = endCoords.Select(l => l.Split(',')[0]).ToArray();
        string[] allEndPointsY = endCoords.Select(l => l.Split(',')[2]).ToArray();
        string[] allEndPointsZ = endCoords.Select(l => l.Split(',')[1]).ToArray();

        for (int i = 0; i < startCoords.Length; i++)
        {
            Vector3 startPoint = new Vector3(float.Parse(allStartPointsX[i]), float.Parse(allStartPointsY[i]), float.Parse(allStartPointsZ[i]));
            Vector3 endPoint = new Vector3(float.Parse(allEndPointsX[i]), float.Parse(allEndPointsY[i]), float.Parse(allEndPointsZ[i]));

            Line line = new Line(startPoint, endPoint);
            result.Add(line);
        }

        return result;
    }
}

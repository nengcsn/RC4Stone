using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LoadTest : MonoBehaviour
{
    List<Stone> _stones;
    // Start is called before the first frame update
    void Start()
    {
        _stones = new List<Stone>();

        var files = Resources.LoadAll<TextAsset>("SavedStones");
        foreach (var file in files)
        {
            Stone stone = JsonConvert.DeserializeObject<Stone>(file.text);
            stone.CreateLoadedStone();
            _stones.Add(stone);
        }

        for (int i = 0; i < _stones.Count; i ++)
        {
            var stone = _stones[i];
            Vector3 point = new Vector3(i * 5, 0, 0);
            stone.MoveStartToPosition(point);
            stone.OrientNormal(Vector3.up);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

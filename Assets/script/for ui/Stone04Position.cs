using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stone04Position : MonoBehaviour


{
    private Vector3 stonePosition04 = GameObject.Find("Stone_4").transform.position;
    public Text positionText04;

    private void Update()
    {
        positionText04.text = "Stone_4 Position:" + stonePosition04;


        //
        positionText04 = GameObject.Find("TextStone04").GetComponent<Text>();
        positionText04.transform.position = GameObject.Find("Stone_4").transform.position;
    }

}

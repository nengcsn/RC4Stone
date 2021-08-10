
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StonePosition : MonoBehaviour
{
    private Vector3 stonePosition = GameObject.Find("Stone_1").transform.position;
    public Text positionText;

    private void Update()
    {
        positionText.text = "Stone_1 Position:" + stonePosition;


        //
        positionText = GameObject.Find("TextStone01").GetComponent<Text>();
        positionText.transform.position = GameObject.Find("Stone_1").transform.position;
    }

}

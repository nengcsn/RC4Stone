using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stone02Position : MonoBehaviour

{
    private Vector3 stonePosition02 = GameObject.Find("Stone_2").transform.position;
    public Text positionText02;

    private void Update()
    {
        positionText02.text = "Stone_2 Position:" + stonePosition02;


        //
        positionText02 = GameObject.Find("TextStone02").GetComponent<Text>();
        positionText02.transform.position = GameObject.Find("Stone_2").transform.position;
    }

}
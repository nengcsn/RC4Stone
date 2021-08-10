using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stone03Position : MonoBehaviour


{
    private Vector3 stonePosition03 = GameObject.Find("Stone_3").transform.position;
    public Text positionText03;

    private void Update()
    {
        positionText03.text = "Stone_3 Position:" + stonePosition03;


        //
        positionText03 = GameObject.Find("TextStone03").GetComponent<Text>();
        positionText03.transform.position = GameObject.Find("Stone_3").transform.position;
    }

}

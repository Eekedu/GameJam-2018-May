using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaker : MonoBehaviour {


    public ControllerJoiner prefControllerJoiner;

	// Use this for initialization
	void Start () {
        float fXStart = Screen.width * -0.75f;
        float fXStep = Screen.width * 0.5f;
        float fYStart = Screen.height * -0.55f;
        float fYStep = Screen.height * 0.5f;
        int iControllerIndex = 1;
        for (int Y = 0; Y < 2; Y++)
        {
            for (int X = 0; X < 2; X++)
            {
                ControllerJoiner cjoin = Instantiate(prefControllerJoiner, this.transform);
                cjoin.gameObject.transform.localPosition = new Vector3(fXStart + (fXStep * (X + 1)), fYStart - (fYStep * (Y - 1)));
                cjoin.SetControllerNumber(iControllerIndex);
                iControllerIndex++;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : GameManager {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override int GetControllerCount()
    {
        return 2;
    }

    public override int GetControllerIndex(int index)
    {
        switch (index)
        {
            case 0: return 1;
            case 1: return 2;
        }
        return -1;
    }
}

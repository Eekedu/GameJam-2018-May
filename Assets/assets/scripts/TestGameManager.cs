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
        return 3;
    }

    public override int GetControllerIndex(int index)
    {
        return index + 1;
        switch (index)
        {
            case 0: return 1;
            case 1: return 2;
        }
        return -1;
    }
}

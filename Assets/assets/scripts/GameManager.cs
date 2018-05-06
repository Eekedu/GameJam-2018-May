using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    bool[] m_bActiveController;

	// Use this for initialization
	void Start () {
        m_bActiveController = new bool[4];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetControllerActive(int iIndex,bool bState)
    {
        m_bActiveController[iIndex] = bState;
    }

    public int GetControllerCount()
    {
        int iResult = 0;
        foreach (bool control in m_bActiveController)
        {
            if (control) iResult++;
        }
        return iResult;
    }

    public int GetControllerIndex(int index)
    {
        int iResult = -1;
        int iIterator = 0;
        foreach (bool control in m_bActiveController)
        {
            if (control) {
                if (iIterator == index)
                {
                    return iIterator+1;
                }
                iIterator++;
            }
        }
        return -1;

    }

    public void SetWinner(int iControllerIndex)
    {

    }

    public int GetWinner()
    {
        return -1;
    }
}

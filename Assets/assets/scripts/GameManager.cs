using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

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

    virtual public int GetControllerCount()
    {
        int iResult = 0;
        foreach (bool control in m_bActiveController)
        {
            if (control) iResult++;
        }
        return iResult;
    }

    virtual public int GetControllerIndex(int index)
    {
        int iResult = 0;
        int iIterator = 0;
        foreach (bool control in m_bActiveController)
        {
            if (control) {
                if (iIterator == index)
                {
                    Debug.Log("Player " + index.ToString() + " of " + GetControllerCount().ToString()+ " is Control Ordinal " + (iIterator + 1).ToString());
                    return iResult+1;
                }
                iIterator++;
            }
            iResult++;
        }
        return -1;

    }

    int m_iWinner;
    public void SetWinner(int iControllerIndex)
    {
        m_iWinner = iControllerIndex;
    }

    public int GetWinner()
    {
        return m_iWinner;
    }
}

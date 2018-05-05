using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour {

    public float amount = 100f;

    void changeInHealth(float amt)
    {
        amount += amt;
        if (amount <= 0f)
        {
            SendMessage("die", null);
        }
    }
}

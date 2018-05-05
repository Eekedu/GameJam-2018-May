using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEffector : MonoBehaviour {
    int status = 0;

    void pickup(int type)
    {
        status = type;
        changeSprite();
    }

    void changeSprite()
    {

    }

    void attack(Vector2 dir)
    {
        switch (status)
        {
            case 1: { fireAttack(dir); } break;
            case 2: { waterAttack(dir); } break;
            case 3: { lightAttack(dir); } break;
            case 4: { earthAttack(dir); } break;
            case 5: { windAttack(dir); } break;
        }
    }

    void fireAttack(Vector2 dir) { }
    void waterAttack(Vector2 dir) { }
    void lightAttack(Vector2 dir) { }
    void earthAttack(Vector2 dir) { }
    void windAttack(Vector2 dir) { }

    private void Update()
    {
        
    }
}

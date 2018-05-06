using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockNRoll : MonoBehaviour {

	// Use this for initialization
	void Start () {
        bod2d = GetComponent<Rigidbody2D>();
	}
    Rigidbody2D bod2d;
    float m_fPeakHorizontal;
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(bod2d.velocity.x) > Mathf.Abs(m_fPeakHorizontal))
        {
            m_fPeakHorizontal = bod2d.velocity.x;
        } else
        {
            if (Mathf.Abs(bod2d.velocity.x) < (Mathf.Abs(m_fPeakHorizontal)*0.5f))
            {
                Destroy(this.gameObject);
            }
        }
        float scalar = Mathf.Lerp(transform.localScale.x, 5, Time.deltaTime * 0.5f);
        Debug.Log(scalar);
        transform.localScale = new Vector3(scalar, scalar, 1);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}

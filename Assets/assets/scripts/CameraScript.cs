using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    // Use this for initialization
    Camera m_oCamera;

    private float fMinZoom = 5.0f;
	void Start () {
        m_oCamera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetBounds(Vector2 TL, Vector2 BR)
    {
        float fXDiff = BR.x - TL.x;
        float fYDiff = BR.y - TL.y;
        float fMaxDiff = Mathf.Max(fXDiff, fYDiff);
        Vector2 vCenter = new Vector2((TL.x + BR.x) * 0.5f, (TL.y + BR.y) * 0.5f);

        this.transform.position = new Vector3(vCenter.x, vCenter.y,-8);
        float fDesiredZoom = fMaxDiff * 0.3f;
        m_oCamera.orthographicSize = Mathf.Max(fMinZoom, fDesiredZoom);

    }
}

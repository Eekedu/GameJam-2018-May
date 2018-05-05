using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneBoss.g_oSceneBoss.FadeIn();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayButtonClick()
    {
        SceneBoss.g_oSceneBoss.FadeOut();
        while (!SceneBoss.g_oSceneBoss.FadeComplete())
        {

        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}

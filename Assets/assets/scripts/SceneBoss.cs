using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class SceneBoss : MonoBehaviour
{
    public static SceneBoss g_oSceneBoss;
    private RawImage m_oFadeUIImage;
    public float m_fFadeSpeed = 0.8f;
    public enum FadeDirection
    {
        FD_In, //Alpha = 1
        FD_Out // Alpha = 0
    }
    public enum SceneSelect
    {
        SS_Null,
        SS_Splash,
        SS_Title
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(FadeDirection.FD_Out));
    }
    public void FadeIn()
    {
        StartCoroutine(Fade(FadeDirection.FD_In));
    }
    private void Start()
    {
        if (g_oSceneBoss != null) return;
        g_oSceneBoss = this;
        DontDestroyOnLoad(this);
        m_oFadeUIImage = GetComponentInChildren<RawImage>();
        Debug.Log(m_oFadeUIImage);
        FadeIn();
    }

    public int GetSceneIndex(SceneSelect scene)
    {
        switch (scene)
        {
            case SceneSelect.SS_Null: return -1;
        }

        return -1;
    }

    private IEnumerator Fade(FadeDirection fadeDirection)
    {
        float alpha = (fadeDirection == FadeDirection.FD_In) ? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.FD_In) ? 0 : 1;
        if (fadeDirection == FadeDirection.FD_In)
        {
            while (alpha >= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
            m_oFadeUIImage.enabled = false;
        }
        else
        {
            m_oFadeUIImage.enabled = true;
            while (alpha <= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
        }
    }
    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        m_oFadeUIImage.color = new Color(m_oFadeUIImage.color.r, m_oFadeUIImage.color.g, m_oFadeUIImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / m_fFadeSpeed) * ((fadeDirection == FadeDirection.FD_In) ? -1 : 1);
    }

}
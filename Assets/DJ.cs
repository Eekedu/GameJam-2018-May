using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJ : MonoBehaviour {

    // Use this for initialization
    public AudioClip m_acTitle;
    public AudioClip m_acLevel;
    public float m_fMaxVolume;


    private AudioClip m_acCurrentSong;
    private AudioSource m_asSource;

    private AudioClip m_acNextSong;
    private enum DJPhase
    {
        DJStopped,
        DJPlaying,
        DJWaxing,DJWaning
    }
    private DJPhase m_pPhase;

    void Start()
    {
        m_asSource = GetComponentInChildren<AudioSource>();
        m_pPhase = DJPhase.DJStopped;
        PlaySong("title");
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_pPhase==DJPhase.DJWaning)
        {
            m_asSource.volume = Mathf.Lerp(m_asSource.volume,0.0f,Time.deltaTime*5.0f);
            if (m_asSource.volume<=0.01f)
            {
                PlaySong();
            }
        }
    }

    public void PlaySong(string song)
    {
        AudioClip l_acNextSong = TitleToClip(song);

        if ((l_acNextSong != m_acCurrentSong) || (m_acCurrentSong == null))
        {
            m_asSource.Stop();
            m_acCurrentSong = l_acNextSong;
            if (l_acNextSong != null)
            {
                m_asSource.clip = m_acCurrentSong;
                m_asSource.Play();
                m_pPhase = DJPhase.DJPlaying;
                Debug.Log("wtf mate");
            }
        }
    }
    private void PlaySong()
    {
        m_asSource.Stop();
        m_acCurrentSong = m_acNextSong;
        m_acNextSong = null;
        m_asSource.volume = m_fMaxVolume;
        m_asSource.clip = m_acCurrentSong;
        m_asSource.Play();
        m_pPhase = DJPhase.DJPlaying;
    }
    private AudioClip TitleToClip(string title)
    {
        switch (title)
        {
            case "title":
                return m_acTitle;
                break;
            case "battle":
                return m_acLevel;
                break;
        }
        return null;
    }
    public void LoadSong(string song)
    {
        m_acNextSong = TitleToClip(song);
    }
    public void PlayNextSong()
    {
        m_pPhase = DJPhase.DJWaning;
    }

}

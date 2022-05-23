using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{




    public AudioSource m_AudioSource;
    public AudioSource m_AudioSource2;

    public splineJump m_splineJump;

    public delegate void m_SoundPlayer();

    public m_SoundPlayer soundsetter;

    public AudioClip iceSkateaudioClip;
    public AudioClip windaudioClip;
    public AudioClip m_jumpHumanAudioClip;
    public AudioClip m_baloonBoomAudioClip;
    public AudioClip m_loseSoundAudioClip;
    public AudioClip m_RunAudioClip;
    public AudioClip m_ForwardAudioClip;
    
    
    
    public static SoundController Instance;
    
    

    private void Awake()
    {
        if(Instance==null)
        { Instance = this; }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_ForwardAudioClip == null)
        {
            m_ForwardAudioClip = iceSkateaudioClip;

        }
        if (m_AudioSource == null)
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        if (m_splineJump == null)
        {
            m_splineJump = GetComponent<splineJump>();
        }

        // ikisi de oluyor
       //soundsetter =new m_SoundPlayer( PlaySounds);
        soundsetter = PlaySounds;

    }

    // Update is called once per frame
    void Update()
    {
        soundsetter();
        
    }

    public void RunStarted()
    {
        m_ForwardAudioClip = m_RunAudioClip;
    }

    public void SkateStarted()
    {
        m_ForwardAudioClip = iceSkateaudioClip;
    }



    public void PlaySounds()
    {
        if(m_splineJump.isFollowingTheSpline)
        {


            // Play  skate sound
            // m_AudioSource.clip=
            // 
                m_AudioSource.clip =m_ForwardAudioClip;
              //   if (!m_AudioSource.isPlaying) { m_AudioSource.Play(); }
            
            

        }
        
        else
        {
          //  m_AudioSource.Stop();
            m_AudioSource.clip =null;
            
        }
        PlayTheSound();
        /*
         */

    }

    public void PlayTheSound()
    {
        if (!m_AudioSource.isPlaying ) { m_AudioSource.Play(); }
    }

    
    public void HumanJumpsSoundPlaysOnce()
    {
        m_AudioSource.PlayOneShot( m_jumpHumanAudioClip);
    }
    public void baloonBoom()
    {
        m_AudioSource2.PlayOneShot(m_baloonBoomAudioClip);
        
       
    }
    
    public void LoseSoundPlay()
    {
        m_AudioSource.PlayOneShot(m_loseSoundAudioClip);

    }
    
    
}

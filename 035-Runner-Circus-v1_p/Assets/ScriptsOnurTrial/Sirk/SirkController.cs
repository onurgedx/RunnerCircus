using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SirkController : MonoBehaviour
{

    public float aspectTimeSpeed = 0.7f;
    public float AspectJumpHeight = 1.7f;
    public AudioSource m_audioSource;

    public ControllerScriptOnur y_ControllerScriptOnur;
    public SoundController y_SoundController;
    public CameraControllerOnur y_CameraControllerOnur;

    public bool isSirkOpen = true;

    public List<GameObject> muzzleFireball2List = new List<GameObject>();

    public List<splineJump> ListsPlayerssplineJump = new List<splineJump>();

    public static bool enemiesSpeedChangeAccess = false;
    // Start is called before the first frame update
    void Start()
    {
        if(m_audioSource ==null)
        {
            m_audioSource = GetComponent<AudioSource>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        TouchCheck();
    }

    public void TouchCheck()
    {
        if(isSirkOpen)
        { 
            if(y_ControllerScriptOnur.isTouch )
            {
                if (y_ControllerScriptOnur.isTouchEnded)
                {
                    y_ControllerScriptOnur.enabled=true;
                    
                    FireCanon();
                }
            }
        }
    }   

    public void FireCanon()
    {
        isSirkOpen = false;

        m_audioSource.Play();
        for(int i=0 ; i<ListsPlayerssplineJump.Count ; i++)
        {
            ListsPlayerssplineJump[i].VoidGoWithBeizer(0, true,aspectTimeSpeed,AspectJumpHeight);
            // Vector3 closestV3 =  ListsPlayerssplineJump[i].m_SplineProjector.result.position;
            //ListsPlayerssplineJump[i].transform.DOJump(closestV3, 20, 1, 1).OnComplete(() => SetActiveSkate(ListsPlayerssplineJump[i]));

            muzzleFireball2List[i].SetActive(true);
        }

        enemiesSpeedChangeAccess = true;



    }

    public void SetActiveSkate(splineJump sj)
    {
        sj.enabled = true;
      //  sj.m_SplineFollower.enabled = true;
        sj.m_SplineFollower.follow = true;


    }


}

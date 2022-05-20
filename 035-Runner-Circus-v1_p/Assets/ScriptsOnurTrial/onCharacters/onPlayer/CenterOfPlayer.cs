using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CenterOfPlayer : MonoBehaviour
{
    
    public float distanceToLook = 9f;
    public float CamRotateSpeed = 5f;
    public splineJump PlayerSplineJumpScript;
    public static CenterOfPlayer _centerofPlayer;
    public bool access2Rotate = true;
    public bool canRotateUpAndDown = true;
    public bool canGoUpAndDown = true;
    
    

    private void Awake()
    { 
        access2Rotate = true;
        
        if (_centerofPlayer == null)
        {
            _centerofPlayer = this;
        }

        
    }

    // Update is called once per frame
    void LateUpdate()
    {Translate2IWillBe();
        Rotate2IWillBe();
        
    }
    public void Rotate2IWillBe()
    {
        if (access2Rotate && PlayerSplineJumpScript.m_AccessToGo)
        {
            Quaternion WillBeLookRotate =Quaternion.identity;
            if (canRotateUpAndDown)
            {
                 WillBeLookRotate =
                  Quaternion.LookRotation(PlayerSplineJumpScript.GetTDestination(distanceToLook) - transform.position);
            }
            else
            {
                WillBeLookRotate =
                Quaternion.LookRotation(PlayerSplineJumpScript.GetTDestination(distanceToLook) - transform.position, Vector3.up);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, WillBeLookRotate, Time.deltaTime *CamRotateSpeed);
        }

    }
    

    public void SetRotateToDesiredPoint(Vector3 desiredPoint)
    {
        Quaternion WillBeLookRotate =  Quaternion.LookRotation(desiredPoint-transform.position );
        transform.rotation = Quaternion.Lerp(transform.rotation,WillBeLookRotate,Time.deltaTime*5.5f);
        
    }

    public void Translate2IWillBe()
    {
        Vector3 pospla = PlayerSplineJumpScript.transform.position;
        if (!PlayerSplineJumpScript.m_AccessToGo && !canGoUpAndDown)
        {
            pospla.y = transform.position.y;
        }
        
        transform.position = pospla;
    }
    
    
    
}

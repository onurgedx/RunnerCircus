using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLovesThisObject : MonoBehaviour
{
    private Camera asikCam;

    private Vector3 recordedFirstLocalPos;
    public splineJump PlayerSplineJumpScript;

    // Start is called before the first frame update
    void Start()
    {
        if (asikCam == null)
        {
            asikCam = Camera.main;
            
        }

        recordedFirstLocalPos = transform.localPosition;


    }

    private void LateUpdate()
    {
        
        PlacementAccording2SplineThatWeOn(PlayerSplineJumpScript.CamLocalStatu());
        if(PlayerSplineJumpScript.m_AccessToGo){
            
        transform.rotation = Quaternion.LookRotation(PlayerSplineJumpScript.transform.position -transform.position );
        
        }
    }


  
    
    public void PlacementAccording2SplineThatWeOn(int i)
    {
        
        Vector3 tempLocalPos = transform.localPosition;
        tempLocalPos.x = (i-1)*2.5f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, tempLocalPos,Time.deltaTime*5);
        
    }

    

  
    
    
    
    
    
    
    
    }




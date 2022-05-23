using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraControllerOnur360Turn : MonoBehaviour
{

    public splineJump y_splineJumpScriptOnPlayer;

    public float TotalAngle = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
    
        
    }

    
    
    // Update is called once per frame
    void LateUpdate()
    {
        Movement();
        
    }



    private void Movement()
    {
        if(TotalAngle<90)
        {
        float plusAngleAmount = Time.deltaTime * 35;
        TotalAngle += plusAngleAmount*2;
        transform.RotateAround(y_splineJumpScriptOnPlayer.gameObject.transform.position,Vector3.up,plusAngleAmount);
        
       
        }
        transform.rotation = Quaternion.Lerp( transform.rotation ,Quaternion.LookRotation(y_splineJumpScriptOnPlayer.transform.position-transform.position),Time.deltaTime*4);

 
    }
    
    private void OnEnable()
    {
        //TotalAngle i resetlemece
        TotalAngle = 1f;
        
        // ziplama ve atlama engellendi///////////////
       // y_splineJumpScriptOnPlayer.DisableGoAndJump();
        ////////////////////////////////////////////
        //y_splineJumpScriptOnPlayer.ChangeFollowSpeedVoidSmoothly(y_splineJumpScriptOnPlayer.followSpeed*2.5f);

    }

    
    private void OnDisable()
    {
        ///ziplama ve atlama erisim izni verildi ////////////////////////
        //y_splineJumpScriptOnPlayer.EnableGoAndJump();
        /////////////////////////////////////////////////////////
        //y_splineJumpScriptOnPlayer.ChangeFollowSpeedVoidSmoothly(y_splineJumpScriptOnPlayer.firstFollowSpeed);
        
        
    }
    
    
    
    
}

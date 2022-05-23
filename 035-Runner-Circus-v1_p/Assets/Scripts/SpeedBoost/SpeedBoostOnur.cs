using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostOnur : MonoBehaviour
{

    public bool canJump = false;
    public bool q_ChangeCamera = true;
    public CameraControllerOnur y_camController;
    public CameraControllerOnur360Turn y_camController360;
    public float MaxSpeedAsNumberFirstSpeed=1.5f;

    // Start is called before the first frame update
    void Start()
    {
         y_camController = Camera.main.gameObject.GetComponent<CameraControllerOnur>();
         y_camController360 = Camera.main.gameObject.GetComponent<CameraControllerOnur360Turn>();

    }









    

    
    
    private void OnTriggerEnter(Collider other)
    {
        
        // other.isTrigger dedigim zaman çarptıgı collider triggersa donuyor  (!!! colliderlerinden birisi triggerse degil !!!!!)
        
        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") ) && !other.isTrigger)
        {
                            if (other.gameObject.CompareTag("Player") && q_ChangeCamera)
                            {
                                y_camController.enabled =false;
                                y_camController360.enabled = true;
            
                            }

            splineJump _splineJump =  other.gameObject.GetComponent<splineJump>();
            
            // bunu kapiyorum 1 seferlik bakalim nolacak
           //_splineJump.SetSpeedBoostStatuSpeedBoostOnur();
            if (!canJump)
            { 
                _splineJump.DisableGoAndJump(); 
            }
            
            _splineJump.ChangeFollowSpeedVoidSmoothly(_splineJump.followSpeed*MaxSpeedAsNumberFirstSpeed);

        }
        
    }

    private void OnTriggerExit(Collider other)
    {

        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))&& !other.isTrigger)
        {  if (other.gameObject.CompareTag("Player") && q_ChangeCamera)
                     {
                         y_camController.enabled = true;
                         y_camController360.enabled = false;
         
                     }
            splineJump _splineJump =  other.gameObject.GetComponent<splineJump>();
            
            //_splineJump.SetSpeedBoostStatunoBoost();
            

                _splineJump.EnableGoAndJump();
            

            _splineJump.ChangeFollowSpeedVoidSmoothly(_splineJump.firstFollowSpeed);

          
        }
        
        
        

    }
}

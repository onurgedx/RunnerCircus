using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpeedAdjusterOneTicket : MonoBehaviour
{




    public float MaxSpeedAsNumberFirstSpeed = 1.5f;
   
    public float duration = 2f;
    public float rotateSpeed =90f;
    
    public Vector3 firstPos;
    public float firstRotateSpeed;
    public Vector3 firstScale;
    public Quaternion firstRotation;
    private void Awake()
    {
        firstPos = transform.position;
        firstRotateSpeed = rotateSpeed;
        firstScale = transform.localScale;
        firstRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        
        transform.Rotate(rotateSpeed*Vector3.up*Time.fixedDeltaTime);
        
    }

    public IEnumerator AffectSmoothly( splineJump _splineJump)
    {

        rotateSpeed =0;
        
        transform.parent = _splineJump.transform;
        
        transform.DOLocalMove(_splineJump.m_CollectableAbilityGO.transform.localPosition, 0.3f);
        //transform.DORotate(Quaternion.LookRotation(_splineJump.m_CollectableAbilityGO.transform.position+Vector3.up - transform.position).eulerAngles,0.5f);
        transform.DORotate(Quaternion.LookRotation(_splineJump.camMain.transform.position -transform.position).eulerAngles ,0.2f);
        //transform.DOScale(Vector3.zero, durat;,ion * 0.5f).OnComplete(() => Destroy(gameObject));

        yield return new WaitForSeconds(0.3f);
          
        
        transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.InCubic);//.SetEase(Ease.OutExpo);
        transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(0.2f);
        

        _splineJump.ChangeFollowSpeedVoidSmoothly(_splineJump.followSpeed * MaxSpeedAsNumberFirstSpeed,true,duration);
        _splineJump.DashAnimate();
        
        yield return new WaitForSeconds(2f);
        
        
        ResetValues();
        
    }


    private void ResetValues()
    {
        transform.parent = null;
        transform.position = firstPos;
        rotateSpeed = firstRotateSpeed;
        transform.localScale = firstScale;
        transform.rotation = firstRotation;
    }
    
    
    // Reset FONKSİYONU  SADECE EDİTORDE ÇALIŞIR
    private void Reset()
    {
        
    }
    
    
    


    private void OnTriggerEnter(Collider other)
    {

        // other.isTrigger dedigim zaman �arpt�g� collider triggersa donuyor  (!!! colliderlerinden birisi triggerse degil !!!!!)

        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player")) && !other.isTrigger)
        {

            splineJump _splineJump = other.gameObject.GetComponent<splineJump>();
            StartCoroutine(AffectSmoothly(_splineJump));
           
        }

    }
    
    

}

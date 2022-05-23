using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerOnur : MonoBehaviour
{
    public EnemyOnSpline y_EnemyOnSplineScript;
    public float goChanceForEnemies = 0.7f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") ||other.gameObject.CompareTag("VacuumCleaner"))
        {
          //  Debug.Log("ontriggerenemy");
            if(y_EnemyOnSplineScript.ChanceEnemy(0.7f))
            {
            y_EnemyOnSplineScript.VoidGoWithBeizer(y_EnemyOnSplineScript.GetEklemeAmountAIMovement());}
            
        }
    }
}

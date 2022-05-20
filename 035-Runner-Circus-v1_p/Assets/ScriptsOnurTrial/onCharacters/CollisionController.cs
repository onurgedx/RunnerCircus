using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public splineJump _splineJump;

    public float fireChanceForEnemies = 0.7f;
    public void Awake()
    {
        if (_splineJump == null)
        {
            _splineJump = GetComponent<splineJump>();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (!other.gameObject.GetComponent<ObstacleAlone>().AmIHitted)
            {
                _splineJump.AddObstacleToList(other.gameObject);
                _splineJump.EnemySetObstacle(0.45f,fireChanceForEnemies);
            }
            
           
        }
        else if( other.CompareTag("RunStart"))
        {
            _splineJump.ChaRunLayerOn();

        }
        else if(other.CompareTag("RunEnd"))
        {
            _splineJump.ChaRunLayerOff();
        }
       
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            _splineJump.RemoveObstacleFromList(other.gameObject);
          
        }
        
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Obstacle")) { AfterCrushObstacle(); }
        else if (coll.gameObject.CompareTag("ObstacleGround")) { AfterCrushObstacle(); }
        else if (coll.gameObject.CompareTag("Spline")) { AfterCollideSpline(coll); }

        else if(coll.gameObject.CompareTag("VacuumCleaner")){AfterCollideVacuumCleaner(); }
            

           
            
           
        
    }


    private void AfterCrushObstacle()
    {
        _splineJump.AfterTouchObstacle();
        
    }

    private void AfterCollideVacuumCleaner()
    {
        _splineJump.AfterTouchVacuumCleaner();
    }

    private void AfterCollideSpline(Collision coll)
    {
        if (!_splineJump.isShouldStopToJump && _splineJump.isChaAlive)
        {
            _splineJump.AfterCollSplineWhileJump(coll.gameObject.GetComponent<SplineComputer>());
            
        }       
    }
    

}


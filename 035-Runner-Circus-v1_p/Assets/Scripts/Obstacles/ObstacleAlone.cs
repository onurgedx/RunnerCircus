using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class ObstacleAlone : MonoBehaviour
{


    public bool DoesMakeHole = false;
    public Color firstBaloonColor;

    public MeshRenderer c_Baloon;
    public bool AmIHitted = false;
    public Animator m_animator;
    public AudioSource m_AudioSource;

    private Vector3 destination;
    public Rigidbody m_Rigidbody;

    public Collider m_Collider;

    private bool AccessToVanish = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (DoesMakeHole)
        {
            gameObject.layer = 11;
        }
        
        
        

        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        if (m_Collider == null)
        {
            m_Collider = GetComponent<Collider>();
        }
        
        if(m_AudioSource ==  null)
        { m_AudioSource = GetComponent<AudioSource>(); }

        firstBaloonColor = c_Baloon.material.color;
        if (m_animator == null)
        {
            m_animator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
     //   destination =  GetComponent<SplineProjector>().result.position+Vector3.up*0.5f*2 ;
    }


    public void colorReset()
    {
        if (c_Baloon.material.color != firstBaloonColor)
        {
            c_Baloon.material.color = firstBaloonColor;
        }
        
        
    }

    public void colorSet()
    {
        
        c_Baloon.material.color = Color.yellow;
        determineCanvasYellow.Instance.setVisibleStatu(true);
         determineCanvasYellow.Instance.SetPosition( c_Baloon.bounds.extents.y*Vector3.up + c_Baloon.transform.position);
    }
    

    public void SetAnimateTrue()
    {
       m_animator.SetTrigger("shouldUPDOWN");

    }

    public void StopAnimate()
    {
        m_animator.SetTrigger("shouldStop");
        m_animator.ResetTrigger("shouldUPDOWN");
        
    }

    public void BalloonPopSound()
    {
        
        m_AudioSource.Play();
    }
    public void GoDesiredPlace()
    {
        
        if (!AmIHitted){
            destination =  GetComponent<SplineProjector>().result.position+Vector3.up*0.5f*2 ;

         
            determineCanvasYellow.Instance.setVisibleStatu(false);
            
            if (DoesMakeHole)
            {
                FallsAndMakesHole();
            }
            else
            {
                FallsAndStops();
            }
            
            
            
            AmIHitted = true;
            
            
        }
        

    }


    public void AdjustToMakerHole()
    {m_Rigidbody.isKinematic = false;
        m_Collider.isTrigger = true;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.velocity = -Vector3.up*30;
        AccessToVanish = true;
        
    }
    
    public void FallsAndMakesHole()
    {
        
                transform.DOMove(destination, 0.35f).SetEase(Ease.InQuad).OnComplete(() =>
                {

                    AdjustToMakerHole();

                } );
                
    }
    public void FallsAndStops()
    { 
         
        transform.DOMove(destination, 0.35f).SetEase(Ease.InQuad);//.OnComplete(()=>determineCanvasYellow.Instance.setVisibleStatu(false));


        
    }

    public void Vanish(Collider other)
    {
        if (AccessToVanish)
        { MeshFilter mf = other.gameObject.GetComponent<MeshFilter>();
            List<Vector3> _vertices = mf.mesh.vertices.ToList();
            float localScaleMagnitude = transform.localScale.magnitude;
            Vector3 temp_mfLocalPosAccordingToMesh = mf.transform.InverseTransformPoint(transform.position);
            List<int> _triangles = mf.mesh.triangles.ToList();
            for (int i = _triangles.Count-1; i >0; i -= 3)
            {
                float t_distance=( temp_mfLocalPosAccordingToMesh- _vertices[_triangles[i]]).magnitude;
                
                if ( t_distance < localScaleMagnitude*2.6f)
                {
                    _triangles.RemoveAt(i);
                    _triangles.RemoveAt(i - 1);
                    _triangles.RemoveAt(i - 2);
                }
                /*          
                 else if(t_distance<localScaleMagnitude*3.25f )
                {
                    float temp_diff = localScaleMagnitude * 3.25f - t_distance;
                    
                    
                    temp_diff *= temp_diff;
                    temp_diff *= 0.25f;
                    _vertices[_triangles[i]] -= temp_diff*Vector3.up;
                    _vertices[_triangles[i-1]] -= temp_diff*Vector3.up;
                    _vertices[_triangles[i-2]] -= temp_diff*Vector3.up;


                }*/

            }

            mf.mesh.triangles = _triangles.ToArray();
          mf.mesh.vertices = _vertices.ToArray();
            
            AccessToVanish = false;
            GameObject GoObstacle = Instantiate(gameObject, transform.position, transform.rotation, null);
         
            GoObstacle.tag = "VacuumCleaner";
            GoObstacle.GetComponent<MeshRenderer>().enabled = false;
            GoObstacle.GetComponent<Collider>().isTrigger =false;
            GoObstacle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GoObstacle.GetComponent<Rigidbody>().useGravity = false;
            GoObstacle.gameObject.transform.localScale *= 1.5f;
        }
    }
    public void OnTriggerStay(Collider other)
    {

        if (other.tag == "Spline")
        {
            Vanish(other);
        }

       /* 
        // burasini boyle yazip birakmak ne kadar dogru emin degilim ama denemek icin yaptim tamamen
        if (!other.isTrigger && (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") && gameObject.tag=="VacuumCleaner")
        {
            other.gameObject.GetComponent<splineJump>().AfterTouchVacuumCleaner();
         
            this.enabled = false;   
        }
        */
        
    }
}

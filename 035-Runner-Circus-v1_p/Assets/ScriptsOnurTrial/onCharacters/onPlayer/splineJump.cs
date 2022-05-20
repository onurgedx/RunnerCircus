using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


public class splineJump : MonoBehaviour
{

    
    
    
    
    public SplineComputer[] m_SplineComputerList = new SplineComputer[3];
    public SplineFollower m_SplineFollower;
    public SplineProjector m_SplineProjector;
    
    private int m_CurrentSplineIndex=0;
    private Vector3 t_destination;
    private int m_CurrentSplineProjectorIndex = 0;
    private double _percent;
    

    public bool m_AccessToGo = true;
    public bool m_AccessToJump = true;

    public float _jumpPower = 10f;
    
    
    public CameraControllerOnur360Turn y_cam360TurnScript;
    public CameraControllerOnur y_camControllerScript;


    public GameObject m_CollectableAbilityGO;
    
    public CamLovesThisObject camLovesThisScript;

    public GameObject playerBodyParent;

    public GameObject playerBody;
    private Vector3 recordedPlayerBodylocalPosition;

    public SplineProjector y_SplineBallProjector;

    public Animator ChaAnimator;


    public float DistanceAspectForBall = 3f;

    public float firstFollowSpeed;

    public float maxFollowSpeed = 45f;
    public float minFollowSpeed = 5f;


    public Vector2 firstTouchPoint;

    public GameObject UstBodyOfPlayer;
    public Quaternion firstRotationOfUstBodyOfPlayer;

    public Camera camMain;

    public int SplineStartIndex;

    public bool isShouldStopToJump = false;

    public ParticleSystem m_electroParticle;

    public LineRenderer m_LineRenderer;
    public Rigidbody m_Rigidbody;

    public Rigidbody c_HipsRigidbody;

    private  bool isChaAlivePrivate = true;


    public ObstacleScript y_obstacleScript;

    public List<GameObject> ObstaclesListICanHit = new List<GameObject>();

    public GameObject Throwobje;

     public GameObject m_Shooter;

     public SpeedBoostStatu m_SpeedBoostStatu;


     public enum SpeedBoostStatu
     {
         noBoost,
         SpeedBoostOnur,
         
     }
    private void Start()
    {
        m_SpeedBoostStatu = SpeedBoostStatu.noBoost;

        //   m_SplineFollower.GetComponent<SplineMesh>().GetChannel(0)


        if (y_obstacleScript == null)
        {
        //    y_obstacleScript = GameObject.FindGameObjectWithTag("ObstaclesListGo").GetComponent<ObstacleScript>();
        }
        
        if (m_Rigidbody == null) { m_Rigidbody = GetComponent<Rigidbody>(); }

        if (m_LineRenderer == null) { m_LineRenderer = GetComponent<LineRenderer>(); }
        
        firstTouchPoint = Vector2.zero;
        
        if (m_SplineFollower == null) { m_SplineFollower = GetComponent<SplineFollower>(); }

        firstFollowSpeed = m_SplineFollower.followSpeed;
        
        
      //  m_SplineFollower.spline = m_SplineComputerList[m_CurrentSplineIndex];
      ThisIsCurrentSplineFollowerIndex = SplineStartIndex;
      
      //m_SplineFollower.onNode += OnNodePassed; // unnecessary code
        
        
        if (playerBody == null) { playerBody = GameObject.FindGameObjectWithTag("PlayerBody"); }
        
        recordedPlayerBodylocalPosition = playerBody.transform.localPosition;
        
        if (ChaAnimator == null) { ChaAnimator = playerBody.GetComponent<Animator>(); }


        firstRotationOfUstBodyOfPlayer = UstBodyOfPlayer.transform.localRotation;

        if (camMain == null) { camMain = Camera.main; }

    }


    

    public bool isChaAlive
    {
        get
        {
            return isChaAlivePrivate;
        }
        set
        {
            
            if (!value)
            {
                StopAllCoroutines();
                DisableGoAndJump();
              
                if(isItsplineJump)
                { SoundController.Instance.LoseSoundPlay(); }
            }
            isChaAlivePrivate = value;
                
        }
    }
    
    public virtual string ScriptName
    {
        get { return "splineJump"; }
    }


    public virtual bool isItsplineJump
    {
        get { return ScriptName == "splineJump"; }
    }
    public virtual bool isItEnemyOnSpline
    {
        get { return ScriptName == "EnemyOnSpline"; }
    }
    
    
    
    public GameObject GetClosestObstacle(bool remove=true)
    {
        
        int closestIndex = 0;
        float _distance = 9999999f;
        for (int i = 0; i < ObstaclesListICanHit.Count; i++)
        {
            Vector3 distanceV3 = ObstaclesListICanHit[i].transform.position - transform.position;
            float temp_distance = distanceV3.sqrMagnitude;
            if (_distance > temp_distance)
            {
                _distance = temp_distance;
                closestIndex = i;
            }


        }

        GameObject closestOne = ObstaclesListICanHit[closestIndex];

        if (remove)
        {
            ObstaclesListICanHit.RemoveAt(closestIndex);
        }

        return closestOne;

        

    }

    public virtual void SetObstacle()
    {
        if (ObstaclesListICanHit.Count > 0)
        {
            StartCoroutine(ThrowObject());    
        }
    }

    public virtual void ThrowAnimationOn()
    {
        ChaAnimator.SetTrigger("throwtrigger"); }

    public virtual void ThrowAnimationResetTrigger()
    {
        ChaAnimator.ResetTrigger("throwtrigger");
    }
   public virtual IEnumerator ThrowObject()
    {
        
         GameObject goObstacle = GetClosestObstacle();

        
       
         Vector3 viewportGG =camMain.WorldToViewportPoint( goObstacle.transform.position);
         if(viewportGG.x>0 && viewportGG.x<1 && viewportGG.y>0 && viewportGG.y<1  )
         {}
         else
         {
             if(ScriptName=="splineJump") {yield break;}
             else
             { 
                 if(m_SplineFollower.spline.Equals(goObstacle.GetComponent<SplineProjector>().spline))
                 {yield break;}
                 
             }
             
         }
         float timeCounter=0f;

        //atis animation 
        ThrowAnimationOn();
         yield return new WaitForSeconds(0.25f);
         Vector3 firstPos = m_Shooter.transform.position;
         Vector3 lastPos = goObstacle.transform.GetChild(0).position; 
         Vector3 directionToDestionationFromfirstPos = lastPos - firstPos;
        Quaternion rotationOfThrowGO = Quaternion.LookRotation(directionToDestionationFromfirstPos);
         GameObject throwGO = Instantiate(Throwobje, firstPos,rotationOfThrowGO, null);


        
        //Debug.Log(ChaAnimator.GetCurrentAnimatorClipInfoCount(1));
        //https://answers.unity.com/questions/1035587/how-to-get-current-time-of-an-animator.html
     //   float waitDuration = (1 - ChaAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime%1) * ChaAnimator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
        //  float waitDuration = ChaAnimator.get;
        //Debug.Log(ChaAnimator.GetCurrentAnimatorClipInfo(1)[0].clip.name);
        //  yield return new WaitForSeconds(waitDuration);
        //   Debug.Log( ChaAnimator.GetAnimatorTransitionInfo(1).durationUnit);
        //  Debug.Log( waitDuration);

        

        throwGO.GetComponent<TrailRenderer>().enabled = true;
         while (timeCounter<1)
         {
             timeCounter += Time.fixedDeltaTime*4;
             
             Vector3 destinationForThrow = firstPos * Mathf.Pow((1 - timeCounter), 3) 
                                           + (firstPos+directionToDestionationFromfirstPos*0.25f +  Vector3.up*4)* Mathf.Pow(1 - timeCounter, 2) * timeCounter * 3 
                                           + 3 * (1 - timeCounter) * timeCounter * timeCounter * (lastPos- directionToDestionationFromfirstPos*0.25f+ Vector3.up*6) 
                                           + timeCounter * timeCounter * timeCounter * lastPos;

             
             yield return null;
             throwGO.transform.position = destinationForThrow;

         }
        goObstacle.GetComponent<ObstacleAlone>().GoDesiredPlace();

        if (viewportGG.x > 0 && viewportGG.x < 1 && viewportGG.y > 0 && viewportGG.y < 1 && viewportGG.z>0)
        { 
         SoundController.Instance.baloonBoom();
         }
        
        goObstacle.transform.GetChild(0).gameObject.SetActive(false);
       

        throwGO.GetComponent<TrailRenderer>().enabled = false;
        Destroy(throwGO);
         yield return null; 

        

    }

   public void AddObstacleToList(GameObject goCanBeHitted)
   {
       ObstaclesListICanHit.Add(goCanBeHitted);
       
       RecalculateWhoYellow();
   }

   public void RecalculateWhoYellow()
   {
       if (isItsplineJump && ObstaclesListICanHit.Count>0)
       {
           for (int i = 0; i < ObstaclesListICanHit.Count; i++)
           {
               ObstaclesListICanHit[i].GetComponent<ObstacleAlone>().colorReset();
           }
            determineCanvasYellow.Instance.setVisibleStatu(false);

            GetClosestObstacle(false).GetComponent<ObstacleAlone>().colorSet();


       }
   }

   public void EnemySetObstacle(float durationWait=0.4f,float _chance =0.45f)
    {
        if(isItEnemyOnSpline && ChanceEnemy(_chance))
        { Invoke("SetObstacle" ,durationWait ); }
        
    }

    public bool ChanceEnemy(float _chance=0.5f)
    {
      return Random.Range(0f, 1f) > 1-_chance; 
    }
    

    public void RemoveObstacleFromList(GameObject goWillBeRemovedFromList)
    {
        
        for (int i = 0; i < ObstaclesListICanHit.Count; i++)
        {
            if (goWillBeRemovedFromList.Equals(ObstaclesListICanHit[i]))
            {
                goWillBeRemovedFromList.GetComponent<ObstacleAlone>().colorReset();
                ObstaclesListICanHit.RemoveAt(i);
                RecalculateWhoYellow();
                    
                break;
            }
            
        }

        


    }

    public IEnumerator ChangeFollowSpeedSmoothly(float targetSpeed, bool oneticked = false, float duration = 1f)
    {
        float timeCounter = 0f;
        float startSpeed = followSpeed;
        while (timeCounter<1)
        {
            timeCounter += Time.fixedDeltaTime*2;
            followSpeed = Mathf.Lerp(startSpeed, targetSpeed, timeCounter);
            yield return null;

        }
        
        if(oneticked)
        {
            yield return new WaitForSeconds(duration);
            ChangeFollowSpeedVoidSmoothly(firstFollowSpeed);
        }
        
    }

    public void DashAnimate()
    {
        ChaAnimator.SetTrigger("Dash");
    }
    

    public void ChangeFollowSpeedVoidSmoothly(float targetSpeed,bool oneticket=false,float duration =1f)
    {
        StartCoroutine(ChangeFollowSpeedSmoothly(targetSpeed,oneticket,duration));
    }
    public float followSpeed
    {
        get
        {
            return m_SplineFollower.followSpeed;
        }

        set
        {
            if (value > minFollowSpeed && value < maxFollowSpeed)
            m_SplineFollower.followSpeed = value;
        }
    }

    public float followSpeedClamped
    {
        get { return Mathf.Clamp(followSpeed, 15, 80); }
    }
    

    // Update is called once per frame
    void Update()
    {


        SpeedArrangementForEnemies();


    }

    public void SpeedArrangementForEnemies()
    {
        if (isItEnemyOnSpline && SirkController.enemiesSpeedChangeAccess)
        {
            float zPosAccordingCam = camMain.WorldToViewportPoint(transform.position).z;
                    

            
                    if (zPosAccordingCam < 10)
                    {
                        followSpeed += Time.fixedDeltaTime*2;
                    }
                    else if(zPosAccordingCam>20)
                    {
                        followSpeed -= Time.fixedDeltaTime;
            
            
                    }
            
        }
    }

    
    
    int ThisIsCurrentSplineProjectorIndex
    {
        get { return m_CurrentSplineProjectorIndex; }
        set
        {
            int t_ekleme = value - m_CurrentSplineProjectorIndex;

            if (t_ekleme == 1)
            {
                // sola gidecek x kucuk olan alınacak 

            }
            else if (t_ekleme == -1)
            {
                // saga gidecek x buyuk olan alınacak
                
            }
            m_CurrentSplineProjectorIndex = GetDesiredSpline(t_ekleme);
            
           // m_CurrentSplineProjectorIndex= Mathf.Clamp(value, 0, 2);
            
            //camLovesThisScript.PlacementAccording2SplineThatWeOn(m_CurrentSplineProjectorIndex);//olduğu konuma göre saga yada sola geçmesi sağlanıyor  mesela en sagdaysa
            
            m_SplineProjector.spline = m_SplineComputerList[m_CurrentSplineProjectorIndex];
            y_SplineBallProjector.spline = m_SplineComputerList[m_CurrentSplineProjectorIndex];
            m_SplineProjector.RebuildImmediate();
            y_SplineBallProjector.RebuildImmediate();

        }
    }

    public int GetDesiredSpline(int t_ekleme,float maxDiffIndicator=1000f)
    {
        float diffIndicator=maxDiffIndicator;
        int closestIndex = m_CurrentSplineIndex;
        y_SplineBallProjector.transform.position = transform.position;
        y_SplineBallProjector.RebuildImmediate();
        for (int i = 0; i < m_SplineComputerList.Length ; i++) 
        {
            if(i==ThisIsCurrentSplineFollowerIndex){continue;}

            y_SplineBallProjector.spline = m_SplineComputerList[i];
            y_SplineBallProjector.RebuildImmediate();
            Vector3 diffLocalV3= transform.InverseTransformPoint(transform.position) - transform.InverseTransformPoint(y_SplineBallProjector.result.position);

            
            
            if ((diffLocalV3.x > 0 && t_ekleme==1) || (diffLocalV3.x < 0 && t_ekleme == -1))
            {
                float mutlakDiffLocalX = diffLocalV3.x * diffLocalV3.x + diffLocalV3.z * diffLocalV3.z; //Vector3.SqrMagnitude(diffLocalV3);
               if (mutlakDiffLocalX < diffIndicator )
               {
                   diffIndicator = mutlakDiffLocalX;
                   closestIndex = i;
               }

            }
            


        }

        y_SplineBallProjector.spline = m_SplineComputerList[closestIndex];

        return closestIndex;

    }

    public int GetEklemeAmountAIMovement()
    {

        int currentindex = ThisIsCurrentSplineFollowerIndex;
        int takenIndex = GetDesiredSpline(1);
        if (takenIndex != currentindex)
        {
            return 1;
        }
        else
        {
            return -1;

        }
        

    }
    
    
    public int ThisIsCurrentSplineFollowerIndex
    { get { return Mathf.Clamp( m_CurrentSplineIndex,0,2); } 
        set { m_CurrentSplineIndex = Mathf.Clamp(value, 0, 2);
            m_SplineFollower.spline = m_SplineComputerList[m_CurrentSplineIndex];
            
        } }


    public int CamLocalStatu()
    {
        
            int willBeReturned=0;
            Vector3 currentPos = transform.InverseTransformPoint(transform.position);

         for (int i = 0; i < m_SplineComputerList.Length; i++)
         { if (ThisIsCurrentSplineFollowerIndex == i) continue;
             m_SplineProjector.spline = m_SplineComputerList[i];
             m_SplineProjector.RebuildImmediate();
             if (currentPos.x > transform.InverseTransformPoint(m_SplineProjector.result.position).x) { willBeReturned++; } 
         }

         m_SplineProjector.spline = m_SplineFollower.spline;
         
         return willBeReturned;


        
    }
    
    private void OnNodePassed(List<SplineTracer.NodeConnection> passed)
    {
        SplineTracer.NodeConnection nodeConnection = passed[0];
        
        double nodePercent = (double) nodeConnection.point/ (m_SplineFollower.spline.pointCount-1);
        double followerPercent = m_SplineFollower.UnclipPercent(m_SplineFollower.result.percent);
        float distancePastNode = m_SplineFollower.spline.CalculateLength(nodePercent, followerPercent);
        
        
        //m_SplineFollower.spline.pointCount 
        //m_SplineProjector.spline.pointCount = b
        //m_SplineProjector.result.percent = a
        // a*b = index 
        Vector3 vec3 =m_SplineProjector.spline.GetPoint((int)(m_SplineProjector.spline.pointCount * m_SplineProjector.result.percent)).position;
        Debug.Log("asdasdasd");
        // point noktalarından birini verir en yakın olanı 
        
        //m_SplineFollower.sampleCount
        int a = m_SplineProjector.sampleCount;

        double b = m_SplineProjector.result.percent;

        //m_SplineProjector.spline.samples[(int) (a * b)].position;


        //m_SplineFollower.get
       // y_SplineBallProjector.spline=
       /*
        y_SplineBallProjector.gameObject.transform.position =
            (m_SplineProjector.result.position + m_SplineProjector.result.forward * 3);
        y_SplineBallProjector.result.position
            */
       
       
    }

    public void SetTDestination()
    {
        
          int lastIndex = m_SplineProjector.sampleCount-1;

          float distanceSample = m_SplineProjector.spline.CalculateLength() / (lastIndex + 1);

          _percent = m_SplineProjector.result.percent +3*distanceSample/m_SplineProjector.spline.CalculateLength();//+(double)//distanceSample*;//*Mathf.Sign(m_SplineFollower.followSpeed)

         if (_percent > 1)
         {
             _percent = 1;
         }
          
          
          t_destination =  m_SplineProjector.spline.samples[(int) (lastIndex * _percent)].position;
          
           
        
    }
    public void SetTDestination2()
    {
        // followSpeedClamped ile çok hizli oldugu durumlarda sinirlama koymuş oldum 
        y_SplineBallProjector.gameObject.transform.position =
            (m_SplineProjector.result.position + m_SplineFollower.result.forward *followSpeedClamped*DistanceAspectForBall);
       
        //y_SplineBallProjector.modifiedResult.percent
        //y_SplineBallProjector.result.percent
        int lastIndex = m_SplineProjector.sampleCount-1;

        
       
        float distanceSample = m_SplineProjector.spline.CalculateLength() / (lastIndex + 1);
        
        
        y_SplineBallProjector.RebuildImmediate();     //+3*distanceSample/m_SplineProjector.spline.CalculateLength();//+(double)//distanceSample*;//*Mathf.Sign(m_SplineFollower.followSpeed)
        _percent = y_SplineBallProjector.result.percent;     //+3*distanceSample/m_SplineProjector.spline.CalculateLength();//+(double)//distanceSample*;//*Mathf.Sign(m_SplineFollower.followSpeed)

        if (_percent > 1)
        {
            _percent = 1;
        }
          
          
        t_destination =  m_SplineProjector.spline.samples[(int) (lastIndex * _percent)].position;

      


    }


   public Vector3 GetTDestination(float distanceWeGet=3f)
    {
        m_SplineProjector.RebuildImmediate();
        int lastIndex = m_SplineProjector.sampleCount-1;

        float distanceSample = m_SplineProjector.spline.CalculateLength() / (lastIndex + 1);

      double temp_percent = m_SplineProjector.result.percent +distanceWeGet*distanceSample/m_SplineProjector.spline.CalculateLength();//+(double)//distanceSample*;//*Mathf.Sign(m_SplineFollower.followSpeed)

        if (temp_percent > 1)
        {
            temp_percent = 1;
        }
          
          
       Vector3 t_destinationTemp =  m_SplineProjector.spline.samples[(int) (lastIndex * temp_percent)].position;
        return t_destinationTemp ;
    }



  public void VoidGoWithBeizer(int ekleme,bool fromsirk=false,float AspectOfhalfSecondDuration =1f, float AspectJumpHeight = 1)
  {
      StartCoroutine(GoWithBeizer(ekleme,fromsirk,AspectOfhalfSecondDuration, AspectJumpHeight ));
  }



    public IEnumerator GoWithBeizer(int ekleme, bool fromsirk = false, float AspectOfhalfSecondDuration = 1f, float AspectJumpHeight = 1)
    { if(m_AccessToGo)
        {
            

            
            ThisIsCurrentSplineProjectorIndex += ekleme;
            SetTDestination2();
            if(ThisIsCurrentSplineFollowerIndex == ThisIsCurrentSplineProjectorIndex && !fromsirk)
            { yield break; }
            ThisIsCurrentSplineFollowerIndex = ThisIsCurrentSplineProjectorIndex;


            // ses

            if (isItsplineJump)
            {
                SoundController.Instance.HumanJumpsSoundPlaysOnce();
            }
            
            
            
            ChaJumpAnimation();
            SetFollowFalse();
       SetAccessToGo(false);
       
        

           
           
           /*
            _percent = m_SplineProjector.result.percent;
                    //+0.2f;
                   // m_SplineProjector.EvaluatePosition(_percent);
            
                   
                  
                        
                      // t_destination = m_SplineFollower.result.position;
            
            Vector3 vec3 =m_SplineProjector.spline.GetPoint((int)(double)((m_SplineProjector.spline.pointCount * (m_SplineProjector.result.percent+0.1d)))).position;
              //_percent = (m_SplineProjector.result.percent + 0.1d);
          // m_SplineProjector.result.percent -= m_SplineProjector.span;
        _percent = m_SplineProjector.result.percent+0.1; 
       
         m_SplineProjector.spline.GetSamplingValues(_percent,out int index_destination,out double lerrp);
         t_destination = m_SplineProjector.spline.GetPoint(index_destination,SplineComputer.Space.Local).position;
       //t_destination = m_SplineProjector.result.position;
        
        
       //t_destination=  m_SplineProjector.Evaluate(m_SplineProjector.result.percent).position;
        
        // m_SplineProjector.GetSampleRaw(2).;
           */
        
        Vector3 firstPos = transform.position;
        Vector3 directionToDestionationFromfirstPos = t_destination - firstPos;
        Quaternion rotationToGo = Quaternion.LookRotation(directionToDestionationFromfirstPos);

            float timeCounter = 0f;

          playerBodyParent.transform.rotation =rotationToGo;

          centerOfPlayerAccesFalse();
            
            while (timeCounter<1)
        {
            timeCounter += Time.fixedDeltaTime*2*AspectOfhalfSecondDuration;
             
        
            
            Vector3 m_positionLinePoint = 
                                      firstPos * Mathf.Pow((1 - timeCounter), 3) 
                                      + (firstPos+directionToDestionationFromfirstPos*0.25f +  Vector3.up*4*AspectJumpHeight)* Mathf.Pow(1 - timeCounter, 2) * timeCounter * 3 
                                      + 3 * (1 - timeCounter) * timeCounter * timeCounter * (t_destination- directionToDestionationFromfirstPos*0.25f+ Vector3.up*6*AspectJumpHeight) 
                                      + timeCounter * timeCounter * timeCounter * t_destination;


            yield return null;
            transform.position = m_positionLinePoint;


            
                if(timeCounter >0.8f)
                { ChaJumpAnimationOff(); }
            centerOfPlayerSetRotateToDesiredPoint(t_destination);

        }
            playerBodyParent.transform.localRotation = Quaternion.identity;
            centerOfPlayerAccesTrue();
    
        /*
        transform.DOJump(t_destination, 5, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        */
    //    ThisIsCurrentSplineFollowerIndex += ekleme;
    
        SetTrueFollowForSpline();
        yield return null;
        SetDesiredPositionOfPlayer();
        if (m_SpeedBoostStatu == SpeedBoostStatu.noBoost)
        {
            SetAccessToGo(true);
        }

        }

    }

    public void ChangeSpeedBoostStatu(SpeedBoostStatu m_sbs )
    {
        m_SpeedBoostStatu = m_sbs;
    }

    public void SetSpeedBoostStatuSpeedBoostOnur()
    {
        ChangeSpeedBoostStatu(SpeedBoostStatu.SpeedBoostOnur);
    }
    
    public void SetSpeedBoostStatunoBoost()
    {
        ChangeSpeedBoostStatu(SpeedBoostStatu.noBoost);
    }
    
  public virtual void centerOfPlayerSetRotateToDesiredPoint(Vector3 t_destination)
  
  {
      if (isItsplineJump)
      {
          CenterOfPlayer._centerofPlayer.SetRotateToDesiredPoint(t_destination);
      }
  }

  public virtual void centerOfPlayerAccesTrue()
  {
      if (isItsplineJump)
      {
          CenterOfPlayer._centerofPlayer.access2Rotate = true;
      }
  }
  
  public virtual void centerOfPlayerAccesFalse()
  {
      if (isItsplineJump)
      {
          CenterOfPlayer._centerofPlayer.access2Rotate =false;
      }
  }


  
    public void SetTrueFollowForSpline()
    {
        m_SplineFollower.follow = true;
    }

    public void ChangeCurrentSplineComputerKeyboard() {
        if (Input.GetKeyDown(KeyCode.LeftArrow) )
        { 
          StartCoroutine( GoWithBeizer(1));
           
           
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        { 
            StartCoroutine(GoWithBeizer(-1));
            
        }

        if (Input.GetKeyDown(KeyCode.Space)) {  
            ChaJumpAnimation();  
            StartCoroutine(SpaceJump());
            //offset ekliyor belli yerler arasına pek istemiyorum bunu 
            //m_SplineFollower.offsetModifier.AddKey(JumpAmount,m_SplineFollower.result.percent,m_SplineFollower.result.percent+0.02d);
            }
        
        

    }


    /*
    public virtual void ChangeCurrentSplineComputerMouse()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            firstTouchPoint =  mousePosViewPoint;
        }
        
        else if (Input.GetMouseButtonUp(1))
        {
            Vector2 directionMouseMovement=firstTouchPoint  - (Vector2) mousePosViewPoint ;
            
            //saga sola hareket daha yuksektir
            if (Mathf.Abs(directionMouseMovement.x) > Mathf.Abs(directionMouseMovement.y) && _accessAccordingToAngle)
            {
                StartCoroutine(GoWithBeizer((int)Mathf.Sign(directionMouseMovement.x)));
            }
            else if(_accesAccordingToDistance)// yukari assagi hareket daha yuksektir
            {
                ChaJumpAnimation();  
                StartCoroutine(SpaceJump());
            }
            else        // basti cekti Hareket ettirmedi pek
            {
                Debug.Log("basti kacti");
            }
            
            
        }
        else if(Input.GetMouseButton(1))
        {
            Vector2 directionMouseMovement = firstTouchPoint  - (Vector2) mousePosViewPoint ;

            directionMouseMovement.x = 50*Mathf.Clamp(directionMouseMovement.x, -0.5f, 0.5f);
          Vector3 ustbodyRotation = (firstRotationOfUstBodyOfPlayer.eulerAngles + Vector3.forward *directionMouseMovement.x);
            UstBodyOfPlayer.transform.localRotation =Quaternion.Lerp(UstBodyOfPlayer.transform.localRotation , Quaternion.Euler(ustbodyRotation),1);//Time.deltaTime*4
                
        }
        else
        {
        }
        
        
        
    }
    */
    
    // Converge : Yakınsamak
    public void LocalRotationConverge()
    {
        UstBodyOfPlayer.transform.localRotation = Quaternion.Lerp(UstBodyOfPlayer.transform.localRotation, firstRotationOfUstBodyOfPlayer, Time.fixedDeltaTime*10);

    }

    //Diverge : ıraksamak

    public void LocalRotationDiverge(float directionMouseMovementx )
    {
        Vector3 ustbodyRotation = (firstRotationOfUstBodyOfPlayer.eulerAngles + Vector3.forward *directionMouseMovementx);
       UstBodyOfPlayer.transform.localRotation =Quaternion.Lerp(UstBodyOfPlayer.transform.localRotation , Quaternion.Euler(ustbodyRotation),1);//Time.deltaTime*4
    }
    

    
    public IEnumerator SpaceJump2()
    {
        if(m_AccessToJump)
        {
            SetAccessToJump(false);
        float timeCounter = 0;

        Vector3 firstPos = playerBody.transform.localPosition;
       
        
        while (timeCounter<1)
        {
            timeCounter += Time.deltaTime;
            Vector3 m_positionLinePoint = 
                firstPos * Mathf.Pow((1 - timeCounter), 3) +
                (firstPos+ Vector3.up*_jumpPower)* Mathf.Pow(1 - timeCounter, 2) * timeCounter * 3 
                + 3 * (1 - timeCounter) * timeCounter * timeCounter * (firstPos + Vector3.up*_jumpPower) 
                + timeCounter * timeCounter * timeCounter * firstPos;


            yield return null;
            
            playerBody.transform.localPosition = m_positionLinePoint;
            
            
        }
        playerBody.transform.localPosition =recordedPlayerBodylocalPosition ;
        
        yield return null;
       SetAccessToJump(true);
        }
        
        
    }


    public IEnumerator SpaceJump()
    {
        if (m_AccessToJump)
        {
            isShouldStopToJump = false;
            float timeCounter = 0;
            Vector3 firstPos = transform.position;
            
            Vector3 destinationPos = firstPos + transform.forward * m_SplineFollower.followSpeed;
            ChaJumpAnimation();
            SetFollowFalse();
           
            while (timeCounter<2)
            {
                
                
                timeCounter += Time.fixedDeltaTime;
                Vector3 m_positionLinePoint = 
                    firstPos * Mathf.Pow((1 - timeCounter), 3) +
                    (firstPos+ Vector3.up*_jumpPower)* Mathf.Pow(1 - timeCounter, 2) * timeCounter * 3 
                    + 3 * (1 - timeCounter) * timeCounter * timeCounter * ( destinationPos + Vector3.up*_jumpPower) 
                    + timeCounter * timeCounter * timeCounter * destinationPos;

                
                
                
                if (isShouldStopToJump) { break; }
                else { transform.position = m_positionLinePoint; }
                
                yield return null;
            }
            
            
        }
        
        

    }

    public void AfterCollSplineWhileJump(SplineComputer _splineComputer)
    {
      //////////////////////// SetSplineFollowerAutoUpdateFalse();
        isShouldStopToJump = true;
        
        // spline projector u guncelle 
        m_CurrentSplineProjectorIndex =FindSplineIndexInSplineList(_splineComputer);
        m_SplineProjector.spline = m_SplineComputerList[m_CurrentSplineProjectorIndex];
        
        // follower splinin indexsini ve spline ını yeniler
        //////////////////////////////ThisIsCurrentSplineFollowerIndex = ThisIsCurrentSplineProjectorIndex;
        
        // rebuild ediyor follower vve projector u
     //////////////////   m_SplineProjector.RebuildImmediate();
        ////////////////////m_SplineFollower.RebuildImmediate();
         
        //
      /////////////////////////  m_SplineFollower.SetPercent(m_SplineProjector.result.percent);

        // StartCoroutine(ElectroPlacement());
        // SetTrueFollowForSpline();
        
        StartCoroutine(PullingRope());




    }
 public int FindSplineIndexInSplineList(SplineComputer _splineComputer)
    {
        for (int i = 0; i < m_SplineComputerList.Length; i++)
        {
            if(_splineComputer.Equals(m_SplineComputerList[i]))
            {return  i;}
            
            
        }

        return 1;


    }

    public IEnumerator PullingRope()
    {
        TurnOnElectricy();
        
        m_SplineProjector.RebuildImmediate();
        float timeCounter =0.0f;
        Vector3 destinationVector3 =m_SplineProjector.result.position ;
        Quaternion destinationQuaternion = m_SplineProjector.result.rotation;
        double destinationPercent = m_SplineProjector.result.percent;

        Vector3 firstPos = transform.position;
        Quaternion firstRotation = transform.rotation;

        Vector3 diffFinishStart = (destinationVector3 - firstPos);
        float diffFinishStartMagnitude = 1/diffFinishStart.magnitude;
        
        
         while (timeCounter<1f)
        {
            
            timeCounter += Time.fixedDeltaTime*10*diffFinishStartMagnitude ;
            transform.position = Vector3.Slerp(firstPos, destinationVector3, timeCounter);
            transform.rotation = Quaternion.Slerp(firstRotation,destinationQuaternion,timeCounter );
            yield return null;
        }
        
        ThisIsCurrentSplineFollowerIndex = ThisIsCurrentSplineProjectorIndex;
        m_SplineFollower.SetPercent(destinationPercent);
        m_SplineFollower.RebuildImmediate();
        TurnOffElectricy();
        SetTrueFollowForSpline();
       // SetSplineFollowerAutoUpdateTrue();
    
    }
   
    
    
    public void TurnOnElectricy() {
    //    m_electroParticle.gameObject.SetActive(true); 
    }
    public void TurnOffElectricy()
    {//m_electroParticle.gameObject.SetActive(false);
     }
    
    public void SetSplineFollowerAutoUpdateFalse()
    {
        
        m_SplineFollower.autoUpdate = false;
    }

    public void SetSplineFollowerAutoUpdateTrue()
    {
        m_SplineFollower.RebuildImmediate();
        m_SplineFollower.autoUpdate = true;
    }

    public IEnumerator ElectroPlacement()
    {

        
        float timeCounter = 0;
        Vector3 firstPos = transform.position;
        
        Vector3 destinationPos = m_SplineProjector.result.position;

        Vector3 directionAndDiff = destinationPos - m_electroParticle.transform.position;
        m_electroParticle.transform.rotation = Quaternion.LookRotation(directionAndDiff);
        m_electroParticle.gameObject.SetActive(true);
        
        while (timeCounter<1)
        {
            timeCounter += Time.deltaTime;
            Vector3 m_positionLinePoint = Vector3.Lerp(firstPos, destinationPos, timeCounter);
                
                /*firstPos * Mathf.Pow((1 - timeCounter), 3) +
                (firstPos+ Vector3.up*_jumpPower)* Mathf.Pow(1 - timeCounter, 2) * timeCounter * 3 
                + 3 * (1 - timeCounter) * timeCounter * timeCounter * ( destinationPos + Vector3.up*_jumpPower) 
                + timeCounter * timeCounter * timeCounter * destinationPos;
                */
                
            transform.position = m_positionLinePoint;    
            yield return null;
            
            
        }
        
        m_electroParticle.gameObject.SetActive(false);
        SetTrueFollowForSpline();
    }
    

   
    
    
    
    
    public void SetFollowFalse() {
        m_SplineFollower.follow = false;
    }


    public bool isIt360DegreeCamOnline{ get {return !y_cam360TurnScript.enabled; }}

    public bool isItCamControllerOnline
    {
        get
        {
            if (isItsplineJump)
            {
                return y_camControllerScript.enabled;
            }
            else
            {
                return true;
            }
        }
    }


    public bool isFollowingTheSpline
    {
        get
        {
            return m_SplineFollower.follow;
        }

    }

    public void SetAccessToGo(bool statu ) { m_AccessToGo = statu && isItCamControllerOnline; }
        

    public void SetAccessToJump(bool statu) { m_AccessToJump = statu; }

   
    
    
    public void DisableGoAndJump()
    {
        SetAccessToGo(false);
        SetAccessToJump(false);
    }

    public void EnableGoAndJump()
    {
        SetAccessToGo(true);
        SetAccessToJump(true);
        
    }


    public void ChaRunLayerOn()
    { StartCoroutine(RunLayerSmoothPass(1));
        if (isItsplineJump)
        {
            SoundController.Instance.RunStarted();
           
            
        }

    }

    public void ChaRunLayerOff()
    {StartCoroutine(RunLayerSmoothPass(0));
        if (isItsplineJump)
        {SoundController.Instance.SkateStarted();
            

        }
    }

    public IEnumerator RunLayerSmoothPass(float weightDesired)
    {
        float timeCounter = 0;
        float layerWeightAtBegan =  ChaAnimator.GetLayerWeight(1);
        while (timeCounter<1)
        {
            timeCounter += Time.deltaTime*2;
            float weightCurrent = Mathf.Lerp(layerWeightAtBegan, weightDesired, timeCounter);;
             ChaAnimator.SetLayerWeight(1, weightCurrent);
             yield return null;
        }
       
        
    }
    public void SetDesiredPositionOfPlayer() { m_SplineFollower.SetPercent(_percent); }
    
    public void ChaJumpAnimation() {
     //   if (!ChaAnimator.GetCurrentAnimatorStateInfo(0).IsName("jumpup2")) { }
       
            
            ChaAnimator.SetBool("isJumping",true);


       
        /*if (!ChaAnimator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
        {
            ChaAnimator.SetTrigger("Jump");

        }*/

    }
    
    
    

    public void ChaJumpAnimationOff()
    {
        ChaAnimator.SetBool("isJumping",false);
    }
    public void AfterTouchObstacle()
    {setDead();
        CommonDeadThings();
        
        // Ragdoll geldiginden dolayi fall animation degil direkt animatoru kapiyoruz
        //ChaFallAnimation();
        BeingFreeAndGravityOn();
        
        SetCenterOfPlayerFalse();
    }

    public void AfterTouchVacuumCleaner()
    {
        setDead();
        CommonDeadThings();

        SetLayerSplineBlind();
        
        BeingFreeAndGravityOn(false);
        
        SetCenterOfPlayerFalse();
        
    }

    public void CommonDeadThings()
    {
        SetCameraForDead();
        // Ragdoll geldiginden dolayi fall animation degil direkt animatoru kapiyoruz
        //ChaFallAnimation();
        ChaRagdollActivateAndAnimatorDisable();
    }

   
    public void SetCameraForDead()
    {
        if (isItsplineJump)
        {
            y_camControllerScript.SetCameraForDead();
        }
    }

    public void SetLayerSplineBlind()
    {
        gameObject.layer =7;
    }
    public void ChaFallAnimation()
    {
        if (!ChaAnimator.GetCurrentAnimatorStateInfo(0).IsName("sweepfallonur"))
        {
            ChaAnimator.SetTrigger("fall");
        }
    }


    public void ChaRagdollActivateAndAnimatorDisable()
    {
        ChaAnimator.enabled = false;

    }
    
    
    // carpma isleminden sonra olacaklar  (Engele carpma isleminden sonra )
    public void BeingFreeAndGravityOn(bool isForcingLikeCrush = true)
    {SetRigidbodyConstraintsUnselected();
        SetSplineFollowerAndProjectorDisable();
        SetGravityTrue();

        if (isForcingLikeCrush) {
        ManipulateVelocityToGoBack(2);}
        else
        {
           ManipulateVelocityToGoForwardDown(1.5f);
        }
        
        
    }

    public void setDead()
    {
        isChaAlive = false;
    }
    
    //Gravity using True
    public void SetGravityTrue(){ m_Rigidbody.useGravity = true;}

    public void SetSplineFollowerAndProjectorDisable()
    {
        m_SplineFollower.enabled = false;
        m_SplineProjector.enabled = false;
    }

    public void SetCenterOfPlayerFalse()
    {
        if (ScriptName == "splineJump")
        {
         CenterOfPlayer._centerofPlayer.gameObject.SetActive(false);   
        }
    }
    
    //constraintlerini unselected yapiyor hepsini 
    public void SetRigidbodyConstraintsUnselected()
    {
        m_Rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void ManipulateVelocityToGoBack(float forceAmount )
    {

        c_HipsRigidbody.velocity = c_HipsRigidbody.transform.forward * -1 * followSpeed*forceAmount;

    }

    public void ManipulateVelocityToGoForwardDown(float forceAmount)
    {
      
        c_HipsRigidbody.velocity =c_HipsRigidbody.transform.forward*followSpeed*forceAmount;
    }
    

    public Vector3 vector101
    {
        get
        {
            return new Vector3(1, 0, 1);
        }
    }
    
}

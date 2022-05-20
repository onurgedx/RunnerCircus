using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScriptOnur : MonoBehaviour
{
    public Vector2 firstTouchPoint;
    public splineJump playerSplineJump;
    public Camera camMain;
    public SoundController y_SoundController;

    public static ControllerScriptOnur Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(camMain == null)
        {camMain= Camera.main;}
        
    }

    // Update is called once per frame
    void Update()
    {
    ControllingWithFinger();



    }
    /*
    public virtual void ControllingWithMouse()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            firstTouchPoint = mousePosViewPoint;
        }
        
        else if (Input.GetMouseButtonUp(1))
        {
            Vector2 directionMouseMovement=firstTouchPoint  - (Vector2) mousePosViewPoint ;
            
            //saga sola hareket daha yuksektir
            if (Mathf.Abs(directionMouseMovement.x) > Mathf.Abs(directionMouseMovement.y) && _accessAccordingToAngle)
            {
               playerSplineJump.VoidGoWithBeizer((int)Mathf.Sign(directionMouseMovement.x));
            }
            else if(_accesAccordingToDistance)// yukari assagi hareket daha yuksektir
            {
                playerSplineJump.ChaJumpAnimation();  
                StartCoroutine(playerSplineJump.SpaceJump());
            }
            else        // basti cekti Hareket ettirmedi pek
            {
                Debug.Log("basti kacti");
                    playerSplineJump.SetObstacle();
            }
            
            
        }
        else if(Input.GetMouseButton(1))
        {
            Vector2 directionMouseMovement = firstTouchPoint  - (Vector2) mousePosViewPoint ;

            directionMouseMovement.x = 50*Mathf.Clamp(directionMouseMovement.x, -0.5f, 0.5f);
            playerSplineJump.LocalRotationDiverge(directionMouseMovement.x);
            
        }
        else
        {// yakinsiyor boş durumdayken eski haline geliyor
            playerSplineJump.LocalRotationConverge();
        }
        
        
        
    }
        */
    public virtual void ControllingWithFinger()
    {
        if (isTouch)
        {
           // Touch temp_touch = Touch0;
            
        
        if (isTouchBegan)
        {
            firstTouchPoint = Touch0.position;
        }
        
        else if (isTouchEnded)
        {
            Vector2 directionMouseMovement=firstTouchPoint  - (Vector2) Touch0.position ;
            
            //saga sola hareket daha yuksektir
            if (Mathf.Abs(directionMouseMovement.x) > Mathf.Abs(directionMouseMovement.y) && _accessAccordingToAngleTouch)
            {
                   
                playerSplineJump.VoidGoWithBeizer((int)Mathf.Sign(directionMouseMovement.x));

            }
            else if(_accesAccordingToDistanceTouch)// yukari assagi hareket daha yuksektir
            {
                //playerSplineJump.ChaJumpAnimation();  
                //StartCoroutine(playerSplineJump.SpaceJump());
            }
            else        // basti cekti Hareket ettirmedi pek
            {
               // Debug.Log("basti kacti");
                playerSplineJump.SetObstacle();
            }
            
            
        }
        else 
        {
            Vector2 directionMouseMovement = camMain.ScreenToViewportPoint( firstTouchPoint  - (Vector2) Touch0.position) ;

            directionMouseMovement.x =50*Mathf.Clamp(directionMouseMovement.x, -0.5f, 0.5f);
            playerSplineJump.LocalRotationDiverge(directionMouseMovement.x);
            
        }
        
        }
        else
        {// yakinsiyor boş durumdayken eski haline geliyor
            playerSplineJump.LocalRotationConverge();
        }
        
        
        
    }


    public Touch Touch0
    {
        get { return Input.GetTouch(0); }
    }
    
    
    public bool isTouchBegan
    {
        get { return Touch0.phase == TouchPhase.Began; }
    }

    public bool isTouchEnded
    {
        get
        {
            return Touch0.phase == TouchPhase.Ended;
        }
    }

    public bool isTouch
    {
        get { return Input.touchCount > 0; }
    }
    
    /*
    public Vector3 mousePosViewPoint
    {
        get
        {
            return camMain.ScreenToViewportPoint(Input.mousePosition);
        }
    }
    */
    public Vector3 touchPosViewPoint
    {
        get
        {
            return camMain.ScreenToViewportPoint(Touch0.position);
        }
    }
    /*
    public bool _accessAccordingToAngle
    {
        get
        {
            return Mathf.Abs((firstTouchPoint - (Vector2) mousePosViewPoint).x) >0.1f ;
        }
    }*/
    public bool _accessAccordingToAngleTouch
    {
        get
        {
            return Mathf.Abs((camMain.ScreenToViewportPoint(firstTouchPoint) - touchPosViewPoint).x) >0.1f ;
        }
    }
    /*
    public bool _accesAccordingToDistance
    {
        get
        {return Mathf.Abs((firstTouchPoint - (Vector2) mousePosViewPoint).y) >0.1f ;
            
        }
    }*/
    public bool _accesAccordingToDistanceTouch
    {
        get
        {return Mathf.Abs(camMain.ScreenToViewportPoint( (firstTouchPoint - (Vector2) Touch0.position)).y) >0.1f ;
            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraControllerOnur : MonoBehaviour
{
    public float xLerp = 5.5f;
    public float yLerp = 5.5f;
    public float zLerp = 5.5f;
    
    public GameObject m_ObjectCameraFollows;
    public GameObject y_playerPivot;
    public GameObject y_LooksHere;

    public GameObject y_playerHip;
    // Update is called once per frame
    void LateUpdate()
    {
        FollowDesiredObject();
    
        
    }

    public void SetCameraForDead()
    {
        y_LooksHere = y_playerHip;
    }
    void FollowDesiredObject()
    {
    
       
       //transform.position = Vector3.Lerp(transform.position, m_ObjectCameraFollows.transform.position, Time.deltaTime*4.7f);

       transform.position = new Vector3(posX(xLerp),posY(yLerp),posZ(zLerp));

        //  
        
         transform.rotation =Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(y_LooksHere.transform.position - transform.position),Time.deltaTime*5);
       
       // denedigim bir sey 
       //transform.rotation =Quaternion.Lerp(CenterOfPlayer._centerofPlayer.transform.rotation, m_ObjectCameraFollows.transform.rotation,0.5f);
       
       //transform.rotation = Quaternion.Lerp( transform.rotation,m_ObjectCameraFollows.transform.rotation,Time.deltaTime*2);

       //   transform.rotation = new Quaternion(AngleX(0.2f), AngleY(0.4f), AngleZ(0.2f), AngleW(0.3f));
       //   transform.rotation =Quaternion.Euler(AngleX(0.4f),AngleY(0.1f),AngleZ(0.2f));

    }


    float posX(float aspect)
    { return Mathf.Lerp(transform.position.x, m_ObjectCameraFollows.transform.position.x, Time.deltaTime*aspect); }
    
    float posY(float aspect)
    {return Mathf.Lerp(transform.position.y, m_ObjectCameraFollows.transform.position.y, Time.deltaTime*aspect);}

    float posZ(float aspect)
    {return Mathf.Lerp(transform.position.z, m_ObjectCameraFollows.transform.position.z, Time.deltaTime*aspect);}

    
    Quaternion rotationToLove
    {
        get
        {
            return Quaternion.LookRotation(m_ObjectCameraFollows.transform.position - transform.position);

        }
    }

    float AngleX(float aspect)
    {
        return Mathf.Lerp(transform.rotation.x, m_ObjectCameraFollows.transform.rotation.x, aspect);

    }float AngleY(float aspect)
    {
        return Mathf.Lerp(transform.rotation.y, m_ObjectCameraFollows.transform.rotation.y, aspect);

    }float AngleZ(float aspect)
    {
        return Mathf.Lerp(transform.rotation.z, m_ObjectCameraFollows.transform.rotation.z, aspect);

    }

    float AngleW(float aspect)
    {
        return Mathf.Lerp(transform.rotation.w, m_ObjectCameraFollows.transform.rotation.w, aspect);

    }
    
    
    
}

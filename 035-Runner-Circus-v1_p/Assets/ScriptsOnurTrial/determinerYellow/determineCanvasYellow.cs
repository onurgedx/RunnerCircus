using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class determineCanvasYellow : MonoBehaviour
{

    public static determineCanvasYellow Instance;

   public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {   
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);

    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void setVisibleStatu(bool statu )
    {
        gameObject.SetActive(statu);

    }
    
}

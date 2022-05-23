using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{

    [Serializable]
   public class ObstaclesFeatureClass
    {
        public GameObject go;
        public Vector3 worldPos;
      public  ObstaclesFeatureClass(GameObject gotemp)
        {
            go = gotemp;

            worldPos = go.transform.position;
        }
        
    }

    // private i public yapinca hata veriyor buna sonra bak bakim bi /// AAQQQ USTTEKI CLASSI PRIVATE DE UNUTMUSUM ONDANMIS  COZDUM SORUNU 
   
   public List<ObstaclesFeatureClass> m_ObstaclesList = new List<ObstaclesFeatureClass>();
  

    // Start is called before the first frame update
    void Start()
    {
        int temp_ChildCount = transform.childCount;
        for (int i = 0; i < temp_ChildCount; i++)
        {
            GameObject goObstacle = transform.GetChild(i).gameObject;
            ObstaclesFeatureClass newObstacle = new ObstaclesFeatureClass(goObstacle);
           m_ObstaclesList.Add(newObstacle); 
           
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

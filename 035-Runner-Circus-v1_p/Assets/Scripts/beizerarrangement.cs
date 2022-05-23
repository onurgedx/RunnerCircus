using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class beizerarrangement : MonoBehaviour
{
    public List<Transform> BezierPointsList= new List<Transform>();
    public LineRenderer m_lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (float i = 0; i < 100; i++)
        {
            float k = i / 100;
            Vector3 m_positionLinePoint = BezierPointsList[0].position * Mathf.Pow((1 - k), 3) +
                                          BezierPointsList[1].position * Mathf.Pow(1 - k, 2) * k * 3 +
                                          3 * (1 - k) * k * k * BezierPointsList[2].position + k * k * k * BezierPointsList[3].position;
            m_lineRenderer.SetPosition((int) i,m_positionLinePoint);
        }
        
    }
}

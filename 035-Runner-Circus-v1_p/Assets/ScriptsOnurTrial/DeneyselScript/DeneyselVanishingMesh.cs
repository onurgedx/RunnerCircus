using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DeneyselVanishingMesh : MonoBehaviour
{
    public bool AccesToVanish = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(ControllerScriptOnur.Instance.isTouch&& ControllerScriptOnur.Instance.isTouchBegan &&false ){ AccesToVanish = true;
        transform.DOScale(transform.localScale *2, 0.4f).OnComplete(() =>
            {
               
                transform.DOScale(transform.localScale * 0, 0.3f);
            });
        
        }

        



    }

    private void OnTriggerStay(Collider other)
    {
        if (AccesToVanish)
        {
            MeshFilter mf = other.gameObject.GetComponent<MeshFilter>();
            List<Vector3> _vertices = mf.mesh.vertices.ToList();

            List<int> _triangles = mf.mesh.triangles.ToList();
            for (int i = _triangles.Count-1; i >=0; i -= 3)
            {
                if ((mf.transform.InverseTransformPoint(transform.position) - _vertices[_triangles[i]]).magnitude <
                    transform.localScale.magnitude*0.5f)
                {
                    _triangles.RemoveAt(i);
                    _triangles.RemoveAt(i - 1);
                     _triangles.RemoveAt(i - 2);
                }

            }

            mf.mesh.triangles = _triangles.ToArray();
            AccesToVanish = false;
            GameObject GoObstacle = Instantiate(gameObject, transform.position, transform.rotation, null);
            GoObstacle.GetComponent<DeneyselVanishingMesh>().enabled = false;
            GoObstacle.tag = "VacuumCleaner";
            GoObstacle.GetComponent<MeshRenderer>().enabled = false;
            GoObstacle.GetComponent<Collider>().isTrigger = false;
        }
    }
}

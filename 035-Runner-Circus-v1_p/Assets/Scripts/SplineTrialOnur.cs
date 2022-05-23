using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class SplineTrialOnur : MonoBehaviour
{


    public SplineComputer m_SplineComputer;
    public SplineUser m_SplineUser;

    private void Awake()
    {
        if (m_SplineComputer == null){ m_SplineComputer = GetComponent<SplineComputer>();}
        if (m_SplineUser == null){ m_SplineUser = GetComponent<SplineUser>();}

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void trialSomethings() {
        //m_SplineComputer.
        //m_SplineUser.
        
        // splineın ne kadarını kullanacagımı belirlerken
        // m_splineUser.clipFrom 0 ile 1 arasinda bir sayi alir baslangic eyrini belirler
        m_SplineUser.clipFrom = 0.2f; 
        
        //m_splineUser.clipTo 0 ile 1 arasinda bir sayi alir bitis yerini belriler
        m_SplineUser.clipTo = 0.7f;
        
        
        //autoRebuiid false ise bunu yapmam lazım elimle 
        //m_SplineUser.RebuildImmediate();
        
    }
}

using System;
using UnityEngine;


namespace AtomSim.Bohr{
    public class ElectronOrbit : MonoBehaviour{

        [SerializeField] private Transform m_Electron;
        [SerializeField] private float m_Radius = 4f;
        [SerializeField] private float m_AngularSpeed = 2.4f;
        private float m_Angle;
        private void Start(){
            Debug.Log("ElectronOrbit ishga tushdi");
            Debug.Log($"ElectronOrbit ishga tushdi. Elektron: {m_Electron.name}");
        }

        private void Update(){
            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * m_Radius;
            m_Angle += m_AngularSpeed * Time.deltaTime;
            Debug.Log($"Kadr {Time.frameCount}: dt = {Time.deltaTime}");
        }
    }
}

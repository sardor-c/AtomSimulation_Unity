using System.Reflection.Metadata;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AtomSim.Bohr {
    /// <summary>
    /// Elektronni yadro atrofida aylantiradi.
    /// Yuqori va pastki strelkalar orbita raqamini (n) o'zgartiradi.
    /// </summary>
    public sealed class ElectronOrbit : MonoBehaviour {

        const int MaxLevel = 6;

        [SerializeField] private Transform m_Electron;
        [SerializeField] private float m_LevelSpacing = 4f;
        [SerializeField] private float m_AngularSpeed = 2.4f;
        int m_Level = 1;
        float m_Angle;
        void Start() {
            Debug.Log("ElectronOrbit ishga tushdi");
            Debug.Log($"ElectronOrbit ishga tushdi. Elektron: {m_Electron.name}");
        }

        private void Update() {
            HandleKeyboard();

            m_Angle += m_AngularSpeed * Time.deltaTime;
            float radius = m_Level * m_LevelSpacing;
            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * radius;
        }

        void HandleKeyboard() {
            Keyboard current = Keyboard.current;
            if (current == null) return;

            if (current.upArrowKey.wasPressedThisFrame) SetLevel(m_Level + 1);
            if (current.downArrowKey.wasPressedThisFrame) SetLevel(m_Level - 1);
        }

        void SetLevel(int level) {
            m_Level = Mathf.Clamp(level, 1, MaxLevel);
            Debug.Log($"Orbita n = {m_Level}, radius = {m_Level * m_LevelSpacing}");
        }
    }
}

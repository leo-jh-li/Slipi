using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float m_sizePerTile;       // Size to increase camera for each tile in the level
    [SerializeField] private float m_widthBuffer;       // Extra value to add on to size
    

    private void Start() {
        cam = Camera.main; 
    }

    public void ResizeCamera(int tiles) {
        cam.orthographicSize = m_sizePerTile * tiles + m_widthBuffer;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Método que corrige a direção da barra de vida do inimigo (fica sempre de frente para o player) 
public class LookAtPlayer : MonoBehaviour
{
    
    public Transform cam;

    void LateUpdate()
    {
        transform.LookAt(cam);        
    }
}

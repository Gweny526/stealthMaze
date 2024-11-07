using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorModeControl : MonoBehaviour
{

    [SerializeField] bool visible;
    [SerializeField] CursorLockMode lockMode;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
    }
    
}

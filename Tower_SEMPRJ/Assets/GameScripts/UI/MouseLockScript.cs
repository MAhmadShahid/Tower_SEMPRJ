using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLockScript : MonoBehaviour
{
    private bool _isMouseLocked = false;

    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(this);    
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isMouseLocked) LockMouse(); else UnLockMouse(); 
        }

        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            LockMouse();
        }
    }

    void LockMouse()
    {
        if(!_isMouseLocked) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _isMouseLocked = true;
        }
    }

    void UnLockMouse()
    {
        if(_isMouseLocked) 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _isMouseLocked = false;
        }
    }

    private bool IsMouseOverUI()
    {
        // Check if the mouse is over any UI element
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}

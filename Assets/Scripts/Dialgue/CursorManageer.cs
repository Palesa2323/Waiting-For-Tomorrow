using UnityEngine;

public class CursorManageer : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // Keep forcing it in case Unity tries to hide it again
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

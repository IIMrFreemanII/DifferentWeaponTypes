using System;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public CharacterStatus characterStatus;
    public event Action Fire;

    private void Update()
    {
        characterStatus.vertical = Input.GetAxis("Vertical");
        characterStatus.horizontal = Input.GetAxis("Horizontal");
        characterStatus.isSprint = Input.GetKey(KeyCode.LeftShift);
        characterStatus.isAiming = Input.GetMouseButton(1);
        characterStatus.jump = Input.GetAxis("Jump");
        
        if (Input.GetMouseButtonDown(0))
        {
            Fire?.Invoke();
        }
    }

    private void OnApplicationQuit()
    {
        characterStatus.vertical = 0;
        characterStatus.horizontal = 0;
        characterStatus.isSprint = false;
        characterStatus.isAiming = false;
        characterStatus.jump = 0;
        characterStatus.isGrounded = false;
    }
}

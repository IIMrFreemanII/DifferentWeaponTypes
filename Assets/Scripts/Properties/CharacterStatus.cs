using UnityEngine;

[CreateAssetMenu(menuName = "Character/status")]
public class CharacterStatus : ScriptableObject
{
    public bool isAiming;
    public bool isSprint;
    public bool isGrounded;

    public float vertical;
    public float horizontal;
    public float jump;

    public float speed;
    public float jumpForce;
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NonPhysicalCharacterMovement : MonoBehaviour
{
    public CharacterStatus characterStatus;
    public Transform cameraHolderTransform;
    
    private Rigidbody _rb;
    private CapsuleCollider _collider;
    
    //!!!!Нацепите на него нестандартный Layer, например Player!!!!
    public LayerMask GroundLayer = 1; // 1 == "Default"

    //что бы эта переменная работала добавьте тэг "Ground" на вашу поверхность земли
    private bool _isGrounded
    {
        get {
            var bottomCenterPoint = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);
            //создаем невидимую физическую капсулу и проверяем не пересекает ли она обьект который относится к полу

            //_collider.bounds.size.x / 2 * 0.9f -- эта странная конструкция берет радиус обьекта.
            // был бы обязательно сферой -- брался бы радиус напрямую, а так пишем по-универсальнее

            bool isGrounded = Physics.CheckCapsule(_collider.bounds.center, bottomCenterPoint,
                _collider.bounds.size.x / 2 * 0.9f, GroundLayer);
            characterStatus.isGrounded = isGrounded;
            
            return characterStatus.isGrounded;
            // если можно будет прыгать в воздухе, то нужно будет изменить коэфициент 0.9 на меньший.
        }
    }
    
    private Vector3 _movementVector
    {
        get
        {
            var horizontal = characterStatus.horizontal;
            var vertical = characterStatus.vertical;

            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
                
            return moveDirection;
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    private void LateUpdate()
    {
        HandleRotation();
    }

    void FixedUpdate()
    {
        JumpLogic();
        MovementLogic();
    }

    private void MovementLogic()
    {
            transform.Translate(_movementVector * (characterStatus.speed * Time.fixedDeltaTime));
    }

    private void JumpLogic()
    {
        if (_isGrounded && characterStatus.jump > 0)
        {
            _rb.AddForce(Vector3.up * characterStatus.jumpForce, ForceMode.Impulse);
        }
    }
    
    private void HandleRotation()
    {
        Vector3 rotationDirection = cameraHolderTransform.forward;
        Quaternion lookRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = lookRotation;
    }
}
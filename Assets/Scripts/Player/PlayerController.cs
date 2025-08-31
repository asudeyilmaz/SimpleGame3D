using UnityEditor.ShaderGraph;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _orientationTransform;
    private float _horizontalInput, _verticalInput;
    private Rigidbody _playerRigidbody;
    [Header("Move Settings")]
    private Vector3 _movementDirection;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private KeyCode _jumpKey;
    private bool _canJump;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
    }
	void Start()
	{
        _canJump = true;
	}
	private void Update()
    {
        SetInputs();
       
    }
    void FixedUpdate()
    {
        SetPlayerMovement();

    }
    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(_jumpKey) &&  _canJump && IsGrounded())
        {
            SetPlayerJump();
            _canJump = false;
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }
    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);
    }

    private void SetPlayerJump()
    {
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

   private bool IsGrounded()
{
    Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // biraz yukarıdan başlat
    float rayLength = _playerHeight / 2 + 0.2f;
    return Physics.Raycast(rayOrigin, Vector3.down, rayLength, _groundLayer);
}

    private void ResetJump()
    {
        _canJump = true;
    }
}

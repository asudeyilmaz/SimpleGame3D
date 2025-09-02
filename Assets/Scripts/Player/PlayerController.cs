using UnityEditor.ShaderGraph;
using UnityEngine;

public class PlayerController : MonoBehaviour
{[Header("References")]
    [SerializeField] private Transform _orientationTransform;
    private float _horizontalInput, _verticalInput;
    private Rigidbody _playerRigidbody;

    [Header("Move Settings")]
    private Vector3 _movementDirection;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private KeyCode _movementKey;
    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private KeyCode _jumpKey;
    private bool _canJump;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _playerHeight;
    [Header("Slide Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private float _slideDrag;
    private bool _isSiliding;

    [Header("Ground Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;

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
        SetPlayerDrag();
        LimitedPlayerSpeed();
       
    }
    void FixedUpdate()
    {
        SetPlayerMovement();

    }
    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(_slideKey))
        {
            _isSiliding = true;
           
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSiliding = false;
        }
        else if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            SetPlayerJump();
            _canJump = false;
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
        
    }
    private void SetPlayerMovement()
    {
        if (_isSiliding)
        {
            _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
            _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed * _slideMultiplier, ForceMode.Force);
        }
        else
        {
        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);           
        }
    }
    private void LimitedPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        if (flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 LimitedVelocity = flatVelocity.normalized * _movementSpeed;
            _playerRigidbody.linearVelocity = new Vector3(LimitedVelocity.x, _playerRigidbody.linearVelocity.y, LimitedVelocity.z);
        }
    }

    private void SetPlayerJump()
    {
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
    private void SetPlayerDrag()
    {
        if (_isSiliding)
        {
            _playerRigidbody.linearDamping = _slideDrag;
        }
        else
        {
            _playerRigidbody.linearDamping = _groundDrag;
        }
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

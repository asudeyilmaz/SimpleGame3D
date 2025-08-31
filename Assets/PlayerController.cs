using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _orientationTransform;
    private float _horizontalInput, _verticalInput;
    private Rigidbody _playerRigidbody;
    private Vector3 _movementDirection;
    [SerializeField] private float _movementSpeed;
    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
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
    }
    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);
    }

}

using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Joystick _joystick;
    [Range(0, 10)] public float moveSpeed = 1;
    [Range(10, 50)] public float rotateSpeed;
    private Vector3 _moveDir;
    private Quaternion _lookRot;
    private Animator _animator;
    private float hitDelay;

    private void Start()
    {
        _joystick = FindObjectOfType<Joystick>();
        GetLocalComponents();
    }

    private void GetLocalComponents()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move(_joystick.Direction);
    }

    private void Move(Vector2 direction)
    {
        _moveDir = Vector3.right * direction.x + Vector3.forward * direction.y;
        _controller.Move(_moveDir * moveSpeed * Time.deltaTime);
        _lookRot = Quaternion.LookRotation(_moveDir.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRot,
            Time.deltaTime * rotateSpeed * _moveDir.sqrMagnitude);
        ControlAnimations(direction);
    }

    private void ControlAnimations(Vector2 direction)
    {
        _animator.SetFloat("Speed", direction.sqrMagnitude);
    }
}
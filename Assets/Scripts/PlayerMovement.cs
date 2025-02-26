using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D _playerRb;
    private Vector2 moveDirection;
    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetMoveAxis();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void GetMoveAxis()
    {
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void MovePlayer()
    {
        _playerRb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public Vector2 GetMoveDirection(){
        return moveDirection;
    }
}

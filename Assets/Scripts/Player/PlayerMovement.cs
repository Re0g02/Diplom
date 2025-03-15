using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Vector2 lastMoveVector;
    private Rigidbody2D _playerRb;
    private Vector2 moveDirection;
    private PlayerStats player;
    void Start()
    {
        player = GetComponent<PlayerStats>();
        _playerRb = GetComponent<Rigidbody2D>();
        lastMoveVector = new Vector2(1, 0f);
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
        if (GameManager.instance.IsGameOver) return;
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        if (moveX != 0 || moveY != 0)
            lastMoveVector = new Vector2(moveX, moveY).normalized;
    }

    void MovePlayer()
    {
        if (GameManager.instance.IsGameOver) return;
        _playerRb.linearVelocity = new Vector2(moveDirection.x * player.CurrentMoveSpeed,
                                               moveDirection.y * player.CurrentMoveSpeed);
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }
}

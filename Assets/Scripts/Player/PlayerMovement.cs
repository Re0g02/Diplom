using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Vector2 lastMoveVector;
    private Rigidbody2D _playerRb;
    private Vector2 moveDirection;
    private PlayerStats player;
    private Collider2D _playerCollider;

    [Header("Dash Parameters")]
    [SerializeField] private float dashSpeedMultiplier = 2f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private GameObject dashGhostPrefab;
    [SerializeField] private float ghostLifetime = 0.1f;
    [SerializeField] private float ghostInterval = 0.03f;

    private bool _isDashing = false;
    private float _dashTimer = 0f;
    private float _dashCooldownTimer = 0f;
    private int _originalCollisionMask;

    void Start()
    {
        player = GetComponent<PlayerStats>();
        _playerRb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<Collider2D>();
        lastMoveVector = new Vector2(1, 0f);


        _originalCollisionMask = gameObject.layer;
    }

    private void Update()
    {
        GetMoveAxis();
        HandleDashInput();
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

        if (_isDashing)
        {
            _dashTimer += Time.fixedDeltaTime;
            if (_dashTimer >= dashDuration)
            {
                _isDashing = false;
                _dashTimer = 0f;
                _dashCooldownTimer = dashCooldown;

                gameObject.layer = _originalCollisionMask;
            }

            _playerRb.linearVelocity = lastMoveVector * player.CurrentMoveSpeed * dashSpeedMultiplier;
        }
        else
        {
            _playerRb.linearVelocity = new Vector2(moveDirection.x * player.CurrentMoveSpeed,
                                               moveDirection.y * player.CurrentMoveSpeed);
        }

        if (_dashCooldownTimer > 0)
        {
            _dashCooldownTimer -= Time.fixedDeltaTime;
        }
    }

    private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isDashing && _dashCooldownTimer <= 0 && !GameManager.instance.IsGameOver)
        {
            _isDashing = true;

            gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");

            StartCoroutine(CreateDashGhosts());
        }
    }

    private IEnumerator CreateDashGhosts()
    {
        float timer = 0f;
        while (timer < dashDuration)
        {
            GameObject ghost = Instantiate(dashGhostPrefab, transform.position, transform.rotation);
            SpriteRenderer ghostSprite = ghost.GetComponent<SpriteRenderer>();
            SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
            ghostSprite.flipX = CheckSpriteDirection();

            if (ghostSprite != null && playerSprite != null)
            {
                ghostSprite.sprite = playerSprite.sprite;
                ghostSprite.color = new Color(1f, 1f, 1f, 0.5f);
            }
            else
            {
                Debug.LogWarning("Dash ghost prefab is missing SpriteRenderer component!");
            }

            Destroy(ghost, ghostLifetime);
            timer += ghostInterval;
            yield return new WaitForSeconds(ghostInterval);
        }
    }

    bool CheckSpriteDirection()
    {
        bool spriteDirection;

        if (moveDirection.x < 0) spriteDirection = true;
        else if (moveDirection.x > 0) spriteDirection = false;
        else spriteDirection = GetComponent<SpriteRenderer>().flipX;

        return spriteDirection;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }
}

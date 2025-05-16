using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _playerAnimator;
    private PlayerMovement _playerMovementScript;
    private SpriteRenderer _playerSpriteRenderer;

    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _playerMovementScript = GetComponent<PlayerMovement>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_playerMovementScript.GetMoveDirection().x != 0 || _playerMovementScript.GetMoveDirection().y != 0)
        {
            _playerAnimator.SetBool("IsMoving", true);
            FlipSpriteToMoveDirection();
        }
        else
            _playerAnimator.SetBool("IsMoving", false);
    }

    public bool CheckSpriteDirection()
    {
        bool spriteDirection;

        if (_playerMovementScript.GetMoveDirection().x < 0) spriteDirection = true;
        else if (_playerMovementScript.GetMoveDirection().x > 0) spriteDirection = false;
        else spriteDirection = _playerSpriteRenderer.flipX;

        return spriteDirection;
    }

    void FlipSpriteToMoveDirection()
    {
        _playerSpriteRenderer.flipX = CheckSpriteDirection();
    }
}

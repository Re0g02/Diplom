using UnityEditor;
using UnityEngine;

public class AvailableChunks : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        UpLeft,
        DownLeft,
        UpRight,
        DownRight,
    }

    [SerializeField] private GameObject Right;
    [SerializeField] private GameObject Left;
    [SerializeField] private GameObject Up;
    [SerializeField] private GameObject Down;
    [SerializeField] private GameObject UpLeft;
    [SerializeField] private GameObject DownLeft;
    [SerializeField] private GameObject UpRight;
    [SerializeField] private GameObject DownRight;

    public GameObject GetChunkByDirection(string direction)
    {
        if (direction == Direction.Right.ToString()) return Right;
        if (direction == Direction.Left.ToString()) return Left;
        if (direction == Direction.Up.ToString()) return Up;
        if (direction == Direction.Down.ToString()) return Down;
        if (direction == Direction.UpRight.ToString()) return UpRight;
        if (direction == Direction.DownRight.ToString()) return DownRight;
        if (direction == Direction.UpLeft.ToString()) return UpLeft;
        if (direction == Direction.DownLeft.ToString()) return DownLeft;
        return null;
    }

    public void SetUpAndDown(GameObject up, GameObject down)
    {
        Down = down;
        Up = up;
        int[] a;
    }


}

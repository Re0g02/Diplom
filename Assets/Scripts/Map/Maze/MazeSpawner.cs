using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public Cell CellPrefab;
    public Vector3 CellSize = new Vector3(2, 1, 0);
    public GameObject cellsContainer;
    public Maze maze;

    private void Start()
    {
        CellPrefab = LevelSelector.GetLevelData().CellPrefab;
        MazeGenerator generator = new MazeGenerator();
        maze = generator.GenerateMaze();
        var mazeOffsetX = 0;
        var mazeOffsetY = 0;

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x - mazeOffsetX, y * CellSize.y - mazeOffsetY, 0), Quaternion.identity);
                c.gameObject.transform.SetParent(cellsContainer.transform);
                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
            }
        }

    }
}
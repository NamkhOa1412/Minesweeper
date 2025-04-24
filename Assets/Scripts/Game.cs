using UnityEngine;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height = 16;

    private Board board;
    private Cell[,] state;

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    private void Start()
    {
        int newWidth = PlayerPrefs.GetInt("Width", 16);  // mặc định 16
        int newHeight = PlayerPrefs.GetInt("Height", 16);
        NewGameWithLevel(newWidth, newHeight);
    }

    public void NewGameWithLevel(int newWidth, int newHeight )
    {
        width = newWidth;
        height = newHeight;
        NewGame();
    }

    private void NewGame()
    {
        state = new Cell[width, height];
        GenerateCells();
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        board.Draw(state);
    }

    private void GenerateCells()
    {
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(i, j, 0);
                cell.type = Cell.Type.Empty;
                state[i,j] = cell;
            }
        }
    }
}

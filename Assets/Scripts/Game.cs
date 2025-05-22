using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public int mineCount = 32;

    private Board board;
    private Cell[,] state;
    public SliderController sliderController;
    private bool gameOver;
    private bool gameWin;

    private GameObject settingsPopup;
    private bool isZooming = false;
    public GameController gameController;

    private void OnValidate()
    {
        mineCount = Mathf.Clamp(mineCount, 0, width* height);
    }

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
        gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        int newWidth = PlayerPrefs.GetInt("Width", 16);
        int newHeight = PlayerPrefs.GetInt("Height", 16);
        NewGameWithLevel(newWidth, newHeight);
    }

    public void NewGameWithLevel(int newWidth, int newHeight )
    {
        width = newWidth;
        height = newHeight;
        if (width == 9)
        {
            mineCount = 10;
        }
        else if (width == 16)
        {
            mineCount = 40;
        }
        else
        {
            mineCount = 150;
        }
        NewGame();
    }

    private void NewGame()
    {
        state = new Cell[width, height];
        gameOver = false;
        gameWin = false;
        GenerateCells();
        GenerateMines();
        GenerateNumbers();
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

    private void GenerateMines()
    {
        for (int i = 0; i < mineCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);


            while (state[x,y].type == Cell.Type.Mine)
            {
                x++;

                if (x >= width)
                {
                    x = 0;
                    y++;

                    if (y >= height)
                    {
                        y = 0;
                    }
                }
            }
            state[x,y].type = Cell.Type.Mine;
            //state[x, y].revealed = true;
        }
    }

    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                if (cell.type == Cell.Type.Mine)
                {
                    continue;
                }

                cell.number = CountMines(x, y);

                if (cell.number > 0)
                {
                    cell.type = Cell.Type.Number;
                }

                //cell.revealed = true;
                state[x, y] = cell;
            }
        }
    }

    private int CountMines(int cellX, int cellY)
    {
        int count = 0;

        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if (adjacentX == 0 && adjacentY == 0)
                {
                    continue;
                }

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                //if (x < 0 || x >= width || y < 0 || y >= height)
                //{
                //    continue;
                //}

                //if (state[x, y].type == Cell.Type.Mine)
                if (GetCell(x,y).type == Cell.Type.Mine)
                    {
                    count++;
                }
            }
        }

        return count;
    }

    private void Update()
    {
        if (settingsPopup == null)
        {
            settingsPopup = GameObject.FindWithTag("SettingPopup");
        }
        if (!gameOver && !gameWin)
        {
            if (settingsPopup != null && settingsPopup.activeSelf)
            {
                return;
            }

            if (Input.touchCount == 2)
            {
                isZooming = true;
                return;
            }

            if (Input.touchCount == 0)
            {
                isZooming = false;
            }

            if (Input.touchCount == 1 && !isZooming)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    int sliderValue = sliderController.ValueSlider();
                    if (sliderValue == 1)
                    {
                        Flag(touch.position);
                    }
                    else
                    {
                        Reveal(touch.position);
                        if (gameOver == true)
                        {
                            gameController.ShowGameOverUI();
                        }
                        if (gameWin == true)
                        {
                            gameController.ShowGameWinUI();
                        }
                    }
                }
            }
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    int sliderValue = sliderController.ValueSlider();
        //    Flag(sliderValue);
        //}
        //else if (Input.GetMouseButtonDown(0))
        //{
        //    Reveal();
        //}
    }

    private void Flag(Vector2 touchPosition)
    //private void Flag(int valueSlider)
    {
        //if (valueSlider == 1)
        //{
            // lay vi tri của chuot
            //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //lay vi tri cua ngon tay nhan
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));
            Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
            Cell cell = GetCell(cellPosition.x, cellPosition.y);

            if (cell.type == Cell.Type.Invalid)
            {
                return;
            }

            cell.flagged =  !cell.flagged;
            state[cellPosition.x, cellPosition.y] = cell;

            board.Draw(state);
        //}
    }

    private Cell GetCell(int x, int y)
    {
        if (IsValid(x,y))
        {
            return state[x,y];
        }
        else
        {
            return new Cell();
        }
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < width && y>= 0 && y < height;
    }

    private void Reveal(Vector2 touchPosition)
    {
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //lay vi tri cua ngon tay nhan
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged ) 
        {
            return ;
        }

        switch (cell.type)
        {
            case Cell.Type.Mine:
                Explode(cell);
                break;

            case Cell.Type.Empty:
                Flood(cell);
                CheckWinCondition();
                break;

            default:
                cell.revealed = true;
                state[cellPosition.x, cellPosition.y] = cell;
                CheckWinCondition();
                break;
        }

        //if (cell.type == Cell.Type.Empty)
        //{
        //    Flood(cell);
        //}

        //cell.revealed = true;
        //state[cellPosition.x, cellPosition.y] = cell;
        board.Draw(state);
    }

    private void Flood(Cell cell)
    {
        if (cell.revealed) return;
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) return;
        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;
        if (cell.type == Cell.Type.Empty)
        {
            Flood(GetCell(cell.position.x - 1, cell.position.y));
            Flood(GetCell(cell.position.x + 1, cell.position.y));
            Flood(GetCell(cell.position.x, cell.position.y - 1));
            Flood(GetCell(cell.position.x, cell.position.y + 1));
        }
    }

    private void Explode(Cell cell)
    {
        Debug.Log("gameover");
        gameOver = true;

        cell.revealed = true;
        cell.exploded = true;

        state[cell.position.x, cell.position.y] = cell;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.revealed = true;
                    state[x,y] = cell;
                }
            }
        }
    }

    private void CheckWinCondition()
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type != Cell.Type.Mine && !cell.revealed)
                {
                    return;
                }
            }
        }

        Debug.Log("you win");
        gameWin = true;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.flagged = true;
                    state[x, y] = cell;
                }
            }
        }
    }
}

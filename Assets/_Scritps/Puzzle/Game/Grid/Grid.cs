using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public ShapeStorage shapeStorage;
    public int colums = 0;
    public int rows = 0;
    public float squareGrap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffet = 0.0f;
    public SquareTextureData squareTextureData;
    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();
    private LineIndicator _lineIndicator;
    private Config.SquareColor currentActiveSquareColor = Config.SquareColor.NotSet;
    private List<Config.SquareColor> colorsInTheGrid_ = new List<Config.SquareColor>();

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor += OnUpdateSquareColor;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor -= OnUpdateSquareColor;
    }

    void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
        currentActiveSquareColor = squareTextureData.activeSquareTextures[0].squareColor;
    }

    private void OnUpdateSquareColor(Config.SquareColor color)
    {
        currentActiveSquareColor = color;
    }

    private List<Config.SquareColor> GetAllSquareColorsInTheGrid()
    {
        var colors = new List<Config.SquareColor>();

        foreach(var square in _gridSquares)
        {
            var gridSqaure = square.GetComponent<GridSquare>();
            if (gridSqaure.SquareOcccuied)
            {
                var color = gridSqaure.GetCurrentColor();
                if(colors.Contains(color) == false)
                {
                    colors.Add(color);
                }
            }
        }
        return colors;
    }
    
    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquarePositions();
    }

    private void SpawnGridSquares()
    {
        int square_index = 0;

        for(var row = 0; row < rows; ++row)
        {
            for(var colum = 0; colum < colums; ++colum)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(_lineIndicator.GetGridSquareIndex(square_index) %2 == 0);
                square_index++;
            }
        }

    }

    private void SetGridSquarePositions()
    {
        int colum_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();

        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffet;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffet;


        foreach(GameObject square in _gridSquares)
        {
            if(colum_number + 1 > colums)
            {
                square_gap_number.x = 0;

                colum_number = 0;
                row_number++;
                row_moved = true;
            }

            var pos_x_offset = _offset.x * colum_number + (square_gap_number.x * squareGrap);
            var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squareGrap);

            if(colum_number > 0 && colum_number %3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squareGrap;
            }

            if(row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squareGrap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);
            colum_number++;
        }
    }

    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexs = new List<int>();

        foreach(var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();

            if(gridSquare.Selected && !gridSquare.SquareOcccuied)
            {
                squareIndexs.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
                //gridSquare.ActiveSquare();
            }
        }

        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return;

        if(currentSelectedShape.TotalSquareNumber == squareIndexs.Count)
        {
            foreach(var squareIndex in squareIndexs)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard(currentActiveSquareColor);
            }

            var shapeLeft = 0;
            foreach(var shape in shapeStorage.shapeList)
            {
                if(shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }
            //currentSelectedShape.DeactivateShape();
            if(shapeLeft == 0)
            {
                GameEvents.RequestNewShapes();
            }
            else
            {
                GameEvents.SetShapeInactive();
            }
            CheckAnyLineIsCompleted();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }
    }

    void CheckAnyLineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();


        foreach(var colum in _lineIndicator.columIndexs)
        {
            lines.Add(_lineIndicator.GetVerticallLine(colum));
        }

        for (var row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);
            for(var index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]);
            }
            lines.Add(data.ToArray());
        }

        for(var squares = 0; squares < 9; squares++)
        {
            List<int> data = new List<int>(9);

            for(var index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.square_data[squares, index]);
            }
            lines.Add(data.ToArray());
        }

        colorsInTheGrid_ = GetAllSquareColorsInTheGrid();

        var completedLines = CheckIfSquareAreCompleted(lines);

        if(completedLines >= 2)
        {
            GameEvents.ShowCongratulationWritings();
        }
        if(completedLines >= 1)
        {
            AudioManager.Instance.PlaySFX("GetPoint");
        }

        var totalScores = 10 * completedLines;
        var bonusScores = ShouldPlayColorBonusAnimation();
        GameEvents.AddScores(totalScores + bonusScores);
        AudioManager.Instance.PlaySFX("Move");
        CheckIfPlayerLost();
    }

    private int ShouldPlayColorBonusAnimation()
    {
        var colorsInTheGridAfterLineRemoved = GetAllSquareColorsInTheGrid();
        Config.SquareColor colorToPlayBonusFor = Config.SquareColor.NotSet;

        foreach(var squareColor in colorsInTheGrid_)
        {
            if(colorsInTheGridAfterLineRemoved.Contains(squareColor) == false)
            {
                colorToPlayBonusFor = squareColor;
            }
        }

        if(colorToPlayBonusFor == Config.SquareColor.NotSet)
        {
            return 0;
        }

        if(colorToPlayBonusFor == currentActiveSquareColor)
        {
            return 0;
        }

        GameEvents.ShowBonusScreen(colorToPlayBonusFor);

        return 50;
    }

    private int CheckIfSquareAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();
        var linesCompleted = 0;

        foreach(var line in data)
        {
            var lineCompleted = true;
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();

                if(comp.SquareOcccuied == false)
                {
                    lineCompleted = false;
                }
            }
            if (lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach(var line in completedLines)
        {
            var completed = false;
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactive();
                completed = true;
            }
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccpied();
            }
            if (completed)
            {
                linesCompleted++;
            }
        }
        return linesCompleted;
    }

    private void CheckIfPlayerLost()
    {
        var vaildShapes = 0;
        for(var index = 0; index < shapeStorage.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorage.shapeList[index].IsAnyOfShapeSquareActive();

            if(CheckIfShapeCanBePlacedOnGrid(shapeStorage.shapeList[index]) && isShapeActive)
            {
                shapeStorage.shapeList[index]?.ActiveShape();
                vaildShapes++;
            }
        }

        if(vaildShapes == 0)
        {
            GameEvents.GameOver(false);
            AudioManager.Instance.PlaySFX("Lose");
        }
        
    }

    private bool CheckIfShapeCanBePlacedOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.CurrentShapeData;
        int shapeColums = currentShapeData.colums;
        int shapeRows = currentShapeData.rows;
        List<int> originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;

        for(var rowIndex = 0; rowIndex < shapeRows; rowIndex++)
        {
            for(var columIndex = 0; columIndex < shapeColums; columIndex++)
            {
                if (currentShapeData.board[rowIndex].colum[columIndex])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }
                squareIndex++;
            }
        }

        if (currentShape.TotalSquareNumber != originalShapeFilledUpSquares.Count)
        {

        }

        var squareList = GetAllSquaresCombination(shapeColums, shapeRows);

        bool canBePlaced = false;

        foreach(var number in squareList)
        {
            bool shapeCanBePlacedOnTheBoard = true;
            foreach(var sqaureIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[number[sqaureIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquareOcccuied)
                {
                    shapeCanBePlacedOnTheBoard = false;
                }
            }

            if (shapeCanBePlacedOnTheBoard)
            {
                canBePlaced = true;
            }
        }
        return canBePlaced;
    }

    private List<int[]> GetAllSquaresCombination(int colums, int rows)
    {
        var sqaureList = new List<int[]>();
        var lastColumIndex = 0;
        var lastRowIndex = 0;
        int safeIndex = 0;

        while(lastRowIndex + (rows - 1 )< 9)
        {
            var rowData = new List<int>();

            for(int row = lastRowIndex; row < lastRowIndex+rows; row++)
            {
                for(int colum = lastColumIndex; colum < lastColumIndex +colums; colum++)
                {
                    rowData.Add(_lineIndicator.line_data[row, colum]);
                }
            }
            sqaureList.Add(rowData.ToArray());
            lastColumIndex++;
            if(lastColumIndex + (colums - 1) >= 9)
            {
                lastRowIndex++;
                lastColumIndex = 0;
            }
            safeIndex++;

            if (safeIndex > 100) break;
        }
        return sqaureList;
    }
}

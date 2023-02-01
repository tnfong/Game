using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject squareShapeImage;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0f, 700);
    [HideInInspector]
    public ShapeData CurrentShapeData;
    public int TotalSquareNumber { get; set; }
    private List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
    //private bool _shapeDraggable = true;
    private Canvas _canvas;
    private Vector3 _startPosition;
    private bool _shapeActive = true;

    public void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        //_shapeDraggable = true;
        _startPosition = _transform.localPosition;
        _shapeActive = true;
    }

    void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvents.SetShapeInactive -= SetShapeInactive;
    }

    void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvents.SetShapeInactive += SetShapeInactive;
    }

    public void DeactivateShape()
    {
        if (_shapeActive)
        {
            foreach(var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeactivateShape();
            }
        }
        _shapeActive = false;
    }

    public void ActiveShape()
    {
        if (!_shapeActive)
        {
            foreach(var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape();
            }
        }
        _shapeActive = true;
    }

    public void SetShapeInactive()
    {
        if(IsOnStartPosition() == false && IsAnyOfShapeSquareActive())
        {
            foreach(var square in _currentShape)
            {
                square.gameObject.SetActive(false);
            }
        }
    }

    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }

    public bool IsAnyOfShapeSquareActive()
    {
        foreach(var square in _currentShape)
        {
            if (square.gameObject.activeSelf)
                return true;
        }
        return false;
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        _transform.localPosition = _startPosition;
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        var totalSquareNumber = GetNumberOfSquare(shapeData);
        TotalSquareNumber = GetNumberOfSquare(shapeData);

        while (_currentShape.Count <= totalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }

        foreach(var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);


        int currentIndexList = 0;

        for(var row = 0; row < shapeData.rows; row++)
        {
            for(var colum = 0; colum < shapeData.colums; colum++)
            {
                if (shapeData.board[row].colum[colum])
                {
                    _currentShape[currentIndexList].SetActive(true);
                    _currentShape[currentIndexList].GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPositionForShapeSquare(shapeData, colum, moveDistance), GetYPositionForShapeSquare(shapeData, row, moveDistance));
                    currentIndexList++;
                }
            }
        }
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;

        if (shapeData.rows > 1)
        {
            if (shapeData.rows % 2 != 0)
            {
                var middleSquareIndex = (shapeData.rows - 1) / 2;
                var multiplier = (shapeData.rows - 1) / 2;
                if (row < middleSquareIndex)
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex)
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : (shapeData.rows - 2);
                var multiplier = shapeData.rows / 2;

                if (row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if (row == middleSquareIndex2) shiftOnY = moveDistance.y / 2 * -1;
                    if (row == middleSquareIndex1) shiftOnY = (moveDistance.y / 2);
                }

                if (row < middleSquareIndex1 && row < middleSquareIndex2)
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }

                else if (row > middleSquareIndex1 && row > middleSquareIndex2)
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
            }
        }
        return shiftOnY;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int colum, Vector2 moveDistance)
    {
        float shiftOnX = 0f;

        if(shapeData.colums > 1)
        {
            if(shapeData.colums % 2 != 0)
            {
                var middleSquareIndex = (shapeData.colums - 1) / 2;
                var multiplier = (shapeData.colums - 1) / 2;
                if(colum < middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                } else if(colum > middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.colums == 2) ? 1 : (shapeData.colums / 2);
                var middleSquareIndex1 = (shapeData.colums == 2) ? 0 : (shapeData.colums - 1);
                var multiplier = shapeData.colums / 2;

                if(colum == middleSquareIndex1 || colum == middleSquareIndex2)
                {
                    if (colum == middleSquareIndex2) shiftOnX = moveDistance.x / 2;
                    if (colum == middleSquareIndex1) shiftOnX = (moveDistance.x / 2) * -1;
                }

                if(colum < middleSquareIndex1 && colum < middleSquareIndex2)
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }

                else if(colum > middleSquareIndex1 && colum > middleSquareIndex2)
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
        }
        return shiftOnX;
    }

    private int GetNumberOfSquare(ShapeData shapeData)
    {
        int number = 0;
        foreach(var rowData in shapeData.board)
        {
            foreach(var active in rowData.colum)
            {
                if (active)
                    number++;
            }
        }
        return number;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
        GameEvents.CheckIfShapeCanBePlaced();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    private void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = _startPosition;
        AudioManager.Instance.PlaySFX("DeadZone");
    }
}

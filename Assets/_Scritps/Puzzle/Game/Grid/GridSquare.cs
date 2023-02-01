using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image hoverImage;
    public Image activeImage;
    public Image normalImage;
    public List<Sprite> normalImages;

    private Config.SquareColor currentSquareColor_ = Config.SquareColor.NotSet;

    public Config.SquareColor GetCurrentColor()
    {
        return currentSquareColor_;
    }

    public bool Selected { get; set; }
    public int SquareIndex {get; set;}
    public bool SquareOcccuied { get; set; }

    void Start()
    {
        Selected = false;
        SquareOcccuied = false;
    }

    public bool CanWeUseThisSquare()
    {
        return hoverImage.gameObject.activeSelf;
    }
    
    public void PlaceShapeOnBoard(Config.SquareColor color)
    {
        currentSquareColor_ = color;
        ActiveSquare();
    }
    public void ActiveSquare()
    {
        hoverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOcccuied = true;
    }

    public void Deactive()
    {
        currentSquareColor_ = Config.SquareColor.NotSet;
        activeImage.gameObject.SetActive(false);
    }

    public void ClearOccpied()
    {
        currentSquareColor_ = Config.SquareColor.NotSet;
        Selected = false;
        SquareOcccuied = false;
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(SquareOcccuied == false)
        {
            hoverImage.gameObject.SetActive(true);
            Selected = true;
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if (SquareOcccuied == false)
        {
            hoverImage.gameObject.SetActive(true);           
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }
    
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (SquareOcccuied == false)
        {
            hoverImage.gameObject.SetActive(false);
            Selected = false;
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();
        }
    }
   
}

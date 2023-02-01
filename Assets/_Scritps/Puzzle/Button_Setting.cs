using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Setting : MonoBehaviour
{
    public GameObject backGroundSetting;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Button_touch()
    {
        //GetComponent<Animation>().Play("Setting");
        ActiveObject();     
    }

    public void ActiveObject()
    {
        if (backGroundSetting.activeSelf != true)
        {
            backGroundSetting.SetActive(true);
        }
        else
        {
            backGroundSetting.SetActive(false);
        }
    }
}

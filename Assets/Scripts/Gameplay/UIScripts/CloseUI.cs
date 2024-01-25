using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    public GameObject myUI;
    public void OnClick()
    {
        myUI.SetActive(false);
    }
}

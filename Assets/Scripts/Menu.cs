using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public bool isMenu;
    public bool isHuongDan;
    public GameObject MenuBor;
    public GameObject huongdan;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            showMenu();
        }
    }

    public void showMenu()
    {
        isMenu = !isMenu;
        MenuBor.SetActive(isMenu);
    }

    public void _huongdan()
    {
        isHuongDan = !isHuongDan;
        huongdan.SetActive(isHuongDan);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryImage;
    bool isTrue;
    // Start is called before the first frame update
    void Start()
    {
        isTrue = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTrue = true;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isTrue = false; 
        }

        if(isTrue)
        {
            inventoryImage.SetActive(true);
        }
        else
        {
            inventoryImage.SetActive(false);
        }
    }
}

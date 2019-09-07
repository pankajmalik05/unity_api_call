using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
   
    private Data _data;
    public Text _title;

    public Data data
    {
        get => _data;
        set => _data = value;
    }

    // setting title for each item
    public void SetTitle(string title)
    {
        _title.text = title;
    }

   
}

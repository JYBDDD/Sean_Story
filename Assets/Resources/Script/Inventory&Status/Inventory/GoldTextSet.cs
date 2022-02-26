using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldTextSet : MonoBehaviour
{
    Text goldText;
    public static int GoldWareHouse;

    private void Start()
    {
        goldText = GetComponent<Text>();
    }

    private void Update()
    {
        goldText.text = $"{string.Format("{0:#,###}", GoldWareHouse)}";
    }
}

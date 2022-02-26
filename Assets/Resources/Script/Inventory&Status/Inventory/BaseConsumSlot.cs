using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class BaseConsumSlot : MonoBehaviour   // �Һ������ �����͸� �޾ƿ������� �����
{
    public static ConsumptionSlot[] consumSlot;
    public static bool ConsumBool = false;

    public void Awake()
    {
        ConsumptionSlot.consumList.Clear();   // ������ ����Ʈ �ʱ�ȭ �� ������ ����
        consumSlot = GetComponentsInChildren<ConsumptionSlot>();
    }
    public void Start()
    {
        if(ConsumBool == true)
        {
            GameManager.Json.ConsumLoadData("SaveData");
            ConsumBool = false;
        }
    }
}

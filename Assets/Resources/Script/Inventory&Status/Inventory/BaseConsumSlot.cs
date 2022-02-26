using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class BaseConsumSlot : MonoBehaviour   // 소비아이템 데이터를 받아오기위해 사용중
{
    public static ConsumptionSlot[] consumSlot;
    public static bool ConsumBool = false;

    public void Awake()
    {
        ConsumptionSlot.consumList.Clear();   // 시작전 리스트 초기화 및 아이템 설정
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

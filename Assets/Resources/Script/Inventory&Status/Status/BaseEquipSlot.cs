using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEquipSlot : MonoBehaviour
{
    public static EquipSlot[] equipSlot;
    public static bool EquipBool = false;

    public void Awake()
    {
        equipSlot = GetComponentsInChildren<EquipSlot>();
    }
    public void Start()
    {
        if(EquipBool == true)
        {
            GameManager.Json.EquipLoadData("SaveData");
            EquipBool = false;
        }
    }
}

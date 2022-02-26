using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class InventorySet : MonoBehaviour
{
    private Slot slot;

    private void Awake() // SettingButton에서 인벤토리 켜져있는걸 꺼주기전 호출
    {
        for (int i = 0; i < 24; i++)
        {
            slot = GameManager.Resource.Instantiate("UI/Inventory/Slot", gameObject.transform).GetComponentInChildren<Slot>();
            InventoryManager.slotList.Add(slot);
        }
    }
}

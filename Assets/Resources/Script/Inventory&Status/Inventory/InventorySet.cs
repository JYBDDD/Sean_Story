using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class InventorySet : MonoBehaviour
{
    private Slot slot;

    private void Awake() // SettingButton���� �κ��丮 �����ִ°� ���ֱ��� ȣ��
    {
        for (int i = 0; i < 24; i++)
        {
            slot = GameManager.Resource.Instantiate("UI/Inventory/Slot", gameObject.transform).GetComponentInChildren<Slot>();
            InventoryManager.slotList.Add(slot);
        }
    }
}

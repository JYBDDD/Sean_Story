using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public static List<Slot> slotList = new List<Slot>();

    public void InsertItemInformation(GameObject item)
    {
        ItemBaseInstance itemBase = item.GetComponent<ItemBaseInstance>();  // 해당 아이템의 GetComponent
        Item.ItemData itemData = new Item.ItemData();

        itemData.ItemName = itemBase.itemObject.ItemName;
        itemData.sprite = itemBase.itemObject.sprite;
        itemData.ItemTypes = itemBase.itemObject.ItemTypes;
        itemData.Amount = itemBase.itemObject.Amount;
        itemData.DropRate = itemBase.itemObject.DropRate;
        itemData.ItemExplanation = itemBase.itemObject.ItemExplanation;
        itemData.ItemUseStrength = itemBase.itemObject.ItemUseStrength;

        if(itemData.ItemTypes != Define.ItemType.Equipment) // 만약 장비가 아니고
        {
            foreach(Slot slot in slotList)
            {
                if(slot.Items == null)
                    return;

                if(slot.Items.ItemName == itemData.ItemName) // 슬롯안에 있는 아이템이름이 동일하다면 Amount를 늘려줌
                {
                    slot.Items.Amount += itemData.Amount;
                    return;
                }
            }
        }

        if(itemData.ItemTypes == Define.ItemType.Coin)  // 코인이라면 슬롯으로 넣어주지않고 골드 텍스트로 넘겨줌
        {
            GoldTextSet.GoldWareHouse += itemData.Amount;
            return;
        }

        InSlot().Items = itemData;
    }

    public void InsertItemInformation(Item.ItemData item)  // 스탯창 장비Item 이동용
    {
        Item.ItemData itemData = new Item.ItemData();

        itemData.ItemName = item.ItemName;
        itemData.sprite = item.sprite;
        itemData.ItemTypes = item.ItemTypes;
        itemData.Amount = item.Amount;
        itemData.DropRate = item.DropRate;
        itemData.ItemExplanation = item.ItemExplanation;
        itemData.ItemUseStrength = item.ItemUseStrength;

        InSlot().Items = itemData;
    }

    public Slot InSlot()
    {
        foreach(Slot slot in slotList)
        {
            if(slot.Items == null)
                return slot;

            if (slot.Items.sprite == null)  // 슬롯의 Sprite가 Null 이라면 그 슬롯에 Item의 데이터를 넣어준다
            {
                return slot;
            }
        }

        return null;
    }

}

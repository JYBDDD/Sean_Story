using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public static List<Slot> slotList = new List<Slot>();

    public void InsertItemInformation(GameObject item)
    {
        ItemBaseInstance itemBase = item.GetComponent<ItemBaseInstance>();  // �ش� �������� GetComponent
        Item.ItemData itemData = new Item.ItemData();

        itemData.ItemName = itemBase.itemObject.ItemName;
        itemData.sprite = itemBase.itemObject.sprite;
        itemData.ItemTypes = itemBase.itemObject.ItemTypes;
        itemData.Amount = itemBase.itemObject.Amount;
        itemData.DropRate = itemBase.itemObject.DropRate;
        itemData.ItemExplanation = itemBase.itemObject.ItemExplanation;
        itemData.ItemUseStrength = itemBase.itemObject.ItemUseStrength;

        if(itemData.ItemTypes != Define.ItemType.Equipment) // ���� ��� �ƴϰ�
        {
            foreach(Slot slot in slotList)
            {
                if(slot.Items == null)
                    return;

                if(slot.Items.ItemName == itemData.ItemName) // ���Ծȿ� �ִ� �������̸��� �����ϴٸ� Amount�� �÷���
                {
                    slot.Items.Amount += itemData.Amount;
                    return;
                }
            }
        }

        if(itemData.ItemTypes == Define.ItemType.Coin)  // �����̶�� �������� �־������ʰ� ��� �ؽ�Ʈ�� �Ѱ���
        {
            GoldTextSet.GoldWareHouse += itemData.Amount;
            return;
        }

        InSlot().Items = itemData;
    }

    public void InsertItemInformation(Item.ItemData item)  // ����â ���Item �̵���
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

            if (slot.Items.sprite == null)  // ������ Sprite�� Null �̶�� �� ���Կ� Item�� �����͸� �־��ش�
            {
                return slot;
            }
        }

        return null;
    }

}

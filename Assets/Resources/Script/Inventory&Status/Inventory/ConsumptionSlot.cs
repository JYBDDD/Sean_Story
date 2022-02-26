using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsumptionSlot : MonoBehaviour, IPointerClickHandler
{
    public bool UseBool = false;

    private Image icon;

    private Text amountText;

    public Item.ItemData itemData = new Item.ItemData();

    public Item.ItemData Items
    {
        get { return itemData; }
        set { itemData = value; }
    }

    // SaveData ��
    public static List<Item.ItemData> consumList = new List<Item.ItemData>();

    private void Start()
    {
        icon = GetComponent<Image>();
        amountText = GetComponentInChildren<Text>();

        for (int i = 0; i < 24; i++)  // �������� �ִ»��¿��� ��Ż�� ź�ٸ� �κ��丮�� �����۰� ���������ش�
        {
            if (Items.ItemName == InventoryDragAndDrop.slot[i].Items.ItemName)
            {
                Items = InventoryDragAndDrop.slot[i].Items;
            }
        }

    }

    private void Update()
    {
        if (icon.sprite == null)  // �̹����� ���ٸ� ���İ� 0���� ����
        {
            SetColor(0);
        }
        if (icon.sprite != null)  // �̹����� �ִٸ� ���İ� 1�� ����
        {
            SetColor(1);
        }

        if (Items.sprite != null && Items.Amount >= 0)
        {
            amountText.text = $"X{Items.Amount}";
            icon.sprite = itemData.sprite;
        }
        if (Items.sprite == null)
        {
            amountText.text = "";
            icon.sprite = null;
        }
    }

    private void SetColor(float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemUse();
    }

    public void ItemUse()
    {
        if (Items.ItemName.Contains("��������") && Items.Amount > 0)
        {
            GameManager.Player.PlayerController.Stat.Hp += Items.ItemUseStrength;
            GameManager.Sound.PlayEffectSound("Item/PotionHeal");
            Items.Amount -= 1;
            UseBool = true;
        }
        UseBool = false;
    }
}

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

    // SaveData 용
    public static List<Item.ItemData> consumList = new List<Item.ItemData>();

    private void Start()
    {
        icon = GetComponent<Image>();
        amountText = GetComponentInChildren<Text>();

        for (int i = 0; i < 24; i++)  // 아이템이 있는상태에서 포탈을 탄다면 인벤토리의 아이템과 연동시켜준다
        {
            if (Items.ItemName == InventoryDragAndDrop.slot[i].Items.ItemName)
            {
                Items = InventoryDragAndDrop.slot[i].Items;
            }
        }

    }

    private void Update()
    {
        if (icon.sprite == null)  // 이미지가 없다면 알파값 0으로 조정
        {
            SetColor(0);
        }
        if (icon.sprite != null)  // 이미지가 있다면 알파값 1로 조정
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
        if (Items.ItemName.Contains("빨강물약") && Items.Amount > 0)
        {
            GameManager.Player.PlayerController.Stat.Hp += Items.ItemUseStrength;
            GameManager.Sound.PlayEffectSound("Item/PotionHeal");
            Items.Amount -= 1;
            UseBool = true;
        }
        UseBool = false;
    }
}

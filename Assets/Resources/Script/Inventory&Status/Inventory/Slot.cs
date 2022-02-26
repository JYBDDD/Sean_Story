using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Image icon;

    private Text amountText;

    public Item.ItemData itemData = new Item.ItemData();
    
    public Item.ItemData Items
    {
        get { return itemData; }
        set { itemData = value; }
    }

    private void Start()
    {
        icon = GetComponent<Image>();
        amountText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if(icon.sprite == null)  // 이미지가 없다면 알파값 0으로 조정
        {
            SetColor(0);
        }
        if(icon.sprite != null)  // 이미지가 있다면 알파값 1로 조정
        {
            SetColor(1);
        }

        if (Items == null)
            icon.sprite = null;
        if (Items != null)
            icon.sprite = itemData.sprite;

        if (Items == null)
        {
            amountText.text = "";
            return;
        }
        
        if (itemData.ItemTypes == Define.ItemType.Equipment | itemData.Amount == 0) // 장비라면 텍스트를 쓰지않는다
            amountText.text = "";
        if(itemData.ItemTypes != Define.ItemType.Equipment)  // 장비가 아니라면 텍스트를 쓴다
            amountText.text = $"X{itemData.Amount}";
    }

    private void SetColor(float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }

}

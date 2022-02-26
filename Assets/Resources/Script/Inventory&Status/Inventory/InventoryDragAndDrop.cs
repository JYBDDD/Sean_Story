using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class InventoryDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    [SerializeField]
    private Image itemBlankContainer;
    private Text ContainerText;

    public static Slot[] slot;  // 슬롯다중정보

    private Slot StartDragSlot;  // 시작 드래그정보 임시저장
    private Slot EndDragSlot;    // 끝 드래그정보 임시저장
    private bool CheckStartBool = false;  // 빈칸을 드래그한다면 true

    // 아이템 툴팁 설정
    [Space]
    [SerializeField]
    private Image itemTooltipBackImg;
    [SerializeField]
    private Image itemTooltipImg;
    [SerializeField]
    private Text itemName;
    [SerializeField]
    private Text itemTooltipText;
    [Space]

    // 상태창 장비 설정
    [SerializeField]
    private GameObject Casque_G;
    [SerializeField]
    private GameObject Armor_G;
    [SerializeField]
    private GameObject Leggings_G;
    [SerializeField]
    private GameObject Weapon_G;
    [SerializeField]
    private GameObject Gloves_G;
    [SerializeField]
    private GameObject Boots_G;

    //saveData용
    public static List<Slot> InvenList = new List<Slot>();
    public static bool InvenLoadBool = false;

    public void Awake()
    {
        slot = GetComponentsInChildren<Slot>();   // InventorySet에 [DefaultExecutionOrder(-2)] 를 주어 먼저 슬롯들을 생성시켜주고 그다음 슬롯값을 찾도록 설정함
    }

    public void OnEnable()  
    {
        itemTooltipBackImg.gameObject.SetActive(false);
        ContainerText = itemBlankContainer.GetComponentInChildren<Text>();
    }

    public void Start()
    {
        if (slot != null)  // slot 없을시 오류 방지
        {
            if (InvenLoadBool == true)
            {
                ItemListClear();
                GameManager.Json.InvenLoadData("SaveData");
                InvenLoadBool = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CheckStartBool = false;
        var result = eventData.pointerCurrentRaycast;

        if(result.gameObject.GetComponentInChildren<Slot>().Items.sprite == null | result.gameObject.GetComponentInChildren<Slot>() == null)
        {
            CheckStartBool = true;
            itemBlankContainer.gameObject.SetActive(false);
            itemBlankContainer.GetComponent<Slot>().Items = null;
            return;
        }

        for (int i = 0; i < 24; i++)
        {
            if (result.gameObject.GetComponentInChildren<Slot>().Items == InventoryManager.slotList[i].Items)
            {
                itemBlankContainer.gameObject.SetActive(true);
                StartDragSlot = slot[i];
                itemBlankContainer.GetComponent<Slot>().Items = StartDragSlot.Items;
                itemBlankContainer.sprite = StartDragSlot.Items.sprite;

                if (StartDragSlot.Items.ItemTypes == Define.ItemType.Equipment)
                    ContainerText.text = "";
                if (StartDragSlot.Items.ItemTypes != Define.ItemType.Equipment)
                    ContainerText.text = $"X{StartDragSlot.Items.Amount}";
            }
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        itemBlankContainer.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (StartDragSlot == null | CheckStartBool == true) 
        {
            itemBlankContainer.gameObject.SetActive(false);
            itemBlankContainer.GetComponent<Slot>().Items = null;
            return;
        }

        var result = eventData.pointerCurrentRaycast;
        if (result.gameObject == null)  // 놓을곳이 없다면 리턴
        {
            itemBlankContainer.gameObject.SetActive(false);
            itemBlankContainer.GetComponent<Slot>().Items = null;
            return;
        }


        var consumptionSlot = result.gameObject.GetComponent<ConsumptionSlot>();
        if (consumptionSlot != null)   // 만약 놓는곳이 소비슬롯이라면      (소비 슬롯 설정하는 구문)
        {
            Item.ItemData consumItem = new Item.ItemData();
            consumItem = StartDragSlot.Items;  // 위에서 StartDragSlot 은 Slot[i]를 받고 있기 때문에 Items 값이 연결되어 소비슬롯에서 아이템 사용시 인벤토리 안에있는 아이템도 같이 소모 된다

            if (consumItem.ItemTypes == Define.ItemType.Consumption)
            {
                if (consumptionSlot.Items != consumItem && consumptionSlot.Items.sprite != null)
                {
                    if (ConsumptionSlot.consumList[0].ItemName != consumItem.ItemName)  // 1번째 슬롯리스트
                        ConsumptionSlot.consumList.RemoveAt(0);

                    if(ConsumptionSlot.consumList.Count > 1)
                        if (ConsumptionSlot.consumList[1].ItemName != consumItem.ItemName)  // 2번째 슬롯리스트
                            ConsumptionSlot.consumList.RemoveAt(1);
                }
                    
                consumptionSlot.Items = consumItem;
                ConsumptionSlot.consumList.Add(consumItem);
                if (ConsumptionSlot.consumList.Count > 2)  // 갯수가 3개가 될때 마지막 리스트 삭제
                    ConsumptionSlot.consumList.RemoveAt(2);
                
            }

            itemBlankContainer.gameObject.SetActive(false);
            itemBlankContainer.GetComponent<Slot>().Items = null;
            return;

        }


        EndDragSlot = result.gameObject.GetComponentInChildren<Slot>();  /// 1.  비어있거나 들어있는 Slot값을 가져온다
        Item.ItemData item = EndDragSlot.Items;  // 아이템값 임시 저장

        for (int i = 0; i < 24; i++)
        {
            if (StartDragSlot == slot[i])  // 2 ~ 3  처음 시작시 드래그한 값이라면 처음 시작 드래그슬롯에 놓을때 슬롯데이터를 가져워 넣는다
            {
                slot[i].Items = item;
            }
            if (EndDragSlot == slot[i])  // 2 ~ 3  놓을때 슬롯데이터라면 그 데이터에 시작시드래그한 데이터값을 넣는다
            {
                slot[i].Items = itemBlankContainer.GetComponent<Slot>().Items; 
            }

            itemBlankContainer.gameObject.SetActive(false);
        }
        itemBlankContainer.GetComponent<Slot>().Items = null;
        
    }

    public void OnPointerEnter(PointerEventData eventData) // 아이템 툴팁
    {
        var result = eventData.pointerCurrentRaycast;
        if (result.gameObject.GetComponentInChildren<Slot>().Items == null)  // 아이템 데이터가 Null이라면 리턴
        {
            itemTooltipBackImg.gameObject.SetActive(false);
            return;
        }

        if(result.gameObject.GetComponentInChildren<Slot>().Items.sprite == null)  // 아이템 이미지가 Null 이라면 리턴  (오류방지를 위해 따로 리턴시켜줌)
        {
            itemTooltipBackImg.gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < 24; i++)
        {
            if(slot[i].Items != null)
            {
                if (result.gameObject.GetComponentInChildren<Slot>().Items.sprite == slot[i].Items.sprite)
                {
                    itemTooltipBackImg.gameObject.SetActive(true);
                    itemTooltipImg.sprite = slot[i].Items.sprite;
                    itemName.text = $"{slot[i].Items.ItemName}";
                    itemTooltipText.text = $"{slot[i].Items.ItemExplanation}";
                }
            }
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemTooltipBackImg.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)  // 더블클릭시 해당 아이템 사용 (ETC가 아닐경우)
    {
        float currentTimeClick = 0;  // 클릭 이벤트
        currentTimeClick += Time.deltaTime;

        int ClickCount = eventData.clickCount;

        if (currentTimeClick <= 0.75f && ClickCount >= 2)  // 0.75초안에 한번더누른다면
        {
            var result = eventData.pointerCurrentRaycast;
            Item.ItemData Tempitem = new Item.ItemData();

            #region 아이템 사용 구현
            if (result.gameObject.GetComponentInChildren<Slot>().Items != null)
            {
                for (int i = 0; i < 24; i++)
                {
                    if(result.gameObject.GetComponentInChildren<Slot>().Items == slot[i].Items && result.gameObject.GetComponentInChildren<Slot>().Items.sprite != null)
                    {
                        if (slot[i].Items.ItemTypes == Define.ItemType.Equipment) // 아이템타입이 장비라면
                        {
                            #region 장비장착 (검)
                            if (slot[i].Items.ItemName.Contains(" 검 "))
                            {
                                if (Weapon_G.GetComponent<EquipSlot>().Items.sprite != null)  // 장비창에 장비가 장착중일시 장비를 서로스왑    (장비아이템들은 서로 코드가 동일함)
                                {
                                    Tempitem = Weapon_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Weapon_G.GetComponent<EquipSlot>().Items);

                                    Weapon_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.damage -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.damage += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Weapon_G.GetComponent<EquipSlot>().Items.sprite == null)  // 장비창에 장비가 장착되어있지 않다면 즉시 장착    (장비아이템들은 서로 코드가 동일함)
                                {
                                    Weapon_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.damage += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                            }
                            #endregion
                            #region 장비장착 (각반)
                            if (slot[i].Items.ItemName.Contains("각반"))
                            {
                                if (Leggings_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Leggings_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Leggings_G.GetComponent<EquipSlot>().Items);

                                    Leggings_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Leggings_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Leggings_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                            }
                            #endregion
                            #region 장비장착 (갑옷)
                            if (slot[i].Items.ItemName.Contains("갑옷"))
                            {
                                if (Armor_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Armor_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Armor_G.GetComponent<EquipSlot>().Items);

                                    Armor_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Armor_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Armor_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                } 
                            }
                            #endregion
                            #region 장비장착 (신발)
                            if (slot[i].Items.ItemName.Contains("신발"))
                            {
                                if (Boots_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Boots_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Boots_G.GetComponent<EquipSlot>().Items);

                                    Boots_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Boots_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Boots_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                            }
                            #endregion
                            #region 장비장착 (투구)
                            if (slot[i].Items.ItemName.Contains("투구"))
                            {
                                if (Casque_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Casque_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Casque_G.GetComponent<EquipSlot>().Items);

                                    Casque_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Casque_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Casque_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }  
                            }
                            #endregion
                            #region 장비장착 (장갑)
                            if (slot[i].Items.ItemName.Contains("장갑"))
                            {
                                if (Gloves_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Gloves_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Gloves_G.GetComponent<EquipSlot>().Items);

                                    Gloves_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }

                                if (Gloves_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Gloves_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                            }
                            #endregion

                        }
                        if (slot[i].Items.ItemTypes == Define.ItemType.Consumption) // 아이템타입이 소비라면
                        {
                            #region 소비아이템 (빨강물약) / Hp 회복
                            if (slot[i].Items.ItemName.Contains("빨강물약"))
                            {
                                GameManager.Sound.PlayEffectSound("Item/PotionHeal");
                                GameManager.Player.PlayerController.Stat.Hp += slot[i].Items.ItemUseStrength;
                                slot[i].Items.Amount -= 1;
                            }
                            #endregion
                        }
                        if (slot[i].Items.Amount <= 0)  // 갯수가 0이라면 Items데이터 Null로 변경
                        {
                            slot[i].Items = null;
                            itemTooltipBackImg.gameObject.SetActive(false);
                            return;
                        }
                    }
                    
                }
            }
            #endregion

        }
    }

    public static void ItemSave()
    {
        for (int i = 0; i < 24; i++)   
            InvenList.Add(slot[i]);
    }

    public static void ItemListClear()  // 리스트가 중첨되지 않도록 클리어
    {
        InvenList.Clear();
    }
   
}


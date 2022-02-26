using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [System.Serializable]
    public class ItemData
    {
        public string ItemName;  // 아이템 이름
        public Sprite sprite;    // 아이템 Sprite
        public int Amount;       // 아이템 갯수 
        public int DropRate;    // 드랍확률
        public Define.ItemType ItemTypes;  // 아이템타입 -> 장비,소비,기타,코인
        public string ItemExplanation;  // 아이템 설명
        public int ItemUseStrength;   // 아이템 사용 효과
    }
}

public class ItemBaseInstance : MonoBehaviour
{
    public ScriptableObjectC itemObject;
    private Rigidbody _rigid;

    private void OnEnable()
    {
        _rigid = GetComponent<Rigidbody>();
        _rigid.AddExplosionForce(50.0f, transform.position, 3.0f, 50.0f);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Inven.InsertItemInformation(gameObject);
            GameManager.Pool.InsertPool(gameObject);
            GameManager.Sound.PlayEffectSound("Player/Pickup");
        }
    }
}

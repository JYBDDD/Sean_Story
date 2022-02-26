using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [System.Serializable]
    public class ItemData
    {
        public string ItemName;  // ������ �̸�
        public Sprite sprite;    // ������ Sprite
        public int Amount;       // ������ ���� 
        public int DropRate;    // ���Ȯ��
        public Define.ItemType ItemTypes;  // ������Ÿ�� -> ���,�Һ�,��Ÿ,����
        public string ItemExplanation;  // ������ ����
        public int ItemUseStrength;   // ������ ��� ȿ��
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

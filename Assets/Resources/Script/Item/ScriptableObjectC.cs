using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class ScriptableObjectC : ScriptableObject
{
    public string ItemName;  // ������ �̸�
    public Sprite sprite;    // ������ Sprite
    public int Amount;       // ������ ���� 
    public int DropRate;    // ���Ȯ��
    public Define.ItemType ItemTypes;  // ������Ÿ�� -> ���,�Һ�,��Ÿ,����
    public string ItemExplanation;  // ������ ����
    public int ItemUseStrength;      // ������ ��� ȿ��

    // �߰� ������ ȿ���� �� �����ۿ� ���� �޾��ٰ���
}

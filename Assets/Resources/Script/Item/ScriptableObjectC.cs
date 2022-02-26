using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class ScriptableObjectC : ScriptableObject
{
    public string ItemName;  // 아이템 이름
    public Sprite sprite;    // 아이템 Sprite
    public int Amount;       // 아이템 갯수 
    public int DropRate;    // 드랍확률
    public Define.ItemType ItemTypes;  // 아이템타입 -> 장비,소비,기타,코인
    public string ItemExplanation;  // 아이템 설명
    public int ItemUseStrength;      // 아이템 사용 효과

    // 추가 아이템 효과는 각 아이템에 직접 달아줄것임
}

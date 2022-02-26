using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    [SerializeField]
    private Button attackButton;

    public static int pushAttackCount = 0;

    void Awake()
    {
        attackButton = GetComponent<Button>();
    }

    public void OnPushAttack()
    {
        if(attackButton && GameManager.Player.PlayerController.Stat.Hp > 0)
        {
            pushAttackCount++;
        }
    }

    // 스킬이 개인으로 이미지와 값을 가지고 있도록 설정해주고 빈슬롯에 넣어주는 방향으로 해줄것 TODO
}

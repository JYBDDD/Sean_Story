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

    // ��ų�� �������� �̹����� ���� ������ �ֵ��� �������ְ� �󽽷Կ� �־��ִ� �������� ���ٰ� TODO
}

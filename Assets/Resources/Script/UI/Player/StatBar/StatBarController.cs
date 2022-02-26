using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarController : MonoBehaviour
{
    [SerializeField]
    private Image SetHpBar;
    [SerializeField]
    private Text SetHpText;
    [SerializeField]
    private Text SetLevelText;

    [SerializeField]
    private Image SetExpBar;

    float originHp;
    float Hp;
    float originMaxHp;
    float MaxHp;

    float originExp;
    float Exp;
    int originLev;
    int Lev;

    private void Update()
    {
        originHp = GameManager.Player.PlayerController.Stat.Hp;
        originMaxHp = GameManager.Player.PlayerController.Stat.maxHp;
        if (originHp != Hp | originMaxHp != MaxHp)  // 체력이 깎이면 실행
            PlayerHpBar();

        originExp = GameManager.Player.PlayerController.Stat.Exp;
        originLev = GameManager.Player.PlayerController.Stat.Level;
        if (originExp != Exp | originLev != Lev)  // 경험치가 증가하면 실행
            PlayerExpBar();

    }

    private void PlayerHpBar()
    {
        Hp = GameManager.Player.PlayerController.Stat.Hp;
        float MaxHp = GameManager.Player.PlayerController.Stat.maxHp;
        SetHpBar.fillAmount = Hp / MaxHp;
        SetHpText.text = $"{GameManager.Player.PlayerController.Stat.Hp}";
    }

    private void PlayerExpBar()  // 레벨텍스트도 같이 넣어놓음
    {
        Exp = GameManager.Player.PlayerController.Stat.Exp;
        float MaxExp = GameManager.Player.PlayerController.Stat.maxExp;
        SetExpBar.fillAmount = Exp / MaxExp;

        Lev = GameManager.Player.PlayerController.Stat.Level;
        SetLevelText.text = $"Lv.{GameManager.Player.PlayerController.Stat.Level}";
    }
}

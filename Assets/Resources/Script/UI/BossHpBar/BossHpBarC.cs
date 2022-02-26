using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarC : MonoBehaviour
{

    Image hpBarImg;

    int currentHpInt = 0;
    int originHpInt = 0;

    private void Start()
    {
        hpBarImg = GetComponent<Image>();

        currentHpInt = DragonController.dragonController.Stat.Hp;
        originHpInt = DragonController.dragonController.Stat.Hp;
    }

    private void Update()
    {
        currentHpInt = DragonController.dragonController.Stat.Hp;

        if (currentHpInt == 0)
            gameObject.SetActive(false);

        if (currentHpInt != originHpInt)
            HpSet();
    }

    private void HpSet()
    {
        originHpInt = DragonController.dragonController.Stat.Hp;
        float hp = DragonController.dragonController.Stat.Hp;
        float maxHp = DragonController.dragonController.Stat.maxHp;
        hpBarImg.fillAmount = hp / maxHp;
    }
}

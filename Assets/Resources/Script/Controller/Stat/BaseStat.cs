using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    public int level;
    public int hp;
    public int maxHp;
    public int damage;
    public int critical_Chance;

    public virtual int Level
    {
        get => level;
        set => level = value;
    }

    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            hp = Mathf.Min(maxHp, hp);
            hp = Mathf.Clamp(hp, 0, maxHp);
        }
    }

    public int Critical_Chance  // ũ��Ƽ�� ������ ����
    {
        get => critical_Chance;
        set
        {
            critical_Chance = value;
            critical_Chance = Mathf.Clamp(critical_Chance, 10, 100);
        }
    }

    public static int CriticalRange;  // ĳ���� ġ��ŸȮ�� ������
    public static int MCriticalRange; // ���� ġ��ŸȮ�� ������
    public static int MDamageRange;  // ���� ������ ������
    public static int DamageRange;   // �÷��̾� ������ ������

    public void Attack_Damage(BaseStat target,GameObject target1)  // target1 �� �´� Ÿ���� �������� ����
    {
        float target1Random = UnityEngine.Random.Range(-0.5f, 0.5f);  // Ÿ������Ʈ ������ ����
        Vector3 target1RandomPositon = new Vector3(target1.transform.position.x + target1Random, target1.transform.position.y + 1.5f, target1.transform.position.z + target1Random);  // Ÿ������Ʈ ������

        if (target1.GetComponent<CharacterBaseController>() != null)  // �÷��̾ �ƴ϶��  (�´� Ÿ���� �÷��̾���)
        {
            if (MCriticalRange > critical_Chance)  // �Ϲ� ����
            {
                target.Hp -= MDamageRange;
                GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/MonsterAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
                GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/MonsterDamageEffect", target1.transform.position + (Vector3.up * 3), Quaternion.identity, GameManager.staticHitDamageParent.transform);
                return;
            }
            if (MCriticalRange <= critical_Chance)  // ġ��Ÿ ����
            {
                target.Hp -= MDamageRange * 2;
                GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/MonsterCriticalAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
                GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/MonsterCriticalDamageEffect", target1.transform.position + (Vector3.up * 3), Quaternion.identity, GameManager.staticHitDamageParent.transform);
                return;
            }
        }

        if (target1.GetComponent<CharacterBaseController>() == null)  // �÷��̾���    (�´� Ÿ���� ���Ͷ��)
        {
            if (CriticalRange > critical_Chance)  // �Ϲ� ����
            {
                target.Hp -= DamageRange;
                GameManager.Sound.PlayEffectSound("HitSound");
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/UIEffect/PlayerAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerDamageEffect", target1.transform.position + (Vector3.up * 3), Quaternion.identity, GameManager.staticHitDamageParent.transform);
                return;
            }
            if (CriticalRange <= critical_Chance)  // ġ��Ÿ ����
            {
                target.Hp -= DamageRange * 2;
                GameManager.Sound.PlayEffectSound("CriticalHitSound");
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/UIEffect/PlayerCriticalAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerCriticalDamageEffect", target1.transform.position + (Vector3.up * 3), Quaternion.identity, GameManager.staticHitDamageParent.transform);
                return;
            }
        }

    }
}


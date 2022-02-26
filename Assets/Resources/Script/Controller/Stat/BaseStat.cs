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

    public int Critical_Chance  // 크리티컬 일정값 고정
    {
        get => critical_Chance;
        set
        {
            critical_Chance = value;
            critical_Chance = Mathf.Clamp(critical_Chance, 10, 100);
        }
    }

    public static int CriticalRange;  // 캐릭터 치명타확률 랜덤값
    public static int MCriticalRange; // 몬스터 치명타확률 랜덤값
    public static int MDamageRange;  // 몬스터 데미지 랜덤값
    public static int DamageRange;   // 플레이어 데미지 랜덤값

    public void Attack_Damage(BaseStat target,GameObject target1)  // target1 은 맞는 타겟을 가져오고 있음
    {
        float target1Random = UnityEngine.Random.Range(-0.5f, 0.5f);  // 타격이펙트 랜덤값 설정
        Vector3 target1RandomPositon = new Vector3(target1.transform.position.x + target1Random, target1.transform.position.y + 1.5f, target1.transform.position.z + target1Random);  // 타격이펙트 랜덤값

        if (target1.GetComponent<CharacterBaseController>() != null)  // 플레이어가 아니라면  (맞는 타겟이 플레이어라면)
        {
            if (MCriticalRange > critical_Chance)  // 일반 공격
            {
                target.Hp -= MDamageRange;
                GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/MonsterAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
                GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/MonsterDamageEffect", target1.transform.position + (Vector3.up * 3), Quaternion.identity, GameManager.staticHitDamageParent.transform);
                return;
            }
            if (MCriticalRange <= critical_Chance)  // 치명타 공격
            {
                target.Hp -= MDamageRange * 2;
                GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/MonsterCriticalAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
                GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/MonsterCriticalDamageEffect", target1.transform.position + (Vector3.up * 3), Quaternion.identity, GameManager.staticHitDamageParent.transform);
                return;
            }
        }

        if (target1.GetComponent<CharacterBaseController>() == null)  // 플레이어라면    (맞는 타겟이 몬스터라면)
        {
            if (CriticalRange > critical_Chance)  // 일반 공격
            {
                target.Hp -= DamageRange;
                GameManager.Sound.PlayEffectSound("HitSound");
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/UIEffect/PlayerAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerDamageEffect", target1.transform.position + (Vector3.up * 3), Quaternion.identity, GameManager.staticHitDamageParent.transform);
                return;
            }
            if (CriticalRange <= critical_Chance)  // 치명타 공격
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


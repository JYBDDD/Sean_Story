using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DragonController : MonsterBaseController
{
    bool isDead = false;             // 죽었나 살았나 체크
    CapsuleCollider capsuleCollider;

    public static DragonController dragonController;  

    public MonsterStat Stat { get => _stat; }

    private void OnEnable()
    {
        _stat = JsonConvert.DeserializeObject<StatData>(GameManager.Resource.Load<TextAsset>("Data/Monster/DragonStatData").text).Mstat;

        isDead = false;
        dragonController = GetComponent<DragonController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = true;
        state = Define.State.Idle;
    }

    private void LateUpdate()
    {
        if (Stat.Hp <= 0 && isDead == false)
        {
            for (int i = 10001; i < QuestNPCBase.QuestDict.Count + 10001; i++)
            {
                if (QuestNPCBase.QuestDict[i].monsterName == gameObject.name)
                    QuestNPCController.QuestProgress++;
            }

            GameManager.Player.PlayerController.Stat.Exp += _stat.dropExp;
            Invoke("DeadCall", 5f);
            _anim.Play("Die");
            isDead = true;
            capsuleCollider.enabled = false;
            GameManager.Resource.SpawnItem(gameObject, Stat.level);
            state = Define.State.Die;
            return;
        }
    }

    protected override void StateIdle()
    {
        if ((GameManager.Player.player.transform.position - transform.position).magnitude < attackRange) // 공격 사거리보다 가깝다면
        {
            state = Define.State.Attack;
            return;
        }

        if ((GameManager.Player.player.transform.position - transform.position).magnitude < recognition_Distance) // 인식거리보다 가깝다면 Run 상태
        {
            _agent.SetDestination(GameManager.Player.player.transform.position);
            state = Define.State.Run;
            _anim.SetBool("IsRand", true);
            _anim.SetBool("IsRun", true);
            return;
        }

        _anim.SetBool("IsRand", false);
        _anim.SetBool("IsRun", false);
        _agent.SetDestination(_destVec);
    }

    protected override void StateRun()
    {
        if ((GameManager.Player.player.transform.position - transform.position).magnitude > recognition_Distance) // 인식거리보다 멀다면 Idle
        {
            _agent.SetDestination(GameManager.Player.player.transform.position);
            state = Define.State.Idle;
            return;
        }

        if ((GameManager.Player.player.transform.position - transform.position).magnitude < attackRange) // 공격 사거리보다 가깝다면
        {
            state = Define.State.Attack;
            return;
        }

        _anim.SetInteger("IsAttack", 0);
        _anim.SetBool("IsRun", true);
        _agent.SetDestination(GameManager.Player.player.transform.position);
    }

    protected override void StateAttack()
    {
        base.StateAttack();
        if ((GameManager.Player.player.transform.position - transform.position).magnitude > attackRange) // 공격 사거리보다 멀다면
        {
            state = Define.State.Run;
            _anim.SetInteger("IsAttack",0);
            return;
        }

        _anim.SetInteger("IsAttack", Random.Range(1,3));
        transform.LookAt(GameManager.Player.player.transform.position);
        _agent.SetDestination(transform.position);
    }

    protected override void StateHit()
    {
        if (_anim.GetInteger("IsAttack") == 1 && _anim.GetInteger("IsAttack") == 2)
            return;
        _anim.Play("GetHit");

        if (_anim.GetBool("IsRun"))
            state = Define.State.Run;
        if (_anim.GetBool("IsAttack"))
            state = Define.State.Attack;
    }

    protected override void StateDie()
    {

    }

    private void DeadCall()  // 죽을시 DeadEffect 효과와 어우러지게 넣어줌
    {
        GameManager.Resource.Destroy(gameObject);
        GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/DeadEffect_Moster", transform.position, Quaternion.identity, GameManager.staticParent.transform);
    }


    private void AttackAnimEvent()  // 공격 애니메이션에 넣어줌
    {
        GameManager.Sound.PlayEffectSound("Monster/Dragon/DragonAttack");

        BaseStat.MCriticalRange = UnityEngine.Random.Range(0, 100);
        BaseStat.MDamageRange = UnityEngine.Random.Range(Stat.damage / 2, Stat.damage);

        Collider[] targetArr = Physics.OverlapSphere(transform.position, 3.5f, LayerMask.GetMask("Player"));

        for (int i = 0; i < targetArr.Length; i++)
        {
            Stat.Attack_Damage(targetArr[i].gameObject.GetComponent<CharacterBaseController>().Stat, targetArr[i].gameObject);
            GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/Dragon/GragonAttackEffect", targetArr[i].transform.position + (Vector3.up), Quaternion.identity, GameManager.staticParent.transform);
            GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/Dragon/DragonBite", targetArr[i].transform.position + (Vector3.up * 2), Quaternion.identity, GameManager.staticParent.transform);
            targetArr[i].GetComponent<CharacterBaseController>().state = Define.State.Hit;  // 맞은캐릭터 Hit상태값으로 변경
        }
    }

    public void AnimScreem()
    {
        GameManager.Sound.PlayEffectSound("Monster/Dragon/DragonHowling");
    }
}

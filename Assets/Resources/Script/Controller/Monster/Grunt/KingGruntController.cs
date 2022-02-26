using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class KingGruntController : MonsterBaseController
{
    bool isDead = false;             // �׾��� ��ҳ� üũ
    CapsuleCollider capsuleCollider;

    public MonsterStat Stat { get => _stat; }

    private void OnEnable()
    {
        _stat = JsonConvert.DeserializeObject<StatData>(GameManager.Resource.Load<TextAsset>("Data/Monster/KingGruntStatData").text).Mstat;

        isDead = false;
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
            GameManager.Sound.PlayEffectSound("Monster/Grunt/GruntDeadSound");
            Invoke("DeadCall", 1.5f);
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
        if ((GameManager.Player.player.transform.position - transform.position).magnitude < attackRange) // ���� ��Ÿ����� �����ٸ�
        {
            state = Define.State.Attack;
            return;
        }

        if ((GameManager.Player.player.transform.position - transform.position).magnitude < recognition_Distance) // �νİŸ����� �����ٸ� Run ����
        {
            _agent.SetDestination(GameManager.Player.player.transform.position);
            state = Define.State.Run;
            _anim.SetBool("IsRun", true);
            return;
        }

        _anim.SetBool("IsRun", false);
        _agent.SetDestination(_destVec);
    }

    protected override void StateRun()
    {
        if ((GameManager.Player.player.transform.position - transform.position).magnitude > recognition_Distance) // �νİŸ����� �ִٸ� Idle
        {
            _agent.SetDestination(GameManager.Player.player.transform.position);
            state = Define.State.Idle;
            return;
        }

        if ((GameManager.Player.player.transform.position - transform.position).magnitude < attackRange) // ���� ��Ÿ����� �����ٸ�
        {
            state = Define.State.Attack;
            return;
        }

        _anim.SetBool("IsAttack", false);
        _anim.SetBool("IsRun", true);
        _agent.SetDestination(GameManager.Player.player.transform.position);
    }

    protected override void StateAttack()
    {
        base.StateAttack();
        if ((GameManager.Player.player.transform.position - transform.position).magnitude > attackRange) // ���� ��Ÿ����� �ִٸ�
        {
            state = Define.State.Run;
            _anim.SetBool("IsAttack", false);
            return;
        }

        _anim.SetBool("IsAttack", true);
        transform.LookAt(GameManager.Player.player.transform.position);
        _agent.SetDestination(transform.position);
    }

    protected override void StateHit()
    {
        _anim.Play("GetHit");

        if (_anim.GetBool("IsRun"))
            state = Define.State.Run;
        if (_anim.GetBool("IsAttack"))
            state = Define.State.Attack;
    }

    protected override void StateDie()
    {

    }

    private void DeadCall()  // ������ DeadEffect ȿ���� ��췯���� �־���
    {
        GameManager.Resource.Destroy(gameObject);
        GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/DeadEffect_Moster", transform.position, Quaternion.identity, GameManager.staticParent.transform);
    }


    private void AttackAnimEvent()  // ���� �ִϸ��̼ǿ� �־���
    {
        BaseStat.MCriticalRange = UnityEngine.Random.Range(0, 100);
        BaseStat.MDamageRange = UnityEngine.Random.Range(Stat.damage / 2, Stat.damage);

        Collider[] targetArr = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("Player"));
        GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/KingGrunt/KingGruntAttackEffect", transform.position + (transform.forward * 3) + (Vector3.up * 2), Quaternion.identity, GameManager.staticParent.transform);

        for (int i = 0; i < targetArr.Length; i++)
        {
            Stat.Attack_Damage(targetArr[i].gameObject.GetComponent<CharacterBaseController>().Stat, targetArr[i].gameObject);

            targetArr[i].GetComponent<CharacterBaseController>().state = Define.State.Hit;  // ����ĳ���� Hit���°����� ����
        }
    }

    public void AnimSound1()
    {
        GameManager.Sound.PlayEffectSound("Monster/Grunt/GruntAttack1");
    }

    public void AnimSound2()
    {
        GameManager.Sound.PlayEffectSound("Monster/Grunt/GruntAttack2");
    }
}

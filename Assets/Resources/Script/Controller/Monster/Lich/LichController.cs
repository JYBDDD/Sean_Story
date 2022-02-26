using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LichController : MonsterBaseController
{
    [SerializeField]
    private GameObject attackPoint;  // Lich ������ ������
    float _time = 0;                 // State ���� �ð� üũ��
    bool isDead = false;             // �׾��� ��ҳ� üũ
    CapsuleCollider capsuleCollider;

    public static LichController staticLich;  // IceMagic���� ���� �ҷ����°Ϳ� �����

    public MonsterStat Stat { get => _stat; }

    private void OnEnable()
    {
        _stat = JsonConvert.DeserializeObject<StatData>(GameManager.Resource.Load<TextAsset>("Data/Monster/LichStatData").text).Mstat;

        isDead = false;
        staticLich = GetComponent<LichController>();
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
            GameManager.Sound.PlayEffectSound("Monster/Lich/LichDeadSound");
            Invoke("DeadCall", 6.4f);
            _anim.Play("Die");
            isDead = true;
            capsuleCollider.enabled = false;
            GameManager.Resource.SpawnItem(gameObject, Stat.level);
            GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/Lich/MonsterEffect/LichDeadEffect", transform.position + Vector3.up, Quaternion.identity, GameManager.staticParent.transform);
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
            return;
        }

        if ((_destVec - transform.position).magnitude < 1f)
        {
            _time += Time.deltaTime;
            _anim.SetBool("IsRun", false);

            if (_time >= 4)
            {
                StopAllCoroutines();
                StartCoroutine(nameof(LookAroundMoving));

                _time = 0;
            }
            _anim.SetBool("IsRun", false);
            return;
        }

        _anim.SetBool("IsRun", true);
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
    }

    private void AttackArua()  // ���� �ִϸ��̼� �պκп� �־���
    {
        GameManager.Sound.PlayEffectSound("Monster/Lich/LichStartMagicSound");
        GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/Lich/MonsterEffect/LichAttackAura", transform.position,
            Quaternion.Euler(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z), GameManager.staticParent.transform);
    }

    private void AttackAnimEvent()  // ���� �ִϸ��̼ǿ� �־���
    {
        GameManager.Sound.PlayEffectSound("Monster/Lich/Magic_Glow_01");
        GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/Lich/MonsterEffect/IceMagic_Lich(Monster)", attackPoint.transform.position,
            Quaternion.LookRotation(attackPoint.transform.forward),GameManager.staticParent.transform);
    }

    

    
}
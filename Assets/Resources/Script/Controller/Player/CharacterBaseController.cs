using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class CharacterBaseController : MonoBehaviour
{
    public Define.State state = Define.State.Idle;

    [SerializeField]
    public GameObject sword;   // -> ���� ��ų ���� Į ������ �κп� ��ų�� �־��ٶ� ����Ұ� TODO

    [SerializeField]
    public GameObject Trajectory; // ���� ����

    private Animator _anim;
    bool isDead = false;  // �׾��� ��ҳ� üũ

    #region ���Ⱥҷ�����
    public PlayerStat Stat { get; set; } = new PlayerStat();
    protected int pL = 0; // �÷��̾� ����

    //PlayerStatData player_stat = new PlayerStatData();
    #endregion

    #region ��ų�ҷ����� and ��ų ����
    public static int SkillPoint = 1;  // ��ų����Ʈ
    public PlayerSkill Skill { get; set; } = new PlayerSkill();
    //PlayerSkillData player_skill = new PlayerSkillData();


    // ��ų : ����Ʈ�� ��þƮ ���� ����
    bool IsEnchant = false;
    public static float EnchantactiveTime = 0;  // ��ų ���� �ð�
    public static bool IsEnChantUse = false;  // ��æƮ ��� static ����  (BlankSkillButtonSet ���� �����)
    public static float CheckEnchantTime = 0.0f;  // ��Ÿ�� üũ

    // ��ų : ������ ���� ����
    bool IsSlash = false;
    public static float SlashactiveTime = 0;
    public static bool IsSlashUse = false; // ������ ��� static ���� (BlankSkillButtonSet ���� �����)
    public static float CheckSlachTime = 0.0f;  // ��Ÿ�� üũ
    #endregion

    private void OnEnable()
    {
        #region �÷��̾� ����
        /*for (int i = 1; i < player_stat.playerStat.Length + 1; i++)
        {
            player_stat.playerStat[i - 1] = new PlayerStat();
            player_stat.playerStat[i - 1].level = i;
            player_stat.playerStat[i - 1].hp = ((i * 100) + 900);
            player_stat.playerStat[i - 1].maxHp = ((i * 100) + 900);
            player_stat.playerStat[i - 1].damage = ((i * 10) + 90);
            player_stat.playerStat[i - 1].exp = 0;
            player_stat.playerStat[i - 1].maxExp = (i * 1000);
            player_stat.playerStat[i - 1].critical_Chance = 10;  // ġ��Ÿ Ȯ��
        }

        JsonConvert.SerializeObject(player_stat);
        GameManager.Json.ToJson(player_stat, "PlayerStatData");*/

        #endregion

        #region �÷��̾� ��ų
        /*for (int i = 1; i < player_skill.playerSkill.Length + 1; i++)
        {
            player_skill.playerSkill[i - 1] = new PlayerSkill();
            player_skill.playerSkill[i - 1].skillName = "Slash";
            player_skill.playerSkill[i - 1].skillLev = i;
            player_skill.playerSkill[i - 1].skillDamage = (i * 50);
            player_skill.playerSkill[i - 1].coolTime = 12; // ��Ÿ��  12��  ����
        }
        JsonConvert.SerializeObject(player_skill);
        GameManager.Json.ToJson(player_skill, "PlayerSkill_Slash");*/

        #endregion

        pL = 1;  // �÷��̾� ���� �ӽ� ����
        Stat = PlayerAllData.StatDict[pL] as PlayerStat;

        isDead = false;

        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        #region ��æƮ(��ų)
        if (CheckEnchantTime > 0)  // ��ų 
        {
            CheckEnchantTime -= Time.deltaTime;
            
        }

        if(IsEnChantUse == true && CheckEnchantTime <= 0)
        {
            LightningEnChant();
        }
        #endregion

        #region ������(��ų)
        if (CheckSlachTime > 0)
        {
            CheckSlachTime -= Time.deltaTime;
        }

        if(IsSlashUse == true && CheckSlachTime <= 0)
        {
            Slash();
        }
        #endregion

        if (Stat.Hp == 0 && isDead == false)
        {
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/Dead/PlayerDeadEffect", transform.position + (Vector3.up * 7), Quaternion.identity, GameManager.staticPlayerParent.transform);
            Invoke("DeadCall", 0.5f);
            isDead = true;
            _anim.Play("Stun");
            state = Define.State.Die;
            return;
        }

        switch (state)
        {
            case Define.State.Idle:
                StateIdle();
                break;
            case Define.State.Run:
                StateRun();
                break;
            case Define.State.Attack:
                StateAttack();
                break;
            case Define.State.Hit:
                StateHit();
                break;
            case Define.State.Die:
                StateDie();
                break;
        }

        if (PlayerButton.pushAttackCount >= 1)  // �⺻����
        {
            StartCoroutine(nameof(BasicAttack));
        }
    }

    public void Movement(Vector2 inputDirection)  // PlayerJoyStick���� ���̽�ƽ �̵������� ȣ����
    {
        Vector2 moveInput = inputDirection;
        bool isMove = moveInput.magnitude != 0;

        if(isMove) // Run
        {
            // ī�޶� �ٶ󺸴� ���� (�����̵�)
            Vector3 lookForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;

            // ī�޶��� ������ ���� (�����̵�)
            Vector3 lookRight = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

            // �̵�����
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // �̵��� �� �̵����� �ٶ󺸱�
            gameObject.transform.forward = moveDir;

            // �̵�
            transform.position += moveDir * Time.deltaTime * 7f;

            _anim.SetBool("Run", true);
            PlayerButton.pushAttackCount = 0;
            _anim.SetInteger("Attack", 0);
            Trajectory.SetActive(false);
            state = Define.State.Run;
        }
        if(!isMove) // Idle
        {
            _anim.SetBool("Run", false);
            PlayerButton.pushAttackCount = 0;
            _anim.SetInteger("Attack", 0);
            Trajectory.SetActive(false);
            state = Define.State.Idle;
        }

    }

    #region �⺻����
    IEnumerator BasicAttack()
    {
        Trajectory.SetActive(true);
        _anim.SetInteger("Attack", PlayerButton.pushAttackCount);
        yield return null;
    }
    
    private void AnimAttack01Event() // Attack01 �ִϸ��̼� �������κп� �־����
    {
        if (PlayerButton.pushAttackCount < 2)
        {
            _anim.SetInteger("Attack", 0);
            PlayerButton.pushAttackCount = 0;
            state = Define.State.Attack;
        }
    }

    public void AnimAttack02Event()  // Attack02 �ִϸ��̼� �������κп� �־����
    {
        PlayerButton.pushAttackCount = 0;
        _anim.SetInteger("Attack", 0);
    }

    public void AnimAttackDamageEvent()  // Attack01, Attack02 �߰��κ� ������ ó��
    {
        BaseStat.CriticalRange = UnityEngine.Random.Range(0, 100);  
        BaseStat.DamageRange = UnityEngine.Random.Range(Stat.damage / 2, Stat.damage + 50);   

        Collider[] targetArr = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Monster"));
        for(int i = 0; i < targetArr.Length; i ++)
        {
            Stat.Attack_Damage(targetArr[i].gameObject.GetComponent<MonsterBaseController>().MStat,targetArr[i].gameObject);

            targetArr[i].GetComponent<MonsterBaseController>().state = Define.State.Hit;  // ����ĳ���� Hit���°����� ����
        }
    }
    public void AnimEnchantDamageEvent() // Attack �ִϸ��̼ǿ� �־��
    {
        Collider[] targetArr = Physics.OverlapSphere(transform.position, 2.5f, LayerMask.GetMask("Monster"));
        for (int i = 0; i < targetArr.Length; i++)
        {
            if (IsEnChantUse == true)
            {
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerSkillEffect/Enchant/EnchantHit", targetArr[i].transform.position + Vector3.up, Quaternion.identity, GameManager.staticHitEffectParent.transform);
                Skill.SkillAttackDamage(targetArr[i].gameObject.GetComponent<MonsterBaseController>().MStat,targetArr[i].gameObject,Stat);  // Enchant�� ������Ͻ� �߰� ����
            }
            
        }
            
    }

    public void AnimSound1()
    {
        GameManager.Sound.PlayEffectSound("Player/PlayerAttack1");
    }

    public void AnimSound2()
    {
        GameManager.Sound.PlayEffectSound("Player/PlayerAttack2");
    }
    #endregion

    #region ����Ʈ����æƮ
    public void LightningEnChant()  // 13�� ����(��ų) , 7����Ÿ��  (�ǰݸ鿪)
    {
        if(IsEnchant == false)
        {
            GameManager.Sound.PlayEffectSound("Player/EnchantSound");
            Stat.Critical_Chance += 20;  // �Ͻ������� ġ��Ÿ ���
            EnchantactiveTime = 13f;
            _anim.Play("LightningEnchantment_Skill");
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerSkillEffect/Enchant/EnchantStartEffect",
                transform.position + Vector3.up, Quaternion.identity, GameManager.staticPlayerParent.transform);
        }
        EnchantactiveTime -= Time.deltaTime;

        IsEnchant = true;
        GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerSkillEffect/Enchant/Lightning_Enchant(Player)",
             sword.transform.position, Quaternion.identity, GameManager.staticPlayerParent.transform);

        if (EnchantactiveTime < 0)
        {
            
            IsEnchant = false;
            IsEnChantUse = false;
            EnchantactiveTime = 0;
            CheckEnchantTime = 0;
            CheckEnchantTime = PlayerAllData.SkillEnchantDict[EnchantDataSet.staticEnchantLev].coolTime;
            Stat.Critical_Chance -= 20;  // ġ��Ÿ ���󺹱�
        }
    }
    

    #endregion

    #region ������
    public void Slash()
    {
        BaseStat.CriticalRange = UnityEngine.Random.Range(0, 100); 
        BaseStat.DamageRange = UnityEngine.Random.Range(Stat.damage / 2, Stat.damage + 50); 

        if (IsSlash == false)
        {
            GameManager.Sound.PlayEffectSound("Player/SlashStart");
            Stat.Critical_Chance += 60;  // �Ͻ������� ġ��Ÿ ���
            SlashactiveTime = 3f;
            _anim.Play("Slash_Skill");
        }
        SlashactiveTime -= Time.deltaTime;
        IsSlash = true;

        if(SlashactiveTime < 0)
        {
            IsSlash = false;
            IsSlashUse = false;
            SlashactiveTime = 0;
            CheckSlachTime = 0;
            CheckSlachTime = PlayerAllData.SkillSlashDict[SlashDataSet.staticSlashLev].coolTime;
            PlayerJoyStick.IsMovingJoyStick = true;
            Stat.Critical_Chance -= 60;  // ġ��Ÿ ���󺹱�
        }
    }

    public void AnimSlashEffect()
    {
        GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerSkillEffect/Slash/SlashRaising",
            transform.position + (Vector3.up * 2) + (transform.forward / 3), Quaternion.identity, GameManager.staticPlayerParent.transform);
    }

    public void AnimSlashStartEffect()
    {
        GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerSkillEffect/Slash/SlashStart", transform.position + Vector3.up, Quaternion.identity, GameManager.staticPlayerParent.transform);
    }

    public void AnimSlashDamage()
    {
        GameManager.Sound.PlayEffectSound("Player/SlashSound");
        Collider[] targetArr = Physics.OverlapSphere(transform.position, 3.0f, LayerMask.GetMask("Monster"));
        for (int i = 0; i < targetArr.Length; i++)
        {
            Skill.SkillAttackDamage(targetArr[i].gameObject.GetComponent<MonsterBaseController>().MStat, targetArr[i].gameObject,Stat);
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerSkillEffect/Slash/SlashHit", targetArr[i].transform.position + Vector3.up, Quaternion.identity, GameManager.staticHitEffectParent.transform);

            targetArr[i].GetComponent<MonsterBaseController>().state = Define.State.Hit;  // ����ĳ���� Hit���°����� ����
        }
    }

    #endregion

    public void AnimMovingStop()  // �ִϸ��̼� ���۽� ��� ����
    {
        PlayerJoyStick.IsMovingJoyStick = false;
    }
    public void AnimMoving()  // �ִϸ��̼� ������ �����ϼ��ְ� ����
    {
        PlayerJoyStick.IsMovingJoyStick = true;
    }

    private void StateIdle()
    {
        _anim.SetBool("Run", false);
        Trajectory.SetActive(false);
    }

    private void StateRun()
    {
        _anim.SetBool("Run", true);
    }

    private void StateAttack()
    {
        
    }

    private void StateHit()
    {
        if (IsSlashUse == true)  // ������ ��ų���� ��ø���
            return;
        if (IsEnChantUse == true) // ��æƮ ��ų���� �ִϸ��̼��� ��������ʰ� ��� ����
            return;


        if (_anim.GetInteger("Attack") == 0) // �������Ͻ� �ǰݾִϸ��̼��� ��� ���� �ʴ´�
            _anim.Play("Stun");

        if(_anim.GetBool("Run"))
            state = Define.State.Run;
        if(!_anim.GetBool("Run"))
            state = Define.State.Idle;

    }

    private void StateDie()
    {

    }

    private void DeadCall()  // ������ DeadEffect ȿ���� ��췯���� �־���
    {
        GameManager.Resource.Destroy(gameObject);
    }
}

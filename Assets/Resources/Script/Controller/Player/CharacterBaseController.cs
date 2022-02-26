using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class CharacterBaseController : MonoBehaviour
{
    public Define.State state = Define.State.Idle;

    [SerializeField]
    public GameObject sword;   // -> 추후 스킬 사용시 칼 포지션 부분에 스킬을 넣어줄때 사용할것 TODO

    [SerializeField]
    public GameObject Trajectory; // 검의 궤적

    private Animator _anim;
    bool isDead = false;  // 죽었나 살았나 체크

    #region 스탯불러오기
    public PlayerStat Stat { get; set; } = new PlayerStat();
    protected int pL = 0; // 플레이어 레벨

    //PlayerStatData player_stat = new PlayerStatData();
    #endregion

    #region 스킬불러오기 and 스킬 변수
    public static int SkillPoint = 1;  // 스킬포인트
    public PlayerSkill Skill { get; set; } = new PlayerSkill();
    //PlayerSkillData player_skill = new PlayerSkillData();


    // 스킬 : 라이트닝 인첸트 변수 모음
    bool IsEnchant = false;
    public static float EnchantactiveTime = 0;  // 스킬 시전 시간
    public static bool IsEnChantUse = false;  // 인챈트 사용 static 변수  (BlankSkillButtonSet 에서 사용중)
    public static float CheckEnchantTime = 0.0f;  // 쿨타임 체크

    // 스킬 : 슬래쉬 변수 모음
    bool IsSlash = false;
    public static float SlashactiveTime = 0;
    public static bool IsSlashUse = false; // 슬래쉬 사용 static 변수 (BlankSkillButtonSet 에서 사용중)
    public static float CheckSlachTime = 0.0f;  // 쿨타임 체크
    #endregion

    private void OnEnable()
    {
        #region 플레이어 스탯
        /*for (int i = 1; i < player_stat.playerStat.Length + 1; i++)
        {
            player_stat.playerStat[i - 1] = new PlayerStat();
            player_stat.playerStat[i - 1].level = i;
            player_stat.playerStat[i - 1].hp = ((i * 100) + 900);
            player_stat.playerStat[i - 1].maxHp = ((i * 100) + 900);
            player_stat.playerStat[i - 1].damage = ((i * 10) + 90);
            player_stat.playerStat[i - 1].exp = 0;
            player_stat.playerStat[i - 1].maxExp = (i * 1000);
            player_stat.playerStat[i - 1].critical_Chance = 10;  // 치명타 확률
        }

        JsonConvert.SerializeObject(player_stat);
        GameManager.Json.ToJson(player_stat, "PlayerStatData");*/

        #endregion

        #region 플레이어 스킬
        /*for (int i = 1; i < player_skill.playerSkill.Length + 1; i++)
        {
            player_skill.playerSkill[i - 1] = new PlayerSkill();
            player_skill.playerSkill[i - 1].skillName = "Slash";
            player_skill.playerSkill[i - 1].skillLev = i;
            player_skill.playerSkill[i - 1].skillDamage = (i * 50);
            player_skill.playerSkill[i - 1].coolTime = 12; // 쿨타임  12초  고정
        }
        JsonConvert.SerializeObject(player_skill);
        GameManager.Json.ToJson(player_skill, "PlayerSkill_Slash");*/

        #endregion

        pL = 1;  // 플레이어 레벨 임시 설정
        Stat = PlayerAllData.StatDict[pL] as PlayerStat;

        isDead = false;

        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        #region 인챈트(스킬)
        if (CheckEnchantTime > 0)  // 스킬 
        {
            CheckEnchantTime -= Time.deltaTime;
            
        }

        if(IsEnChantUse == true && CheckEnchantTime <= 0)
        {
            LightningEnChant();
        }
        #endregion

        #region 슬래쉬(스킬)
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

        if (PlayerButton.pushAttackCount >= 1)  // 기본공격
        {
            StartCoroutine(nameof(BasicAttack));
        }
    }

    public void Movement(Vector2 inputDirection)  // PlayerJoyStick에서 조이스틱 이동값으로 호출중
    {
        Vector2 moveInput = inputDirection;
        bool isMove = moveInput.magnitude != 0;

        if(isMove) // Run
        {
            // 카메라가 바라보는 방향 (수직이동)
            Vector3 lookForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;

            // 카메라의 오른쪽 방향 (수평이동)
            Vector3 lookRight = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

            // 이동방향
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // 이동할 때 이동방향 바라보기
            gameObject.transform.forward = moveDir;

            // 이동
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

    #region 기본공격
    IEnumerator BasicAttack()
    {
        Trajectory.SetActive(true);
        _anim.SetInteger("Attack", PlayerButton.pushAttackCount);
        yield return null;
    }
    
    private void AnimAttack01Event() // Attack01 애니메이션 마지막부분에 넣어놓음
    {
        if (PlayerButton.pushAttackCount < 2)
        {
            _anim.SetInteger("Attack", 0);
            PlayerButton.pushAttackCount = 0;
            state = Define.State.Attack;
        }
    }

    public void AnimAttack02Event()  // Attack02 애니메이션 마지막부분에 넣어놓음
    {
        PlayerButton.pushAttackCount = 0;
        _anim.SetInteger("Attack", 0);
    }

    public void AnimAttackDamageEvent()  // Attack01, Attack02 중간부분 데미지 처리
    {
        BaseStat.CriticalRange = UnityEngine.Random.Range(0, 100);  
        BaseStat.DamageRange = UnityEngine.Random.Range(Stat.damage / 2, Stat.damage + 50);   

        Collider[] targetArr = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Monster"));
        for(int i = 0; i < targetArr.Length; i ++)
        {
            Stat.Attack_Damage(targetArr[i].gameObject.GetComponent<MonsterBaseController>().MStat,targetArr[i].gameObject);

            targetArr[i].GetComponent<MonsterBaseController>().state = Define.State.Hit;  // 맞은캐릭터 Hit상태값으로 변경
        }
    }
    public void AnimEnchantDamageEvent() // Attack 애니메이션에 넣어둠
    {
        Collider[] targetArr = Physics.OverlapSphere(transform.position, 2.5f, LayerMask.GetMask("Monster"));
        for (int i = 0; i < targetArr.Length; i++)
        {
            if (IsEnChantUse == true)
            {
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerSkillEffect/Enchant/EnchantHit", targetArr[i].transform.position + Vector3.up, Quaternion.identity, GameManager.staticHitEffectParent.transform);
                Skill.SkillAttackDamage(targetArr[i].gameObject.GetComponent<MonsterBaseController>().MStat,targetArr[i].gameObject,Stat);  // Enchant를 사용중일시 추가 공격
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

    #region 라이트닝인챈트
    public void LightningEnChant()  // 13초 지속(스킬) , 7초쿨타임  (피격면역)
    {
        if(IsEnchant == false)
        {
            GameManager.Sound.PlayEffectSound("Player/EnchantSound");
            Stat.Critical_Chance += 20;  // 일시적으로 치명타 상승
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
            Stat.Critical_Chance -= 20;  // 치명타 원상복귀
        }
    }
    

    #endregion

    #region 슬래쉬
    public void Slash()
    {
        BaseStat.CriticalRange = UnityEngine.Random.Range(0, 100); 
        BaseStat.DamageRange = UnityEngine.Random.Range(Stat.damage / 2, Stat.damage + 50); 

        if (IsSlash == false)
        {
            GameManager.Sound.PlayEffectSound("Player/SlashStart");
            Stat.Critical_Chance += 60;  // 일시적으로 치명타 상승
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
            Stat.Critical_Chance -= 60;  // 치명타 원상복귀
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

            targetArr[i].GetComponent<MonsterBaseController>().state = Define.State.Hit;  // 맞은캐릭터 Hit상태값으로 변경
        }
    }

    #endregion

    public void AnimMovingStop()  // 애니메이션 시작시 잠시 멈춤
    {
        PlayerJoyStick.IsMovingJoyStick = false;
    }
    public void AnimMoving()  // 애니메이션 끝날시 움직일수있게 설정
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
        if (IsSlashUse == true)  // 슬래쉬 스킬사용시 즉시리턴
            return;
        if (IsEnChantUse == true) // 인챈트 스킬사용시 애니메이션을 재생하지않고 즉시 리턴
            return;


        if (_anim.GetInteger("Attack") == 0) // 공격중일시 피격애니메이션을 재생 하지 않는다
            _anim.Play("Stun");

        if(_anim.GetBool("Run"))
            state = Define.State.Run;
        if(!_anim.GetBool("Run"))
            state = Define.State.Idle;

    }

    private void StateDie()
    {

    }

    private void DeadCall()  // 죽을시 DeadEffect 효과와 어우러지게 넣어줌
    {
        GameManager.Resource.Destroy(gameObject);
    }
}

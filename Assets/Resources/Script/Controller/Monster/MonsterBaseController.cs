using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Newtonsoft.Json;
using UnityEngine.UI;

public class MonsterBaseController : MonoBehaviour
{
    public Define.State state = Define.State.Idle;

    protected Vector3 _destVec = Vector3.zero;
    protected NavMeshAgent _agent;
    protected Animator _anim;

    // ���� �̸��ؽ�Ʈ ����
    [SerializeField]
    private GameObject MonsterName;
    private bool NameBool = false;
    int currentLev = 0;
    int originLev = 0;

    [SerializeField, Header("�ν� �Ÿ�")]
    public float recognition_Distance = 0;
    [SerializeField, Header("���� ��Ÿ�")]
    public float attackRange = 0;

    protected MonsterStat _stat = new MonsterStat();
    public MonsterStat MStat { get => _stat; }

    [System.Serializable]
    public class StatData
    {
        public MonsterStat Mstat = new MonsterStat();
    }


    private void Start()
    {
        #region ���� ���Ȱ� ����
        /*StatData Stat = new StatData();


        Stat.Mstat = new MonsterStat();
        Stat.Mstat.Name = "Drgon";
        Stat.Mstat.level = 20;
        Stat.Mstat.hp = 3500;
        Stat.Mstat.maxHp = 3500;
        Stat.Mstat.damage = 150;
        Stat.Mstat.dropExp = 3000;
        Stat.Mstat.critical_Chance = 10;

        JsonConvert.SerializeObject(Stat);
        GameManager.Json.ToJson(Stat, "DragonStatData");*/
        #endregion

        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();

        if (!gameObject.name.Contains("Dragon"))
        {
            _destVec = transform.position + Random.onUnitSphere * 5;
            _destVec.y = transform.position.y;
        }
        if (gameObject.name.Contains("Dragon"))
        {
            _destVec = transform.position;
            _destVec.y = transform.position.y;
        }


            MonsterName = GameManager.Resource.Instantiate("UI/MonsterNameText", transform.position,
            Quaternion.identity, gameObject.transform);
    }

    private void Update()
    {
        if(!gameObject.name.Contains("Dragon"))
            SetTextPos();

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
    }

    protected IEnumerator LookAroundMoving()
    {
        yield return null;

        while(true)
        {
            _destVec = transform.position + Random.onUnitSphere * 4;
            _destVec.y = transform.position.y;

            NavMeshPath path = new NavMeshPath();
            if (_agent.CalculatePath(_destVec, path))
                yield break;
        }

    }

    protected virtual void StateIdle()
    {

    }

    protected virtual void StateRun()
    {

    }

    protected virtual void StateAttack()
    {
        BaseStat.MCriticalRange = UnityEngine.Random.Range(0, 100);
        BaseStat.MDamageRange = UnityEngine.Random.Range(_stat.damage / 2, _stat.damage);
    }

    protected virtual void StateHit()
    {

    }

    protected virtual void StateDie()
    {

    }

    protected void SetTextPos()
    {
        currentLev = GameManager.Player.PlayerController.Stat.Level;

        if (NameBool == false)
        {
            MonsterName.transform.position = transform.position + (MonsterName.transform.localScale + (Vector3.up * 1.6f) - (Vector3.right * 2.5f) - (Vector3.forward / 2));
            MonsterName.GetComponent<TextMesh>().text = $"Lv.{MStat.Level} / {MStat.Name}";
        }

        if (MonsterName.transform.parent.lossyScale.magnitude < 1f && NameBool == false)  // �θ��� �������� 1���� �۴ٸ� ������ŭ TextMesh�� ũ�⸦ �÷��ش� (1ȸ�� ����)
        {
            Vector3 Parentscale = MonsterName.transform.parent.localScale;
            MonsterName.transform.localScale += Parentscale;
            NameBool = true;
        }

        if (currentLev != originLev)
        {
            originLev = currentLev;
            if (MStat.Level <= GameManager.Player.PlayerController.Stat.Level + 3)   // �÷��̾�� 3�����̻� �����ʴ´ٸ� �ػ�
                MonsterName.GetComponent<TextMesh>().color = Color.white;
            if (MStat.Level > GameManager.Player.PlayerController.Stat.Level + 3)  // �÷��̾�� 3������ ���ٸ� �����
                MonsterName.GetComponent<TextMesh>().color = Color.yellow;
            if (MStat.Level > GameManager.Player.PlayerController.Stat.Level + 5) // �÷��̾�� 5������ ���ٸ� �����
                MonsterName.GetComponent<TextMesh>().color = Color.magenta;
            if (MStat.Level > GameManager.Player.PlayerController.Stat.Level + 7) // �÷��̾�� 7������ ���ٸ� ������
                MonsterName.GetComponent<TextMesh>().color = Color.red;
        }
        

    }
}

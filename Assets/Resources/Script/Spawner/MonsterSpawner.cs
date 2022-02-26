using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private List<GameObject> SpawnList = new List<GameObject>();

    // ���͸� �߻���ų �ֱ�
    [Header("���� �߻� �ֱ�")]
    public float createTime;

    // ������ �ִ� �߻� ��
    [SerializeField,Header("�ִ� ���� ��")]
    private int maxMonster = 5;

    // ���� ���� ��
    [SerializeField, Header("���� ���� ��")]
    private int currentMonster = 0;

    [Header("������ų ����")]
    public string MonsterName;

    private float _time = 0;
    private float _deadTime = 0;
    private int checkliveCount = 0;


    private void Start()
    {
        GameObject monster_Lich = Resources.Load<GameObject>("Prefabs/Monster/Lich/Lich");
        GameObject monster_Grunt = Resources.Load<GameObject>("Prefabs/Monster/Grunt/Grunt");
        GameObject monster_Golem = Resources.Load<GameObject>("Prefabs/Monster/Golem/Golem");

        SpawnList.Add(monster_Golem);   // �������͸� ���� ������   (�������ʹ� Ư����ġ�� ���� ���������ٰ���)
        SpawnList.Add(monster_Grunt);   // �������͸� ���� ������
        SpawnList.Add(monster_Lich);   // �������͸� ���� ������

    }
    private void Update()
    {
        _time += Time.deltaTime;
        if (SpawnList.Count > 0 && _time >= createTime)
        {
            _time = 0;
            StartCoroutine(SpawnObject());
        }

        _deadTime += Time.deltaTime;

        if(_deadTime > 10f) 
        {
            for (int i = 0; i < transform.childCount; i++)       
            {
                if(transform.GetChild(i).gameObject.activeSelf)      // SetActive�� �����ִ� Count
                    checkliveCount++;
            }
            _deadTime = 0;
            currentMonster = checkliveCount;   
            checkliveCount = 0;
        }
    }

    private IEnumerator SpawnObject()
    {
        if (currentMonster < maxMonster)
        {
            for (int i = 0; i < SpawnList.Count; i++)
            {
                if(MonsterName == SpawnList[i].name)
                {
                    GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/SpawnEffect", transform.position + (Vector3.up), Quaternion.Euler(90,0,0), GameManager.staticParent.transform);
                    GameManager.Resource.SpawnMonster(SpawnList[i], transform.position, Quaternion.identity,transform);
                    currentMonster++;
                }
            }
        }
        else
            yield return null;
    }



}

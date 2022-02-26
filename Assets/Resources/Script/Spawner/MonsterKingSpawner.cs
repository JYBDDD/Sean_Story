using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKingSpawner : MonoBehaviour
{
    private List<GameObject> SpawnList = new List<GameObject>();

    // ���͸� �߻���ų �ֱ�
    [SerializeField, Header("�������� �߻� �ֱ�")]
    private float createTime;

    // ������ �ִ� �߻� ��
    [SerializeField, Header("�ִ� ���� ���� ��")]
    private int maxMonster = 1;

    // ���� ���� ��
    [SerializeField, Header("���� ���� ���� ��")]
    private int currentMonster = 0;

    [Header("������ų ���� ����")]
    public string MonsterName;

    private float _time = 0;
    private float _deadTime = 0;
    private int checkliveCount = 0;

    private void Start()
    {
        GameObject monster_LichKing = Resources.Load<GameObject>("Prefabs/Monster/Lich/LichKing");
        GameObject monster_GruntKing = Resources.Load<GameObject>("Prefabs/Monster/Grunt/GruntKing");
        GameObject monster_GolemKing = Resources.Load<GameObject>("Prefabs/Monster/Golem/GolemKing");

        SpawnList.Add(monster_GolemKing);   //(�������ʹ� Ư����ġ�� ���� ���������ٰ���)
        SpawnList.Add(monster_GruntKing); 
        SpawnList.Add(monster_LichKing);  
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

        if (_deadTime > 10f)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)      // SetActive�� �����ִ� Count
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
                if (MonsterName == SpawnList[i].name)
                {
                    GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/BossSpawnEffect", transform.position, Quaternion.Euler(90,0,0), GameManager.staticParent.transform);
                    GameManager.Resource.SpawnMonster(SpawnList[i], transform.position, Quaternion.identity, transform);
                    currentMonster++;
                }
            }
        }
        else
            yield return null;
    }

}

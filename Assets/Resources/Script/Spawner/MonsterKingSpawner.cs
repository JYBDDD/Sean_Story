using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKingSpawner : MonoBehaviour
{
    private List<GameObject> SpawnList = new List<GameObject>();

    // 몬스터를 발생시킬 주기
    [SerializeField, Header("보스몬스터 발생 주기")]
    private float createTime;

    // 몬스터의 최대 발생 수
    [SerializeField, Header("최대 보스 몬스터 수")]
    private int maxMonster = 1;

    // 현재 몬스터 수
    [SerializeField, Header("현재 보스 몬스터 수")]
    private int currentMonster = 0;

    [Header("스폰시킬 보스 몬스터")]
    public string MonsterName;

    private float _time = 0;
    private float _deadTime = 0;
    private int checkliveCount = 0;

    private void Start()
    {
        GameObject monster_LichKing = Resources.Load<GameObject>("Prefabs/Monster/Lich/LichKing");
        GameObject monster_GruntKing = Resources.Load<GameObject>("Prefabs/Monster/Grunt/GruntKing");
        GameObject monster_GolemKing = Resources.Load<GameObject>("Prefabs/Monster/Golem/GolemKing");

        SpawnList.Add(monster_GolemKing);   //(보스몬스터는 특정위치에 따로 생성시켜줄것임)
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
                if (transform.GetChild(i).gameObject.activeSelf)      // SetActive가 켜져있는 Count
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

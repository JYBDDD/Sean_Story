using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadEffectController : EffectBaseController
{
    int count = 0;
    float time = 0;
    public static bool pushGround = false;  // 비석이 땅에 닿았을시 true 체크
    public static bool IsDead = false;

    private void Awake()
    {
        InventoryDragAndDrop.ItemSave();
    }

    private void OnEnable()
    {
        count = 0;
        pushGround = false;
        time = 0.2f;
        GameManager.Json.SaveData();
    }

    private void Update()
    {
        if(count == 1)
        {
            _time += Time.deltaTime;
            if(_time >= time)
            {
                pushGround = false;
                if(_time > 1)
                {
                    _time = 0;
                    IsDead = true;
                    goPool();
                }
            }
        }

    }

    private void goPool()
    {
        GameManager.Pool.InsertPool(gameObject);
        count = 0;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(count == 0)
        {
            count = 1;
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/Dead/DeadEffect1_1", transform.position, Quaternion.identity, GameManager.staticPlayerParent.transform);
            pushGround = true;
        }
    }
}

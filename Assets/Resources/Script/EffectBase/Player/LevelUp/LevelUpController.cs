using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpController : EffectBaseController
{
    private float deleteTime = 1.0f;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            _time += Time.deltaTime;
            if (_time >= deleteTime)
            {
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/UIEffect/LevelUp2", transform.position, Quaternion.identity, GameManager.staticPlayerParent.transform);
                GameManager.Sound.PlayEffectSound("Player/LevelUp");
                goPool();
                _time = 0;
            }
        }
    }

    private void goPool()
    {
        GameManager.Pool.InsertPool(gameObject);
    }
}

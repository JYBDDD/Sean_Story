using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichDeadEffectController : EffectBaseController
{
    private float deleteTime = 10f;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            _time += Time.deltaTime;
            if (_time >= deleteTime)
            {
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

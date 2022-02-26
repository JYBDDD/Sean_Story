using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageEffectController : EffectBaseController
{
    private float deleteTime = 0.8f;
    float moveUp = 0.0f;

    TextMesh textMesh;

    private void OnEnable()
    {
        textMesh = GetComponent<TextMesh>();
        moveUp = 0.015f;
        textMesh.text = $"{BaseStat.MDamageRange}";
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            _time += Time.deltaTime;
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + moveUp, transform.position.z);

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

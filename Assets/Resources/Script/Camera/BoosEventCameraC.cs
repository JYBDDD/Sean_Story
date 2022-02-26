using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosEventCameraC : MonoBehaviour
{
    private float _time = 0;
    [SerializeField]
    private GameObject worldCanvas;

    private void Start()
    {
        worldCanvas.SetActive(false);
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > 2.5f)
        {
            gameObject.SetActive(false);
            worldCanvas.SetActive(true);
        }
    }
}

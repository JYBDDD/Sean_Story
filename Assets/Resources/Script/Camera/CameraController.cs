using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ī�޶� ���� ������
    Vector3 originVec = Vector3.zero;

    // ī�޶� ���� ������
    Vector3 currentVec = Vector3.zero;

    [SerializeField]
    Vector3 cameraPos;
    [SerializeField]
    Vector3 cameraRot;

    private void Start()
    {
        currentVec = GameManager.Player.player.transform.position + cameraPos;
    }

    private void Update()
    {
        if (GameManager.Player.PlayerController.Stat.Hp <= 0 && PlayerDeadEffectController.pushGround == true)  // �׾����� ���� �������� ī�޶� ���� �ش�
        {
            StartCoroutine(CameraShake(0.15f, 0.4f));
        }

        currentVec = GameManager.Player.player.transform.position + cameraPos;
        if (originVec != currentVec && GameManager.Player.player != null)
        {
            CameraMovement();
            originVec = transform.position;
        }
    }

    public void CameraMovement()
    {
        if(GameManager.Player.PlayerController.Stat.Hp > 0)
        {
            transform.position = Vector3.Lerp(originVec, currentVec, Time.deltaTime * 5);
            transform.rotation = Quaternion.Euler(cameraRot);
        }
        
    }

    public IEnumerator CameraShake(float time,float magnitude)  // �׾����� ���� �������� ī�޶� ���� �ش�
    {
        Vector3 originPos = transform.position;

        float duration = 0.0f;

        while(duration < time)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originPos.x + x, originPos.y + y, originPos.z);
            duration += Time.deltaTime;
            yield return null;
        }
        transform.position = originPos;
    }
}

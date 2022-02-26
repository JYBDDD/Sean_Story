using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMagic : MonoBehaviour
{
    [SerializeField]
    private int moveSpeed = 0;
    private float crossRoad;
    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if(gameObject != null)
        {
            crossRoad = (Camera.main.transform.position - transform.position).magnitude;
            L_iceMagic();
        }
    }

    private void L_iceMagic()
    {
        if(crossRoad > 20f)
        {
            GameManager.Resource.Destroy(gameObject);
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Map") && !other.gameObject.CompareTag("Monster"))
        {
            GameManager.Resource.Destroy(gameObject);
            GameManager.Sound.PlayEffectSound("Monster/Lich/LichIceBallHitSound");
            GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/Lich/MonsterEffect/Explosion_hit", transform.position, Quaternion.identity, GameManager.staticParent.transform);
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Resource.Destroy(gameObject);
            GameManager.Sound.PlayEffectSound("Monster/Lich/LichIceBallHitSound");
            GameManager.Resource.Instantiate("UI/Particle_Effect/Monster/Lich/MonsterEffect/Explosion_hit", transform.position, Quaternion.identity, GameManager.staticParent.transform);

            LichController.staticLich.Stat.Attack_Damage(other.gameObject.GetComponent<CharacterBaseController>().Stat,other.gameObject);
            other.gameObject.GetComponent<CharacterBaseController>().state = Define.State.Hit;

            return;
        }
    }
}

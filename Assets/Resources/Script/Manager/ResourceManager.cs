using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : IManager
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        T obj = Resources.Load<T>(path);

        if (obj == null)
        {
            Debug.Log("잘못된 경로입니다.");
            return null;
        }

        return obj;
    }
    public GameObject Instantiate(string path,Transform parent = null)
    {
        GameObject go = Load<GameObject>($"Prefabs/{path}");
        GameObject clone = Object.Instantiate(go,parent);
        return clone;
    }

    public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (parent == GameManager.staticItemParent.transform)
        {
            for (int i = 0; i < GameManager.staticItemParent.transform.childCount - 1; i++)
            {
                if (GameManager.staticItemParent.transform.GetChild(i).name.Contains(prefab.name) && !GameManager.staticItemParent.transform.GetChild(i).gameObject.activeSelf)  // 타격 데미지 오브젝트 재생성
                {
                    return GameManager.Pool.RecyclePool(prefab.name, position, rotation);
                }
            }
        }
        GameObject go = Object.Instantiate(prefab, position, rotation, parent);
        go.name = prefab.name;
        return go;
    }

    public GameObject Instantiate(string path, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject go = Load<GameObject>($"Prefabs/{path}");

        if (go == null)
            return null;

        if(parent == GameManager.staticParent.transform)
        {
            for (int i = 0; i < GameManager.staticParent.transform.childCount - 1; i++)
            {
                if (GameManager.staticParent.transform.GetChild(i).name.Contains(path) && !GameManager.staticParent.transform.GetChild(i).gameObject.activeSelf)  // 몬스터 오브젝트 재생성
                {
                    return GameManager.Pool.RecyclePool(path, position, rotation);
                }
            }
        }
       
        if(parent == GameManager.staticPlayerParent.transform)
        {
            for (int i = 0; i < GameManager.staticPlayerParent.transform.childCount - 1; i++)
            {
                if (GameManager.staticPlayerParent.transform.GetChild(i).name.Contains(path) && !GameManager.staticPlayerParent.transform.GetChild(i).gameObject.activeSelf)  // 플레이어 오브젝트 재생성
                {
                    return GameManager.Pool.RecyclePool(path, position, rotation);
                }
            }
        }
        
        if(parent == GameManager.staticHitEffectParent.transform)
        {
            for (int i = 0; i < GameManager.staticHitEffectParent.transform.childCount - 1; i++)
            {
                if (GameManager.staticHitEffectParent.transform.GetChild(i).name.Contains(path) && !GameManager.staticHitEffectParent.transform.GetChild(i).gameObject.activeSelf)  // 타격 이펙트 오브젝트 재생성
                {
                    return GameManager.Pool.RecyclePool(path, position, rotation);
                }
            }
        }

        if (parent == GameManager.staticHitDamageParent.transform)
        {
            for (int i = 0; i < GameManager.staticHitDamageParent.transform.childCount - 1; i++)
            {
                if (GameManager.staticHitDamageParent.transform.GetChild(i).name.Contains(path) && !GameManager.staticHitDamageParent.transform.GetChild(i).gameObject.activeSelf)  // 타격 데미지 오브젝트 재생성
                {
                    return GameManager.Pool.RecyclePool(path, position, rotation);
                }
            }
        }



        GameObject clone = UnityEngine.Object.Instantiate(go, position, rotation, parent);
        clone.name = path;

        return clone;
    }

    public void Destroy(GameObject Gobject)
    {
        if (Gobject == null)
        {
            Debug.Log("오브젝트가 Null 입니다.");
            return;
        }

        GameManager.Pool.InsertPool(Gobject);
        return;
    }

    public GameObject CharacterRecycle(string path, Vector3 position, Quaternion rotation, Transform parent = null)  // 캐릭터 재생성 전용
    {
        return GameManager.Pool.RecyclePool(path, position, rotation);
    }

    public GameObject SpawnMonster(GameObject prefabs,Vector3 position,Quaternion rotation,Transform parent)  // 몬스터 스폰
    {
        for (int i = 0; i < parent.transform.childCount - 1; i++)
        {
            if (parent.transform.GetChild(i).name.Contains(prefabs.name) && !parent.transform.GetChild(i).gameObject.activeSelf)
            {
                return GameManager.Pool.RecyclePool(prefabs.name, position, rotation);
            }
        }
        GameObject clone = UnityEngine.Object.Instantiate(prefabs, position, rotation, parent);
        clone.name = prefabs.name;
        return clone;
    }

    public List<GameObject> Items = new List<GameObject>();
    public List<ItemBaseInstance> ItemInstanceList = new List<ItemBaseInstance>();
    public void SpawnItem(GameObject dropMonster, int level)
    {
        bool GloveBool = false;
        bool CasqueBool = false;
        bool ArmorBool = false;
        bool BootsBool = false;
        bool LeggingsBool = false;
        bool SwordBool = false;

        bool equipmentBool = false;

        int GlovesDropRate = 100;
        int CasqueDropRate = 100;
        int ArmorDropRate = 100;
        int BootsDropRate = 100;
        int LeggingsDropRate = 100;
        int SwordDropRate = 100;

        foreach (ItemBaseInstance item in ItemInstanceList)
        {
            int randDropRate = Random.Range(0, 100); // 코인,Etc,소비 아이템 드랍률

            Vector3 dropPos = dropMonster.transform.position + (Vector3.up * 2);  // 아이템 위치값 설정
            dropPos.y = dropMonster.transform.position.y;

            if (randDropRate <= item.itemObject.DropRate)
            {
                if (item.itemObject.ItemTypes == Define.ItemType.Coin)  // 아이템 타입이 코인이라면
                {
                    item.itemObject.Amount = 100;  // 골드값 100으로 재초기화  (여기 안해주면 골드값 중첩됨)
                    int CoinRate = Random.Range((item.itemObject.Amount * level) / 3, (item.itemObject.Amount * level) / 2);  // 코인 랜덤드랍골드
                    item.itemObject.Amount = CoinRate;
                }

                if(item.itemObject.ItemTypes == Define.ItemType.Coin | item.itemObject.ItemTypes == Define.ItemType.Consumption | item.itemObject.ItemTypes == Define.ItemType.Etc)
                {
                    GameManager.Resource.Instantiate(item.gameObject, dropPos, Quaternion.identity, GameManager.staticItemParent.transform); // 코인,Etc,소비 아이템 드랍률에 들어왔을 경우 생성 
                }
            }
            if(item.itemObject.ItemTypes == Define.ItemType.Equipment)
            {
                if (equipmentBool == false)  // 호출시 한번만 돌도록함
                {
                    GlovesDropRate = Random.Range(0, 100);   // 장갑드랍률
                    CasqueDropRate = Random.Range(0, 100);   // 투구드랍률
                    ArmorDropRate = Random.Range(0, 100);    // 갑옷드랍률
                    BootsDropRate = Random.Range(0, 100);    // 신발드랍률
                    LeggingsDropRate = Random.Range(0, 100); // 각반드랍률
                    SwordDropRate = Random.Range(0, 100);    // 상검드랍률
                    equipmentBool = true;
                }
                

                if (GlovesDropRate <= item.itemObject.DropRate && GloveBool == false)  // 장갑드랍률에 들어온다면
                {
                    GameManager.Resource.Instantiate("Item/Gloves", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    GloveBool = true;
                }
                if (CasqueDropRate <= item.itemObject.DropRate && CasqueBool == false)  // 투구드랍률에 들어온다면
                {
                    GameManager.Resource.Instantiate("Item/Casque", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    CasqueBool = true;
                }
                if (ArmorDropRate <= item.itemObject.DropRate && ArmorBool == false)  // 갑옷드랍률에 들어온다면
                {
                    GameManager.Resource.Instantiate("Item/Armor", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    ArmorBool = true;
                }
                if (BootsDropRate <= item.itemObject.DropRate && BootsBool == false)  // 신발드랍률에 들어온다면
                {
                    GameManager.Resource.Instantiate("Item/Boots", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    BootsBool = true;
                }
                if (LeggingsDropRate <= item.itemObject.DropRate && LeggingsBool == false) // 각반드랍률에 들어온다면
                {
                    GameManager.Resource.Instantiate("Item/Leggings", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    LeggingsBool = true;
                }
                if (SwordDropRate <= item.itemObject.DropRate && SwordBool == false)  // 검드랍률에 들어온다면
                {
                    GameManager.Resource.Instantiate("Item/Sword", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    SwordBool = true;
                }
            }
            


        }

    }

    public void Init()
    {
        GameObject[] itemArr = Resources.LoadAll<GameObject>("Prefabs/Item");
        for (int i = 0; i < itemArr.Length; i++)
        {
            Items.Add(itemArr[i]);
            ItemInstanceList.Add(Items[i].GetComponent<ItemBaseInstance>());       
        }
    }

    public void OnUpdate()
    {

    }

    public void Clear()
    {
        
    }
}

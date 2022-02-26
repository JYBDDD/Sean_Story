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
            Debug.Log("�߸��� ����Դϴ�.");
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
                if (GameManager.staticItemParent.transform.GetChild(i).name.Contains(prefab.name) && !GameManager.staticItemParent.transform.GetChild(i).gameObject.activeSelf)  // Ÿ�� ������ ������Ʈ �����
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
                if (GameManager.staticParent.transform.GetChild(i).name.Contains(path) && !GameManager.staticParent.transform.GetChild(i).gameObject.activeSelf)  // ���� ������Ʈ �����
                {
                    return GameManager.Pool.RecyclePool(path, position, rotation);
                }
            }
        }
       
        if(parent == GameManager.staticPlayerParent.transform)
        {
            for (int i = 0; i < GameManager.staticPlayerParent.transform.childCount - 1; i++)
            {
                if (GameManager.staticPlayerParent.transform.GetChild(i).name.Contains(path) && !GameManager.staticPlayerParent.transform.GetChild(i).gameObject.activeSelf)  // �÷��̾� ������Ʈ �����
                {
                    return GameManager.Pool.RecyclePool(path, position, rotation);
                }
            }
        }
        
        if(parent == GameManager.staticHitEffectParent.transform)
        {
            for (int i = 0; i < GameManager.staticHitEffectParent.transform.childCount - 1; i++)
            {
                if (GameManager.staticHitEffectParent.transform.GetChild(i).name.Contains(path) && !GameManager.staticHitEffectParent.transform.GetChild(i).gameObject.activeSelf)  // Ÿ�� ����Ʈ ������Ʈ �����
                {
                    return GameManager.Pool.RecyclePool(path, position, rotation);
                }
            }
        }

        if (parent == GameManager.staticHitDamageParent.transform)
        {
            for (int i = 0; i < GameManager.staticHitDamageParent.transform.childCount - 1; i++)
            {
                if (GameManager.staticHitDamageParent.transform.GetChild(i).name.Contains(path) && !GameManager.staticHitDamageParent.transform.GetChild(i).gameObject.activeSelf)  // Ÿ�� ������ ������Ʈ �����
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
            Debug.Log("������Ʈ�� Null �Դϴ�.");
            return;
        }

        GameManager.Pool.InsertPool(Gobject);
        return;
    }

    public GameObject CharacterRecycle(string path, Vector3 position, Quaternion rotation, Transform parent = null)  // ĳ���� ����� ����
    {
        return GameManager.Pool.RecyclePool(path, position, rotation);
    }

    public GameObject SpawnMonster(GameObject prefabs,Vector3 position,Quaternion rotation,Transform parent)  // ���� ����
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
            int randDropRate = Random.Range(0, 100); // ����,Etc,�Һ� ������ �����

            Vector3 dropPos = dropMonster.transform.position + (Vector3.up * 2);  // ������ ��ġ�� ����
            dropPos.y = dropMonster.transform.position.y;

            if (randDropRate <= item.itemObject.DropRate)
            {
                if (item.itemObject.ItemTypes == Define.ItemType.Coin)  // ������ Ÿ���� �����̶��
                {
                    item.itemObject.Amount = 100;  // ��尪 100���� ���ʱ�ȭ  (���� �����ָ� ��尪 ��ø��)
                    int CoinRate = Random.Range((item.itemObject.Amount * level) / 3, (item.itemObject.Amount * level) / 2);  // ���� ����������
                    item.itemObject.Amount = CoinRate;
                }

                if(item.itemObject.ItemTypes == Define.ItemType.Coin | item.itemObject.ItemTypes == Define.ItemType.Consumption | item.itemObject.ItemTypes == Define.ItemType.Etc)
                {
                    GameManager.Resource.Instantiate(item.gameObject, dropPos, Quaternion.identity, GameManager.staticItemParent.transform); // ����,Etc,�Һ� ������ ������� ������ ��� ���� 
                }
            }
            if(item.itemObject.ItemTypes == Define.ItemType.Equipment)
            {
                if (equipmentBool == false)  // ȣ��� �ѹ��� ��������
                {
                    GlovesDropRate = Random.Range(0, 100);   // �尩�����
                    CasqueDropRate = Random.Range(0, 100);   // ���������
                    ArmorDropRate = Random.Range(0, 100);    // ���ʵ����
                    BootsDropRate = Random.Range(0, 100);    // �Źߵ����
                    LeggingsDropRate = Random.Range(0, 100); // ���ݵ����
                    SwordDropRate = Random.Range(0, 100);    // ��˵����
                    equipmentBool = true;
                }
                

                if (GlovesDropRate <= item.itemObject.DropRate && GloveBool == false)  // �尩������� ���´ٸ�
                {
                    GameManager.Resource.Instantiate("Item/Gloves", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    GloveBool = true;
                }
                if (CasqueDropRate <= item.itemObject.DropRate && CasqueBool == false)  // ����������� ���´ٸ�
                {
                    GameManager.Resource.Instantiate("Item/Casque", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    CasqueBool = true;
                }
                if (ArmorDropRate <= item.itemObject.DropRate && ArmorBool == false)  // ���ʵ������ ���´ٸ�
                {
                    GameManager.Resource.Instantiate("Item/Armor", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    ArmorBool = true;
                }
                if (BootsDropRate <= item.itemObject.DropRate && BootsBool == false)  // �Źߵ������ ���´ٸ�
                {
                    GameManager.Resource.Instantiate("Item/Boots", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    BootsBool = true;
                }
                if (LeggingsDropRate <= item.itemObject.DropRate && LeggingsBool == false) // ���ݵ������ ���´ٸ�
                {
                    GameManager.Resource.Instantiate("Item/Leggings", dropPos, Quaternion.identity, GameManager.staticItemParent.transform);
                    LeggingsBool = true;
                }
                if (SwordDropRate <= item.itemObject.DropRate && SwordBool == false)  // �˵������ ���´ٸ�
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNameTextCam : MonoBehaviour
{
    private void Update()
    {
         SetRotation();
    }
    
    private void SetRotation()
    {
        Quaternion Qut = Quaternion.Euler(Camera.main.transform.rotation.x + 60f, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z);
        transform.rotation = Qut;
    }
}

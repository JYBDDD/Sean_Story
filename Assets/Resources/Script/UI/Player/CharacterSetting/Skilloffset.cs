using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skilloffset : MonoBehaviour
{
    [SerializeField]
    private Text skillPointCount;
    
    private void Update()
    {
        skillPointCount.text = $"스킬포인트 : {CharacterBaseController.SkillPoint}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static CharacterBaseController characterBaseController;

    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10f, 60f)]
    private float leverRange;

    private Vector2 inputDirection;
    private bool isInput;

    public static bool IsMovingJoyStick = true;  // 움직임을 멈춰야할때 써줄 static 변수
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        characterBaseController = GameManager.Player.PlayerController;
    }

    void Update()
    {
        if(isInput && GameManager.Player.PlayerController.Stat.Hp > 0 && IsMovingJoyStick == true)
        {
            InputControlerVector();
        }
    }

    private void InputControlerVector()
    {
        if(characterBaseController)
        {
            characterBaseController.Movement(inputDirection);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
        isInput = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        characterBaseController.Movement(Vector2.zero);

    }

    public void ControlJoyStickLever(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
        inputDirection = clampedDir / leverRange;
    }
}

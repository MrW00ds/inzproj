using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour
{
    [Header("Joysticks")]
    [SerializeField]
    private GameObject leftJoystickBackground;
    [SerializeField]
    private GameObject leftJoystickThumb;
    [SerializeField]
    private GameObject rightJoystickBackground;
    [SerializeField]
    private GameObject rightJoystickThumb;

    [Header("Character Movement")]
    [SerializeField]
    private float moveSpeed = 5f;

    private RectTransform leftJoystickRect;
    private RectTransform rightJoystickRect;
    private Vector2 leftJoystickOriginalPosition;
    private Vector2 rightJoystickOriginalPosition;
    private Vector2 leftJoystickDirection;
    private Vector2 rightJoystickDirection;

    private bool leftJoystickVisible = false;
    private bool rightJoystickVisible = false;

    private void Start()
    {
        leftJoystickRect = leftJoystickThumb.GetComponent<RectTransform>();
        rightJoystickRect = rightJoystickThumb.GetComponent<RectTransform>();

        // Get the initial positions of the joysticks
        leftJoystickOriginalPosition = leftJoystickRect.anchoredPosition;
        rightJoystickOriginalPosition = rightJoystickRect.anchoredPosition;

        // Hide the joysticks at the start
        SetJoystickVisibility(true);
    }

    private void Update()
    {
        HandleJoystick(leftJoystickRect, leftJoystickThumb.transform, ref leftJoystickDirection, ref leftJoystickVisible);
        HandleJoystick(rightJoystickRect, rightJoystickThumb.transform, ref rightJoystickDirection, ref rightJoystickVisible);

        // Move the character based on the joystick directions
        Vector3 movement = new Vector3(leftJoystickDirection.x, 0, leftJoystickDirection.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void HandleJoystick(RectTransform background, Transform thumb, ref Vector2 direction, ref bool joystickVisible)
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (RectTransformUtility.RectangleContainsScreenPoint(background, touch.position))
                {
                    direction = (touch.position - background.anchoredPosition) / (background.sizeDelta.x / 2);

                    float clampMagnitude = Mathf.Min(direction.magnitude, 1);
                    thumb.position = background.position + new Vector3(direction.x, direction.y, 0) * (background.sizeDelta.x / 2 * clampMagnitude);

                    joystickVisible = true;
                }
            }
        }
        else
        {
            thumb.position = background.position;
            direction = Vector2.zero;
            joystickVisible = true;
        }

        SetJoystickVisibility(joystickVisible);
    }

    private void SetJoystickVisibility(bool isVisible)
    {
        leftJoystickBackground.SetActive(isVisible);
        leftJoystickThumb.SetActive(isVisible);
        rightJoystickBackground.SetActive(isVisible);
        rightJoystickThumb.SetActive(isVisible);
    }
}

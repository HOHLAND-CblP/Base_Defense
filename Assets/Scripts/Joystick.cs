using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    [Header("Touch Marker")]
    public GameObject handle;
    private RectTransform handleRect;
    private Image handleImage;

    private RectTransform rectTransform;
    private Image image;

    Vector2 targetVector;

    [Header("Player")]
    [SerializeField]
    Player player;

    float clampPos;


    void Start()
    {
        handleRect = handle.GetComponent<RectTransform>();
        handleImage = handle.GetComponent<Image>();

        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        clampPos = rectTransform.rect.width / 2 - handleRect.rect.width / 2; ;

        handle.transform.position = transform.position;

        HideJoystick();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            handleImage.color = new Color(handleImage.color.r, handleImage.color.g, handleImage.color.b, 1);

            transform.position = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && handleRect.position != Input.mousePosition)
        {
            handleRect.position = Input.mousePosition;

            if (handleRect.anchoredPosition.magnitude > clampPos)
            {
                Vector2 normalizedTmRect = handleRect.anchoredPosition.normalized;
                handleRect.anchoredPosition = new Vector2(clampPos * normalizedTmRect.x, clampPos * normalizedTmRect.y);
            }


            player.SetDirection(new Vector2(handleRect.anchoredPosition.x / clampPos, handleRect.anchoredPosition.y / clampPos));
        }

        if (Input.GetMouseButtonUp(0))
        {
            HideJoystick();

            handle.transform.localPosition = Vector3.zero;
            player.SetDirection(new Vector2());
        }
    }

    void HideJoystick()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        handleImage.color = new Color(handleImage.color.r, handleImage.color.g, handleImage.color.b, 0);
    }
}
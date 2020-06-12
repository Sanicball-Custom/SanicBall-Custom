using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public Image bgImage, ThumbImage;
    public Vector2 Inputv;

	void Start ()
    {
        bgImage = GetComponent<Image>();
        ThumbImage = GetComponentInChildren<Image>();
	}
	
	void Update ()
    {
		
	}

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {

    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            Debug.Log("yeet");
        }
    }
}

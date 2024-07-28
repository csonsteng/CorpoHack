using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesktopShortcut : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	private bool _dragging = false;
	private Vector3 _mouseOffset;
	private float _lastClickTime;
	private DesktopWindow _window;

	public void Register(DesktopWindow window)
	{
		_window = window;
	}

	private void Update()
	{
		if (_dragging)
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mouseOffset);
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		_dragging = true; 
		_mouseOffset = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		transform.SetAsLastSibling();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_dragging = false;
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		var time = Time.time;
		if (time - _lastClickTime < 0.5f)
		{
			_window.Open();
		}
		_lastClickTime = time;
	}
}

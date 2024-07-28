using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DesktopWindowBar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	private Action _onDragStart;
	private Action _onDrag;

	private bool _dragging = false;

	public void Register(Action onDragStart, Action onDrag)
	{
		_onDrag = onDrag;
		_onDragStart = onDragStart;
	}

	private void Update()
	{
		if (_dragging)
		{
			_onDrag?.Invoke();
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		_dragging = true;
		_onDragStart?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_dragging = false;
	}
}

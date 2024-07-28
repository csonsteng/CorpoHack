using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DesktopWindow : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private DesktopShortcut _shortcut;
	[SerializeField] private bool _startOpen;
	[SerializeField] private DesktopWindowBar _headerBar;
	[SerializeField] private Image _frame;
	[SerializeField] private GameObject _inactiveBlocker;
	[SerializeField] private UnityEvent _onOpen;
	[SerializeField] private UnityEvent _onClose;

	private bool _open;
	private Vector3 _mouseOffset;
	private bool _isActive;

	private static Color BaseColor => new Color(0.54f, 0.54f, 0.54f, 0.54f);
	private static Color SelectedColor => new Color(0f, 1f, 1, 0.8f);

	private void Awake()
	{
		if (_shortcut != null)
		{
			_shortcut.Register(this);
		}
		_headerBar.Register(OnDragStart, OnDrag);
		
		transform.localScale = _startOpen ? Vector3.one : Vector3.zero;
		_open = _startOpen;
	}

	private void Start()
	{
		SetAsActive(transform.GetSiblingIndex() == transform.parent.childCount - 1);
	}

	private void Update()
	{
		if(!_isActive)
		{
			return;
		}
		if(transform.GetSiblingIndex() != transform.parent.childCount - 1)
		{
			SetAsActive(false);
		}
	}

	private void SetAsActive(bool active)
	{
		if (active)
		{
			transform.SetAsLastSibling();
		}
		_inactiveBlocker.SetActive(!active);
		_isActive = active;
		_frame.color = active ? SelectedColor : BaseColor;
	}

	public void Open()
	{
		_onOpen?.Invoke();
		SetAsActive(true);
		if (!_open)
		{
			(transform as RectTransform).anchoredPosition = (_shortcut.transform as RectTransform).anchoredPosition;
		}
		_open = true;
		transform.DOScale(1f, 0.25f);
		(transform as RectTransform).DOAnchorPos(Vector2.zero, 0.25f);
	}
	public void Close()
	{
		SetAsActive(false);
		_open = false;
		transform.DOScale(0f, 0.25f);
		(transform as RectTransform).DOAnchorPos((_shortcut.transform as RectTransform).anchoredPosition, 0.25f);
		_onClose?.Invoke();
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		SetAsActive(true);
	}

	private void OnDragStart()
	{
		_mouseOffset = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		SetAsActive(true);
	}

	private void OnDrag()
	{
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mouseOffset);
	}
}

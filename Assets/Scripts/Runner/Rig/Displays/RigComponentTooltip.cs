using Runner.Deck;
using Runner.Target;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RigComponentTooltip : Singleton<RigComponentTooltip>
{

	[SerializeField] private RectTransform _container;
	[SerializeField] private GameObject _cancelBlocker;
	[SerializeField] private TextMeshProUGUI _title;

	private Action _cancelled;

	private IRigComponentDisplay _target;
	private bool _locked = false;

	private void Awake()
	{
		_container.gameObject.SetActive(false);
		_cancelBlocker.SetActive(false);
	}

	public bool IsLocked => _locked;

	public void Register(Action onCancel)
	{
		_cancelled += onCancel;
	}

	public void Show<T>(AbstractRigComponentDisplay<T> target) where T : RunnerRigComponent
	{
		if (_locked)
		{
			return;
		}
		_target = target;
		_title.text = typeof(T).ToString();

		var mousePosition = Input.mousePosition;
		var viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);

		var pivotX = viewportPosition.x < 0.5 ? 0f : 1f;
		var pivotY = viewportPosition.y < 0.5 ? 0f : 1f;

		_container.pivot = new Vector2(pivotX, pivotY);
		_container.localScale = new Vector2(1f, 1f);
		_container.localPosition = new Vector2(mousePosition.x - Screen.width / 2f, mousePosition.y - Screen.height / 2f);
		_container.localRotation = Quaternion.identity;
		_container.sizeDelta = new Vector2(225, 250);
		_container.gameObject.SetActive(true);
	}

	public void Lock()
	{
		_locked = true;
		_cancelBlocker.SetActive(true);
	}

	public void Cancel()
	{
		_locked = false;
		_cancelBlocker.SetActive(false);
		_cancelled.Invoke();
	}

	public void Hide<T>(AbstractRigComponentDisplay<T> target) where T : RunnerRigComponent
	{
		if ((target as IRigComponentDisplay) != _target)
		{
			return;
		}
		_container.gameObject.SetActive(false);
		_locked = false;
	}
}

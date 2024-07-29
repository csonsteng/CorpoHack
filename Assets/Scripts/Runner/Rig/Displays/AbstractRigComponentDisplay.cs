using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRigComponentDisplay<T> : MonoBehaviour, IRigComponentDisplay where T: RunnerRigComponent
{
	[SerializeField] private GameObject _hoverEffect;

	protected T _data;
	private bool _selected = false;
	private bool _mouseOver = false;

	private void Start()
	{
		RigComponentTooltip.Instance.Register(EditCancelled);
	}

	public void Setup(T data)
	{
		_data = data;
	}

	public void EditCancelled()
	{
		var wasSelected = _selected;

		_selected = false;
		if (_mouseOver)
		{
			OnMouseEnter();
			if (!wasSelected)
			{
				// this means we clicked on this slot when the tooltip was locked elsewhere
				// should change lock focus
				OnMouseUpAsButton();
			}
		} else
		{
			OnMouseExit();
		}
	}
	private void Awake()
	{
		_hoverEffect.SetActive(false);
	}
	private void OnMouseEnter()
	{
		_mouseOver = true;
		if (RigComponentTooltip.Instance.IsLocked)
		{
			return;
		}
		_hoverEffect.SetActive(true);
		RigComponentTooltip.Instance.Show(this);
	}
	private void OnMouseExit()
	{
		_mouseOver = false;
		if (_selected)
		{
			return;
		}
		_hoverEffect.SetActive(false);
		RigComponentTooltip.Instance.Hide(this);
	}

	private void OnMouseUpAsButton()
	{
		_selected = true;
		RigComponentTooltip.Instance.Lock();
	}
}

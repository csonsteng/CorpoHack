using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : AbstractCardManagerComponent
{

	[SerializeField] private GameObject _validTargetIndicator;
	[SerializeField] private GameObject _targetedIndicator;

	private bool _hasCard;
	private bool _hovered;

	private void Start()
	{
		_validTargetIndicator.SetActive(false);
		_targetedIndicator.SetActive(false);

	}

	public void OnCardDragged(Card card)
	{
		// todo: check if valid target for card
		_hasCard = true;
		_validTargetIndicator.SetActive(true);
	}

	public bool OnCardDropped()
	{
		_hasCard = false;
		var wasHovered = _hovered;
		MouseExit();
		_validTargetIndicator.SetActive(false);
		return wasHovered;
	}

	private void Update()
	{
		if (!_hasCard)
		{
			return;
		}

		var hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
		if (hits == null)
		{
			return;
		}
		foreach(var hit in hits)
		{
			if (hit.collider.gameObject != gameObject)
			{
				continue;
			}
			MouseEnter();
			return;
		}
		MouseExit();
	}

	private void MouseEnter()
	{
		_targetedIndicator.SetActive(true);
		_hovered = true;
	}
	private void MouseExit()
	{
		_targetedIndicator.SetActive(false);
		_hovered = false;
	}
}

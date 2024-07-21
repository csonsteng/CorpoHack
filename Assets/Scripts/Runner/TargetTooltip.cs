using Runner.Deck;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTooltip : Singleton<TargetTooltip>
{
	[SerializeField] private RectTransform _container;

	[SerializeField] private TargetTooltipLine _title;
	[SerializeField] private TargetTooltipLine _status;
	[SerializeField] private TargetTooltipLine _color;
	[SerializeField] private TargetTooltipLine _strength;
	[SerializeField] private TargetTooltipLine _type;
	[SerializeField] private TargetTooltipLine _trigger;
	[SerializeField] private TargetTooltipLine _description;

	private RunnerTargetData _target;

	private void Awake()
	{
		_container.gameObject.SetActive(false);
	}

	public void Show(RunnerTargetData target)
	{

		SetupData(target);
		
		var mousePosition = Input.mousePosition;
		var viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);

		var pivotX = viewportPosition.x < 0.5 ? 0f : 1f;
		var pivotY = viewportPosition.y < 0.5 ? 0f : 1f;

		_container.pivot = new Vector2(pivotX, pivotY);
		_container.localScale = new Vector2(1f, 1f);
		_container.localPosition = new Vector2(mousePosition.x - Screen.width /2f, mousePosition.y - Screen.height / 2f);
		_container.localRotation = Quaternion.identity;
		_container.sizeDelta = new Vector2(225, 250);
		_container.gameObject.SetActive(true);
	}

	private void SetupData(RunnerTargetData target)
	{
		_target = target;
		if(_target == null)
		{
			Debug.LogError("null target");
			return;
		}
		SetupLine(_color, _target.Color.ToString());
		SetupLine(_strength, _target.Strength.ToString());
		SetupLine(_type);
		switch (_target.TargetType)
		{
			case RunnerTargetType.Firewall:
				SetupLine(_status, target.IsBroken ? "broken" : "secured");
				SetupLine(_title, "Firewall");
				SetupLine(_trigger);
				SetupLine(_description, "The firewall is composed of two security measures. Once both are broken, the firewall will disable and allow access to other targets.");
				break;
			case RunnerTargetType.ICE:
				SetupLine(_status, target.IsBroken ? "broken" : "secured");
				if (target is not IceData iceData)
				{
					Debug.LogError("Non Ice Data target has Ice target type");
					break;
				}
				SetupLine(_title, iceData.Title());
				SetupLine(_trigger, iceData.Trigger());
				SetupLine(_description, iceData.Description());
				break;
			case RunnerTargetType.Node:
				if (target is not NodeData nodeData)
				{
					Debug.LogError("Non Node Data target has Node target type");
					break;
				}
				SetupLine(_title, nodeData.Title());
				SetupLine(_status, nodeData.Status.ToString());
				SetupLine(_trigger);
				SetupLine(_description, nodeData.Description());
				break;
		}
	}

	private void SetupLine(TargetTooltipLine line, string value = "")
	{
		line.gameObject.SetActive(!string.IsNullOrEmpty(value));
		line.SetLabel(value);
	}

	public void Hide(RunnerTargetData target)
	{
		if (target != _target)
		{
			return;
		}
		_container.gameObject.SetActive(false);
	}
}

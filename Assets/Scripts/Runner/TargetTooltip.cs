using Runner.Deck;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTooltip : MonoBehaviour
{
	public static TargetTooltip Instance => GetInstance();

	private static TargetTooltip GetInstance()
	{
		if (_instance == null)
		{
			_instance = FindObjectOfType<TargetTooltip>();
		}
		return _instance;
	}
	private static TargetTooltip _instance;

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
		SetupLine(_status, "secured");
		SetupLine(_color, _target.Color.ToString());
		SetupLine(_strength, _target.Strength.ToString());
		switch (_target.TargetType)
		{
			case RunnerTargetType.Firewall:
				SetupLine(_title, "Firewall");
				SetupLine(_type);
				SetupLine(_trigger);
				SetupLine(_description, "The firewall is composed of two security measures. Once both are broken, the firewall will disable and allow access to other targets.");
				break;
			case RunnerTargetType.ICE:
				if (target is not IceData iceData)
				{
					Debug.LogError("Non Ice Data target has Ice target type");
					break;
				}
				SetupLine(_title, "ICE");
				SetupLine(_type, iceData.Type());
				SetupLine(_trigger, iceData.Trigger());
				SetupLine(_description, iceData.Description());
				break;
			case RunnerTargetType.Node:
				SetupLine(_title, "Node");
				SetupLine(_type, "unknown");
				SetupLine(_trigger);
				SetupLine(_description, "A node typical stores data. This could be incrimnating data, user data, a crypto wallet, or mundane data.");
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

using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunnerRigComponent : UniqueScriptableObject, IShopItem
{
	public string Name;
	public int Cost;
	[TextArea] public string Description;

	public abstract string ComponentType { get; }
	public abstract string ComponentDescription { get; }

	public string ItemName() => Name;
}

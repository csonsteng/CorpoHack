using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Rig/RAM")]
public class RunnerRigRAM : RunnerRigComponent
{
	public int HandSizeContribution;

	public override string ComponentType => "RAM";
	public override string ComponentDescription => "RAM increases your maximum hand size. It takes up a RAM slot.";
}

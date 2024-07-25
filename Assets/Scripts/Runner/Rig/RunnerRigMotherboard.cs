using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Rig/Motherboard")]
public class RunnerRigMotherboard : RunnerRigComponent
{
	public int RAMSlots;
	public int MaximumProcessorTier;
	public int DriveSlots;
	public int InterfaceSlots;


	public override string ComponentType => "Motherboard";
	public override string ComponentDescription => "Your rig requires exactly one motherboard. " +
		"Different motherboards can support different tiers of processors and have a differing number of available slots.";
}

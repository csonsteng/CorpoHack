using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Rig/Drive")]
public class RunnerRigDrive : AbstractRunnerRigDrive
{
	public override bool IsBackup => false;
	public override string ComponentType => "Drive";
	public override string ComponentDescription => "Drives increases your deck size threshold before impacting hand size. It takes up a drive slot.";
}

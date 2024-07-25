using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Rig/Backup")]
public class RunnerRigBackup : AbstractRunnerRigDrive
{
	public override bool IsBackup => true;
	public override string ComponentType => "Backup Drive";
	public override string ComponentDescription => "A drive that you can easily pull from your rig when the corps inevitably track you down. It takes up a drive slot and contributes to your deck size.";
}

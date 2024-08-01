using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRunnerRigDrive : RunnerRigComponent
{
	public abstract bool IsBackup { get; }
	public int DeckSizeContribution;


	public override string Effect() {
		var text = $"Increases deck size by {DeckSizeContribution}.";
		if (IsBackup)
		{
			text += " Cards are backed up if the rig is destroyed.";
		}
		return text;
	}
}

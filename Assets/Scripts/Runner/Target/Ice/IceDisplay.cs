using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{
    public class IceDisplay : RunnerTargetDisplay
    {
        [SerializeField] private Animator _animator;
		protected override void OnSecurityBroken()
		{
			base.OnSecurityBroken();
			_animator.SetTrigger("Melt");
		}
	}
}

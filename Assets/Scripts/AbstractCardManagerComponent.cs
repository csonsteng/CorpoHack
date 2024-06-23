using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCardManagerComponent : MonoBehaviour
{
    protected CardManager _manager;

    public void SetManager(CardManager manager)
	{
        _manager = manager;
	}
    
}

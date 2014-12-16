using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class GameStateCriteria {
	
	public abstract bool satisfied(Level level);
	
	public virtual string ToString() {
		return this.GetType().ToString();
	}
	
}

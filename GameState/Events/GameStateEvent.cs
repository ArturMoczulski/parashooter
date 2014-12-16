using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateEvent : object
{
	
	protected List<GameStateEventHandler> receivedBy = null;
	
	public List<GameStateEventHandler> ReceivedBy {
		get {
			if( receivedBy == null )
				throw new UnityException("Receivers have not been saved in the event");
			else
				return receivedBy;
		} set {
			receivedBy = value;
		}
	}
	
}

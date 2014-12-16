using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameStateCriteriaSet : CollectionBase {
	
	public bool satisfied(Level level) {
		foreach( GameStateCriteria criteria in List) {
			if( !criteria.satisfied(level) )
				return false;
		}
		return true;
	}
	
	public void Add(GameStateCriteria criteria) {
		List.Add(criteria);
	}
	
	public void Remove(int index) {
		if( index > Count - 1 || index < 0 ) {
			throw new UnityException("Index out of bounds.");
		} else {
			this.List.RemoveAt(index);
		}
	}
	
	public GameStateCriteria Item(int index) {
		return (GameStateCriteria) List[index];
	}
	
	public override string ToString() {
		string[] criteriaStrings = new string[List.Count];
		
		int i = 0;
		foreach( GameStateCriteria criteria in List ) {
			criteriaStrings[i] = criteria.ToString();
			i++;
		}
		
		return String.Join(", ", criteriaStrings);
	}
	
}

using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts{
	public class DebugTesting : MonoBehaviour{
		private MatchingPresenter _matching;
		[ValueDropdown("GetPersonalityID")] public List<string> person1;
		[ValueDropdown("GetPersonalityID")] public List<string> person2;

		private void Start(){
			_matching = GetComponent<MatchingPresenter>();

		}

		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
		[Button]
		public void Test(){
			_matching.MatchMaking(person1, person2);
		}
	}
}
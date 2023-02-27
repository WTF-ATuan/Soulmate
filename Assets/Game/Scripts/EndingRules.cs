using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts{
	[Serializable]
	public class EndingRules{
		public string name;
		[Range(0, 25)]public int generatePoint = 10;
		[ValueDropdown("GetPersonalityID")] public List<string> notContainPersonality;
		[ValueDropdown("GetPersonalityID")] public List<string> containPersonality;


		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}
}
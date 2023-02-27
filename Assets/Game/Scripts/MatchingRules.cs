using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts{
	[Serializable]
	public class MatchingRules{
		public string name;
		[PreviewField] public Sprite image;
		[Range(0, 12)] public int matchLove = 6;
		[ValueDropdown("GetPersonalityID")] public List<string> lastedPersonality;
		[ValueDropdown("GetPersonalityID")] public List<string> bothPersonality;

		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}
}
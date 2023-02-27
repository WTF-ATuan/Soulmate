using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts{
	[System.Serializable]
	public class PersonalityRules{
		public Personality binding;
		[ValueDropdown("GetPersonalityID")] public List<string> disallow;
		[ValueDropdown("GetPersonalityID")] public List<string> attractive;
		[ValueDropdown("GetPersonalityID")] public List<string> repulsive;

		public int CalculateLoveValue(string personalityID){
			var isAttractive = attractive.Contains(personalityID);
			var isRepulsive = repulsive.Contains(personalityID);
			if(isAttractive){
				return 2;
			}

			return isRepulsive ? 0 : binding.loveValue;
		}

		public bool CheckConflict(string personalityID){
			return !disallow.Contains(personalityID);
		}

		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}
}
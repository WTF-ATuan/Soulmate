using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace Game.Scripts{
	[System.Serializable]
	public class PersonalityRule{
		public Personality binding;
		[ValueDropdown("GetPersonalityID")] public List<string> disallow;
		[ValueDropdown("GetPersonalityID")] public List<string> attractive;
		[ValueDropdown("GetPersonalityID")] public List<string> repulsive;

		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}
}
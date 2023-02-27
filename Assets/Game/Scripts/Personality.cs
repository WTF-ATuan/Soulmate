using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace Game.Scripts{
	[Serializable]
	public class Personality{
		[ValueDropdown("GetPersonalityID")] public string id;
		public int loveValue;
		public List<string> name;

		public string GetRandomName()
		{
			return name[Random.Range(0, name.Count)];
		}
		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace Game.Scripts{
	[Serializable]
	public class Personality{
		[ValueDropdown("GetPersonalityID")] public string id;
		public int loveValue;
		public List<string> name;

		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}
}
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts{
	[CreateAssetMenu(fileName = "Data", menuName = "Data/Personality", order = 0)]
	public class PersonalityDataSet : ScriptableObject{
		public List<PersonalityRules> personalityRuleList;
		public List<MatchingRules> matchingRulesList;
	}
}
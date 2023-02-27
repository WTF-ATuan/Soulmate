using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Scripts{
	[CreateAssetMenu(fileName = "Data", menuName = "Data/Personality", order = 0)]
	public class PersonalityDataSet : ScriptableObjectInstaller<PersonalityDataSet>{
		public List<PersonalityRules> personalityRuleList;
		public List<MatchingRules> matchingRulesList;
		public List<EndingRules> endingRulesList;
		public override void InstallBindings(){
			Container.Bind<PersonalityDataSet>().FromInstance(this);
		}

		public PersonalityRules GetBindingData(string binding){
			return personalityRuleList.Find(x =>
					x.binding.id.Equals(binding));
		}
	}
}
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Scripts{
	[CreateAssetMenu(fileName = "Data", menuName = "Data/Personality", order = 0)]
	public class PersonalityDataSet : ScriptableObjectInstaller<PersonalityDataSet>{
		[Searchable] public List<PersonalityRules> personalityRuleList;
		[Searchable] public List<MatchingRules> matchingRulesList;
		[Searchable] public List<EndingRules> endingRulesList;
		public override void InstallBindings(){
			Container.Bind<PersonalityDataSet>().FromInstance(this);
		}

		public PersonalityRules GetBindingData(string binding){
			return personalityRuleList.Find(x =>
					x.binding.id.Equals(binding));
		}
	}
}
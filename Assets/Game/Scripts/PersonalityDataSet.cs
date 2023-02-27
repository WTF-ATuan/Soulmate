using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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

		public MatchingRules GetCloseMatching(List<string> person1, List<string> person2, int lovePoint){
			var combineList = person1.Concat(person2);
			var containList = new List<string>();
			person1.ForEach(x => {
				if(person2.Contains(x)){
					containList.Add(x);
				}
			});
			var matchEndingList = new List<MatchingRules>();
			foreach(var rules in matchingRulesList){
				var isLast = false;
				var isContain = false;
				var isGrater = lovePoint > rules.matchLove;
				foreach(var unused in rules.lastedPersonality
								.Where(last => combineList.Contains(last))){
					isLast = true;
				}

				foreach(var unused in rules.bothPersonality
								.Where(last => containList.Contains(last))){
					isContain = true;
				}

				if(isLast && isContain && isGrater){
					matchEndingList.Add(rules);
				}
			}

			if(matchEndingList.IsNullOrEmpty()) throw new Exception("NotMatching");
			return matchEndingList[0];
		}
	}
}
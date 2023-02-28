using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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
			var combineList = person1.Concat(person2).ToList();
			var containList = new List<string>();
			person1.ForEach(x => {
				if(person2.Contains(x)){
					containList.Add(x);
				}
			});

			var matchEndingList = new List<MatchingRules>();
			foreach(var rules in matchingRulesList){
				var isGrater = lovePoint >= rules.matchLove;
				var isLast = combineList.Intersect(rules.lastedPersonality).Count()
							 >= rules.lastedPersonality.Count;
				var isContain = containList.Intersect(rules.bothPersonality).Count()
								>= rules.bothPersonality.Count;
				if(rules.lastedPersonality.IsNullOrEmpty()){
					isLast = true;
				}

				if(rules.bothPersonality.IsNullOrEmpty()){
					isContain = true;
				}

				if(isLast && isContain && isGrater){
					matchEndingList.Add(rules);
				}
			}

			if(matchEndingList.IsNullOrEmpty()) throw new Exception("NotMatching");
			var orderByLovePoint = matchEndingList.OrderByDescending(x => x.matchLove)
					.ThenByDescending(x => x.lastedPersonality.Count)
					.ThenByDescending(x => x.bothPersonality.Count);

			return orderByLovePoint.First();
		}

		public EndingRules GetCloseEnding(List<EndingResult> results){
			var generatePoint = 0;
			results.ForEach(x => {
				if(x.LovePoint >= x.ResultMatching.generateStandard){
					generatePoint++;
				}
			});
			Debug.Log($"generatePoint = {generatePoint}");
			var allPersonality = new List<string>();
			foreach(var result in results){
				allPersonality.AddRange(result.Person1);
				allPersonality.AddRange(result.Person2);
			}

			var matchEndingList = new List<EndingRules>();
			foreach(var endingRules in endingRulesList){
				var isGreater = generatePoint >= endingRules.generatePoint;
				var isContain = endingRules.containPersonality.Intersect(allPersonality).Count()
								>= endingRules.containPersonality.Count;
				var isNotContain = endingRules.notContainPersonality.Intersect(allPersonality).Count()
								   >= endingRules.notContainPersonality.Count;

				if(isGreater && isContain && isNotContain){
					matchEndingList.Add(endingRules);
				}
			}

			if(matchEndingList.IsNullOrEmpty()) throw new Exception("NotMatching");
			matchEndingList.ForEach(x => Debug.Log(x.name));
			var orderBy = matchEndingList.OrderByDescending(x => x.generatePoint)
					.ThenByDescending(x => x.notContainPersonality.Count)
					.ThenByDescending(x => x.containPersonality.Count);
			return orderBy.First();
		}
	}
}
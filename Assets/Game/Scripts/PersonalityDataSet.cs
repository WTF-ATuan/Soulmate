﻿using System;
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
				var isGrater = lovePoint >= rules.matchLove;
				foreach(var unused in rules.lastedPersonality
								.Where(last => combineList.Contains(last))){
					isLast = true;
				}

				foreach(var unused in rules.bothPersonality
								.Where(last => containList.Contains(last))){
					isContain = true;
				}

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
			return matchEndingList[Random.Range(0, matchEndingList.Count)];
		}

		public EndingRules GetCloseEnding(List<EndingResult> results){
			var generatePoint = 0;
			results.ForEach(x => {
				if(x.ResultMatching.matchLove > x.ResultMatching.generateStandard){
					generatePoint++;
				}
			});
			var allPersonality = new List<string>();
			foreach(var result in results){
				allPersonality.AddRange(result.Person1);
				allPersonality.AddRange(result.Person2);
			}

			var matchEndingList = new List<EndingRules>();
			foreach(var endingRules in endingRulesList){
				var isGreater = generatePoint >= endingRules.generatePoint;
				var isContain = endingRules.containPersonality.Intersect(allPersonality).Count() >=
								endingRules.containPersonality.Count;
				var isNotContain = !endingRules.notContainPersonality.Intersect(allPersonality).Any();

				if(isGreater && isContain && isNotContain){
					matchEndingList.Add(endingRules);
				}
			}

			if(matchEndingList.IsNullOrEmpty()) throw new Exception("NotMatching");
			return matchEndingList[Random.Range(0, matchEndingList.Count)];
		}
	}
}
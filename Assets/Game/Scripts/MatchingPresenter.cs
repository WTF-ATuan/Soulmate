using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts{
	public class MatchingPresenter : MonoBehaviour{
		[Inject] private readonly PersonalityDataSet _dataSet;

		[ValueDropdown("GetPersonalityID")] public List<string> leftPersonData;
		[ValueDropdown("GetPersonalityID")] public List<string> rightPersonData;

		public Image image;
		[ReadOnly] public List<EndingResult> resultList = new List<EndingResult>();

		[Button]
		public void MatchMaking(){
			var rightRules = rightPersonData.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();

			var lovePoint = (from personality in leftPersonData
				from rightRule in rightRules
				select rightRule.CalculateLoveValue(personality)).Sum();
			var matching = _dataSet.GetCloseMatching(leftPersonData, rightPersonData, lovePoint);
			Debug.Log($"matching = {matching.name}");
			image.sprite = matching.image;
			resultList.Add(new EndingResult(leftPersonData, rightPersonData, matching));
		}


		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}

	[Serializable]
	public class EndingResult{
		public List<string> Person1;
		public List<string> Person2;
		public MatchingRules ResultMatching;

		public EndingResult(List<string> person1, List<string> person2, MatchingRules resultMatching){
			Person1 = person1;
			Person2 = person2;
			ResultMatching = resultMatching;
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Scripts{
	public class MatchingPresenter : MonoBehaviour{
		[Inject] private readonly PersonalityDataSet _dataSet;

		[ValueDropdown("GetPersonalityID")] public List<string> leftPersonData;
		[ValueDropdown("GetPersonalityID")] public List<string> rightPersonData;

		[Button]
		public void MatchMaking(){
			var rightRules = rightPersonData.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();

			var lovePoint = (from personality in leftPersonData
				from rightRule in rightRules
				select rightRule.CalculateLoveValue(personality)).Sum();
			Debug.Log($"lovePoint = {lovePoint}");
		}


		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}
}
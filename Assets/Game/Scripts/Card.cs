using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Scripts{
	public class Card : MonoBehaviour{
		[Required] [ValueDropdown("GetPersonalityID")]
		public string cardID;
		public string cardName;
		public string attachOwner;

		[Inject] private readonly PersonalityDataSet _dataSet;
		[Inject] private readonly CardRepository _repository;
		private PersonalityRules _rules;

		private void Start(){
			_rules = _dataSet.GetBindingData(cardID);
			cardName = _rules.binding.name[0];
			attachOwner = transform.parent.name;
			_repository.Add(this);
		}

		public bool CanAddTo(string id){
			return _rules.CheckConflict(id);
		}

		public void SwitchOwner(string ownerName){
			attachOwner = ownerName;
		}

		public bool IsConflict(string id){
			return _rules.CheckConflict(id);
		}

		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Scripts{
	public class Card : MonoBehaviour
	{
		public CardDrag Drag;
		[Required] [ValueDropdown("GetPersonalityID")]
		public string cardID;
		[SerializeField] string cardName;
		public CardsPanelCtrl Panel => GetComponentInParent<CardsPanelCtrl>();

		[Inject] protected readonly PersonalityDataSet _dataSet;
		public PersonalityRules _rules { private set; get; }

		private void Start(){
			_rules = _dataSet.GetBindingData(cardID);
			cardName = _rules.binding.name[0];
			Drag.Setup(() => {
				EventAggregator.Publish(new OnCardDragFinish(this));
				Drag.ResetPosition();
			});
		}
		
		public void SwitchOwner(CardsPanelCtrl ctrl) {
			transform.parent = ctrl.transform;
		}
		private List<ValueDropdownItem> GetPersonalityID(){
			return ShareLibrary.PersonalityIDs
					.Select(personalityID => new ValueDropdownItem(personalityID, personalityID)).ToList();
		}
	}

	public class OnCardDragFinish {
		public Card Card;

		public OnCardDragFinish(Card card) {
			Card = card;
		}
	}
}
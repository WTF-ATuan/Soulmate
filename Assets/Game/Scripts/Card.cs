using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts{
	public class Card : MonoBehaviour
	{
		public CardDrag Drag;
		[Required] [ValueDropdown("GetPersonalityID")]
		public string cardID;
		[SerializeField] Text cardName;
		[SerializeField] GameObject Lock;
		[SerializeField] CardDrag DragCtrl;
		public CardsPanelCtrl Panel => GetComponentInParent<CardsPanelCtrl>();

		protected PersonalityDataSet _dataSet => GameCtrl.Instance._dataSet;
		public PersonalityRules _rules { private set; get; }
		
		private void Start() {
			Drag.Setup(() => {
				if(!Lock.activeSelf)EventAggregator.Publish(new OnCardDragFinish(this));
				Drag.ResetPosition();
			});
		}

		public void Setup(CardData data) {
			_rules = _dataSet.GetBindingData(_dataSet.personalityRuleList[data.IDIndex].binding.id);
			cardName.text = _rules.binding.GetRandomName();
			Lock.SetActive(data.IsLock);
			DragCtrl.enabled = !data.IsLock;
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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.Scripts{
	public class CardController : MonoBehaviour{
		[Inject] private readonly CardRepository _repository;
		[Inject] private readonly PersonalityDataSet _dataSet;

		public void SwitchOwner(Card card, string owner){
			var cardWithOwner = _repository.GetCardWithOwner(owner);
			if(cardWithOwner.Any(ownerCard => ownerCard.IsConflict(card.cardID))){
				return;
			}

			card.SwitchOwner(owner);
		}

		public void SwitchToInventory(Card card){
			card.SwitchOwner("Bottom");
		}

		public List<Card> GenerateCard(int cardCount){
			var selectCard = new List<Card>();
			var orderBy = _dataSet.personalityRuleList.OrderBy(x => Guid.NewGuid());
			return null;
		}
	}
}
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.Scripts{
	public class CardPresenter : MonoBehaviour{
		[Inject] private readonly CardRepository _repository;

		public void SwitchOwner(Card card, string owner){
			var cardWithOwner = _repository.GetCardWithOwner(owner);
			if(cardWithOwner.Any(ownerCard => ownerCard.IsConflict(card.cardID))){
				return;
			}
			card.SwitchOwner(owner);
		}
	}
}
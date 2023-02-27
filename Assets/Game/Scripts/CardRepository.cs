using System.Collections.Generic;

namespace Game.Scripts{
	public class CardRepository{
		private readonly List<Card> _cards = new List<Card>();

		public List<Card> GetCardWithOwner(string owner){
			return _cards.FindAll(x => x.attachOwner.Equals(owner));
		}

		public void Add(Card card){
			_cards.Add(card);
		}
	}
}
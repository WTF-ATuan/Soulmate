using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class SoulPanelCtrl : CardsPanelCtrl {

    public void Setup(SoulData data)
    {
        Clean();
        foreach (var card in data.CardDatas) {
            Add(card);
        }
    }
    
    protected override bool CanCardDragIn(Card c) {
        var _rules = c._rules;
        var list = CardsList;
        if(list.Count>=3) return false;
        foreach (var card in list) {
            if (_rules.CheckConflict(card.cardID)) return false;
        }
        return true;
    }
}

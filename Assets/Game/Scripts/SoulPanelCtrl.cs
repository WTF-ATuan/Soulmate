using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class SoulPanelCtrl : CardsPanelCtrl {
    protected override bool CanCardDragIn(Card c) {
        var _rules = c._rules;
        foreach (var card in CardsList) {
            if (_rules.CheckConflict(card.cardID)) return false;
        }
        return true;
    }
}

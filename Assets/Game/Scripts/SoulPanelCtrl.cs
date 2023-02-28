using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SoulPanelCtrl : CardsPanelCtrl
{
    public Image Head;
    public void Setup(SoulData data)
    {
        Clean();
        foreach (var card in data.CardDatas) {
            Add(card);
        }

        Head.sprite = data.HeadImg;
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

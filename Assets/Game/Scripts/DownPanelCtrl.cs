using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;
using Zenject;

public class DownPanelCtrl : CardsPanelCtrl
{
    protected override bool CanCardDragIn(Card c) {
        if(CardsList.Count>=9) return false;
        return true;
    }
}

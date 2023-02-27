using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;
using Zenject;

public class DownPanelCtrl : CardsPanelCtrl
{
    protected override bool CanCardDragIn(Card c) {
        return true;
    }
}

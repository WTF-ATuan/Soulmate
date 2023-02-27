using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public abstract class CardsPanelCtrl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public List<Card> CardsList => GetComponentsInChildren<Card>().ToList();
    protected abstract bool CanCardDragIn(Card c);
    private void Awake() {
        
    }

    private void Start() {
        EventAggregator.OnEvent<OnCardDragFinish>().Subscribe(e => {
            if (!IsMouseEnter) return;
            if(e.Card.Panel==this) return;
            if (!CanCardDragIn(e.Card)) return;
            e.Card.SwitchOwner(this);
        });
    }

    private bool IsMouseEnter;
    
    public void OnPointerEnter(PointerEventData eventData) {
        IsMouseEnter = true;
  
    }

    public void OnPointerExit(PointerEventData eventData) {
        IsMouseEnter = false;
    }
}

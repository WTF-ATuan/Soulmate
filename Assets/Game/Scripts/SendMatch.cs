using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SendMatch : MonoBehaviour
{
    public List<SoulPanelCtrl> Panels;
    private SelfCompoment<Button> B;

    private void Awake() {
        B.Get().onClick.AddListener(() => {
            EventAggregator.Publish(new OnSendMatch(Panels));
        });
    }
}

public class OnSendMatch
{
    public List<List<string>> Data;

    public OnSendMatch(List<SoulPanelCtrl> list)
    {
        Data = new List<List<string>>();
        foreach (var panel in list) {
            List<string> l = new List<string>();
            foreach (var card in panel.CardsList) {
                l.Add(card.cardID);
            }
            Data.Add(l);
        }
    }
}

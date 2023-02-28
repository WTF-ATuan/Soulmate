using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameCtrl : MonoBehaviour
{
    public static GameCtrl Instance;
    [SerializeField] SoulPanelCtrl LeftSoulPanel,RightSoulPanel;
    [SerializeField] GameObject Card;
    
    [Inject] public readonly PersonalityDataSet _dataSet;


    private int NowIndex;
    private List<MatchData> LevelData;

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        B_GameStart();
        EventAggregator.OnEvent<SendMatch>().Subscribe(enabled =>
        {
            NowIndex += 1;
            Setup();
        });
    }

    public Card GenCard(Transform parent)
    {
        return Instantiate(Card, parent).GetComponent<Card>();
    }
    
    public void B_GameStart() {
        LevelData = CreateGameData();
        NowIndex = 0;
        Setup();
    }

    void Setup()
    {
        MatchData data = LevelData[NowIndex];
        LeftSoulPanel.Setup(data.SoulDatas[0]);
        RightSoulPanel.Setup(data.SoulDatas[1]);
    }
    
    List<MatchData> CreateGameData()
    {
        List<MatchData> data = new List<MatchData>();
        for (int i = 0; i < 20; i++)
        {
            MatchData matchData = new MatchData();
            matchData.SoulDatas = new List<SoulData>();
            for (int j = 0; j < 2; j++)
            {
                var soul = new SoulData();
                List<CardData> CardDatas = new List<CardData>();
                while (CardDatas.Count<3) {
                    var newCard = new CardData(){IDIndex = Random.Range(0,_dataSet.personalityRuleList.Count),IsLock = Random.value>0.5};
                    bool noConflict = true;
                    foreach (var card in CardDatas) {
                        if (_dataSet.personalityRuleList[card.IDIndex]
                            .CheckConflict(_dataSet.personalityRuleList[newCard.IDIndex].binding.id)) {
                            noConflict = false;
                            break;
                        }
                    }
                    if(noConflict) CardDatas.Add(newCard);
                }

                soul.CardDatas = CardDatas;
                soul.Name = "Todo";
                soul.HeadImg = null;
                matchData.SoulDatas.Add(soul);
            }
            data.Add(matchData);
        }

        return data;
    }
    
}

public struct MatchData
{
    public List<SoulData> SoulDatas;
}

public struct SoulData
{
    public List<CardData> CardDatas;
    public Sprite HeadImg;
    public string Name;
}

public struct CardData
{
   public int IDIndex;
   public bool IsLock;
}

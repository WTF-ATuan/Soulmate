using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class GameCtrl : MonoBehaviour
{
    public const int MaxMatch = 20;
    
    public static GameCtrl Instance;
    [SerializeField] SoulPanelCtrl LeftSoulPanel,RightSoulPanel;
    [SerializeField] DownPanelCtrl DownPanelCtrl;
    [SerializeField] GameObject Card;
    [SerializeField] Text Count;
    [SerializeField] List<Sprite> Heads;
    
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
        EventAggregator.OnEvent<OnSendMatch>().Subscribe(enabled =>
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
        for (int i = 0; i < 6; i++) {
            DownPanelCtrl.Add(new CardData(){IDIndex = Random.Range(0,_dataSet.personalityRuleList.Count),IsLock = false});
        }
        NowIndex = 0;
        Setup();
    }

    void Setup()
    {
        if (NowIndex >= LevelData.Count) {
            //todo plantResult
            return;
        }
        MatchData data = LevelData[NowIndex];
        LeftSoulPanel.Setup(data.SoulDatas[0]);
        RightSoulPanel.Setup(data.SoulDatas[1]);
        Count.text = $"匹配進度 {NowIndex+1}/{MaxMatch}";
    }
    
    List<MatchData> CreateGameData()
    {
        List<MatchData> data = new List<MatchData>();
        for (int i = 0; i < MaxMatch; i++)
        {
            MatchData matchData = new MatchData();
            matchData.SoulDatas = new List<SoulData>();
            for (int j = 0; j < 2; j++)
            {
                var soul = new SoulData();
                List<CardData> CardDatas = new List<CardData>();
                int lockIndex = Random.Range(0, 3);
                while (CardDatas.Count<3) {
                    var newCard = new CardData(){IDIndex = Random.Range(0,_dataSet.personalityRuleList.Count),IsLock = lockIndex==CardDatas.Count};
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
                soul.HeadImg = Heads[Random.Range(0,Heads.Count)];
                soul.Name = soul.HeadImg.name;
              
      
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

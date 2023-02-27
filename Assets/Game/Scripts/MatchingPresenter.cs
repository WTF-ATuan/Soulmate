using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts{
	public class MatchingPresenter : MonoBehaviour{
		[Inject] private readonly PersonalityDataSet _dataSet;

		[Required] [SerializeField] private GameObject imageRoot;
		[ReadOnly] public List<EndingResult> resultList = new List<EndingResult>();

		[InlineButton("Test")] public bool debug = false;
		[ShowIf("debug")] public List<string> personA;
		[ShowIf("debug")] public List<string> personB;

		private void Start(){
			EventAggregator.OnEvent<OnSendMatch>().Subscribe(x => {
				var person1 = x.Data[0];
				var person2 = x.Data[1];
				MatchMaking(person1, person2);
			});
			imageRoot.GetComponentsInChildren<Image>(true)[1].OnPointerClickAsObservable().Subscribe(x => { imageRoot.SetActive(false); });
		}

		public void Test(){
			MatchMaking(personA, personB);
		}

		private void MatchMaking(List<string> person1Data, List<string> person2Data){
			var person2 = person2Data.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();
			var person1 = person1Data.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();

			var lovePointA = (from personality in person1Data
				from peron2Rule in person2
				select peron2Rule.CalculateLoveValue(personality)).Sum();

			var lovePointB = (from personality in person2Data
				from peron1Rule in person1
				select peron1Rule.CalculateLoveValue(personality)).Sum();

			var allLovePoint = lovePointA + lovePointB;
			var matching = _dataSet.GetCloseMatching(person1Data, person2Data, allLovePoint);
			resultList.Add(new EndingResult(person1Data, person2Data, matching));
			UpdateUI(matching);
		}

		private void UpdateUI(MatchingRules matching){
			imageRoot.SetActive(true);
			imageRoot.GetComponentsInChildren<Image>(true)[1].sprite = matching.image;
			imageRoot.GetComponentInChildren<Text>(true).text = matching.name;
		}
	}
}
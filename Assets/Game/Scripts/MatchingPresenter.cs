using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts{
	public class MatchingPresenter : MonoBehaviour{
		[Inject] private readonly PersonalityDataSet _dataSet;

		[Required] [SerializeField] private GameObject matchingUI;
		[Required] [SerializeField] private GameObject endingUI;
		[SerializeField] private int resultGoal = 20;
		[ReadOnly] public List<EndingResult> resultList = new List<EndingResult>();

		private void Start(){
			EventAggregator.OnEvent<OnSendMatch>().Subscribe(x => {
				var person1 = x.Data[0];
				var person2 = x.Data[1];
				MatchMaking(person1, person2);
			});
			matchingUI.GetComponentsInChildren<Image>(true)[1].OnPointerClickAsObservable()
					.Subscribe(x => { matchingUI.SetActive(false); });
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
			CalculateResult(person1Data, person2Data, matching);
			UpdateMatchingUI(matching);
		}

		private void CalculateResult(List<string> person1Data, List<string> person2Data, MatchingRules matching){
			resultList.Add(new EndingResult(person1Data, person2Data, matching));
			if(resultList.Count < resultGoal){
				return;
			}

			var closeEnding = _dataSet.GetCloseEnding(resultList);
			UpdateEndingUI(closeEnding);
		}

		private void UpdateMatchingUI(MatchingRules matching){
			if(endingUI.activeSelf) return;
			matchingUI.SetActive(true);
			matchingUI.GetComponentsInChildren<Image>(true)[1].sprite = matching.image;
			matchingUI.GetComponentInChildren<Text>(true).text = matching.name;
		}

		private void UpdateEndingUI(EndingRules ending){
			endingUI.SetActive(true);
			endingUI.GetComponentsInChildren<Image>(true)[1].sprite = ending.image;
			endingUI.GetComponentInChildren<Text>(true).text = ending.name;
		}
	}
}
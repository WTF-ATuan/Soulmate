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

		public void MatchMaking(List<string> person1Data, List<string> person2Data){
			var allLovePoint = CalculateLovePoint(person1Data, person2Data);
			var matching = _dataSet.GetCloseMatching(person1Data, person2Data, allLovePoint);
			resultList.Add(new EndingResult(person1Data, person2Data, matching));
			if(resultList.Count < resultGoal){
				UpdateMatchingUI(allLovePoint, matching);
			}
			else{
				var closeEnding = _dataSet.GetCloseEnding(resultList);
				UpdateEndingUI(closeEnding);
			}
		}

		private int CalculateLovePoint(List<string> person1Data, List<string> person2Data){
			var person1 = person1Data.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();
			var person2 = person2Data.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();

			var lovePoint1 = person1.Sum(rules1 =>
					person2.Select(rules2 => rules1.CalculateLoveValue(rules2.binding.id)).Prepend(0).Max());
			var lovePoint2 = person2.Sum(rules2 =>
					person1.Select(rules1 => rules1.CalculateLoveValue(rules1.binding.id)).Prepend(0).Max());

			return lovePoint1 + lovePoint2;
		}

		private void UpdateMatchingUI(int lovePoint, MatchingRules matching){
			matchingUI.SetActive(true);
			matchingUI.GetComponentsInChildren<Image>(true)[1].sprite = matching.image;
			matchingUI.GetComponentsInChildren<Text>(true)[0].text = matching.name;
			matchingUI.GetComponentsInChildren<Text>(true)[1]
					.text = $"幸福指數 \r {lovePoint}/12";
		}

		private void UpdateEndingUI(EndingRules ending){
			endingUI.SetActive(true);
			endingUI.GetComponentsInChildren<Image>(true)[1].sprite = ending.image;
			endingUI.GetComponentInChildren<Text>(true).text = ending.name;
		}
	}
}
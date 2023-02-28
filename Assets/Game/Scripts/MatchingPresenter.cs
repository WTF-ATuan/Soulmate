using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
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
			endingUI.GetComponentsInChildren<Image>(true)[1].OnPointerClickAsObservable()
					.Subscribe(x => {
						SceneManager.LoadScene("CoreScene");
					});
		}

		public void MatchMaking(List<string> person1Data, List<string> person2Data){
			var allLovePoint = CalculateLovePoint(person1Data, person2Data);
			var matching = _dataSet.GetCloseMatching(person1Data, person2Data, allLovePoint);
			resultList.Add(new EndingResult(person1Data, person2Data, matching, allLovePoint));
			if(resultList.Count < resultGoal){
				UpdateMatchingUI(allLovePoint, matching);
			}
			else{
				var closeEnding = _dataSet.GetCloseEnding(resultList);
				PlayerPrefs.SetInt(closeEnding.name, 1);
				UpdateEndingUI(closeEnding);
			}
		}

		private int CalculateLovePoint(List<string> person1Data, List<string> person2Data){
			var person1 = person1Data.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();
			var person2 = person2Data.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();

			var lovePoint1 = 0;
			foreach(var rules1 in person1){
				var point = rules1.binding.loveValue;
				foreach(var rules2 in person2){
					var loveValue = rules2.CalculateLoveValue(rules1.binding.id);
					if(loveValue == 0){
						point = 0;
						break;
					}

					if(loveValue > point){
						point = loveValue;
					}
				}

				lovePoint1 += point;
			}

			var lovePoint2 = 0;
			foreach(var rules2 in person2){
				var point = rules2.binding.loveValue;
				foreach(var rules1 in person1){
					var loveValue = rules1.CalculateLoveValue(rules2.binding.id);
					if(loveValue == 0){
						point = 0;
						break;
					}

					if(loveValue > point){
						point = loveValue;
					}
				}

				lovePoint2 += point;
			}

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
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts{
	public class MatchingPresenter : MonoBehaviour{
		[Inject] private readonly PersonalityDataSet _dataSet;

		public Image image;
		[ReadOnly] public List<EndingResult> resultList = new List<EndingResult>();

		private void Start(){
			EventAggregator.OnEvent<OnSendMatch>().Subscribe(x => {
				var person1 = x.Data[0];
				var person2 = x.Data[1];
				MatchMaking(person1, person2);
			});
		}

		[Button]
		public void MatchMaking(List<string> person1Data, List<string> person2Data){
			var rightRules = person2Data.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();

			var lovePoint = (from personality in person1Data
				from rightRule in rightRules
				select rightRule.CalculateLoveValue(personality)).Sum();
			var matching = _dataSet.GetCloseMatching(person1Data, person2Data, lovePoint);
			Debug.Log($"matching = {matching.name}");
			image.transform.parent.gameObject.SetActive(true);
			image.sprite = matching.image;
			resultList.Add(new EndingResult(person1Data, person2Data, matching));
		}
	}
}
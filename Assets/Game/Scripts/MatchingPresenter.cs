using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts{
	public class MatchingPresenter : MonoBehaviour{
		[Inject] private readonly PersonalityDataSet _dataSet;

		public Image image;
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
		}

		public void Test(){
			MatchMaking(personA , personB);
		}

		public void MatchMaking(List<string> person1Data, List<string> person2Data){
			if(person1Data.IsNullOrEmpty() || person2Data.IsNullOrEmpty()){
				return;
			}

			var person2 = person2Data.Select(personality
					=> _dataSet.GetBindingData(personality)).ToList();

			var lovePoint = (from personality in person1Data
				from peron2Rule in person2
				select peron2Rule.CalculateLoveValue(personality)).Sum();
			Debug.Log($"lovePoint = {lovePoint}");
			var matching = _dataSet.GetCloseMatching(person1Data, person2Data, lovePoint);
			Debug.Log($"matching = {matching.name}");
			image.transform.parent.gameObject.SetActive(true);
			image.sprite = matching.image;
			resultList.Add(new EndingResult(person1Data, person2Data, matching));
		}
	}
}
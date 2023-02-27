using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts{
	public class CardPresenter : MonoBehaviour{
		[Required] public List<LayoutGroup> inventoryList;

		private void Start(){
			foreach(var layoutGroup in inventoryList){
				layoutGroup.OnDropAsObservable().Subscribe(x => DropHandler(x, layoutGroup));
			}
		}

		private void DropHandler(PointerEventData eventData, LayoutGroup layout){
			var draggingObj = eventData.pointerDrag;
			draggingObj.transform.SetParent(layout.transform, false);
			draggingObj.GetComponent<CardDrag>().ResetPosition();
			layout.CalculateLayoutInputHorizontal();
			layout.CalculateLayoutInputVertical();
			layout.SetLayoutHorizontal();
			layout.SetLayoutVertical();
		}
		
	}
}
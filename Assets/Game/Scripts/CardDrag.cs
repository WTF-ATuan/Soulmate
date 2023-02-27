using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler{
	private Vector3 _basePos;
	private Vector3 _vec;
	private CanvasGroup _canvasGroup;

	private void Awake(){
		_basePos = transform.localPosition;
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private const float Smooth = 0.1f;

	private void Update(){
		if(_draging) return;
		transform.localPosition = (transform.localPosition - _basePos).magnitude < 0.1f
				? _basePos
				: Vector3.SmoothDamp(transform.localPosition, _basePos, ref _vec, Smooth);
	}

	private bool _draging;
	private Vector2 _deviation;

	public void OnDrag(PointerEventData eventData){
		transform.localPosition = eventData.position + _deviation;
	}

	public void OnBeginDrag(PointerEventData eventData){
		_draging = true;
		_deviation = (Vector2)transform.localPosition - eventData.position;
		_canvasGroup.blocksRaycasts = false;
		_canvasGroup.alpha = 0.7f;
	}

	public void OnEndDrag(PointerEventData eventData){
		_draging = false;
		_canvasGroup.blocksRaycasts = true;
		_canvasGroup.alpha = 1f;
	}

	public void ResetPosition(){
		_basePos = transform.localPosition;
	}
}
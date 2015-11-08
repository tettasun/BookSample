using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/**
*	長押しイベントを受け取るハンドラ
*
*/
[RequireComponent(typeof(Graphic))]
[System.Serializable]
public class LongPressEventHandler : MonoBehaviour,
									 IPointerEnterHandler,
									 IPointerUpHandler,
									 IPointerExitHandler,
									 IPointerDownHandler,
									 IPointerClickHandler 
{
	//長押しイベントをInspectorから外部に渡せるようにするためのUnityEvent
	[Serializable]
	public class LongPressEvent : UnityEvent{};
	public LongPressEvent onLongPress = new LongPressEvent();
	
	//
	// プロパティ
	//
	//長押しと判定するまでの時間
	public float threshold = 0.6f;
	//長押し中にどの程度指のズレを許容するか
	public float tapgap = 1f;

	//長押しキャンセルフラグ	
	private bool _canceled = false;
	//押されている時間
	private float _pressingTime = 0f;
	//押し始めた座標
	private Vector3 _startTapPoint = Vector3.zero;
	
	//オブジェクト上でポインタが押されたとき呼ばれる
	public void OnPointerDown(PointerEventData data)
	{
		Debug.Log("OnPointerDown");
		//押し始めの座標を保存しておく
		_startTapPoint = this.transform.position;
		//長押しの監視を始める
		StartCoroutine(_WatchTap());
	}

	//ポインタが離されたとき呼ばれる
	public void OnPointerUp(PointerEventData data)
	{
		Debug.Log("OnPointerUp");
		_canceled = true;
	}
	
	//オブジェクト上でクリック(押して、離す)されたとき呼ばれる
	public void OnPointerClick(PointerEventData data)
	{
		Debug.Log("OnPointerClick");
		
		if (_pressingTime > threshold)
		{
			return;
		}
		
		//指が動いたか
		if (isMoved() == true)
		{
			_canceled = true;
			return;
		}
		
		_canceled = true;
	}
	
	//オブジェクトの上にポインタが来たとき呼ばれる
	public void OnPointerEnter(PointerEventData data)
	{
		Debug.Log("OnPointerEnter");
	}
	
	//オブジェクト上からポインタが離れたとき呼ばれる
	public void OnPointerExit(PointerEventData data)
	{
		Debug.Log("OnPointerExit");
		_canceled = true;
	}
	
	//長押しの監視
	private IEnumerator _WatchTap()
	{
		_pressingTime = 0;
		_canceled = false;
		
		while (true)
		{
			//押されている時間を更新する
			_pressingTime += Time.deltaTime;
			
			//キャンセルや
			if (_canceled == true)
			{
				break;
			}
			
			//指が動いたら終了
			if (isMoved() == true)
			{
				Debug.Log("LongPress Canceled:" + this.gameObject.name);
				break;
			}
			
			//閾値を超えたらLongPressイベントを発行
			if (_pressingTime >= threshold)
			{
				Debug.Log ("LongPressed!");
				if (onLongPress != null)
				{
					onLongPress.Invoke();
				}
				break;
				
			}
			
			yield return 0;
		}
	}
	
	//指が動いたか
	private bool isMoved()
	{
		if (Mathf.Abs(this.transform.position.x - _startTapPoint.x) >= tapgap)
		{
			return true;
		}
		if (Mathf.Abs(this.transform.position.y - _startTapPoint.y) >= tapgap)
		{
			return true;
		}
		
		return false;
	}
	
	private void OnDisable()
	{
		_canceled = true;
	}
}

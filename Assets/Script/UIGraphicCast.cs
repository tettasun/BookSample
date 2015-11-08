using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.UI
{
	/**
	*	タップ領域の拡張
	*/
	public class UIGraphicCast : Graphic
	{
		//x軸の拡張割合
		public float x = 1.2f;
		//y軸の拡張割合
		public float y = 1.6f;
		
		protected override void OnPopulateMesh (Mesh m)
		{
			base.OnPopulateMesh (m);
			m.Clear();
		}
		
		#if UNITY_EDITOR
		[CustomEditor(typeof(UIGraphicCast))]
		class UIGraphicCastEditor : Editor
		{
			public override void OnInspectorGUI() {
				
				UIGraphicCast obj = target as UIGraphicCast;
				RectTransform parent = obj.transform.parent as RectTransform;
				obj.transform.localPosition = Vector3.zero;
				
				obj.x = EditorGUILayout.FloatField("X", obj.x);
				obj.y = EditorGUILayout.FloatField("Y", obj.y);
				
				obj.rectTransform.sizeDelta = new Vector2(parent.sizeDelta.x * obj.x, parent.sizeDelta.y * obj.y);
			}
		}
		
		#endif
	}
}

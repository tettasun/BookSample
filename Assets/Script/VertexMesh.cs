using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/**
*	UIの頂点加工メッシュ
*	Inspectorから頂点を変更することで変形
*
*/
[RequireComponent (typeof(Graphic))]
public class VertexMesh : BaseMeshEffect {

	//変更する頂点の位置情報	
	public Vector3[] myVertex = new Vector3[6];


#if UNITY_5_2
	//Mesh情報の受け取り
	public override void ModifyMesh(Mesh mesh)
	{
		if (!this.IsActive())
			return;
		
		List<UIVertex> list = new List<UIVertex>();
		using (VertexHelper vertexHelper = new VertexHelper(mesh))
		{
			vertexHelper.GetUIVertexStream(list);
		}
		
		ModifyVertices(list);
		
		using (VertexHelper vertexHelper2 = new VertexHelper())
		{
			vertexHelper2.AddUIVertexTriangleStream(list);
			vertexHelper2.FillMesh(mesh);
		}
	}
#endif

    public override void ModifyMesh(VertexHelper vertexHelper)
    {
        List<UIVertex> vertexList = new List<UIVertex>();
        vertexHelper.GetUIVertexStream(vertexList);
        ModifyVertices(vertexList);
        vertexHelper.Clear();
        vertexHelper.AddUIVertexTriangleStream(vertexList);
    }
	
	//頂点情報の加工
	public void ModifyVertices(List<UIVertex> vList){
		if (IsActive() == false || vList == null || vList.Count == 0)
		{
			return;
		}

		for (int i = 0; i < vList.Count; i++) {
			UIVertex tmpV = vList[i];
			tmpV.position = myVertex[i];
			vList[i] = tmpV;
		}
	}

//	private void FixedUpdate()
//	{
//		for (int i = 0; i < myVertex.Length; i++) {
//			if (i == 1 || i == 4){
//			float x =  myVertex[i].x + Random.Range(-1f,1f);
//			float y =  myVertex[i].y + Random.Range(-1f,1f);
//			myVertex[i] = new Vector3(x,y);
//			}
//		}
//		this.graphic.SetAllDirty ();
//	}
}
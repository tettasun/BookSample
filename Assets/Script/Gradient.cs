/**
 * for Unity5.2 upper 
 * 
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent (typeof(Graphic))]
public class Gradient : BaseMeshEffect
{
	public enum DIRECTION
	{
		Vertical,
		Horizontal
	}
	
	public DIRECTION direction = DIRECTION.Vertical;
	public Color color1 = Color.white;
	public Color color2 = Color.black;
	Graphic graphic;
   
#if UNITY_5_2
	public override void ModifyMesh(Mesh mesh)
	{
		if (!this.IsActive())
			return;
		
		List<UIVertex> list = new List<UIVertex>();
		using (VertexHelper vertexHelper = new VertexHelper(mesh))
		{
			vertexHelper.GetUIVertexStream(list);
		}
		
		ModifyVertices(list);  // calls the old ModifyVertices which was used on pre 5.2
		
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

	public void ModifyVertices(List<UIVertex> vList){
		if (IsActive() == false || vList == null || vList.Count == 0)
		{
			return;
		}
		
		float topX = 0f, topY = 0f, bottomX = 0f, bottomY = 0f;
		foreach (var vertex in vList)
		{
			topX = Mathf.Max(topX, vertex.position.x);
			topY = Mathf.Max(topY, vertex.position.y);
			bottomX = Mathf.Min(bottomX, vertex.position.x);
			bottomY = Mathf.Min(bottomY, vertex.position.y);
		}
		float width = topX - bottomX;
		float height = topY - bottomY;
		
		UIVertex tempVertex = vList [0];
		for (int i = 0; i < vList.Count; i++)
		{
			tempVertex = vList [i];
			Color colorOrg = tempVertex.color;
			Color colorV = Color.Lerp(color2, color1, (tempVertex.position.y - bottomY) / height);
			Color colorH = Color.Lerp(color1, color2, (tempVertex.position.x - bottomX) / width);
			switch (direction)
			{
			case DIRECTION.Vertical:
				tempVertex.color = colorOrg * colorV;
				break;
			case DIRECTION.Horizontal:
				tempVertex.color = colorOrg * colorH;
				break;
			}
			vList [i] = tempVertex;
		}
	}

	/// <summary>
	/// Refresh Gradient Color on playing.
	/// </summary>
	public void Refresh()
	{
		if (graphic == null)
		{
			graphic = GetComponent<Graphic>();
		}
		if (graphic != null)
		{
			graphic.SetVerticesDirty();
		}
	}
}
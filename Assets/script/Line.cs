using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
	public Vector3 Start;
	public Vector3 End;
	LineRenderer _renderer;

	public float Length => Vector3.Distance(Start, End);
	public Vector3 Direction => End - Start;
	public Vector3 Normal => Direction.normalized;

	public Line(Vector3 start, Vector3 end)
    {
		Start = start;
		End = end;

		var rendererPrefab = Resources.Load<GameObject>("Prefabs/LRender");
		var rendererGO = GameObject.Instantiate(rendererPrefab);
		rendererGO.transform.parent = GameObject.Find("LineRenderers").transform;
		_renderer = rendererGO.GetComponent<LineRenderer>();

		_renderer.positionCount = 2;
		_renderer.SetPosition(0, Start);
		_renderer.SetPosition(1, End);
		_renderer.startWidth = 0.1f;
		_renderer.endWidth = 0.1f;
    }

	
}

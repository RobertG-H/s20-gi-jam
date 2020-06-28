using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseHandler : MonoBehaviour
{
	public HoseInputManager hoseInput;
	public Transform baseTrans;

	public Transform handObject;
	public Transform nozzleObject;

	public HoseLevel activeLevel;



	List<PosNorm> listofPoints = new List<PosNorm>();
	float offsetdistance = .735f;
	float additionadistoffset = 0.1f;

	struct PosNorm
	{
		public Vector3 pos;
		public Vector3 norm;
		public Collider2D col2d;
	}

	public LineRenderer line;

	Vector2 v2(Vector3 v3)
	{
		return new Vector2(v3.x, v3.y);
	}

	Vector3 v3(Vector2 v2)
	{
		return new Vector3(v2.x, v2.y,0f);
	}


	void DrawHitMarker(Vector3 wp, Color col, float size= 0.5f, float delay = 0f)
	{
		Debug.DrawLine(wp- Vector3.up * size, wp + Vector3.up* size, col, delay);
		Debug.DrawLine(wp - Vector3.right * size, wp + Vector3.right * size,col, delay);
	}

	void DrawLineChain(Color col, Vector3 mouseWS)
	{
		for (int i = 0; i <= listofPoints.Count; i++)
		{
			Vector3 v1;
			if (i == 0)
				v1 = baseTrans.position;
			else
				v1 = listofPoints[i - 1].pos;

			Vector3 v2;
			if (i < listofPoints.Count)
				v2 = listofPoints[i].pos;
			else
				v2 = mouseWS;

			Debug.DrawLine(v1, v2, col);
		}
	}


	Vector3 GetLastPosition(int index)
	{
		if (index > 0)
			return listofPoints[index - 1].pos;
		else
			return baseTrans.position;
	}


	Vector3 GetNextPosition(int index, Vector3 mouseWS)
	{
		if (index < listofPoints.Count - 2)
			return listofPoints[index + 1].pos;
		else
			return mouseWS;
	}


	List<CircleCollider2D> GetAllClose(Vector3 center, float radius)
	{
		List<CircleCollider2D> closeCols = new List<CircleCollider2D>();
		foreach(CircleCollider2D c in activeLevel.nodes)
		{
			if((c.transform.position - center).magnitude < radius+additionadistoffset)
				closeCols.Add(c);
		}
		return closeCols;
	}

	public void CastToMouseRange(int index, Vector3 LA, Vector3 L1B, Vector3 L2B)
	{
		float circRad;
		Vector3 circCent = HoseUtils.CircleCenter(LA, L1B, L2B, out circRad);
		if(circRad == 0f)
		{
			Debug.Log("circl=0");
			return;
		}

		List<CircleCollider2D> closeColliders = GetAllClose(circCent, circRad);
		if (closeColliders.Count == 0)
		{
			//Debug.Log("no close " + circCent + " rad: " + circRad);
			return;
		}

		List<CircleCollider2D> intersectedColliders = new List<CircleCollider2D>();

		float Ang12 = Vector3.Angle((L1B - LA).normalized, (L2B - LA).normalized);

		Vector3 L1 = L1B - LA;
		Ray ray1 = new Ray(LA, L1.normalized);
		Vector3 L2 = L2B - LA;
		Ray ray2 = new Ray(LA, L2.normalized);
		float D1 = L1.magnitude;
		float D2 = L2.magnitude;

		Vector3 Lab = L2B - L1B;
		Ray raylab = new Ray(L1B, Lab.normalized);
		float Dab = Lab.magnitude;

		RaycastHit2D[] r1hits = Physics2D.RaycastAll(LA, L1.normalized, D1);
		RaycastHit2D[] r2hits = Physics2D.RaycastAll(LA, L2.normalized, D2);

		foreach (CircleCollider2D c in closeColliders)
		{
			if (HoseUtils.PointInTriangle(c.transform.position, LA, L1B, L2B))
			{
				intersectedColliders.Add(c);
			}
			else
			{
				bool didHit = false;

				foreach (RaycastHit2D rhit2d in r1hits)
					if (!didHit)
						didHit = rhit2d.collider.Equals(c);
				if(!didHit)
					foreach (RaycastHit2D rhit2d in r2hits)
						if (!didHit)
							didHit = rhit2d.collider.Equals(c);

				if(didHit)
					intersectedColliders.Add(c);
			}
		}

		if (intersectedColliders.Count == 0)
		{
			//Debug.LogWarning("no intersects");
			return;
		}

		List<float> angleDeltas = new List<float>();
		List<PosNorm> intersectedPoints= new List<PosNorm>();
		RaycastHit2D[] rABhits = Physics2D.RaycastAll(L1B, Lab.normalized, Dab);


		for (int i=0; i < intersectedColliders.Count; i++)
		{
			Vector2 tanA2d, tanB2d;
			HoseUtils.FindTangents(v2(intersectedColliders[i].transform.position), intersectedColliders[i].radius, v2(LA), out tanA2d, out tanB2d);
			float angleA = Vector3.Angle((v3(tanA2d)-LA).normalized, L1.normalized);
			float angleB = Vector3.Angle((v3(tanB2d)-LA).normalized, L1.normalized);
			this.DrawHitMarker(tanA2d, Color.cyan);
			this.DrawHitMarker(tanB2d, Color.blue);

			float promisingAngle = -1;
			Vector3 promisingTangent = Vector3.zero;
			Vector3 promisingNorm;

			if(angleA< angleB && HoseUtils.PointInTriangle(tanA2d, LA, L1B,L2B))
			{
				promisingAngle = angleA;
				promisingTangent = v3(tanA2d);
			}
			else if (HoseUtils.PointInTriangle(tanB2d, LA, L1B, L2B))
			{
				promisingAngle = angleB;
				promisingTangent = v3(tanB2d);
			}


			if (promisingAngle>0)
			{
				angleDeltas.Add(promisingAngle);
				promisingNorm = (intersectedColliders[i].transform.position - promisingTangent);

				PosNorm p = new PosNorm
				{
					pos = promisingTangent,
					norm = promisingNorm,
					col2d = intersectedColliders[i]
				};

				this.DrawHitMarker(p.pos, Color.green, 0.7f, 1f);
				intersectedPoints.Add(p);
			}
			else
			{
				bool keepsearching = true;
				foreach (RaycastHit2D rayhit in rABhits)
				{
					if(keepsearching)
						if(rayhit.collider.Equals(intersectedColliders[i]))
						{
							keepsearching = false;
							Vector3 rayhitpoint = v3(rayhit.point);

							PosNorm p = new PosNorm
							{
								pos = rayhitpoint,
								norm = (rayhit.collider.transform.position - rayhitpoint),
								col2d = intersectedColliders[i]
							};

							intersectedPoints.Add(p);
							Vector3 LRh = rayhitpoint - LA;
							angleDeltas.Add(Vector3.Angle(LRh, L1.normalized));
						}
				}

				if(keepsearching)
				{
					foreach(RaycastHit2D rayhit in r2hits)
						if(keepsearching)
							if (rayhit.collider.Equals(intersectedColliders[i]))
							{
								keepsearching = false;
								Vector3 rayhitpoint = v3(rayhit.point);

								PosNorm p = new PosNorm
								{
									pos = rayhitpoint,
									norm = (rayhit.collider.transform.position - rayhitpoint),
									col2d = intersectedColliders[i]
								};

								intersectedPoints.Add(p);
								angleDeltas.Add(Vector3.Angle(L2.normalized, L1.normalized));
							}
				}
			}
		}

		float lowestAngle = float.MaxValue;
		PosNorm closestPoint = new PosNorm();
		for(int i =0; i < angleDeltas.Count;i++)
		{
			if(angleDeltas[i] < lowestAngle)
			{
				lowestAngle = angleDeltas[i];
				closestPoint = intersectedPoints[i];
			}
		}

		if(lowestAngle < float.MaxValue && lowestAngle > 0f)
		{
			AddPoint(closestPoint, index);
			float anglepercentage = lowestAngle / Ang12;
			Vector3 newL1B = Vector3.Lerp(L1B, L2B, anglepercentage);
			CastToMouseRange(index + 1, listofPoints[index].pos, newL1B, L2B);
		}
	}







	public void CastToMouse(Vector3 mousWS)
	{
		//Transform recentTrans = listofPoints[listofPoints.Count - 1];
		Vector3 currentPos;
		if (listofPoints.Count > 0)
			currentPos = listofPoints[listofPoints.Count - 1].pos;
		else
			currentPos = baseTrans.position;


		float length = ( mousWS - currentPos).magnitude;

		RaycastHit2D rayhit = Physics2D.Raycast(v2(currentPos), v2(mousWS - currentPos), length);

		if (rayhit.collider != null)
		{
			Vector3 diff = rayhit.transform.position - v3(rayhit.point);
			Vector3 dir = diff.normalized;
			float dist = diff.magnitude;

			Vector3 newPos = rayhit.transform.position - dir * offsetdistance;

			AddPoint(newPos, dir, rayhit.collider);
		}
	}

	void TearOff(int index, Vector3 mouseWS)
	{
		if(listofPoints.Count > 0)
		{
			PosNorm curPosNorm = listofPoints[index];
			Vector3 nextPoint = GetNextPosition(index, mouseWS);
			Vector3 nextDir = (nextPoint - curPosNorm.pos).normalized;



			Vector3 lastPoint = GetLastPosition(index);
			Vector3 lastDir = (lastPoint -curPosNorm.pos).normalized;

			float angleLstNrm = Vector3.Angle(curPosNorm.norm, lastDir);
			float angleNrmNxt = Vector3.Angle(curPosNorm.norm, nextDir);
			float angleLstNxt = Vector3.Angle(lastDir, nextDir);

			float combAngles = angleLstNrm + angleNrmNxt;


			if(Mathf.Round(combAngles) != Mathf.Round(angleLstNxt))
			{
				RemovePoint(index);
			}
		}
	}



	void AddPoint(Vector3 point, Vector3 normal, Collider2D col, int index = -1)
	{
		PosNorm p = new PosNorm
		{
			pos = point,
			norm = normal,
			col2d = col
		};

		AddPoint(p, index);
	}



	void AddPoint(PosNorm p, int index = -1)
	{
		p.pos = p.pos - p.norm.normalized * additionadistoffset;

		if (index < 0 || index >= listofPoints.Count-1)
			listofPoints.Add(p);
		else
			listofPoints.Insert(index,p);
	}

	void RemovePoint(int index)
	{
		listofPoints.RemoveAt(index);
	}


	Vector3 UpdateHand(Vector3 mouse3)
	{
		handObject.position = mouse3;
		return nozzleObject.position;
	}

	Vector3 UpdateHandNotDragging()
	{
		if (listofPoints.Count > 0)
		{
			handObject.position = listofPoints[listofPoints.Count - 1].pos;
			if ((handObject.position - nozzleObject.position).magnitude < 0.3f)
			{
				if (listofPoints.Count > 0)
					this.RemovePoint(listofPoints.Count - 1);
			}
		}
		else
			handObject.position = baseTrans.position;


		return nozzleObject.position;
	}


	void UpdateLineRenderer(Vector3 mousWS)
	{
		List<Vector3> pointToLine = new List<Vector3>();
		pointToLine.Add(baseTrans.position);
		for (int i = 0; i < listofPoints.Count; i++)
			pointToLine.Add(listofPoints[i].pos);

		pointToLine.Add(mousWS);
		line.positionCount = pointToLine.Count;
		line.SetPositions(pointToLine.ToArray());
	}


	void CheckForCuts()
	{
		int cutIndex = int.MaxValue;
		for(int i =0; i < listofPoints.Count; i++)
		{
			if(listofPoints[i].col2d.tag == "Cuts")
			{
				cutIndex = i;
				break;
			}
		}

		if(cutIndex < int.MaxValue)
		{
			Vector3 startingPoint = GetLastPosition(cutIndex);

			isDragging = false;
			handObject.position = startingPoint;
			nozzleObject.position = listofPoints[cutIndex].pos;
			
			for (int i = listofPoints.Count-1; i >=cutIndex; i--)
			{
				RemovePoint(i);
			}
		}
	}

	Vector3 lastWp = Vector3.one*float.MaxValue;

	public bool isDragging = false;
	// Update is called once per frame
	void Update()
    {
		Vector3 mp = hoseInput.mousePos;
		Vector3 wp = Camera.main.ScreenToWorldPoint(mp);
		wp.z = 0f;

		if(hoseInput.leftClick)
		{
			float distToNozzle = (nozzleObject.position - wp).magnitude;
			if (distToNozzle < offsetdistance)
				isDragging = true;
		}

		if (!hoseInput.leftdown)
			isDragging = false;


		if (isDragging)
		{
			wp = UpdateHand(wp);

			for (int i = listofPoints.Count - 1; i >= 0; i--)
			{
				TearOff(i, wp);
			}

			//CastToMouse(wp);
			if (lastWp.x < float.MaxValue)
			{
				Vector3 lastpos = GetLastPosition(listofPoints.Count);
				CastToMouseRange(listofPoints.Count, lastpos, lastWp, wp);
			}

			CheckForCuts();
		}
		else
			wp = UpdateHandNotDragging();

		UpdateLineRenderer(wp);
		lastWp = wp;
	}

}

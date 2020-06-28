using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseUtils : MonoBehaviour
{
	/*public static Vector3 CircleCenter(Vector3 A, Vector3 B, Vector3 C)
	{
		float yDelta_a = B.y - A.y;
		float xDelta_a = B.x - A.x;
		float yDelta_b = C.y - B.y;
		float xDelta_b = C.x - B.x;
		Vector3 center = new Vector3(0, 0,0);

		float aSlope = yDelta_a / xDelta_a;
		float bSlope = yDelta_b / xDelta_b;
		center.x = (aSlope * bSlope * (A.y - C.y) + bSlope * (A.x + B.x)
			- aSlope * (B.x + C.x)) / (2f * (bSlope - aSlope));
		center.y = -1 * (center.x - (A.x + B.x) / 2f) / aSlope + (A.y + B.y) / 2f;

		return center;
	}*/



	public static Vector3 CircleCenter(Vector3 A, Vector3 B, Vector3 C, out float radius)
	{
		Vector2 center;
		FindCircle(new Vector2(A.x, A.y), new Vector2(B.x, B.y), new Vector2(C.x, C.y), out center, out radius);
		return center;
	}

	private static void FindIntersection(
	Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
	out bool lines_intersect, out bool segments_intersect,
	out Vector2 intersection,
	out Vector2 close_p1, out Vector2 close_p2)
	{
		// Get the segments' parameters.
		float dx12 = p2.x - p1.x;
		float dy12 = p2.y - p1.y;
		float dx34 = p4.x - p3.x;
		float dy34 = p4.y - p3.y;

		// Solve for t1 and t2
		float denominator = (dy12 * dx34 - dx12 * dy34);

		float t1 =
			((p1.x - p3.x) * dy34 + (p3.y - p1.y) * dx34)
				/ denominator;
		if (float.IsInfinity(t1))
		{
			// The lines are parallel (or close enough to it).
			lines_intersect = false;
			segments_intersect = false;
			intersection = new Vector2(float.NaN, float.NaN);
			close_p1 = new Vector2(float.NaN, float.NaN);
			close_p2 = new Vector2(float.NaN, float.NaN);
			return;
		}
		lines_intersect = true;

		float t2 =
			((p3.x - p1.x) * dy12 + (p1.y - p3.y) * dx12)
				/ -denominator;

		// Find the point of intersection.
		intersection = new Vector2(p1.x + dx12 * t1, p1.y + dy12 * t1);

		// The segments intersect if t1 and t2 are between 0 and 1.
		segments_intersect =
			((t1 >= 0) && (t1 <= 1) &&
			 (t2 >= 0) && (t2 <= 1));

		// Find the closest points on the segments.
		if (t1 < 0)
		{
			t1 = 0;
		}
		else if (t1 > 1)
		{
			t1 = 1;
		}

		if (t2 < 0)
		{
			t2 = 0;
		}
		else if (t2 > 1)
		{
			t2 = 1;
		}

		close_p1 = new Vector2(p1.x + dx12 * t1, p1.y + dy12 * t1);
		close_p2 = new Vector2(p3.x + dx34 * t2, p3.y + dy34 * t2);
	}


	private static void FindCircle(Vector2 a, Vector2 b, Vector2 c,
	out Vector2 center, out float radius)
	{
		// Get the perpendicular bisector of (x1, y1) and (x2, y2).
		float x1 = (b.x + a.x) / 2;
		float y1 = (b.y + a.y) / 2;
		float dy1 = b.x - a.x;
		float dx1 = -(b.y - a.y);

		// Get the perpendicular bisector of (x2, y2) and (x3, y3).
		float x2 = (c.x + b.x) / 2;
		float y2 = (c.y + b.y) / 2;
		float dy2 = c.x - b.x;
		float dx2 = -(c.y - b.y);

		// See where the lines intersect.
		bool lines_intersect, segments_intersect;
		Vector2 intersection, close1, close2;
		FindIntersection(
			new Vector2(x1, y1), new Vector2(x1 + dx1, y1 + dy1),
			new Vector2(x2, y2), new Vector2(x2 + dx2, y2 + dy2),
			out lines_intersect, out segments_intersect,
			out intersection, out close1, out close2);
		if (!lines_intersect)
		{
			center = new Vector2(0, 0);
			radius = 0;
		}
		else
		{
			center = intersection;
			float dx = center.x - a.x;
			float dy = center.y - a.y;
			radius = (float)Mathf.Sqrt(dx * dx + dy * dy);
		}
	}
















	static float sign(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
	}

	public static bool PointInTriangle(Vector3 pt, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		float d1, d2, d3;
		bool has_neg, has_pos;

		d1 = sign(pt, v1, v2);
		d2 = sign(pt, v2, v3);
		d3 = sign(pt, v3, v1);

		has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
		has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

		return !(has_neg && has_pos);
	}


	public static bool LineCircleIntersect(Vector3 circleCenter, float radius, Vector3 A, Vector3 B, out Vector3 closestPointOnLine, out float lineDistance)
	{
		float dot = Vector3.Dot(circleCenter- A, B-A);
		Vector3 linediff = B - A;
		float lineLengt = linediff.magnitude;
		closestPointOnLine = A + linediff.normalized * dot;

		Vector3 lineDirection = (closestPointOnLine - circleCenter).normalized;
		lineDistance = (closestPointOnLine - circleCenter).magnitude;

		return lineDistance < radius && dot < lineLengt && dot > 0;
	}



	public static void FindTangents(Vector2 c, float radius, Vector2 p, out Vector2 ta, out Vector2 tb)
	{
		float dx = c.x - p.x;
		float dy = c.y - p.y;
		float dd = Mathf.Sqrt(dx * dx + dy * dy);
		float a = Mathf.Asin(radius / dd);
		float b = Mathf.Atan2(dy, dx);

		float t = b - a;
		ta = c + new Vector2(radius* Mathf.Sin(t), radius * -Mathf.Cos(t) );

		t = b + a;
		tb = c + new Vector2(radius * -Mathf.Sin(t), radius* Mathf.Cos(t));

	}

	//public static Vector3 ClosestPointOnSphere()
}

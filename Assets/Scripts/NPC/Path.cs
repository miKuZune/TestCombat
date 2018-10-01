using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    public List<GameObject> nodes;
    List<PathSegment> segments;


	void Start ()
    {
		segments = GetSegments();
	}
	
	public List<PathSegment> GetSegments()
    {
        List<PathSegment> segmentsX = new List<PathSegment>();

		int i;
        for(i = 0; i < nodes.Count - 1; i++)
        {
            ////
            Vector3 src = nodes[i].transform.position;
            Vector3 dst = nodes[i+1].transform.position;


            PathSegment s = new PathSegment(src, dst);
            segmentsX.Add(s);

        }

        return segmentsX;
    }
	
	public float GetParam(Vector3 pos, float lastParam)
	{
		// find segment agent is closest to
		float param = 0f;
		PathSegment currSegment = null;
		float tempParam = 0f;
		foreach(PathSegment s in segments)
		{
			tempParam += Vector3.Distance(s.a, s.b);
			if(lastParam <= tempParam)
			{
				currSegment = s;
				break;
			}
		}
		if(currSegment == null) { return 0f; }
		
		// work out dir to go using curr pos
		Vector3 currPos = pos - currSegment.a;
		Vector3 segmentDir = currSegment.b - currSegment.a;
		segmentDir.Normalize();
		// find the point using vector projection
		Vector3 pointInSegment = Vector3.Project(currPos, segmentDir);
		// return next pos to go along path
		param = tempParam - Vector3.Distance(currSegment.a, currSegment.b);
		param += pointInSegment.magnitude;
		return param;
	}
	
	public Vector3 GetPosition(float param)
	{
		// find corresponding segment to current pos on path
		Vector3 position = Vector3.zero;
		PathSegment currSegment = null;
		float tempParam = 0f;
		foreach(PathSegment s in segments)
		{
			tempParam += Vector3.Distance(s.a, s.b);
			if(param <= tempParam)
			{
				currSegment = s;
				break;
			}
		}
		if(currSegment == null) { return Vector3.zero; }
		
		// convert param to a spatial point
		Vector3 segmentDir = currSegment.b - currSegment.a;
		segmentDir.Normalize();
		tempParam -= Vector3.Distance(currSegment.a, currSegment.b);
		tempParam = param - tempParam;
		position = currSegment.a + segmentDir * tempParam;
		return position;
	}
}

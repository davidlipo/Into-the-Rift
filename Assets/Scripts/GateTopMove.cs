using UnityEngine;
using System.Collections;

public class GateTopMove : MonoBehaviour {

	public bool triggered;
	
	private Vector3 topGateStart, topGateEnd;
	float topGateStartTime;
	float topGateJourneyLength;
	
	const float GATE_OPEN_SPEED = 10.0f;
	const float GATE_OPEN_DIST = 10.0f;
	
	// Use this for initialization
	void Start () {
		topGateStart = transform.position;
		topGateEnd = new Vector3 ( transform.position.x, transform.position.y + GATE_OPEN_DIST, transform.position.z);
		topGateStartTime = Time.time;
		topGateJourneyLength = Vector3.Distance(topGateStart, topGateEnd);
	}
	
	// Update is called once per frame
	void Update () {
		if(triggered){
			// Distance moved = time * speed.
			float topGateDistCovered = (Time.time - topGateStartTime) * GATE_OPEN_SPEED;
			// Fraction of journey completed = current distance divided by total distance.
			float topGateFracJourney = topGateDistCovered / topGateJourneyLength;
			transform.position = Vector3.Lerp (topGateStart, topGateEnd, topGateFracJourney);
		}
	}
}

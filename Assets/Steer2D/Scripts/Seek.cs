using System;
using UnityEngine;

namespace Steer2D
{
    public class Seek : SteeringBehaviour
    {
        public Vector2 TargetPoint = Vector2.zero;
				[HideInInspector]
				public Transform SeekTarget;
				public bool SeekMovingTarget;

        public override Vector2 GetVelocity()
        {
						if (SeekMovingTarget)
								TargetPoint = (Vector2)SeekTarget.position;
								
            return ((TargetPoint - (Vector2)transform.position).normalized * agent.MaxVelocity) - agent.CurrentVelocity;   
        }
    }
}

﻿using UnityEngine;

namespace TurnTheGameOn.ArrowWaypointer{		
	public class Waypoint : MonoBehaviour {

		public int radius;
		//[HideInInspector] public WaypointController waypointController;
		[HideInInspector] public int waypointNumber;
        public WaypointController waypointController;

        void Update(){
            
			if (waypointController.player) {
				if(Vector3.Distance(transform.position, waypointController.player.position) < radius){
					waypointController.ChangeTarget ();
				}
			}
		}

		void OnTriggerEnter (Collider col) {
			if(col.gameObject.tag == "PlayerGod"){
				waypointController.WaypointEvent (waypointNumber);
				waypointController.ChangeTarget ();
			}
		}

		#if UNITY_EDITOR
		void OnDrawGizmosSelected(){
			waypointController.OnDrawGizmosSelected (radius);
		}
		#endif
	}
}
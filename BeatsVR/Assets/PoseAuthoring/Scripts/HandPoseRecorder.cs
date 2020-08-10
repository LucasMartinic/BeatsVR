﻿using UnityEngine;

using Grabber = Interaction.Grabber;
using Grabbable = Interaction.Grabbable;

namespace PoseAuthoring
{
    public class HandPoseRecorder : MonoBehaviour
    {
        [SerializeField]
        private HandPuppet puppetHand;
        [SerializeField]
        private Grabber grabber;

        [SerializeField]
        private KeyCode recordKey = KeyCode.Space;
            
        private HandGhost previousGhost;

        private void Update()
        {   
            if(Input.GetKeyDown(recordKey))
            {
                RecordPose();
            }

            HighlightNearestPose();
        }

        private void HighlightNearestPose()
        {
            var grabbable = grabber.FindClosestGrabbable().Item1;
            
            if (grabbable != null && grabbable.Snappable != null)
            {
                HandSnapPose userPose = this.puppetHand.CurrentPoseTracked(grabbable.Snappable.transform);
                HandGhost ghost = grabbable.Snappable.FindNearsetGhost(userPose, out float score, out var bestPose);
                if (ghost != previousGhost)
                {
                    previousGhost?.Highlight(false);
                    previousGhost = ghost;
                }
                ghost?.Highlight(score);
            }
            else if (previousGhost != null)
            {
                previousGhost.Highlight(false);
                previousGhost = null;
            }
        }

        public void RecordPose()
        {
            Grabbable grabbable = grabber.FindClosestGrabbable().Item1;
            grabbable?.Snappable?.AddPose(puppetHand);
        }
    }
}
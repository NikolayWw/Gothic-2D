using Plugins.Creator_Kit___RPG.Scripts.UI;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.Gameplay
{
    /// <summary>
    /// Triggers footsteps sounds during playback of an animation state.
    /// </summary>
    public class FootstepTimer : StateMachineBehaviour
    {
        [Range(0, 1)]
        public float leftFoot, rightFoot;

        public AudioClip[] clips;

        private float lastNormalizedTime;
        private int clipIndex = 0;

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var t = stateInfo.normalizedTime % 1;
            if (lastNormalizedTime < leftFoot && t >= leftFoot)
            {
                UserInterfaceAudio.PlayClip(clips[clipIndex]);
                clipIndex = (clipIndex + 1) % clips.Length;
            }
            if (lastNormalizedTime < rightFoot && t >= rightFoot)
            {
                UserInterfaceAudio.PlayClip(clips[clipIndex]);
                clipIndex = (clipIndex + 1) % clips.Length;
            }
            lastNormalizedTime = t;
        }
    }
}
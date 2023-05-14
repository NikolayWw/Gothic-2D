using Plugins.Creator_Kit___RPG.Scripts.Core;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.Gameplay
{
    public class StopTalking : Event<StopTalking>
    {
        public Animator animator;

        public override void Execute()
        {
            animator.SetBool("Talk", false);
        }
    }
}
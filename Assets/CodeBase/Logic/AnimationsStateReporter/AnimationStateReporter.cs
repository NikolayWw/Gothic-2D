using UnityEngine;

namespace CodeBase.Logic.AnimationsStateReporter
{
    public class AnimationStateReporter : StateMachineBehaviour
    {
        private IAnimatorStateReader _stateReader;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            FindReader(animator.gameObject);
            SendState(stateInfo.shortNameHash);
        }

        private void SendState(int stateInfoShortNameHash) =>
            _stateReader.ExitedState(stateInfoShortNameHash);

        private void FindReader(GameObject animatorGameObject)
        {
            if (_stateReader == null)
                _stateReader = animatorGameObject.GetComponent<IAnimatorStateReader>();
        }
    }
}
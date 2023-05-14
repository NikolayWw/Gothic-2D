using UnityEngine;

namespace CodeBase.AITree.Tree
{
    public abstract class TreeAI : MonoBehaviour
    {
        private Node _root;

        private void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            _root.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}
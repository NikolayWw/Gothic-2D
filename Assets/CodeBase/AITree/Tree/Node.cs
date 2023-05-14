using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.AITree.Tree
{
    public class Node
    {
        private Transform _target;
        protected List<Node> _children = new List<Node>();
        public Node Parent;

        public virtual bool Evaluate()
        {
            return false;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        protected Node()
        {
            Parent = null;
        }

        protected Node(List<Node> children)
        {
            foreach (var child in children)
                SetParents(child);
        }

        protected Transform GetTarget()
        {
            if (_target != null)
                return _target;
            var node = Parent;

            return node?.GetTarget();
        }

        protected void ClearData()
        {
            if (_target)
                _target = null;
            else
                Parent?.ClearData();
        }

        private void SetParents(Node node)
        {
            node.Parent = this;
            _children.Add(node);
        }
    }
}
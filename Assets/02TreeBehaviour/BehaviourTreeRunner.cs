using System;
using UnityEngine;

namespace _02TreeBehaviour
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        [SerializeField] BehaviourTree tree;

        private float frameTime = 0;
        public float UpdateTime = 1;

        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviourTree>();

            var loopNode = ScriptableObject.CreateInstance<RepeatNode>();
            CompositeNode compositeNode1 = ScriptableObject.CreateInstance<CompositeNode>();
            compositeNode1.message = $"CompositeNode1";
            loopNode.childs.Add(compositeNode1);
            CompositeNode compositeNode2 = ScriptableObject.CreateInstance<CompositeNode>();
            compositeNode2.message = $"CompositeNode2";
            loopNode.childs.Add(compositeNode2);
            CompositeNode compositeNode3 = ScriptableObject.CreateInstance<CompositeNode>();
            compositeNode3.message = $"CompositeNode3";
            loopNode.childs.Add(compositeNode3);

            tree.rootNode = loopNode;
        }

        void LateUpdate()
        {
            frameTime += Time.deltaTime;
            if (frameTime > UpdateTime)
            {
                frameTime -= UpdateTime;
                if (tree == null)
                    return;
                if (tree.treeNodeState == BaseNode.NodeState.Running)
                    tree.UpdateState();
            }
        }
    }
}
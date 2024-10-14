using System;

namespace MyEditorView.Runtime
{
    /// <summary>
    /// 执行所有子节点
    /// </summary>
    public class SelectorNode : CompositeNode
    {
        private int currentIndex;

        protected override void OnReset()
        {
            currentIndex = 0;
        }

        protected override State OnUpdate()
        {
            while (currentIndex < children.Count)
            {
                var child = children[currentIndex];
                if (child.Enable == false)
                {
                    currentIndex++;
                    continue;
                }
                else
                {
                    switch (child.UpdateState())
                    {
                        case State.Failure:
                            currentIndex++;
                            break;
                        case State.Success:
                            return State.Success;
                        case State.Running:
                            return State.Running;
                    }
                }
            }

            return State.Success;
        }

        protected override void OnStop()
        {
            currentIndex = 0;
        }

        public void OnSelectIndex(int index)
        {
            currentIndex = Math.Clamp(index, 0, children.Count - 1);
        }
    }
}
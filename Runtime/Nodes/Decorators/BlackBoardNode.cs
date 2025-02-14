using Postive.BehaviourTrees.Runtime.Data;
using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Nodes.Decorators
{
    public class BlackBoardNode : DecoratorNode
    {
        public override string NodeCategory => "BlackBoard";
        public BlackBoard BlackBoard => _blackBoard;
        [SerializeField] private BlackBoard _blackBoard;
        protected override void OnStart(BlackBoard blackBoard) {
            _blackBoard = new BlackBoard();
        }

        protected override BTState OnRun(BlackBoard blackBoard)
        {
            _blackBoard.Parent = blackBoard;
            var result = Child.Run(_blackBoard);
            return result;
        }
        protected override void OnStop(BlackBoard blackBoard) 
        {
            base.OnStop(blackBoard);
            _blackBoard.Clear();
        }
    }
}
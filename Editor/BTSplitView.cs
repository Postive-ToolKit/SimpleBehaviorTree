using UnityEngine.UIElements;

namespace Postive.BehaviourTrees.Editor
{
    public class BTSplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<BTSplitView, TwoPaneSplitView.UxmlTraits> {}
    }
}
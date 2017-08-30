using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Behaviour_tree_tool
{
    public class Node
    {
        public struct Rect
        {
            public int x, y, width, height;
        }

        public Node m_parent = null;
        public List<Node> m_children = new List<Node>();

        public string m_node = "Null";
        public string m_description = "Default Description";
        public string m_nodeType = "Null";

        public Rect m_rect;

        public Node()
        {
            m_rect.x = 0;
            m_rect.y = 0;
            m_rect.width = 0;
            m_rect.height = 0;
        }

        public virtual void OnDraw(PaintEventArgs e) { }

        public virtual void WriteToCPP() { }
        public virtual void WriteToCS() { }
        public virtual void WriteToJava() { }
        public virtual void WriteToPython() { }
    }

    public class ActionNode : Node
    {
        public ActionNode() { }
    }

    public class ConditionNode : Node
    {
        public ConditionNode() { }
    }

    public class SequenceComposite : Node
    {
        public SequenceComposite() { }
    }

    public class SlectorComposite : Node
    {
        public SlectorComposite() { }
    }

    public class RandomSlector : Node
    {
        public RandomSlector() { }
    }

    public class SwitchSlector : Node
    {
        public SwitchSlector() { }
    }

    public class Decorator : Node
    {
        public Decorator() { }
    }

}

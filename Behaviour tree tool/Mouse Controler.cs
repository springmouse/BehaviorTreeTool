using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Behaviour_tree_tool
{
    public enum NodeTypes
    {
        ACTIONNODE,
        CONDITIONNODE,
        SEQUENCECOMPOSITE,
        SLECTORCOMPOSITE,
        DECORATOR,
        RANDOMSLECTOR,
        SWITCHSLECTOR,
        NULL
    }

    public class Mouse_Controler
    {
        public NodeTypes m_nodeToPlace = NodeTypes.NULL;
        public List<Node> m_slectedNodes = new List<Node>();

        public bool m_isDragging = false;

        public virtual void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush2 = new SolidBrush(Color.Magenta);

            foreach (Node node in m_slectedNodes)
            {
                g.FillRectangle(brush2, node.m_rect);
            }
        }
    }
}


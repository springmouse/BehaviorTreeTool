using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Behaviour_tree_tool
{
    /*
    * Enum: NodeTypes
    * ---------------
    *
    * a list of all the node types
    *
    */
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

    /*
    * Class: Mouse_Controler
    * ----------------------
    *
    * holds all the custom mouse info
    *
    * Author: Callum Dunstone
    */
    public class Mouse_Controler
    {
        /*the type of node we want to place*/
        public NodeTypes m_nodeToPlace = NodeTypes.NULL;
        /*the nodes we currently have slected*/
        public List<Node> m_slectedNodes = new List<Node>();

        /*a node we want to connect to another node*/
        public Node m_nodeToConnect = null;
        /*are we connecting the node as a child*/
        public bool m_connectAsChild = false;
        /*are we connecting the node as a parent*/
        public bool m_connectAsParent = false;

        /*are we dragging the mouse*/
        public bool m_isDragging = false;

        /*
        * Function: OnDraw
        * ----------------
        *
        * this draws out a "slected rectangle" around each node in m_slectedNodes
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen), 
        * int offsetX (the offset created by the horizontal scroll bar), 
        * int offsetY(the offset created by the vertical scroll bar)
        *
        * returns: returns nothing as it is a void function
        */
        public virtual void OnDraw(PaintEventArgs e, int offsetX, int offsetY)
        {
            Graphics g = e.Graphics;

            Brush brush2 = new SolidBrush(Color.Magenta);

            foreach (Node node in m_slectedNodes)
            {
                g.FillRectangle(brush2, node.m_rect.X - offsetX, node.m_rect.Y - offsetY, node.m_rect.Width, node.m_rect.Height);
            }
        }
    }
}


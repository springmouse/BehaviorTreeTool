using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Behaviour_tree_tool
{
    public partial class Form2 : Form
    {
        public List<Node> m_nodes = new List<Node>();

        private Mouse_Controler m_mouse = new Mouse_Controler();

        private int sx, sy, lx, ly;

        private int px, py;

        private bool m_isHealdDown = false;

        private bool m_placeNode = false;
        private bool m_shiftPressed = false;

        public Form2()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush2 = new SolidBrush(Color.Brown);

            if (lx < sx)
            {
                int h = sx;
                sx = lx;
                lx = h;
            }

            if (ly < sy)
            {
                int h = sy;
                sy = ly;
                ly = h;
            }

            Pen pen = new Pen(Color.Black);

            if (m_mouse.m_connectAsChild || m_mouse.m_connectAsParent)
            {
                g.DrawLine(pen,
                    px,
                    py,
                    m_mouse.m_nodeToConnect.m_rect.X + (int)(m_mouse.m_nodeToConnect.m_rect.Width * 0.5f),
                    m_mouse.m_nodeToConnect.m_rect.Y + (int)(m_mouse.m_nodeToConnect.m_rect.Height * 0.5f));
            }

            g.FillRectangle(brush2, sx, sy, lx - sx, ly - sy);

            m_mouse.OnDraw(e);

            foreach (Node node in m_nodes)
            {
                node.OnDrawConnections(e);
            }

            foreach (Node node in m_nodes)
            {
                node.OnDraw(e);
            }
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ClearMouseVars();
            }

            DeslectSingle(e);

            DeslectMultiple(e);

            SlectNode(e);

            PlaceNode(e);

        }

        public void ClearMouseVars()
        {
            m_mouse.m_nodeToPlace = NodeTypes.NULL;
            m_mouse.m_slectedNodes.Clear();

            m_mouse.m_nodeToConnect = null;

            m_mouse.m_connectAsChild = false;
            m_mouse.m_connectAsParent = false;

            this.Refresh();
        }

        public void DeslectSingle(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && m_mouse.m_slectedNodes.Count() == 1)
            {
                bool wasClicked = true;
                foreach (Node node in m_mouse.m_slectedNodes)
                {
                    if (node.CheckIfClickedIn(e.X, e.Y) == false)
                    {
                        wasClicked = false;
                    }
                }

                if (wasClicked == false)
                {
                    ClearMouseVars();
                    this.Refresh();
                }
            }
        }

        public void DeslectMultiple(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && m_mouse.m_slectedNodes.Count() > 1)
            {
                int bx, by, tx, ty;
                bx = by = 1000000000;
                tx = ty = 0;

                foreach (Node node in m_mouse.m_slectedNodes)
                {
                    if (node.m_rect.X < bx)
                    {
                        bx = node.m_rect.X;
                    }
                    if (node.m_rect.Y < by)
                    {
                        by = node.m_rect.Y;
                    }
                    if ((node.m_rect.X + node.m_rect.Width) > tx)
                    {
                        tx = (node.m_rect.X + node.m_rect.Width);
                    }
                    if ((node.m_rect.Y + node.m_rect.Height) > ty)
                    {
                        ty = (node.m_rect.Y + node.m_rect.Height);
                    }
                }

                bx -= 50;
                by -= 50;
                tx += 50;
                by += 50;

                if ((bx < e.X && tx > e.X &&
                    by < e.Y && ty > e.Y) == false)
                {
                    ClearMouseVars();
                    this.Refresh();
                }
            }
        }
        
        public void SlectNode(MouseEventArgs e)
        {
            if (m_mouse.m_isDragging == false && m_mouse.m_connectAsChild == false && m_mouse.m_connectAsParent == false)
            {
                foreach (Node node in m_nodes)
                {
                    if (node.CheckIfClickedIn(e.X, e.Y))
                    {
                        if (node.ChecIfClickedOnParentConnector(e.X, e.Y))
                        {
                            m_mouse.m_nodeToConnect = node;
                            m_mouse.m_connectAsChild = true;
                            return;
                        }
                        if (node.CheckIfClickedOnCildConector(e.X, e.Y))
                        {
                            m_mouse.m_nodeToConnect = node;
                            m_mouse.m_connectAsParent = true;
                            return;
                        }

                        m_mouse.m_slectedNodes.Clear();
                        m_mouse.m_slectedNodes.Add(node);

                        this.Refresh();

                        return;
                    }
                }
            }
            else if (m_mouse.m_connectAsParent == true || m_mouse.m_connectAsChild == true)
            {

                if (m_mouse.m_nodeToConnect.CheckIfClickedIn(e.X, e.Y))
                {
                    return;
                }

                foreach (Node node in m_nodes)
                {
                    if (node.CheckIfClickedIn(e.X, e.Y))
                    {
                        if (m_mouse.m_connectAsChild)
                        {
                            node.SetChild(m_mouse.m_nodeToConnect);
                            m_mouse.m_nodeToConnect.SetParent(node);
                            ClearMouseVars();
                            return;
                        }
                        else
                        {
                            node.SetParent(m_mouse.m_nodeToConnect);
                            m_mouse.m_nodeToConnect.SetChild(node);
                            ClearMouseVars();
                            return;
                        }
                    }
                }
            }
        }
        
        public void PlaceNode(MouseEventArgs e)
        {
            if (m_mouse.m_nodeToPlace != NodeTypes.NULL && m_mouse.m_slectedNodes.Count <= 0 && m_placeNode == true)
            {
                m_mouse.m_slectedNodes.Clear();

                Node newNode = NodeToCreat();

                if (m_shiftPressed == false)
                {
                    m_placeNode = false;
                    ClearMouseVars();
                }

                if (newNode != null)
                {
                    newNode.m_rect.X = e.X;
                    newNode.m_rect.Y = e.Y;
                    m_nodes.Add(newNode);
                    this.Refresh();
                }
            }
        }

        public Node NodeToCreat()
        {
            switch (m_mouse.m_nodeToPlace)
            {
                case NodeTypes.ACTIONNODE:
                    return new ActionNode();

                case NodeTypes.CONDITIONNODE:
                    return new ConditionNode();
                    
                case NodeTypes.SEQUENCECOMPOSITE:
                    return new SequenceComposite();
                    
                case NodeTypes.SLECTORCOMPOSITE:
                    return new SlectorComposite();
                    
                case NodeTypes.DECORATOR:
                    return new DecoratorComposite();
                   
                case NodeTypes.RANDOMSLECTOR:
                    return new RandomSlector();
                    
                case NodeTypes.SWITCHSLECTOR:
                    return new SwitchSlector();
                    
                case NodeTypes.NULL:
                    return null;
                    
                default:
                    return null;
            }

            return null;
        }

        private void sequenceNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.SEQUENCECOMPOSITE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        private void slectorNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.SLECTORCOMPOSITE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }
        
        private void decoratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.DECORATOR;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        private void randomSlectorNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.RANDOMSLECTOR;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        private void switchSlectorNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.SWITCHSLECTOR;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        private void actionNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.ACTIONNODE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        private void conditionNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.CONDITIONNODE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        private void Form2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                m_shiftPressed = false;
                ClearMouseVars();
            }
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_mouse.m_slectedNodes.Count() > 0 && e.KeyCode == Keys.Delete)
            {
                foreach (Node node in m_mouse.m_slectedNodes)
                {
                    m_nodes.Remove(node);
                }

                foreach (Node node in m_mouse.m_slectedNodes)
                {
                    foreach (Node p in node.m_parent)
                    {
                        p.m_children.Remove(node);
                    }

                    foreach (Node c in node.m_children)
                    {
                        c.m_parent.Remove(node);
                    }
                }

                m_mouse.m_slectedNodes.Clear();

                this.Refresh();
            }

            if (e.KeyCode == Keys.ShiftKey)
            {
                m_shiftPressed = true;
            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            DeslectSingle(e);

            DeslectMultiple(e);

            if (e.Button == MouseButtons.Left && m_mouse.m_slectedNodes.Count() <= 0 && m_mouse.m_nodeToPlace == NodeTypes.NULL && m_mouse.m_nodeToConnect == null)
            {
                if (m_mouse.m_isDragging == false && m_mouse.m_slectedNodes.Count() <= 0)
                {
                    SlectNode(e);
                    if (m_mouse.m_slectedNodes.Count() <= 0)
                    {
                        if (m_mouse.m_isDragging == false)
                        {
                            sx = e.X;
                            sy = e.Y;
                            m_mouse.m_isDragging = true;
                        }
                    }
                    else
                    {
                        sx = sy = lx = ly = 0;
                    }
                }

                if (m_mouse.m_isDragging == true)
                {
                    lx = e.X;
                    ly = e.Y;
                    this.Refresh();
                }
            }
            else
            {
                if (m_mouse.m_isDragging == true)
                {
                    BoxSlect();
                    m_mouse.m_isDragging = false;
                    this.Refresh();
                }
            }

            if (m_mouse.m_slectedNodes.Count > 0 && e.Button == MouseButtons.Left)
            {
                if (m_isHealdDown == false)
                {
                    SlectNode(e);

                    if (m_mouse.m_slectedNodes.Count <= 0)
                    {
                        return;
                    }
                }

                foreach (Node node in m_mouse.m_slectedNodes)
                {
                    node.m_rect.X += (int)((e.X - px));
                    node.m_rect.Y += (int)((e.Y - py));
                }
                this.Refresh();

                px = e.X;
                py = e.Y;

                m_isHealdDown = true;
            }
            else
            {
                m_isHealdDown = false;
            }
            
            px = e.X;
            py = e.Y;
        }
        
        public void BoxSlect()
        {
            if (lx < sx)
            {
                int h = sx;
                sx = lx;
                lx = h;
            }

            if (ly < sy)
            {
                int h = sy;
                sy = ly;
                ly = h;
            }

            foreach (Node node in m_nodes)
            {
                if (sx < node.m_rect.X && lx > node.m_rect.X &&
                sy < node.m_rect.Y && ly > node.m_rect.Y)
                {
                    m_mouse.m_slectedNodes.Add(node);
                }
            }

            this.Refresh();

            sx = sy = lx = ly = 0;
        }
    }
}

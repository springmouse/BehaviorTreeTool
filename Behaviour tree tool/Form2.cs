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

            g.FillRectangle(brush2, sx, sy, lx - sx, ly - sy);

            m_mouse.OnDraw(e);

            foreach (Node node in m_nodes)
            {
                node.OnDraw(e);
            }            
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                m_mouse.m_nodeToPlace = NodeTypes.NULL;
                m_mouse.m_slectedNodes.Clear();

                this.Refresh();

                return;
            }

            SlectNode(e);
         
            if (m_mouse.m_nodeToPlace !=NodeTypes.NULL && m_mouse.m_slectedNodes.Count <= 0)
            {
                m_mouse.m_slectedNodes.Clear();

                Node newNode = NodeToCreat();

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
        }

        private void slectorNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.SLECTORCOMPOSITE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
        }
        
        private void decoratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.DECORATOR;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
        }

        private void randomSlectorNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.RANDOMSLECTOR;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
        }

        private void switchSlectorNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.SWITCHSLECTOR;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
        }

        private void actionNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.ACTIONNODE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
        }

        private void conditionNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.CONDITIONNODE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_mouse.m_slectedNodes.Count() > 0 && e.KeyCode == Keys.Delete)
            {
                foreach (Node node in m_mouse.m_slectedNodes)
                {
                    m_nodes.Remove(node);
                }

                m_mouse.m_slectedNodes.Clear();

                this.Refresh();
            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && m_mouse.m_slectedNodes.Count() <= 0 && m_mouse.m_nodeToPlace == NodeTypes.NULL)
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

        public void SlectNode(MouseEventArgs e)
        {
            if (m_mouse.m_isDragging == false)
            {
                foreach (Node node in m_nodes)
                {
                    if (node.CheckIfClickedIn(e.X, e.Y))
                    {
                        m_mouse.m_slectedNodes.Clear();
                        m_mouse.m_slectedNodes.Add(node);

                        this.Refresh();

                        return;
                    }
                }
            }
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

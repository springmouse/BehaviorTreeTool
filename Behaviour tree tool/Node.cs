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
        public Node m_parent = null;
        public List<Node> m_children = new List<Node>();

        public string m_node = "Null";
        public string m_description = "Default Description";
        public string m_nodeType = "Null";

        public Rectangle m_rect;

        public Node()
        {

            m_rect.X = 0;
            m_rect.Y = 0;
            m_rect.Width = 100;
            m_rect.Height = 100;
        }

        public bool CheckIfClickedIn(int x, int y)
        {

            if (m_rect.X < x && (m_rect.X + m_rect.Width) > x &&
                m_rect.Y < y && (m_rect.Y + m_rect.Height) > y)
            {
                return true;
            }

            return false;
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



        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Yellow);
            g.FillPie(brush, m_rect, 0.0f, 360.0f);
        }
    }

    public class ConditionNode : Node
    {
        public ConditionNode() { }

        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Green);
            g.FillPie(brush, m_rect, 0.0f, 360.0f);
        }
    }

    public class SequenceComposite : Node
    {
        public SequenceComposite() { }

        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Blue);
            g.FillRectangle(brush, m_rect);
        }
    }

    public class SlectorComposite : Node
    {
        private PointF[] m_pointF = new PointF[4];

        public SlectorComposite()
        {
            m_pointF[0].X = m_rect.X;
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[1].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[1].Y = m_rect.Y;

            m_pointF[0].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);


            m_pointF[0].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[0].Y = m_rect.Y + (m_rect.Height);
        }
        
        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Blue);
            g.FillPolygon(brush, m_pointF);
        }
    }

    public class DecoratorComposite : Node
    {
        public DecoratorComposite() { }

        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

        }
    }

    public class RandomSlector : Node
    {
        public RandomSlector() { }

        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

        }
    }

    public class SwitchSlector : Node
    {
        public SwitchSlector() { }

        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

        }
    }

}

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
        public List<Node> m_parent = new List<Node>();
        public List<Node> m_children = new List<Node>();

        public int m_node = 0;
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

        public virtual void AssignParent() { }
        public virtual void AssignChild() { }

        public float SqrMagnatude(int x, int y)
        {
            return ((x * x) + (y*y));
        }

        public virtual void WriteToCPP() { }
        public virtual void WriteToCS() { }
        public virtual void WriteToJava() { }
        public virtual void WriteToPython() { }
    }
    
    public class ActionNode : Node
    {
        public ActionNode() { m_nodeType = "Action Node"; }



        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Yellow);
            g.FillPie(brush, m_rect, 0.0f, 360.0f);
        }
    }

    public class ConditionNode : Node
    {
        public ConditionNode() { m_nodeType = "Condition Node"; }

        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Green);
            g.FillPie(brush, m_rect, 0.0f, 360.0f);
        }
    }

    public class SequenceComposite : Node
    {
        public SequenceComposite() { m_nodeType = "Sequence Composite Node"; }

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

        public SlectorComposite() { m_nodeType = "Slector Composite Node"; }

        public override void OnDraw(PaintEventArgs e)
        {
            SetPoints();

            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Blue);
            g.FillPolygon(brush, m_pointF);
        }

        public void SetPoints()
        {
            m_pointF[0].X = m_rect.X;
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[1].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[1].Y = m_rect.Y;

            m_pointF[2].X = m_rect.X + (m_rect.Width);
            m_pointF[2].Y = m_rect.Y + (m_rect.Height * 0.5f);


            m_pointF[3].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[3].Y = m_rect.Y + (m_rect.Height);
        }
    }

    public class DecoratorComposite : Node
    {
        private PointF[] m_pointF = new PointF[6];

        public DecoratorComposite() { m_nodeType = "Decorator Node"; }

        public override void OnDraw(PaintEventArgs e)
        {
            SetPoints();

            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.RosyBrown);
            g.FillPolygon(brush, m_pointF);
        }

        public void SetPoints()
        {
            m_pointF[0].X = m_rect.X;
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[1].X = m_rect.X + (m_rect.Width * 0.2f);
            m_pointF[1].Y = m_rect.Y;

            m_pointF[2].X = m_rect.X + (m_rect.Width * 0.8f);
            m_pointF[2].Y = m_rect.Y;
            
            m_pointF[3].X = m_rect.X + (m_rect.Width);
            m_pointF[3].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[4].X = m_rect.X + (m_rect.Width * 0.8f);
            m_pointF[4].Y = m_rect.Y + (m_rect.Height);

            m_pointF[5].X = m_rect.X + (m_rect.Width * 0.2f);
            m_pointF[5].Y = m_rect.Y + (m_rect.Height);
        }
    }

    public class RandomSlector : Node
    {
        private PointF[] m_pointF = new PointF[4];

        public RandomSlector() { m_nodeType = "Random Slector Node"; }

        public override void OnDraw(PaintEventArgs e)
        {
            SetPoints();

            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.LightBlue);
            g.FillPolygon(brush, m_pointF);
        }

        public void SetPoints()
        {
            m_pointF[0].X = m_rect.X;
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[1].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[1].Y = m_rect.Y;

            m_pointF[2].X = m_rect.X + (m_rect.Width);
            m_pointF[2].Y = m_rect.Y + (m_rect.Height * 0.5f);


            m_pointF[3].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[3].Y = m_rect.Y + (m_rect.Height);
        }
    }

    public class SwitchSlector : Node
    {
        private PointF[] m_pointF = new PointF[4];

        public SwitchSlector() { m_nodeType = "Switch Slector Node"; }
        
        public override void OnDraw(PaintEventArgs e)
        {
            SetPoints();

            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Cyan);
            g.FillPolygon(brush, m_pointF);
        }

        public void SetPoints()
        {
            m_pointF[0].X = m_rect.X;
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[1].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[1].Y = m_rect.Y;

            m_pointF[2].X = m_rect.X + (m_rect.Width);
            m_pointF[2].Y = m_rect.Y + (m_rect.Height * 0.5f);


            m_pointF[3].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[3].Y = m_rect.Y + (m_rect.Height);
        }
    }

}
